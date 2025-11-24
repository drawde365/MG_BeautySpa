using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmPedidos : Page
    {
        private readonly PedidoBO pedidoBO = new PedidoBO();

        private const string SESSION_KEY_PEDIDOS = "AdmPedidos_ListaPedidos";

        // ViewModel para la grilla
        private class PedidoViewModel
        {
            public int IdPedido { get; set; }
            public string CodigoTransaccion { get; set; }
            public string NombreCliente { get; set; }
            public estadoPedido Estado { get; set; }
            public DateTime? FechaPago { get; set; }
            public DateTime? FechaListaParaRecojo { get; set; }
            public DateTime? FechaRecojo { get; set; }
            public double Total { get; set; }
        }

        // ViewModel para detalles de pedido (modal)
        private class DetallePedidoViewModel
        {
            public string NombreProducto { get; set; }
            public string TipoPiel { get; set; }
            public int Cantidad { get; set; }
            public double Subtotal { get; set; }
        }

        private List<PedidoViewModel> PedidosEnSesion
        {
            get
            {
                return (List<PedidoViewModel>)(Session[SESSION_KEY_PEDIDOS] ?? new List<PedidoViewModel>());
            }
            set
            {
                Session[SESSION_KEY_PEDIDOS] = value;
            }
        }

        // ===========================
        // PAGE LOAD
        // ===========================
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefrescarPedidosDesdeServicio();
            }
            else
            {
                CargarPedidosDesdeSesion();
            }
        }

        // ===========================
        // CARGA DESDE SERVICIO
        // ===========================
        private void RefrescarPedidosDesdeServicio()
        {
            IList<pedidoDTO> lista = pedidoBO.ListarTodosPedidos() ?? new List<pedidoDTO>();

            // No incluir EnCarrito
            var vm = lista
                .Where(p => p.estadoPedido != estadoPedido.EnCarrito)
                .Select(MapearPedidoDTO)
                .ToList();

            PedidosEnSesion = vm;

            BindPedidos(vm);
        }

        private void CargarPedidosDesdeSesion()
        {
            var vm = PedidosEnSesion ?? new List<PedidoViewModel>();
            BindPedidos(vm);
        }

        private PedidoViewModel MapearPedidoDTO(pedidoDTO p)
        {
            string nombreCliente = p.cliente != null
                ? (p.cliente.nombre + " " +
                   p.cliente.primerapellido + " " +
                   p.cliente.segundoapellido)
                : "(Sin cliente)";

            DateTime? fechaPago = p.fechaPagoSpecified ? (DateTime?)p.fechaPago : null;
            DateTime? fechaLista = p.fechaListaParaRecojoSpecified ? (DateTime?)p.fechaListaParaRecojo : null;
            DateTime? fechaRecojo = p.fechaRecojoSpecified ? (DateTime?)p.fechaRecojo : null;

            return new PedidoViewModel
            {
                IdPedido = p.idPedido,
                CodigoTransaccion = p.codigoTransaccion,
                NombreCliente = nombreCliente,
                Estado = p.estadoPedido,
                FechaPago = fechaPago,
                FechaListaParaRecojo = fechaLista,
                FechaRecojo = fechaRecojo,
                Total = p.total
            };
        }

        private void BindPedidos(List<PedidoViewModel> pedidos)
        {
            if (pedidos == null || !pedidos.Any())
            {
                rptPedidos.Visible = false;
                pnlSinPedidos.Visible = true;
                rptPedidos.DataSource = null;
                rptPedidos.DataBind();
                return;
            }

            rptPedidos.Visible = true;
            pnlSinPedidos.Visible = false;
            rptPedidos.DataSource = pedidos;
            rptPedidos.DataBind();
        }

        // ===========================
        // CSS DE ESTADO
        // ===========================
        protected string ObtenerClaseEstado(object estadoObj)
        {
            if (estadoObj == null) return "badge-confirmado";

            estadoPedido est;
            if (estadoObj is estadoPedido)
            {
                est = (estadoPedido)estadoObj;
            }
            else
            {
                Enum.TryParse(estadoObj.ToString(), out est);
            }

            switch (est)
            {
                case estadoPedido.CONFIRMADO:
                    return "badge-confirmado";
                case estadoPedido.LISTO_PARA_RECOGER:
                    return "badge-listo";
                case estadoPedido.RECOGIDO:
                    return "badge-recogido";
                case estadoPedido.NO_RECOGIDO:
                    return "badge-norecogido";
                default:
                    return "badge-confirmado";
            }
        }

        // ===========================
        // ITEMDATABOUND: VISIBILIDAD DE BOTONES
        // ===========================
        protected void rptPedidos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var vm = (PedidoViewModel)e.Item.DataItem;

            var btnVerPedido = (LinkButton)e.Item.FindControl("btnVerPedido");
            var btnDefinirFecha = (LinkButton)e.Item.FindControl("btnDefinirFecha");
            var btnMarcarRecogido = (LinkButton)e.Item.FindControl("btnMarcarRecogido");
            var btnCancelar = (LinkButton)e.Item.FindControl("btnCancelar");

            // Ver pedido -> siempre visible
            btnVerPedido.Visible = true;

            bool tieneFechaLista = vm.FechaListaParaRecojo.HasValue;

            if (!tieneFechaLista)
            {
                // Sin fecha lista para recoger:
                // solo se puede definir fecha cuando está CONFIRMADO
                btnDefinirFecha.Visible = (vm.Estado == estadoPedido.CONFIRMADO);
                btnMarcarRecogido.Visible = false;
                btnCancelar.Visible = false;
            }
            else
            {
                // Con fecha lista para recoger
                btnDefinirFecha.Visible = false;

                if (vm.Estado == estadoPedido.LISTO_PARA_RECOGER)
                {
                    // Puede marcar recogido o no recogido
                    btnMarcarRecogido.Visible = true;
                    btnCancelar.Visible = true;
                }
                else
                {
                    // RECOGIDO o NO_RECOGIDO -> no hay acciones sobre estado
                    btnMarcarRecogido.Visible = false;
                    btnCancelar.Visible = false;
                }
            }
        }

        // ===========================
        // ACCIONES DEL REPEATER
        // ===========================
        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idPedido = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "VerPedido":
                    CargarDetallePedido(idPedido);
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "showModalDetalles",
                        "var m = new bootstrap.Modal(document.getElementById('modalDetallesPedido')); m.show();",
                        true);
                    break;

                case "DefinirFecha":
                    Response.Redirect("DefinirFechaRecojo.aspx?idPedido=" + idPedido);
                    break;

                case "MarcarRecogido":
                    {
                        var pedido = pedidoBO.ObtenerPorId(idPedido);
                        if (pedido == null) return;

                        if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "msg",
                                "alert('Solo se pueden marcar como recogidos los pedidos que están LISTO_PARA_RECOGER.');",
                                true);
                            return;
                        }

                        pedidoBO.AceptarRecojo(pedido);
                        RefrescarPedidosDesdeServicio();
                    }
                    break;

                case "CancelarPedido":
                    {
                        var pedido = pedidoBO.ObtenerPorId(idPedido);
                        if (pedido == null) return;

                        if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                                "msg",
                                "alert('Solo se pueden marcar como no recogidos los pedidos que están LISTO_PARA_RECOGER.');",
                                true);
                            return;
                        }

                        pedidoBO.RechazarRecojo(pedido);
                        RefrescarPedidosDesdeServicio();
                    }
                    break;
            }
        }

        // ===========================
        // CARGAR DETALLE PARA EL MODAL
        // ===========================
        private void CargarDetallePedido(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            string nombreCliente = pedido.cliente != null
                ? (pedido.cliente.nombre + " " +
                   pedido.cliente.primerapellido + " " +
                   pedido.cliente.segundoapellido)
                : "(Sin cliente)";

            litDetPedido.Text = $"#{pedido.idPedido}";
            litDetCliente.Text = nombreCliente;
            litDetCodTr.Text = pedido.codigoTransaccion ?? "";
            litDetEstado.Text = pedido.estadoPedido.ToString();
            litDetTotal.Text = pedido.total.ToString("F2");

            IList<detallePedidoDTO> detalles = pedidoBO.ObtenerDetallesPorPedido(idPedido) ?? new List<detallePedidoDTO>();

            var vmDetalles = detalles.Select(d =>
            {
                string nombreProducto = "(Producto)";
                string tipoPiel = "";

                try
                {
                    if (d.producto != null)
                    {
                        if (d.producto.producto != null && !string.IsNullOrEmpty(d.producto.producto.nombre))
                            nombreProducto = d.producto.producto.nombre;

                        if (d.producto.tipo != null && !string.IsNullOrEmpty(d.producto.tipo.nombre))
                            tipoPiel = d.producto.tipo.nombre;
                    }
                }
                catch
                {
                    // por si algún campo viene nulo desde el proxy
                }

                return new DetallePedidoViewModel
                {
                    NombreProducto = nombreProducto,
                    TipoPiel = tipoPiel,
                    Cantidad = d.cantidad,
                    Subtotal = d.subtotal
                };
            }).ToList();

            rptDetallesPedido.DataSource = vmDetalles;
            rptDetallesPedido.DataBind();
        }
    }
}
