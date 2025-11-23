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

        private void invalido()
        {
            pnlCarritoVacio.Visible = true;
            pnlCarritoLleno.Visible = false;

            // Asegúrate de que el Repeater esté vacío
            rpCartItems.DataSource = null;
            rpCartItems.DataBind();

            // Pon los totales en 0
            litSubtotal.Text = "0.00";
            litImpuestos.Text = "0.00";
            litTotal.Text = "0.00";
        }

        private void RebindCartAndSummary(pedidoDTO carrito)
        {
            if(carrito.detallesPedido.Count() == 0)
            {
                invalido();
                return;
            }
            if (carrito.detallesPedido[0].producto.producto.idProducto==0)
            {
                invalido();
                return;
            }
            else
            {
                if (carrito.detallesPedido == null || carrito.detallesPedido.Length == 0)
                {
                    invalido();
                    return;
                } else
                {
                    pnlCarritoVacio.Visible = false;
                    pnlCarritoLleno.Visible = true;

                    // Tu bloque IF para el bug de idProducto == 0 (si aún lo necesitas)
                    if (carrito.detallesPedido[0].producto.producto.idProducto == 0)
                    {
                        // (Manejar este caso si es un bug conocido)
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
                }

                // Actualizar el contador del MasterPage (esto debe ir fuera del 'else')
                Cliente masterPage = this.Master as Cliente;
                if (masterPage != null)
                {
                    masterPage.UpdateCartDisplay();
                }
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
                pedidoBO.EliminarDetalle(itemToRemove, carrito.idPedido); // Elimina de BD

                UpdateCartCount(-itemToRemove.cantidad); // Actualiza contador de cabecera

                // Recalcula el total y guarda en BD y Sesión
                carrito.detallesPedido = listaDetalles.ToArray();
                carrito.total = listaDetalles.Sum(d => d.subtotal);
                carrito.totalSpecified = true;
                //pedidoBO.Modificar(carrito);

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
            // Usa Request.Form para obtener los valores directamente del POST
            string cardNumber = Request.Form[txtCardNumber.UniqueID]?.Trim().Replace(" ", "") ?? "";
            string cvv = Request.Form[txtCVV.UniqueID]?.Trim() ?? "";
            string expiry = Request.Form[txtExpiryDate.UniqueID]?.Trim() ?? "";
            string name = Request.Form[txtNameOnCard.UniqueID]?.Trim() ?? "";

            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 15 ||
                string.IsNullOrEmpty(cvv) || cvv.Length < 3 ||
                string.IsNullOrEmpty(expiry) || string.IsNullOrEmpty(name))
            {
                // Vuelve a mostrar el modal con un mensaje de error
                string errorScript = @"
            alert('Por favor completa todos los campos correctamente.');
            setTimeout(function() {
                var modal = new bootstrap.Modal(document.getElementById('paymentModal'));
                modal.show();
            }, 100);
        ";
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

                byte[] pdf = GenerarPdfPedido(carrito);
                SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = (SoftInvBusiness.SoftInvWSUsuario.usuarioDTO)Session["UsuarioActual"];
                EnviarCorreoConPdf(
                    usuario.correoElectronico,
                    "Comprobante de tu compra - MG Beauty SPA",
                    "¡Hola, "+ usuario.nombre +"!\n¡Gracias por tu compra! Adjuntamos el comprobante en PDF.",
                    pdf
                );


                Session["Carrito"] = null;
                Session["CartCount"] = 0;

                // Actualiza el carrito visualmente
                RebindCartAndSummary(new pedidoDTO { detallesPedido = new detallePedidoDTO[0] });

                // Muestra el modal de éxito
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
            }, 100);
        ";
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

        private byte[] GenerarPdfPedido(pedidoDTO carrito)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = (SoftInvBusiness.SoftInvWSUsuario.usuarioDTO)Session["UsuarioActual"];
                // Crear documento
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // === COLORES ===
                BaseColor verde = new BaseColor(0x14, 0x8C, 0x76);   // #148C76
                BaseColor blancoFondo = new BaseColor(0xF4, 0xFB, 0xF8); // #F4FBF8

                // === LOGO ===
                string rutaLogo = HttpContext.Current.Server.MapPath("~/Content/images/MGFavicon.png");
                if (File.Exists(rutaLogo))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.ScaleToFit(120, 120);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }

                // Título
                Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20,verde);
                Paragraph titulo = new Paragraph("Comprobante de Compra - MG BEAUTY SPA", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingBefore = 10;
                titulo.SpacingAfter = 20;
                doc.Add(titulo);

                // LÍNEA SEPARADORA
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

                // === INFORMACIÓN DEL CLIENTE ===
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
                doc.Add(new Paragraph("\n\n"));

                // ===============================================
                //              TABLA DE PRODUCTOS
                // ===============================================

                PdfPTable tabla = new PdfPTable(4);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 20, 40, 20, 20 }); // Imagen - Nombre - Cantidad - Precio

                // Encabezados
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

                string[] headers = { "Imagen", "Servicio", "Cantidad", "Precio" };
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

                // Filas
                foreach (var d in carrito.detallesPedido)
                {
                    // --- IMAGEN ---
                    string rutaImg = HttpContext.Current.Server.MapPath(d.producto.producto.urlImagen);


                    PdfPCell imgCell = null;

                    if (File.Exists(rutaImg))
                    {
                        try
                        {
                            // Leer bytes sin bloquear
                            byte[] imgBytes = File.ReadAllBytes(rutaImg);

                            // iTextSharp reconoce automáticamente el formato (JPG, PNG, etc.)
                            iTextSharp.text.Image prodImg = iTextSharp.text.Image.GetInstance(imgBytes);

                            // Ajuste del tamaño de la imagen
                            prodImg.ScaleToFit(70, 70);

                            imgCell = new PdfPCell(prodImg)
                            {
                                Padding = 5,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                        }
                        catch
                        {
                            // Si por alguna razón falla, muestra texto pero NO detiene el PDF
                            imgCell = new PdfPCell(new Phrase("Imagen inválida", texto))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                        }
                    }
                    else
                    {
                        imgCell = new PdfPCell(new Phrase("Sin imagen", texto))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        };
                    }

                    tabla.AddCell(imgCell);

                    // Nombre
                    tabla.AddCell(new PdfPCell(new Phrase(d.producto.producto.nombre, texto)) { Padding = 5 });

                    // Cantidad
                    tabla.AddCell(new PdfPCell(new Phrase(d.cantidad.ToString(), texto))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    });

                    // Precio
                    tabla.AddCell(new PdfPCell(new Phrase("S/ " + d.producto.producto.precio, texto))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Padding = 5
                    });
                }

                doc.Add(tabla);

                // ===============================================
                //                     TOTAL
                // ===============================================

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
                doc.Add(tablaTotal);

                // ESPACIO FINAL
                doc.Add(new Paragraph("\n\nGracias por tu compra en MG Beauty SPA", texto));

                doc.Close();
                return ms.ToArray();
            }
        }

        private System.Drawing.Image CargarImagenSinBloqueo(string ruta)
        {
            using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                ms.Position = 0;
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private void EnviarCorreoConPdf(string correoDestino, string asunto, string cuerpo, byte[] pdfBytes)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(correoDestino);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.IsBodyHtml = false;

            // Adjuntar PDF
            mensaje.Attachments.Add(new Attachment(
                new MemoryStream(pdfBytes),
                "ComprobanteCompra.pdf",
                "application/pdf"
            ));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;
            smtp.Send(mensaje);
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            // Redirige al usuario al inicio después de que vea el modal de éxito
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