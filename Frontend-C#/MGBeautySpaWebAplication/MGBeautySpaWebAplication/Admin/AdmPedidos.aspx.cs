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

        // ViewModel para la grilla
        private class PedidoViewModel
        {
            public int IdPedido { get; set; }
            public string NombreCliente { get; set; }
            public estadoPedido Estado { get; set; }
            public DateTime? FechaPago { get; set; }
            public DateTime? FechaListaParaRecojo { get; set; }
            public DateTime? FechaRecojo { get; set; }
            public double Total { get; set; }
            public string CodigoTransaccion { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombosFiltros();
                CargarPedidos();
            }
        }

        private void CargarCombosFiltros()
        {
            // Filtro por fecha programada (usa fechaListaParaRecojo)
            ddlFiltroFechaLista.Items.Clear();
            ddlFiltroFechaLista.Items.Add(new ListItem("Sin fecha de recojo", "S"));
            ddlFiltroFechaLista.Items.Add(new ListItem("Con fecha de recojo", "C"));
            ddlFiltroFechaLista.Items.Add(new ListItem("Todos", "T"));
            ddlFiltroFechaLista.SelectedValue = "S"; // vista por defecto: sin fecha

            // Filtro por estado
            ddlFiltroEstado.Items.Clear();
            ddlFiltroEstado.Items.Add(new ListItem("Todos", "T"));
            ddlFiltroEstado.Items.Add(new ListItem("Confirmados", estadoPedido.CONFIRMADO.ToString()));
            ddlFiltroEstado.Items.Add(new ListItem("Listos para recoger", estadoPedido.LISTO_PARA_RECOGER.ToString()));
            ddlFiltroEstado.Items.Add(new ListItem("Recogidos", estadoPedido.RECOGIDO.ToString()));
            ddlFiltroEstado.Items.Add(new ListItem("No recogidos", estadoPedido.NO_RECOGIDO.ToString()));
        }

        protected void Filtros_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            IList<pedidoDTO> lista = pedidoBO.ListarTodosPedidos()??new List<pedidoDTO>();

            // Mapeo a ViewModel
            var pedidos=new List<PedidoViewModel>();
            if (lista.Count > 0)
            {

                pedidos = lista
                    .Where(p => p.estadoPedido != estadoPedido.EnCarrito &&
                                p.estadoPedido != estadoPedido.ELIMINADO)
                    .Select(p => new PedidoViewModel
                    {
                        IdPedido = p.idPedido,
                        CodigoTransaccion = p.codigoTransaccion,
                        NombreCliente = p.cliente != null
                            ? (p.cliente.nombre + " " + p.cliente.primerapellido + " " + p.cliente.segundoapellido)
                            : "(Sin cliente)",
                        Estado = p.estadoPedido,
                        FechaPago = p.fechaPagoSpecified ? (DateTime?)p.fechaPago : null,
                        FechaListaParaRecojo = p.fechaListaParaRecojoSpecified ? (DateTime?)p.fechaListaParaRecojo : null,
                        FechaRecojo = p.fechaRecojoSpecified ? (DateTime?)p.fechaRecojo : null,
                        Total = p.total
                    })
                    .ToList();
            }

            // Vista por defecto: CONFIRMADO + sin fecha programada
            // pero esto se ajusta con los filtros dinámicos:

            // 1) filtro por estado
            if (ddlFiltroEstado.SelectedValue != "T")
            {
                estadoPedido estadoSel =
                    (estadoPedido)Enum.Parse(typeof(estadoPedido), ddlFiltroEstado.SelectedValue);
                pedidos = pedidos.Where(p => p.Estado == estadoSel).ToList();
            }

            // 2) filtro por fechaListaParaRecojo
            switch (ddlFiltroFechaLista.SelectedValue)
            {
                case "S": // sin fecha
                    pedidos = pedidos.Where(p => p.FechaListaParaRecojo == null).ToList();
                    break;
                case "C": // con fecha
                    pedidos = pedidos.Where(p => p.FechaListaParaRecojo != null).ToList();
                    break;
                    // "T" = todos, no se filtra
            }

            if (!pedidos.Any())
            {
                rptPedidos.Visible = false;
                pnlSinPedidos.Visible = true;
                return;
            }

            rptPedidos.Visible = true;
            pnlSinPedidos.Visible = false;

            rptPedidos.DataSource = pedidos;
            rptPedidos.DataBind();
        }

        // CSS según estado
        protected string ObtenerClaseEstado(object estadoObj)
        {
            if (estadoObj == null) return "badge-confirmado";

            var estado = (estadoPedido)estadoObj;

            switch (estado)
            {
                case estadoPedido.CONFIRMADO: return "badge-confirmado";
                case estadoPedido.LISTO_PARA_RECOGER: return "badge-listo";
                case estadoPedido.RECOGIDO: return "badge-recogido";
                case estadoPedido.NO_RECOGIDO: return "badge-norecogido";
                default: return "badge-confirmado";
            }
        }

        protected void rptPedidos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            var vm = (PedidoViewModel)e.Item.DataItem;

            var btnDefinirFecha = (LinkButton)e.Item.FindControl("btnDefinirFecha");
            var btnMarcarRecogido = (LinkButton)e.Item.FindControl("btnMarc" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "" +
                "arRecogido");
            var btnCancelar = (LinkButton)e.Item.FindControl("btnCancelar");

            // Reglas:
            // CONFIRMADO: solo definir fecha
            // LISTO_PARA_RECOGER: definir (modificar), marcar recogido, marcar no recogido
            // RECOGIDO/NO_RECOGIDO: solo lectura
            if (vm.Estado == estadoPedido.CONFIRMADO)
            {
                btnDefinirFecha.Enabled = true;
                btnMarcarRecogido.Visible = false;
                btnCancelar.Visible = false;
            }
            else if (vm.Estado == estadoPedido.LISTO_PARA_RECOGER)
            {
                btnDefinirFecha.Enabled = true;
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

        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idPedido = Convert.ToInt32(e.CommandArgument);
            var pedido = pedidoBO.ObtenerPorId(idPedido);

            switch (e.CommandName)
            {
                case "DefinirFecha":
                    // Nueva pantalla para validar stock + elegir fecha
                    Response.Redirect($"DefinirFechaRecojo.aspx?idPedido={idPedido}");
                    break;

                case "MarcarRecogido":
                    if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('Solo se pueden marcar como recogidos los pedidos que están LISTO_PARA_RECOGER.');", true);
                        return;
                    }

                    // Backend se encarga de actualizar estado, fechaRecojo y stock
                    pedidoBO.AceptarRecojo(pedido);
                    CargarPedidos();
                    break;

                case "CancelarPedido": // Marcar como NO_RECOGIDO
                    if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('Solo se pueden marcar como no recogidos los pedidos que están LISTO_PARA_RECOGER.');", true);
                        return;
                    }

                    pedidoBO.RechazarRecojo(pedido);
                    CargarPedidos();
                    break;
            }
        }
    }
}
