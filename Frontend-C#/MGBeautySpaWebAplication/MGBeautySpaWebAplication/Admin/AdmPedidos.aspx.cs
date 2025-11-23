using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmPedidos : Page
    {
        // ---------------------------------------------
        // LOAD
        // ---------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RepositorioSimulado.InicializarDatosSiEsNecesario();
                CargarCombosFiltros();
                CargarPedidos();
            }
        }

        // ---------------------------------------------
        // FILTROS (combos)
        // ---------------------------------------------
        private void CargarCombosFiltros()
        {
            // Filtro Fecha lista para recoger
            ddlFiltroFechaLista.Items.Clear();
            ddlFiltroFechaLista.Items.Add(new ListItem("Todos", "T"));
            ddlFiltroFechaLista.Items.Add(new ListItem("Con fecha", "C"));
            ddlFiltroFechaLista.Items.Add(new ListItem("Sin fecha", "S"));

            // Filtro estado de recojo
            ddlFiltroRecojo.Items.Clear();
            ddlFiltroRecojo.Items.Add(new ListItem("Todos", "T"));
            ddlFiltroRecojo.Items.Add(new ListItem("Recogidos", "R"));
            ddlFiltroRecojo.Items.Add(new ListItem("No recogidos", "N"));
        }

        protected void Filtros_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        // ---------------------------------------------
        // CARGA LISTA DE PEDIDOS
        // ---------------------------------------------
        private void CargarPedidos()
        {
            var pedidos = RepositorioSimulado
                .ListarPedidos()
                .Where(p => p.Estado == "CONFIRMADO" || p.Estado == "ENTREGADO" || p.Estado == "NoRecogido")
                .ToList();

            // Aplicar filtro 1: Con / Sin fecha de recoger
            switch (ddlFiltroFechaLista.SelectedValue)
            {
                case "C": // con fecha
                    pedidos = pedidos.Where(p => p.FechaListaParaRecoger != null).ToList();
                    break;

                case "S": // sin fecha
                    pedidos = pedidos.Where(p => p.FechaListaParaRecoger == null).ToList();
                    break;
            }

            // Aplicar filtro 2: Recogidos / No recogidos
            switch (ddlFiltroRecojo.SelectedValue)
            {
                case "R": // recogidos
                    pedidos = pedidos.Where(p => p.FechaRecojo != null || p.Estado == "ENTREGADO").ToList();
                    break;

                case "N": // no recogidos
                    pedidos = pedidos.Where(p => p.FechaRecojo == null && p.Estado != "ENTREGADO").ToList();
                    break;
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

        // ---------------------------------------------
        // CLASE CSS PARA ESTADO
        // ---------------------------------------------
        protected string ObtenerClaseEstado(object estadoObj)
        {
            string estado = (estadoObj ?? "").ToString();
            switch (estado)
            {
                case "CONFIRMADO": return "badge-confirmado";
                case "ENTREGADO": return "badge-entregado";
                case "NoRecogido": return "badge-norecogido";
                default: return "badge-confirmado";
            }
        }

        // ---------------------------------------------
        // ACCIONES DEL REPEATER
        // ---------------------------------------------
        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idPedido = Convert.ToInt32(e.CommandArgument);
            var pedido = RepositorioSimulado.ObtenerPedidoPorId(idPedido);

            switch (e.CommandName)
            {
                case "DefinirFecha":
                    if (pedido.FechaListaParaRecoger != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('Este pedido ya tiene asignada una fecha para recoger.');", true);
                        return;
                    }
                    PrepararModalDefinirFecha(idPedido);
                    break;

                case "MarcarRecogido":
                    if (pedido.FechaListaParaRecoger == null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "msg", "alert('Debe asignar una fecha para recoger antes de marcar como recogido.');", true);
                        return;
                    }
                    ProcesarMarcarComoRecogido(idPedido);
                    break;

                case "CancelarPedido":
                    ProcesarCancelarPedido(idPedido);
                    break;
            }
        }

        // ---------------------------------------------
        // DEFINIR FECHA DE RECOJO
        // ---------------------------------------------
        private void PrepararModalDefinirFecha(int idPedido)
        {
            hfPedidoSeleccionado.Value = idPedido.ToString();
            var pedido = RepositorioSimulado.ObtenerPedidoPorId(idPedido);

            lblInfoPedido.Text = $"Pedido #{pedido.PedidoId} — {pedido.NombreCliente}";
            MostrarModalFechaRecojo();
        }

        private void MostrarModalFechaRecojo()
        {
            string script = "var m = new bootstrap.Modal(document.getElementById('modalFechaRecojo')); m.show();";
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalFecha", script, true);
        }

        protected void btnConfirmarFecha_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtFechaRecojo.Text, out DateTime fecha))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('Seleccione una fecha válida.');", true);
                MostrarModalFechaRecojo();
                return;
            }

            // validar > hoy
            if (fecha <= DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", "alert('La fecha debe ser mayor al día de hoy.');", true);
                MostrarModalFechaRecojo();
                return;
            }

            int idPedido = int.Parse(hfPedidoSeleccionado.Value);

            // Validar stock antes de definir
            var problemas = RepositorioSimulado.ValidarStockParaDefinirFecha(idPedido);

            if (problemas.Any())
            {
                string mensaje = "No se puede definir la fecha. Falta stock:\n" +
                                 string.Join("\n", problemas);

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg", $"alert('{mensaje.Replace("'", "")}');", true);

                MostrarModalFechaRecojo();
                return;
            }

            // Aumenta stockDespacho y guarda fecha
            RepositorioSimulado.DefinirFechaDeRecojo(idPedido, fecha);

            CargarPedidos();
        }

        // ---------------------------------------------
        // MARCAR COMO RECOGIDO
        // ---------------------------------------------
        private void ProcesarMarcarComoRecogido(int idPedido)
        {
            RepositorioSimulado.MarcarPedidoComoRecogido(idPedido);
            CargarPedidos();
        }

        // ---------------------------------------------
        // CANCELAR PEDIDO
        // ---------------------------------------------
        private void ProcesarCancelarPedido(int idPedido)
        {
            RepositorioSimulado.CancelarPedidoNoRecogido(idPedido);
            CargarPedidos();
        }
    }


    // ===========================================================
    //  BASE DE DATOS SIMULADA (MISMO CÓDIGO DE ANTES)
    // ===========================================================
    #region BD SIMULADA

    public class PedidoSimulado
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaListaParaRecoger { get; set; }
        public DateTime? FechaRecojo { get; set; }
        public string CodTr { get; set; }
    }

    public class DetallePedidoSimulado
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int TipoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class ProductoTipoStockSimulado
    {
        public int ProductoId { get; set; }
        public int TipoId { get; set; }
        public string NombreProducto { get; set; }
        public int StockFisico { get; set; }
        public int StockDespacho { get; set; }
    }

    public static class RepositorioSimulado
    {
        private static bool _init;
        private static List<PedidoSimulado> _pedidos;
        private static List<DetallePedidoSimulado> _detalles;
        private static List<ProductoTipoStockSimulado> _stocks;

        public static void InicializarDatosSiEsNecesario()
        {
            if (_init) return;
            _init = true;

            // Stocks
            _stocks = new List<ProductoTipoStockSimulado>
            {
                new ProductoTipoStockSimulado { ProductoId = 1, TipoId = 1, NombreProducto = "Crema Vainilla", StockFisico = 50 },
                new ProductoTipoStockSimulado { ProductoId = 2, TipoId = 1, NombreProducto = "Exfoliante Café", StockFisico = 30 },
                new ProductoTipoStockSimulado { ProductoId = 3, TipoId = 1, NombreProducto = "Aceite Almendras", StockFisico = 20 }
            };

            // Pedidos
            _pedidos = new List<PedidoSimulado>
            {
                new PedidoSimulado { PedidoId = 1, ClienteId = 10, NombreCliente = "Mirelly García",
                    Total = 145, Estado = "CONFIRMADO", CodTr = "PED-0001", FechaPago = DateTime.Today.AddDays(-1) },

                new PedidoSimulado { PedidoId = 2, ClienteId = 11, NombreCliente = "Carla Pérez",
                    Total = 60, Estado = "ENTREGADO", CodTr = "PED-0002",
                    FechaPago = DateTime.Today.AddDays(-3),
                    FechaListaParaRecoger = DateTime.Today.AddDays(-2),
                    FechaRecojo = DateTime.Today.AddDays(-2) }
            };

            // Detalles
            _detalles = new List<DetallePedidoSimulado>
            {
                new DetallePedidoSimulado { PedidoId = 1, ProductoId = 1, TipoId = 1, Cantidad = 2 },
                new DetallePedidoSimulado { PedidoId = 1, ProductoId = 2, TipoId = 1, Cantidad = 1 },
                new DetallePedidoSimulado { PedidoId = 2, ProductoId = 1, TipoId = 1, Cantidad = 1 },
                new DetallePedidoSimulado { PedidoId = 2, ProductoId = 3, TipoId = 1, Cantidad = 1 }
            };
        }

        public static List<PedidoSimulado> ListarPedidos() => _pedidos;

        public static PedidoSimulado ObtenerPedidoPorId(int id)
            => _pedidos.FirstOrDefault(p => p.PedidoId == id);

        private static List<DetallePedidoSimulado> Detalles(int idPedido)
            => _detalles.Where(d => d.PedidoId == idPedido).ToList();

        private static ProductoTipoStockSimulado Stock(int prod, int tipo)
            => _stocks.First(s => s.ProductoId == prod && s.TipoId == tipo);

        public static List<string> ValidarStockParaDefinirFecha(int idPedido)
        {
            var errores = new List<string>();

            foreach (var d in Detalles(idPedido))
            {
                var st = Stock(d.ProductoId, d.TipoId);

                if (st.StockFisico < st.StockDespacho + d.Cantidad)
                    errores.Add(st.NombreProducto);
            }

            return errores;
        }

        public static void DefinirFechaDeRecojo(int idPedido, DateTime fecha)
        {
            var p = ObtenerPedidoPorId(idPedido);
            p.FechaListaParaRecoger = fecha;

            foreach (var d in Detalles(idPedido))
                Stock(d.ProductoId, d.TipoId).StockDespacho += d.Cantidad;
        }

        public static void MarcarPedidoComoRecogido(int idPedido)
        {
            var p = ObtenerPedidoPorId(idPedido);
            p.Estado = "ENTREGADO";
            p.FechaRecojo = DateTime.Now;

            foreach (var d in Detalles(idPedido))
            {
                var st = Stock(d.ProductoId, d.TipoId);
                st.StockFisico -= d.Cantidad;
                st.StockDespacho -= d.Cantidad;
            }
        }

        public static void CancelarPedidoNoRecogido(int idPedido)
        {
            var p = ObtenerPedidoPorId(idPedido);
            p.Estado = "NoRecogido";

            foreach (var d in Detalles(idPedido))
                Stock(d.ProductoId, d.TipoId).StockDespacho -= d.Cantidad;
        }
    }

    #endregion
}
