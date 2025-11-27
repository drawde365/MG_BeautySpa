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
        private const string SESSION_KEY_PAGINA_BREAK = "AdmPedidos_PaginaBreak";
        private const string SESSION_KEY_TOTAL_PAGINAS = "AdmPedidos_TotalPaginas";

        private const int PAGE_SIZE = 10;   // 10 pedidos por página
        private const int PAGES_PER_BATCH = 5; // 10 páginas por lote => 100 pedidos

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

            // Página lógica a la que pertenece este pedido (1..N)
            public int PageNumber { get; set; }
        }

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

        private int PaginaBreak
        {
            get => (int)(Session[SESSION_KEY_PAGINA_BREAK] ?? 0);
            set => Session[SESSION_KEY_PAGINA_BREAK] = value;
        }

        private int TotalPaginas
        {
            get => (int)(Session[SESSION_KEY_TOTAL_PAGINAS] ?? 0);
            set => Session[SESSION_KEY_TOTAL_PAGINAS] = value;
        }

        private int PaginaActual
        {
            get => (int)(ViewState["PaginaActual"] ?? 1);
            set => ViewState["PaginaActual"] = value;
        }

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

            // Actualizar campos ocultos y literal de total de páginas SIEMPRE
            hfTotalPaginas.Value = (TotalPaginas > 0 ? TotalPaginas : 1).ToString();
            hfPaginaBreak.Value = PaginaBreak.ToString();
            hfPaginaActual.Value = PaginaActual.ToString();
            litTotalPaginas.Text = hfTotalPaginas.Value;
        }

        /// <summary>
        /// Resetea el estado y carga el primer lote de páginas.
        /// </summary>
        private void RefrescarPedidosDesdeServicio()
        {
            TotalPaginas = pedidoBO.obtenerPaginas();
            PaginaActual = 1;
            PaginaBreak = 0;
            PedidosEnSesion = new List<PedidoViewModel>();

            // Carga el primer lote (páginas 1-10)
            CargarSiguienteLote();

            CargarPedidosDesdeSesion();
        }

        /// <summary>
        /// Carga el siguiente lote de 10 páginas (hasta 100 pedidos) usando pedidoBO.TraerMasPaginas().
        /// </summary>
        private void CargarSiguienteLote()
        {
            // Lote actual = (PaginaBreak / 10) + 1
            int numeroLote = (PaginaBreak / PAGES_PER_BATCH) + 1;

            IList<pedidoDTO> lista = pedidoBO.TraerMasPaginas(numeroLote) ?? new List<pedidoDTO>();

            // Por si el servicio trae EnCarrito, lo filtramos igual que antes.
            var listaFiltrada = lista
                .Where(p => p.estadoPedido != estadoPedido.EnCarrito)
                .ToList();

            var acumulado = PedidosEnSesion ?? new List<PedidoViewModel>();
            int baseIndex = acumulado.Count; // para calcular PageNumber global

            foreach (var dto in listaFiltrada)
            {
                var vm = MapearPedidoDTO(dto);

                int globalIndex = baseIndex; // 0-based
                vm.PageNumber = (globalIndex / PAGE_SIZE) + 1;

                acumulado.Add(vm);
                baseIndex++;
            }

            PedidosEnSesion = acumulado;

            // Recalcular PáginaBreak en función del total de items cargados.
            int totalItems = acumulado.Count;
            PaginaBreak = (int)Math.Ceiling(totalItems / (double)PAGE_SIZE);

            if (PaginaBreak > TotalPaginas)
                PaginaBreak = TotalPaginas;
        }

        private void CargarPedidosDesdeSesion()
        {
            var vm = PedidosEnSesion ?? new List<PedidoViewModel>();
            BindPedidos(vm);

            // Después de bindear, nos aseguramos que la JS muestre la página actual.
            ScriptManager.RegisterStartupScript(this, GetType(),
                "mostrarPaginaInicial",
                $"setTimeout(function(){{ if(window.mgPedidos) window.mgPedidos.mostrarPagina({PaginaActual}); }}, 50);",
                true);
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
                default:
                    return "badge-confirmado";
            }
        }

        protected void rptPedidos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var vm = (PedidoViewModel)e.Item.DataItem;

            var btnVerPedido = (LinkButton)e.Item.FindControl("btnVerPedido");
            var btnDefinirFecha = (LinkButton)e.Item.FindControl("btnDefinirFecha");
            var btnMarcarRecogido = (LinkButton)e.Item.FindControl("btnMarcarRecogido");

            btnVerPedido.Visible = true;

            bool tieneFechaLista = vm.FechaListaParaRecojo.HasValue;

            if (!tieneFechaLista)
            {
                // Solo cuando está CONFIRMADO aún se muestra "Definir fecha"
                btnDefinirFecha.Visible = (vm.Estado == estadoPedido.CONFIRMADO);
                btnMarcarRecogido.Visible = false;
            }
            else
            {
                btnDefinirFecha.Visible = false;

                if (vm.Estado == estadoPedido.LISTO_PARA_RECOGER)
                {
                    btnMarcarRecogido.Visible = true;
                }
                else
                {
                    btnMarcarRecogido.Visible = false;
                }
            }
        }

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
                    PrepararModalRecogido(idPedido);
                    break;
            }
        }

        /// <summary>
        /// Evento del botón oculto: se dispara cuando el JS necesita cargar un nuevo lote.
        /// </summary>
        protected void btnCargarPaginas_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfPaginaSolicitada.Value, out int paginaDestino))
                paginaDestino = PaginaActual;

            // Si la página destino está más allá del break, hay que cargar otro lote.
            if (paginaDestino > PaginaBreak)
            {
                CargarSiguienteLote();
            }

            PaginaActual = paginaDestino;

            CargarPedidosDesdeSesion();
        }

        private void PrepararModalRecogido(int idPedido)
        {
            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgEstadoIncorrecto",
                    "alert('Solo se pueden marcar como RECOGIDOS los pedidos que están LISTO_PARA_RECOGER.');",
                    true);
                return;
            }

            string nombreCliente = pedido.cliente != null
                ? (pedido.cliente.nombre + " " +
                    pedido.cliente.primerapellido + " " +
                    pedido.cliente.segundoapellido)
                : "(Sin cliente)";

            hfldPedidoRecogido.Value = pedido.idPedido.ToString();
            litRecogidoPedido.Text = $"#{pedido.idPedido}";
            litRecogidoCliente.Text = nombreCliente;
            litRecogidoFechaProgramada.Text = pedido.fechaListaParaRecojoSpecified
                ? pedido.fechaListaParaRecojo.ToString("dd/MM/yyyy")
                : "-";

            DateTime hoy = DateTime.Today;
            DateTime? fechaLista = pedido.fechaListaParaRecojoSpecified ? (DateTime?)pedido.fechaListaParaRecojo : null;
            DateTime fechaDefault = fechaLista.HasValue && fechaLista.Value.Date > hoy
                ? fechaLista.Value.Date
                : hoy;

            txtFechaRecojoReal.Text = fechaDefault.ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this, GetType(),
                "showModalRecogido",
                "var m = new bootstrap.Modal(document.getElementById('modalMarcarRecogido')); m.show();",
                true);
        }

        protected void btnConfirmarRecogido_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfldPedidoRecogido.Value, out int idPedido))
                return;

            var pedido = pedidoBO.ObtenerPorId(idPedido);
            if (pedido == null) return;

            if (pedido.estadoPedido != estadoPedido.LISTO_PARA_RECOGER)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgEstadoIncorrecto2",
                    "alert('Solo se pueden marcar como RECOGIDOS los pedidos que están LISTO_PARA_RECOGER.');",
                    true);
                return;
            }

            if (!DateTime.TryParse(txtFechaRecojoReal.Text, out DateTime fechaRecojo))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaInvalida",
                    "alert('Ingrese una fecha de recojo válida.');" +
                    "var m = new bootstrap.Modal(document.getElementById('modalMarcarRecogido')); m.show();",
                    true);
                return;
            }

            DateTime hoy = DateTime.Today;
            if (fechaRecojo.Date > hoy)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaFutura",
                    "alert('La fecha de recojo no puede ser futura.');" +
                    "var m = new bootstrap.Modal(document.getElementById('modalMarcarRecogido')); m.show();",
                    true);
                return;
            }

            if (pedido.fechaListaParaRecojoSpecified &&
                fechaRecojo.Date < pedido.fechaListaParaRecojo.Date)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msgFechaMenorLista",
                    "alert('La fecha de recojo debe ser mayor o igual a la fecha lista para recoger.');" +
                    "var m = new bootstrap.Modal(document.getElementById('modalMarcarRecogido')); m.show();",
                    true);
                return;
            }

            pedido.fechaRecojo = fechaRecojo;
            pedido.fechaRecojoSpecified = true;
            pedido.estadoPedido = estadoPedido.RECOGIDO;
            pedidoBO.AceptarRecojo(pedido);

            litMensajeAccion.Text = "Pedido marcado como <strong>RECOGIDO</strong>.";
            ScriptManager.RegisterStartupScript(this, GetType(),
                "msgOkRecogido",
                "var infoModal = new bootstrap.Modal(document.getElementById('modalMensajeAccion')); infoModal.show();",
                true);

            // Después de actualizar un pedido, recargamos desde el servicio
            RefrescarPedidosDesdeServicio();
        }

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

            IList<detallePedidoDTO> detalles = pedido.detallesPedido;

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
