using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class DefinirFechaRecojo : Page
    {
        private readonly PedidoBO pedidoBO = new PedidoBO();
        private readonly ProductoTipoBO productoTipoBO = new ProductoTipoBO();

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

                // Cargar info básica y detalles
                CargarDatosPedido(idPedido);
                CargarDetalles(idPedido);

                // Verificamos si YA tiene fecha de recojo
                var pedido = pedidoBO.ObtenerPorId(idPedido);
                if (pedido != null && pedido.fechaListaParaRecojoSpecified)
                {
                    // Mostrar fecha ya definida
                    txtFechaRecojo.Text = pedido.fechaListaParaRecojo.ToString("yyyy-MM-dd");

                    // Bloquear edición
                    txtFechaRecojo.Enabled = false;
                    btnGuardarFecha.Enabled = false;

                    // Mensaje informativo
                    lblResumenStock.Text = "Este pedido ya tiene una fecha de recojo definida y no puede modificarse.";
                    lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#047857");

                    return;
                }

                // Si todavía NO tiene fecha, aplicar validación de stock normal
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

        private void ActualizarResumenStock(int idPedido)
        {
            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);

            bool todoOk = comprobacion != null && comprobacion.All(v => v >= 0);

            if (todoOk)
            {
                lblResumenStock.Text = "Stock suficiente para todos los productos. Puede definir la fecha lista para recoger.";
                lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#047857");
                btnGuardarFecha.Enabled = true;
            }
            else
            {
                lblResumenStock.Text = "Hay productos con stock físico insuficiente. Ajuste el stock antes de definir la fecha.";
                lblResumenStock.ForeColor = System.Drawing.ColorTranslator.FromHtml("#B91C1C");
                btnGuardarFecha.Enabled = false;
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

            CargarDetalles(idPedido);
            ActualizarResumenStock(idPedido);
        }

        protected void btnGuardarFecha_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfIdPedido.Value, out int idPedido))
                return;

            // Protección extra: si ya tiene fecha, no dejar guardar
            var pedidoExiste = pedidoBO.ObtenerPorId(idPedido);
            if (pedidoExiste != null && pedidoExiste.fechaListaParaRecojoSpecified)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgBloqueo",
                    "alert('Este pedido ya tiene una fecha de recojo definida y no puede modificarse.');",
                    true);
                return;
            }

            if (!DateTime.TryParse(txtFechaRecojo.Text, out DateTime fecha))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Seleccione una fecha válida.');", true);
                return;
            }

            if (fecha < DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('La fecha debe ser mayor o igual al día de hoy.');", true);
                return;
            }

            var comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);
            if (comprobacion.Any(v => v < 0))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Aún hay productos con stock insuficiente.');", true);
                return;
            }

            var pedido = pedidoBO.ObtenerPorId(idPedido);
            var detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);

            pedido.fechaListaParaRecojo = fecha;
            pedido.fechaListaParaRecojoSpecified = true;
            pedido.estadoPedido = estadoPedido.LISTO_PARA_RECOGER;

            foreach (var det in detalles)
            {
                if (det.producto == null) continue;

                det.producto.stock_despacho += det.cantidad;
                pedidoBO.ModificarProductoTipo(det.producto);
            }

            // Guardar pedido
            pedidoBO.Modificar(pedido);

            // Enviar notificación al cliente (cuando esté implementado en backend)
            pedidoBO.EnviarFechaDeRecojoACliente(pedido);

            // Mostrar modal Bootstrap en lugar de alert + redirect inmediato
            ScriptManager.RegisterStartupScript(this, GetType(),
                "showFechaOk",
                "var m = new bootstrap.Modal(document.getElementById('modalFechaOk')); m.show();",
                true);

        }
    }
}
