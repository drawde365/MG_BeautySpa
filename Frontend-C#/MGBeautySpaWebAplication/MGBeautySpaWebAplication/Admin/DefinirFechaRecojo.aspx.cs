using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class DefinirFechaRecojo : Page
    {
        private readonly PedidoBO pedidoBO = new PedidoBO();
        private readonly ProductoTipoBO productoTipoBO = new ProductoTipoBO();
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";
        private class DetalleViewModel
        {
            public int Index { get; set; }
            public string NombreProducto { get; set; }
            public string TipoPiel { get; set; }
            public int Cantidad { get; set; }
            public int StockFisico { get; set; }
            public int StockDespacho { get; set; }
            public int Faltante { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!int.TryParse(Request.QueryString["idPedido"], out int idPedido))
                {
                    Response.Redirect("AdmPedidos.aspx");
                    return;
                }

                hfIdPedido.Value = idPedido.ToString();

                // Info y detalles
                CargarDatosPedido(idPedido);
                CargarDetalles(idPedido);

                var pedido = pedidoBO.ObtenerPorId(idPedido);

                if (pedido != null && pedido.fechaListaParaRecojoSpecified)
                {
                    // Ya tiene fecha lista para recoger -> mostrarla y bloquear todo
                    lblFechaActual.Text = pedido.fechaListaParaRecojo.ToString("dd/MM/yyyy");
                    hfFechaActual.Value = pedido.fechaListaParaRecojo.ToString("yyyy-MM-dd");

                    btnGuardarFecha.Enabled = false;

                    lblResumenStock.Text = "Este pedido ya tiene una fecha lista para recoger definida y no puede modificarse.";
                    lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#047857");
                    return;
                }

                // Si NO tiene fecha aún, usamos la fecha actual
                DateTime hoy = DateTime.Today;
                lblFechaActual.Text = hoy.ToString("dd/MM/yyyy");
                hfFechaActual.Value = hoy.ToString("yyyy-MM-dd");

                // Validar stock y habilitar/deshabilitar botón
                ActualizarResumenStock(idPedido);
            }
        }

        private void CargarDatosPedido(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null)
            {
                Response.Redirect("AdmPedidos.aspx");
                return;
            }

            string nombreCliente = pedido.cliente != null
                ? $"{pedido.cliente.nombre} {pedido.cliente.primerapellido} {pedido.cliente.segundoapellido}"
                : "(Sin cliente)";

            lblInfoPedido.Text = $"Pedido #{pedido.idPedido} – {nombreCliente} (CODTR: {pedido.codigoTransaccion})";
        }

        private void CargarDetalles(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            IList<detallePedidoDTO> detalles = pedido.detallesPedido;

            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);

            var vm = new List<DetalleViewModel>();

            for (int i = 0; i < detalles.Count; i++)
            {
                var det = detalles[i];
                var prod = det.producto;
                int faltante = comprobacion?[i] ?? 0;

                vm.Add(new DetalleViewModel
                {
                    Index = i,
                    NombreProducto = prod?.producto?.nombre ?? "(Producto)",
                    TipoPiel = prod?.tipo?.nombre ?? "-",
                    Cantidad = det.cantidad,
                    StockFisico = prod?.stock_fisico ?? 0,
                    StockDespacho = prod?.stock_despacho ?? 0,
                    Faltante = faltante
                });
            }

            rptDetalles.DataSource = vm;
            rptDetalles.DataBind();
        }

        /// <summary>
        /// Revisa la comprobación de stock y habilita o bloquea el botón
        /// "Definir fecha actual". Solo cuando TODOS los detalles están OK (>=0)
        /// se puede definir la fecha lista para recoger.
        /// </summary>
        private void ActualizarResumenStock(int idPedido)
        {
            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);

            bool todoOk = comprobacion != null && comprobacion.All(v => v >= 0);

            if (todoOk)
            {
                lblResumenStock.Text = "Stock suficiente para todos los productos. Puede definir la fecha actual como lista para recoger.";
                lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#047857");
                btnGuardarFecha.Enabled = true;   // habilitamos botón
            }
            else
            {
                lblResumenStock.Text = "Hay productos con stock físico insuficiente. Ajuste el stock antes de definir la fecha.";
                lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#B91C1C");
                btnGuardarFecha.Enabled = false;  // bloqueamos botón
            }
        }

        protected void rptDetalles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "AgregarStock") return;

            if (!int.TryParse(hfIdPedido.Value, out int idPedido)) return;

            int index = Convert.ToInt32(e.CommandArgument);

            var txtAgregar = (TextBox)e.Item.FindControl("txtAgregar");
            if (!int.TryParse(txtAgregar.Text, out int cantidad) || cantidad <= 0) return;

            var detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);
            if (index < 0 || index >= detalles.Count) return;

            var det = detalles[index];
            var prodTipo = det.producto;
            if (prodTipo == null) return;

            prodTipo.stock_fisico += cantidad;

            pedidoBO.ModificarProductoTipo(prodTipo);

            // Re-cargar grilla y volver a evaluar si ya se puede habilitar el botón
            CargarDetalles(idPedido);
            ActualizarResumenStock(idPedido);
        }

        protected void btnGuardarFecha_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfIdPedido.Value, out int idPedido))
                return;

            var pedidoExiste = pedidoBO.ObtenerPorId(idPedido);
            if (pedidoExiste == null)
                return;

            // Protección extra: si ya tiene fecha, no dejar guardar
            if (pedidoExiste.fechaListaParaRecojoSpecified)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgBloqueo",
                    "alert('Este pedido ya tiene una fecha lista para recoger definida y no puede modificarse.');",
                    true);
                return;
            }

            // Verificar que todos los detalles sigan siendo válidos
            var comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);
            if (comprobacion == null || comprobacion.Any(v => v < 0))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Aún hay productos con stock insuficiente.');", true);
                return;
            }

            // Usar SIEMPRE la fecha actual
            DateTime fecha = DateTime.Today;

            var pedido = pedidoExiste;
            var detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);

            pedido.fechaListaParaRecojo = fecha;
            pedido.fechaListaParaRecojoSpecified = true;
            pedido.estadoPedido = estadoPedido.LISTO_PARA_RECOGER;

            // Actualizar stock_despacho
            foreach (var det in detalles)
            {
                if (det.producto == null) continue;

                det.producto.stock_despacho += det.cantidad;
                pedidoBO.ModificarProductoTipo(det.producto);
            }

            // Guardar pedido
            pedidoBO.Modificar(pedido);

            // Notificar al cliente (cuando el backend lo implemente)
            EnviarCorreoNotificandoCliente(pedido);

            // Mostrar modal de éxito
            ScriptManager.RegisterStartupScript(this, GetType(),
                "showFechaOk",
                "var m = new bootstrap.Modal(document.getElementById('modalFechaOk')); m.show();",
                true);
        }

        private void EnviarCorreoNotificandoCliente(pedidoDTO pedido)
        {
            // Configurar correo
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(pedido.cliente.correoElectronico);
            mensaje.Subject = "Tu pedido te está esperando | MG Beauty SPA";
            mensaje.Body = "¡Hola, " + pedido.cliente.nombre + "!\n\n" +
                           "¡Buenas noticias! Tu pedido nro "+ pedido.idPedido + " ya está listo para recoger.\n" + "Pásate por la tienda cuando puedas, estaremos encantados de atenderte.\n\n"+ 
                           "Si necesitas algo más, solo avísanos.\n¡Gracias por elegirnos!\n"+"MG Beauty SPA";
            mensaje.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;

            smtp.Send(mensaje);
        }
    }
}
