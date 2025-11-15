using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSProductoTipo;
using SoftInvBusiness.SoftInvWSUsuario;
using System.Globalization;
using System.Web.Script.Serialization;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class DetalleProducto : Page
    {
        private ProductoBO productoBO;
        private ComentarioBO comentarioBO;
        private ProductoTipoBO productoTipoBO;
        private PedidoBO pedidoBO; // <-- 1. AÑADIDO
        private SoftInvBusiness.SoftInvWSProductos.productoDTO producto;
        private IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO> tipos;

        public DetalleProducto()
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO();
            comentarioBO = new ComentarioBO();
            pedidoBO = new PedidoBO(); // <-- 2. AÑADIDO
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
            if (!IsPostBack)
            {
                PintarProducto();
            }
        }

        private void CargarDatos()
        {
            if (!IsPostBack)
            {
                string idProductoStr = Request.QueryString["id"];
                if (int.TryParse(idProductoStr, out int idProducto))
                {
                    producto = productoBO.buscarPorId(idProducto);
                    tipos = productoTipoBO.ObtenerPorIdProductoActivo(idProducto);

                    Session["detalle_producto"] = producto;
                    Session["detalle_tipos"] = tipos;
                }
                else
                {
                    Response.Redirect("~/Cliente/Productos.aspx");
                }
            }
            else
            {
                producto = Session["detalle_producto"] as SoftInvBusiness.SoftInvWSProductos.productoDTO;
                tipos = Session["detalle_tipos"] as IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO>;
            }
        }

        private void PintarProducto()
        {
            if (producto == null)
            {
                Response.Redirect("~/Cliente/Productos.aspx");
                return;
            }

            Page.Title = producto.nombre;
            litNombreProd.Text = producto.nombre;
            litNombre.Text = producto.nombre;
            litDescripcion.Text = producto.descripcion;
            litPrecio.Text = producto.precio.ToString("C", new CultureInfo("es-PE"));
            imgProducto.ImageUrl = ResolveUrl(producto.urlImagen);

            litTamano.Text = producto.tamanho.ToString() + "ml";
            litComoUsar.Text = producto.modoUso;
            litBeneficios.Text = "No especificado.";

            rpPresentaciones.DataSource = tipos;
            rpPresentaciones.DataBind();

            PintarResenas();
        }

        private void PintarResenas()
        {
            var listaComentarios = comentarioBO.ObtenerComentariosPorProducto(producto.idProducto);

            rpComentarios.DataSource = listaComentarios;
            rpComentarios.DataBind();

            if (listaComentarios == null || listaComentarios.Count == 0)
            {
                pnlNoComments.Visible = true;
                litReviewScore.Text = "0.0";
                litReviewCount.Text = "0 reseñas";
            }
            else
            {
                pnlNoComments.Visible = false;
                litReviewScore.Text = producto.promedioValoracion.ToString("0.0");
                litReviewCount.Text = $"{listaComentarios.Count} reseñas";
            }
        }

        protected void rpPresentaciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btn = (LinkButton)e.Item.FindControl("btnIngredientes");
                btn.CommandArgument = e.Item.ItemIndex.ToString();
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(btn);
            }
        }

        protected void btnIngredientes_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out var idx)) return;

            var pres = tipos[idx];
            if (pres == null) return;

            litIngredientes.Text = Server.HtmlEncode(pres.ingredientes).Replace("\n", "<br/>");

            upModalIng.Update();

            ScriptManager.RegisterStartupScript(
                this, GetType(), "openIng", "var modal = new bootstrap.Modal(document.getElementById('modalIng')); modal.show();", true);
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            SoftInvBusiness.SoftInvWSPedido.pedidoDTO carrito = Session["Carrito"] as SoftInvBusiness.SoftInvWSPedido.pedidoDTO;

            if (carrito == null)
            {
                carrito = new SoftInvBusiness.SoftInvWSPedido.pedidoDTO();
                carrito.detallesPedido = new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO[0];
                carrito.cliente = new SoftInvBusiness.SoftInvWSPedido.clienteDTO
                {
                    idUsuario = usuario.idUsuario,
                    idUsuarioSpecified = true
                };
                carrito.estadoPedido = SoftInvBusiness.SoftInvWSPedido.estadoPedido.EnCarrito;
                carrito.estadoPedidoSpecified = true;

                // ----- 3. INSERTA EL NUEVO CARRITO PARA OBTENER ID -----
                int nuevoPedidoID = pedidoBO.Insertar(carrito);
                carrito.idPedido = nuevoPedidoID;
                carrito.idPedidoSpecified = true;
            }

            var listaDetalles = new List<SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO>(carrito.detallesPedido ?? new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO[0]);
            int totalItemsAdded = 0;

            foreach (RepeaterItem item in rpPresentaciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    int itemIndex = item.ItemIndex;

                    if (txtCantidad != null &&
                        int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
                    {
                        SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO presentacion = tipos[itemIndex];

                        var existingItem = listaDetalles.FirstOrDefault(d =>
                            d.producto != null &&
                            d.producto.producto != null &&
                            d.producto.tipo != null &&
                            d.producto.producto.idProducto == this.producto.idProducto &&
                            d.producto.tipo.id == presentacion.tipo.id
                        );

                        if (existingItem != null)
                        {
                            existingItem.cantidad += cantidad;
                            existingItem.subtotal = (double)this.producto.precio * existingItem.cantidad;

                            // ----- 4. LLAMA AL BO PARA MODIFICAR EL DETALLE EN DB -----
                            pedidoBO.ModificarDetalle(existingItem, carrito.idPedido);
                        }
                        else
                        {
                            var nuevoDetalle = new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO
                            {
                                producto = new SoftInvBusiness.SoftInvWSPedido.productoTipoDTO
                                {
                                    producto = new SoftInvBusiness.SoftInvWSPedido.productoDTO { idProducto = this.producto.idProducto, idProductoSpecified = true },
                                    tipo = new SoftInvBusiness.SoftInvWSPedido.tipoProdDTO { id = presentacion.tipo.id, nombre = presentacion.tipo.nombre },
                                    ingredientes = presentacion.ingredientes,
                                    stock_fisico = presentacion.stock_fisico,
                                    stock_fisicoSpecified = presentacion.stock_fisicoSpecified
                                },
                                cantidad = cantidad,
                                cantidadSpecified = true,
                                subtotal = (double)this.producto.precio * cantidad,
                                subtotalSpecified = true
                            };
                            pedidoBO.InsertarDetalle(nuevoDetalle, carrito.idPedido);
                            listaDetalles.Add(nuevoDetalle);
                        }
                        totalItemsAdded += cantidad;
                        txtCantidad.Text = "0";
                    }
                }
            }

            if (totalItemsAdded > 0)
            {
                carrito.detallesPedido = listaDetalles.ToArray();
                carrito.total = carrito.detallesPedido.Sum(d => d.subtotal);
                carrito.totalSpecified = true;

                Session["Carrito"] = carrito;

                int currentCartCount = (Session["CartCount"] as int?) ?? 0;
                Session["CartCount"] = currentCartCount + totalItemsAdded;

                Cliente masterPage = this.Master as Cliente;
                if (masterPage != null)
                {
                    masterPage.UpdateCartDisplay();
                }

                lblCartMessage.Text = "¡Producto(s) añadido(s) al carrito!";
                lblCartMessage.Visible = true;
            }
            else
            {
                lblCartMessage.Text = "Por favor, selecciona una cantidad.";
                lblCartMessage.Visible = true;
            }
        }

        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            /*
            string texto = txtComentario.Text?.Trim();
            if (string.IsNullOrEmpty(texto)) return;

            var usuario = Session["UsuarioActual"] as usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var nuevoComentario = new SoftInvBusiness.SoftInvWSComentario.comentarioDTO();
            nuevoComentario.descripcion = texto;

            nuevoComentario.valoracion = 5;
            nuevoComentario.valoracionSpecified = true;

            nuevoComentario.cliente = new SoftInvBusiness.SoftInvWSComentario.clienteDTO
            {
                idUsuario = usuario.idUsuario,
                idUsuarioSpecified = true
            };

            nuevoComentario.producto = new SoftInvBusiness.SoftInvWSComentario.productoDTO
            {
                idProducto = producto.idProducto,
                idProductoSpecified = true
            };

            comentarioBO.Insertar(nuevoComentario);

            txtNombreComent.Text = "";
            txtComentario.Text = "";
            PintarResenas();
            */
        }
    }
}