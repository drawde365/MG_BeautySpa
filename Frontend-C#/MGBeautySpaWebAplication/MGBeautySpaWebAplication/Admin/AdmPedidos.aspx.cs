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
            get => (List<PedidoViewModel>)(Session[SESSION_KEY_PEDIDOS] ?? new List<PedidoViewModel>());
            set => Session[SESSION_KEY_PEDIDOS] = value;
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

            // Filtro: no considerar EnCarrito
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
                case estadoPedido.CONFIRMADO: return "badge-confirmado";
                case estadoPedido.LISTO_PARA_RECOGER: return "badge-listo";
                case estadoPedido.RECOGIDO: return "badge-recogido";
                case estadoPedido.NO_RECOGIDO: return "badge-norecogido";
                default: return "badge-confirmado";
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
            // - CONFIRMADO: solo definir fecha (verde)
            // - LISTO_PARA_RECOGER: todas las acciones
            // - RECOGIDO / NO_RECOGIDO: solo ver información, sin botones
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
                    AbrirModalRecojo(idPedido);
                    break;

                case "CancelarPedido":
                    AbrirModalCancelar(idPedido);
                    break;
            }
        }

        // ===========================
        // MODAL RECOJO
        // ===========================
        private void AbrirModalRecojo(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgEstadoInvalido",
                    "alert('Solo se pueden registrar recojos para pedidos que están LISTO_PARA_RECOGER.');",
                    true);
                return;
            }

            hfIdPedidoRecojo.Value = idPedido.ToString();

            string nombreCliente = pedido.cliente != null
                ? (pedido.cliente.nombre + " " +
                   pedido.cliente.primerapellido + " " +
                   pedido.cliente.segundoapellido)
                : "(Sin cliente)";

            lblInfoRecojo.Text =
                $"Pedido #{pedido.idPedido} – {nombreCliente} (CODTR: {pedido.codigoTransaccion})";

            // Por defecto, hoy
            txtFechaRecojoModal.Text = DateTime.Today.ToString("yyyy-MM-dd");

            MostrarModalRecojo();
        }

        private void MostrarModalRecojo()
        {
            string script = "var m = new bootstrap.Modal(document.getElementById('modalRecojo')); m.show();";
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalRecojo", script, true);
        }

        protected void btnGuardarFechaRecojo_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfIdPedidoRecojo.Value, out int idPedido))
                return;

            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            if (!DateTime.TryParse(txtFechaRecojoModal.Text, out DateTime fechaRecojo))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaInvalida",
                    "alert('Seleccione una fecha de recojo válida.');",
                    true);
                MostrarModalRecojo();
                return;
            }

            // Validación: no futura
            if (fechaRecojo.Date > DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaFutura",
                    "alert('La fecha de recojo no puede ser futura.');",
                    true);
                MostrarModalRecojo();
                return;
            }

            // Validación: >= fecha lista para recoger (si existe)
            if (pedido.fechaListaParaRecojoSpecified &&
                fechaRecojo.Date < pedido.fechaListaParaRecojo.Date)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaMenorLista",
                    "alert('La fecha de recojo debe ser mayor o igual a la fecha \"lista para recoger\".');",
                    true);
                MostrarModalRecojo();
                return;
            }

            // Actualizar DTO antes de llamar al backend
            pedido.fechaRecojo = fechaRecojo;
            pedido.fechaRecojoSpecified = true;
            pedido.estadoPedido = estadoPedido.RECOGIDO;

            pedidoBO.AceptarRecojo(pedido);

            RefrescarPedidosDesdeServicio();

            ScriptManager.RegisterStartupScript(this, GetType(),
                "msgRecojoOk",
                "alert('Fecha de recojo guardada correctamente.');",
                true);
        }

        // ===========================
        // MODAL CANCELAR (NO RECOGIDO)
        // ===========================
        private void AbrirModalCancelar(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgCancelarEstadoInvalido",
                    "alert('Solo se pueden marcar como NO RECOGIDOS los pedidos que están LISTO_PARA_RECOGER.');",
                    true);
                return;
            }

            hfIdPedidoCancelar.Value = idPedido.ToString();

            string nombreCliente = pedido.cliente != null
                ? (pedido.cliente.nombre + " " +
                   pedido.cliente.primerapellido + " " +
                   pedido.cliente.segundoapellido)
                : "(Sin cliente)";

            lblCancelarInfo.Text =
                $"Pedido #{pedido.idPedido} – {nombreCliente} (CODTR: {pedido.codigoTransaccion})";

            MostrarModalCancelar();
        }

        private void MostrarModalCancelar()
        {
            string script = "var m = new bootstrap.Modal(document.getElementById('modalCancelarPedido')); m.show();";
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalCancelar", script, true);
        }

        protected void btnConfirmarCancelar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfIdPedidoCancelar.Value, out int idPedido))
                return;

            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            // Actualizamos el estado en el DTO antes de llamar al BO
            pedido.estadoPedido = estadoPedido.NO_RECOGIDO;

            pedidoBO.RechazarRecojo(pedido);

            RefrescarPedidosDesdeServicio();

            ScriptManager.RegisterStartupScript(this, GetType(),
                "msgCancelOk",
                "alert('El pedido se marcó como NO RECOGIDO correctamente.');",
                true);
        }
    }
}
