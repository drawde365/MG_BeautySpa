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

        // Asumo que tienes un BO para productoTipo
        // ajusta el nombre de la clase si es distinto
        private readonly ProductoTipoBO productoTipoBO = new ProductoTipoBO();

        private class DetalleViewModel
        {
            public int Index { get; set; }
            public string NombreProducto { get; set; }

            // NUEVO: tipo de piel / tipo de producto
            public string TipoPiel { get; set; }

            public int Cantidad { get; set; }
            public int StockFisico { get; set; }
            public int StockDespacho { get; set; }
            public int Faltante { get; set; }
            public string FaltanteTexto
            {
                get
                {
                    if (Faltante < 0)
                        return $"Faltan {-Faltante} unid.";
                    return "OK";
                }
            }
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
                CargarDatosPedido(idPedido);
                CargarDetalles(idPedido);
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
                ? (pedido.cliente.nombre + " " + pedido.cliente.primerapellido + " " + pedido.cliente.segundoapellido)
                : "(Sin cliente)";

            lblInfoPedido.Text =
                $"Pedido #{pedido.idPedido} – {nombreCliente} (CODTR: {pedido.codigoTransaccion})";

            if (pedido.fechaListaParaRecojoSpecified)
            {
                txtFechaRecojo.Text = pedido.fechaListaParaRecojo.ToString("yyyy-MM-dd");
            }
        }

        private void CargarDetalles(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            //IList<detallePedidoDTO> detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);
            IList<detallePedidoDTO> detalles = pedido.detallesPedido;
            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);

            var vmList = new List<DetalleViewModel>();

            for (int i = 0; i < detalles.Count; i++)
            {
                var det = detalles[i];
                int faltante = (comprobacion != null && comprobacion.Count > i)
                    ? comprobacion[i]
                    : 0;

                // Nombre producto
                string nombreProducto = "(Nombre)";
                try
                {
                    if (det.producto != null &&
                        det.producto.producto != null &&
                        !string.IsNullOrEmpty(det.producto.producto.nombre))
                    {
                        nombreProducto = det.producto.producto.nombre;
                    }
                }
                catch { }

                // NUEVO: tipo de piel (nombre del tipo de producto)
                string tipoPiel = "";
                try
                {
                    if (det.producto != null &&
                        det.producto.tipo != null &&
                        !string.IsNullOrEmpty(det.producto.tipo.nombre))
                    {
                        tipoPiel = det.producto.tipo.nombre;
                    }
                }
                catch { }

                int stockFisico = 0;
                int stockDespacho = 0;
                if (det.producto != null)
                {
                    stockFisico = det.producto.stock_fisico;
                    stockDespacho = det.producto.stock_despacho;
                }

                vmList.Add(new DetalleViewModel
                {
                    Index = i,
                    NombreProducto = nombreProducto,
                    TipoPiel = tipoPiel,          // ← asignamos aquí
                    Cantidad = det.cantidad,
                    StockFisico = stockFisico,
                    StockDespacho = stockDespacho,
                    Faltante = faltante
                });
            }


            rptDetalles.DataSource = vmList;
            rptDetalles.DataBind();
        }

        private void ActualizarResumenStock(int idPedido)
        {
            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);

            if (comprobacion != null && comprobacion.All(v => v == 0))
            {
                lblResumenStock.Text = "Stock suficiente para todos los productos. Puede definir la fecha de recojo.";
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
            if (e.CommandName != "AgregarStock")
                return;

            if (!int.TryParse(hfIdPedido.Value, out int idPedido))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            var txtAgregar = (TextBox)e.Item.FindControl("txtAgregar");

            if (!int.TryParse(txtAgregar.Text, out int cantidadAgregar) || cantidadAgregar <= 0)
                return;

            // Obtener detalle y productoTipo desde el backend
            IList<detallePedidoDTO> detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);
            if (index < 0 || index >= detalles.Count)
                return;

            var det = detalles[index];
            var prodTipo = det.producto;
            if (prodTipo == null)
                return;

            // Aumentar stock físico
            prodTipo.stock_fisico += cantidadAgregar;

            // Actualizar en backend (asumo este método existe)
            pedidoBO.ModificarProductoTipo(prodTipo);

            // Recargar datos
            CargarDetalles(idPedido);
            ActualizarResumenStock(idPedido);
        }

        protected void btnGuardarFecha_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfIdPedido.Value, out int idPedido))
                return;

            if (!DateTime.TryParse(txtFechaRecojo.Text, out DateTime fecha))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('Seleccione una fecha válida.');", true);
                return;
            }

            if (fecha <= DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('La fecha debe ser mayor al día de hoy.');", true);
                return;
            }

            // Verificar stock nuevamente antes de confirmar
            IList<int> comprobacion = pedidoBO.ComprobarDetallesPedidos(idPedido);
            if (comprobacion == null || comprobacion.Any(v => v < 0))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('Aún hay productos con stock insuficiente.');", true);
                return;
            }

            // Obtener pedido y detalles
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null)
                return;

            IList<detallePedidoDTO> detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido);

            // Actualizar fecha y estado del pedido
            pedido.fechaListaParaRecojo = fecha;
            pedido.fechaListaParaRecojoSpecified = true;
            pedido.estadoPedido = estadoPedido.LISTO_PARA_RECOGER;

            // Actualizar stock_despacho en cada productoTipo
            foreach (var det in detalles)
            {
                if (det.producto == null) continue;

                det.producto.stock_despacho += det.cantidad;
                pedidoBO.ModificarProductoTipo(det.producto);
            }

            // Guardar pedido
            pedidoBO.Modificar(pedido);

            // Enviar notificación al cliente (cuando lo tengas implementado en backend)
            pedidoBO.EnviarFechaDeRecojoACliente(pedido);

            ScriptManager.RegisterStartupScript(this, GetType(),
                "msg",
                "alert('Fecha de recojo guardada correctamente.'); window.location='AdmPedidos.aspx';",
                true);
        }
    }
}
