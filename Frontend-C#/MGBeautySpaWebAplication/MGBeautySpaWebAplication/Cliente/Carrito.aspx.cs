using iTextSharp.text;
using iTextSharp.text.pdf;
using MGBeautySpaWebAplication.Cliente.Perfil;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductoTipo;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Carrito : Page
    {
        private const double TASA_IGV = 0.18;
        private PedidoBO pedidoBO;
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";
        private EnvioCorreo envioCorreo;
        public Carrito()
        {
            pedidoBO = new PedidoBO();
            envioCorreo = new EnvioCorreo();
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

        private void invalido()
        {
            pnlCarritoVacio.Visible = true;
            pnlCarritoLleno.Visible = false;


            rpCartItems.DataSource = null;
            rpCartItems.DataBind();


            litSubtotal.Text = "0.00";
            litImpuestos.Text = "0.00";
            litTotal.Text = "0.00";
        }

        private void RebindCartAndSummary(pedidoDTO carrito)
        {

            if (carrito == null || carrito.detallesPedido == null || carrito.detallesPedido.Length == 0)
            {
                invalido();
                return;
            }


            var primero = carrito.detallesPedido[0];

            if (primero == null ||
                primero.producto == null ||
                primero.producto.producto == null ||
                primero.producto.producto.idProducto == 0)
            {
                invalido();
                return;
            }


            pnlCarritoVacio.Visible = false;
            pnlCarritoLleno.Visible = true;

            var itemsParaRepeater = carrito.detallesPedido.Select(d => new
            {
                ProductId = d.producto.producto.idProducto,
                Nombre = d.producto.producto.nombre,
                PrecioUnitario = d.producto.producto.precio,
                Cantidad = d.cantidad,
                ImageUrl = ResolveUrl(d.producto.producto.urlImagen),
                Tamano = d.producto.producto.tamanho + "ml",
                TipoPiel = d.producto.tipo.nombre
            }).ToList();

            rpCartItems.DataSource = itemsParaRepeater;
            rpCartItems.DataBind();

            LoadOrderSummary(carrito);


            Cliente master = this.Master as Cliente;
            master?.UpdateCartDisplay();
        }





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
                        continue;
                    }

                    if (nuevaCantidad == 0)
                    {
                        listaDetalles.Remove(itemToUpdate);
                        pedidoBO.EliminarDetalle(itemToUpdate, carrito.idPedido);
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


            if (totalItemsChanged > 0)
            {
                carrito = pedidoBO.ObtenerCarritoPorCliente(usuario.idUsuario);
                Session["Carrito"] = carrito;


                int currentCount = carrito.detallesPedido.Sum(d => d.cantidad);
                Session["CartCount"] = currentCount;
            }

            RebindCartAndSummary(carrito);
        }


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
                pedidoBO.EliminarDetalle(itemToRemove, carrito.idPedido);

                UpdateCartCount(-itemToRemove.cantidad);


                carrito.detallesPedido = listaDetalles.ToArray();
                carrito.total = listaDetalles.Sum(d => d.subtotal);
                carrito.totalSpecified = true;


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

        protected async void btnProcessPayment_Click(object sender, EventArgs e)
        {

            string cardNumber = Request.Form[txtCardNumber.UniqueID]?.Trim().Replace(" ", "") ?? "";
            string cvv = Request.Form[txtCVV.UniqueID]?.Trim() ?? "";
            string expiry = Request.Form[txtExpiryDate.UniqueID]?.Trim() ?? "";
            string name = Request.Form[txtNameOnCard.UniqueID]?.Trim() ?? "";

            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 15 ||
                string.IsNullOrEmpty(cvv) || cvv.Length < 3 ||
                string.IsNullOrEmpty(expiry) || string.IsNullOrEmpty(name))
            {

                string errorScript = @"
            alert('Por favor completa todos los campos correctamente.');
            setTimeout(function() {
                var modal = new bootstrap.Modal(document.getElementById('paymentModal'));
                modal.show();
            }, 100);";
                ClientScript.RegisterStartupScript(this.GetType(), "PaymentError", errorScript, true);
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
                carrito.IGV = TASA_IGV * carrito.total;
                carrito.IGVSpecified = true;
                carrito.idPedidoSpecified = true;

                pedidoBO.Modificar(carrito);

                string rutaLogo = HttpContext.Current.Server.MapPath("~/Content/images/MGFavicon.png");
                _ = Task.Run(async () =>
                {
                    enviarCorreoCliente(carrito,rutaLogo);
                });

                Session["Carrito"] = null;
                Session["CartCount"] = 0;


                RebindCartAndSummary(new pedidoDTO { detallesPedido = new detallePedidoDTO[0] });

                string successScript = @"
            setTimeout(function() {
                var paymentModal = bootstrap.Modal.getInstance(document.getElementById('paymentModal'));
                if (paymentModal) {
                    paymentModal.hide();
                }
                setTimeout(function() {
                    var successModal = new bootstrap.Modal(document.getElementById('paymentSuccessModal'));
                    successModal.show();
                }, 300);
            }, 100);";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowSuccess", successScript, true);


            }
            else
            {
                string failScript = @"
            alert('El pago no pudo ser procesado. Verifica los datos de tu tarjeta.');
            setTimeout(function() {
                var modal = new bootstrap.Modal(document.getElementById('paymentModal'));
                modal.show();
            }, 100);
        ";
                ClientScript.RegisterStartupScript(this.GetType(), "PaymentFail", failScript, true);
            }
        }

        private async Task enviarCorreoCliente(pedidoDTO carrito,string ruta)
        {
            byte[] pdf = GenerarPdfPedido(carrito,ruta);
            SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = (SoftInvBusiness.SoftInvWSUsuario.usuarioDTO)Session["UsuarioActual"];
            string asunto = "Comprobante de tu compra - MG Beauty SPA";
            string cuerpo = "¡Hola, " + usuario.nombre + "!\n¡Gracias por tu compra! Adjuntamos el comprobante en PDF.";
            await envioCorreo.enviarCorreo(usuario.correoElectronico, asunto, cuerpo, pdf);
        }

        private byte[] GenerarPdfPedido(pedidoDTO carrito,string ruta)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = (SoftInvBusiness.SoftInvWSUsuario.usuarioDTO)Session["UsuarioActual"];

                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();


                BaseColor verde = new BaseColor(0x14, 0x8C, 0x76);
                BaseColor blancoFondo = new BaseColor(0xF4, 0xFB, 0xF8);


                string rutaLogo = ruta;
                if (File.Exists(rutaLogo))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.ScaleToFit(120, 120);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }


                Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, verde);
                Paragraph titulo = new Paragraph("Comprobante de Compra - MG BEAUTY SPA", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingBefore = 10;
                titulo.SpacingAfter = 20;
                doc.Add(titulo);


                PdfPTable linea = new PdfPTable(1);
                linea.WidthPercentage = 100;
                PdfPCell cellSep = new PdfPCell(new Phrase(""))
                {
                    BackgroundColor = verde,
                    FixedHeight = 3,
                    Border = Rectangle.NO_BORDER
                };
                linea.AddCell(cellSep);
                doc.Add(linea);

                doc.Add(new Paragraph("\n"));


                Font label = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, verde);
                Font texto = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                doc.Add(new Paragraph("Cliente:", label));
                doc.Add(new Paragraph($"{usuario.nombre} {usuario.primerapellido} {usuario.segundoapellido}", texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Fecha de Pago:", label));
                doc.Add(new Paragraph(carrito.fechaPago.ToString(), texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Código de Transacción:", label));
                doc.Add(new Paragraph(carrito.codigoTransaccion, texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Nro de Pedido:", label));
                doc.Add(new Paragraph(carrito.idPedido.ToString(), texto));
                doc.Add(new Paragraph("\n\n"));






                PdfPTable tabla = new PdfPTable(5);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 35, 20, 15, 15, 15 });


                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

                string[] headers = { "Producto", "Tipo de piel", "Cantidad", "Precio", "Subtotal" };
                foreach (var h in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(h, headerFont))
                    {
                        BackgroundColor = verde,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 8
                    };
                    tabla.AddCell(headerCell);
                }


                foreach (var d in carrito.detallesPedido)
                {


                    tabla.AddCell(new PdfPCell(new Phrase(d.producto.producto.nombre, texto)) { Padding = 5 });

                    tabla.AddCell(new PdfPCell(new Phrase(d.producto.tipo.nombre, texto)) { Padding = 5 });


                    tabla.AddCell(new PdfPCell(new Phrase(d.cantidad.ToString(), texto))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    });


                    tabla.AddCell(new PdfPCell(new Phrase("S/ " + d.producto.producto.precio, texto))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    });

                    tabla.AddCell(new PdfPCell(new Phrase("S/ " + d.producto.producto.precio * d.cantidad, texto))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    });

                }

                doc.Add(tabla);






                doc.Add(new Paragraph("\n"));

                PdfPTable tablaTotal = new PdfPTable(1);
                tablaTotal.WidthPercentage = 30;
                tablaTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell totalCell = new PdfPCell(new Phrase("Total: S/ " + carrito.total,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE)))
                {
                    BackgroundColor = verde,
                    Padding = 10,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                tablaTotal.AddCell(totalCell);

                PdfPCell igvCell = new PdfPCell(new Phrase("IGV: S/ " + carrito.total * TASA_IGV,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK)))
                {
                    BackgroundColor = blancoFondo,
                    Padding = 10,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                tablaTotal.AddCell(igvCell);

                doc.Add(tablaTotal);


                doc.Add(new Paragraph("\n\n¡Gracias por tu compra en MG Beauty SPA!", texto));

                doc.Close();
                return ms.ToArray();
            }
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Cliente/InicioCliente.aspx");
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