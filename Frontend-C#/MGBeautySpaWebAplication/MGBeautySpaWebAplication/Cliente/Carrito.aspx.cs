using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductoTipo;
using SoftInvBusiness.SoftInvWSUsuario;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Carrito : Page
    {
        private const double TASA_IGV = 0.18;
        private PedidoBO pedidoBO;

        public Carrito()
        {
            pedidoBO = new PedidoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarrito();
            }
        }

        private void CargarCarrito()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            pedidoDTO carrito = Session["Carrito"] as pedidoDTO;

            if (carrito == null)
            {
                carrito = pedidoBO.ObtenerCarritoPorCliente(usuario.idUsuario);
            }

            if (carrito == null)
            {
                carrito = new pedidoDTO
                {
                    detallesPedido = new detallePedidoDTO[0],
                    cliente = new SoftInvBusiness.SoftInvWSPedido.clienteDTO { idUsuario = usuario.idUsuario, idUsuarioSpecified = true },
                    estadoPedido = estadoPedido.EnCarrito,
                    estadoPedidoSpecified = true,
                    total = 0,
                    totalSpecified = true
                };
            }

            Session["Carrito"] = carrito;

            RebindCartAndSummary(carrito);
        }

        private void LoadOrderSummary(pedidoDTO carrito)
        {
            double total = carrito.total;
            double subtotal = total / (1 + TASA_IGV);
            double impuestos = total - subtotal;

            litSubtotal.Text = subtotal.ToString("N2");
            litImpuestos.Text = impuestos.ToString("N2");
            litTotal.Text = total.ToString("N2");
        }

        private void RebindCartAndSummary(pedidoDTO carrito)
        {
            if (carrito.detallesPedido == null)
            {
                carrito.detallesPedido = new detallePedidoDTO[0];
            }

            var itemsParaRepeater = carrito.detallesPedido.Select(d => new
            {
                ProductId = d.producto.producto.idProducto,
                Nombre = d.producto.producto.nombre,
                PrecioUnitario = d.producto.producto.precio,
                Cantidad = d.cantidad,
                ImageUrl = ResolveUrl(d.producto.producto.urlImagen),
                Tamano = d.producto.producto.tamanho.ToString() + "ml",
                TipoPiel = d.producto.tipo.nombre
            }).ToList();

            rpCartItems.DataSource = itemsParaRepeater;
            rpCartItems.DataBind();

            LoadOrderSummary(carrito);

            Cliente masterPage = this.Master as Cliente;
            if (masterPage != null)
            {
                masterPage.UpdateCartDisplay();
            }
        }

        // ▼▼▼ MÉTODO ELIMINADO ▼▼▼
        // protected void Quantity_Click(object sender, EventArgs e) { ... }


        // ▼▼▼ NUEVO MÉTODO DE ACTUALIZACIÓN ▼▼▼
        protected void btnActualizarCarrito_Click(object sender, EventArgs e)
        {
            pedidoDTO carrito = Session["Carrito"] as pedidoDTO;
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (carrito == null || usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var listaDetalles = new List<detallePedidoDTO>(carrito.detallesPedido);
            int totalItemsChanged = 0;

            foreach (RepeaterItem item in rpCartItems.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    var hdnItemKey = (HiddenField)item.FindControl("hdnItemKey");

                    if (txtCantidad == null || hdnItemKey == null ||
                        !int.TryParse(txtCantidad.Text, out int nuevaCantidad))
                    {
                        continue;
                    }

                    string[] parts = hdnItemKey.Value.Split('|');
                    int productId = int.Parse(parts[0]);
                    string tipoPielNombre = parts[1];

                    detallePedidoDTO itemToUpdate = listaDetalles.FirstOrDefault(i =>
                        i.producto.producto.idProducto == productId &&
                        i.producto.tipo.nombre == tipoPielNombre);

                    if (itemToUpdate == null) continue;

                    int cantidadOriginal = itemToUpdate.cantidad;

                    if (nuevaCantidad == cantidadOriginal)
                    {
                        continue; // No hay cambios
                    }

                    if (nuevaCantidad == 0)
                    {
                        listaDetalles.Remove(itemToUpdate);
                        pedidoBO.EliminarDetalle(itemToUpdate);
                    }
                    else
                    {
                        itemToUpdate.cantidad = nuevaCantidad;
                        itemToUpdate.subtotal = itemToUpdate.producto.producto.precio * itemToUpdate.cantidad;
                        pedidoBO.ModificarDetalle(itemToUpdate, carrito.idPedido);
                    }
                    totalItemsChanged++;
                }
            }

            // Si se hizo algún cambio, recarga todo desde la BD para tener el total correcto
            if (totalItemsChanged > 0)
            {
                carrito = pedidoBO.ObtenerCarritoPorCliente(usuario.idUsuario);
                Session["Carrito"] = carrito;

                // Actualiza el contador global de la cabecera
                int currentCount = carrito.detallesPedido.Sum(d => d.cantidad);
                Session["CartCount"] = currentCount;
            }

            RebindCartAndSummary(carrito);
        }

        // ▼▼▼ NUEVO MÉTODO PARA ELIMINAR FILA ▼▼▼
        protected void btnEliminarItem_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string argument = clickedButton.CommandArgument;
            string[] parts = argument.Split('|');

            if (parts.Length != 2 || !int.TryParse(parts[0], out int productId))
                return;

            string tipoPielNombre = parts[1];
            pedidoDTO carrito = Session["Carrito"] as pedidoDTO;
            if (carrito == null || carrito.detallesPedido == null) return;

            var listaDetalles = new List<detallePedidoDTO>(carrito.detallesPedido);

            detallePedidoDTO itemToRemove = listaDetalles.FirstOrDefault(i =>
                i.producto.producto.idProducto == productId &&
                i.producto.tipo.nombre == tipoPielNombre);

            if (itemToRemove != null)
            {
                listaDetalles.Remove(itemToRemove);
                pedidoBO.EliminarDetalle(itemToRemove); // Elimina de BD

                UpdateCartCount(-itemToRemove.cantidad); // Actualiza contador de cabecera

                // Recalcula el total y guarda en BD y Sesión
                carrito.detallesPedido = listaDetalles.ToArray();
                carrito.total = listaDetalles.Sum(d => d.subtotal);
                carrito.totalSpecified = true;
                pedidoBO.Modificar(carrito);

                Session["Carrito"] = carrito;
                RebindCartAndSummary(carrito);
            }
        }


        private void UpdateCartCount(int delta)
        {
            int currentCount = (Session["CartCount"] as int?) ?? 0;
            Session["CartCount"] = Math.Max(0, currentCount + delta);
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            pedidoDTO carrito = Session["Carrito"] as pedidoDTO;
            if (carrito == null || carrito.detallesPedido.Length == 0)
            {
                return;
            }

            ScriptManager.RegisterStartupScript(
                this.Page, this.GetType(),
                "ShowPaymentModal",
                "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();",
                true
            );
        }

        protected void btnProcessPayment_Click(object sender, EventArgs e)
        {
            string cardNumber = txtCardNumber.Text.Trim().Replace(" ", "");
            string cvv = txtCVV.Text.Trim();
            string expiry = txtExpiryDate.Text.Trim();
            string name = txtNameOnCard.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 15 ||
                string.IsNullOrEmpty(cvv) || cvv.Length < 3 ||
                string.IsNullOrEmpty(expiry) || string.IsNullOrEmpty(name))
            {
                ShowPaymentModal();
                return;
            }

            bool paymentSuccess = SimulatePaymentIntegration(cardNumber);

            if (paymentSuccess)
            {
                pedidoDTO carrito = Session["Carrito"] as pedidoDTO;
                if (carrito == null) return;

                carrito.estadoPedido = estadoPedido.CONFIRMADO;
                carrito.estadoPedidoSpecified = true;
                carrito.fechaPago = DateTime.Now;
                carrito.fechaPagoSpecified = true;
                carrito.codigoTransaccion = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8);

                pedidoBO.Modificar(carrito);

                Session["Carrito"] = null;
                Session["CartCount"] = 0;

                Response.Redirect(ResolveUrl("~/Cliente/ConfirmacionPedido.aspx"));
            }
            else
            {
                ShowPaymentModal();
            }
        }

        private bool SimulatePaymentIntegration(string cardNumber)
        {
            if (cardNumber.EndsWith("0"))
            {
                return false;
            }
            return true;
        }

        private void ShowPaymentModal()
        {
            string script = "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ReopenModal", script, true);
        }
    }
}