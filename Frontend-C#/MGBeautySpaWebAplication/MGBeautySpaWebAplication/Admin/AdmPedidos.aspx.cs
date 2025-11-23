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
                // Primera carga: trae pedidos del backend y los guarda en sesión
                RefrescarPedidosDesdeServicio();
            }
            else
            {
                // En postbacks (acciones), volvemos a bindear desde sesión
                CargarPedidosDesdeSesion();
            }
        }

        // ===========================
        // CARGA DESDE SERVICIO
        // ===========================
        private void RefrescarPedidosDesdeServicio()
        {
            IList<pedidoDTO> lista = pedidoBO.ListarTodosPedidos() ?? new List<pedidoDTO>();

            var vm = lista
                .Where(p => p.estadoPedido != estadoPedido.EnCarrito &&
                            p.estadoPedido != estadoPedido.ELIMINADO)
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
                // por si viene como string
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
        // ITEMDATABOUND: HABILITAR / OCULTAR BOTONES
        // ===========================
        protected void rptPedidos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var vm = (PedidoViewModel)e.Item.DataItem;

            var btnDefinirFecha = (LinkButton)e.Item.FindControl("btnDefinirFecha");
            var btnMarcarRecogido = (LinkButton)e.Item.FindControl("btnMarcarRecogido");
            var btnCancelar = (LinkButton)e.Item.FindControl("btnCancelar");

            // Reglas:
            // - CONFIRMADO: solo definir fecha
            // - LISTO_PARA_RECOGER: definir (modificar), marcar recogido, marcar no recogido
            // - RECOGIDO / NO_RECOGIDO: sin acciones
            if (vm.Estado == estadoPedido.CONFIRMADO)
            {
                btnDefinirFecha.Enabled = true;
                btnDefinirFecha.Attributes.Remove("disabled");

                btnMarcarRecogido.Visible = false;
                btnCancelar.Visible = false;
            }
            else if (vm.Estado == estadoPedido.LISTO_PARA_RECOGER)
            {
                btnDefinirFecha.Enabled = true;
                btnDefinirFecha.Attributes.Remove("disabled");

                btnMarcarRecogido.Visible = true;
                btnCancelar.Visible = true;
            }
            else
            {
                btnDefinirFecha.Enabled = false;
                btnDefinirFecha.Attributes["disabled"] = "disabled";

                btnMarcarRecogido.Visible = false;
                btnCancelar.Visible = false;
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
    }
}
