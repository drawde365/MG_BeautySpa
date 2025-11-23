using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness; // Para PedidoBO
using SoftInvBusiness.SoftInvWSPedido; // Para pedidoDTO
using SoftInvBusiness.SoftInvWSUsuario; // Para usuarioDTO

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Pedidos : Page
    {
        private PedidoBO pedidoBO;

        // Propiedad para mantener el límite en ViewState y que persista entre postbacks
        private int LimitePedidos
        {
            get { return (int)(ViewState["LimitePedidos"] ?? 3); }
            set { ViewState["LimitePedidos"] = value; }
        }

        // Propiedad para almacenar la lista completa en Sesión
        private IList<pedidoDTO> ListaCompletaPedidos
        {
            get { return (IList<pedidoDTO>)Session["ListaPedidosCliente"]; }
            set { Session["ListaPedidosCliente"] = value; }
        }

        public Pedidos()
        {
            pedidoBO = new PedidoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos el límite al cargar la página por primera vez
                LimitePedidos = 3;
                CargarPedidos();
            }
        }

        private void CargarPedidos()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }
            ListaCompletaPedidos = pedidoBO.ListarPorCliente(usuario.idUsuario);

            // --- 1. Obtener datos (de la BD o de la Sesión) ---
            if (ListaCompletaPedidos == null)
            {
                // Si no está en caché (Sesión), la pedimos al Web Service
                if (ListaCompletaPedidos == null)
                {
                    var pedidos = pedidoBO.ListarPorCliente(usuario.idUsuario);
                    // Si el WS devuelve null, inicializamos una lista vacía para evitar errores
                    ListaCompletaPedidos = (pedidos != null) ? pedidos.ToList() : new List<pedidoDTO>();
                }
            }

            var listaCompleta = ListaCompletaPedidos;

            if (listaCompleta == null || !listaCompleta.Any())
            {
                // No hay pedidos: Oculta todo y muestra el mensaje
                rptPedidos.Visible = false;
                btnVerMas.Visible = false;
                pnlNoPedidos.Visible = true;
            } else
            {
                rptPedidos.Visible = true;
                pnlNoPedidos.Visible = false;
                var listaMapeada = listaCompleta.Select(p => new
                {
                    NumeroPedido = p.idPedido,
                    // Asumiendo que 'fechaPago' es un DateTime y no el 'date' wrapper
                    FechaCompra = p.fechaPagoSpecified ? p.fechaPago.ToString("dd/MM/yyyy") : "Pendiente",
                    Subtotal = p.total.ToString("C", new CultureInfo("es-PE")) // Formato de moneda
                });

                // Aplicamos el límite
                var listaLimitada = listaMapeada.Take(LimitePedidos).ToList();

                // --- 3. Enlazar Datos y mostrar/ocultar botón ---
                rptPedidos.DataSource = listaLimitada;
                rptPedidos.DataBind();

                // Si el límite es menor que el total, muestra el botón "Ver más"
                btnVerMas.Visible = (LimitePedidos < listaCompleta.Count);
            }
                
        }

        protected void btnDetalles_Command(object sender, CommandEventArgs e)
        {
            string numeroPedido = e.CommandArgument.ToString();
            Response.Redirect($"~/Cliente/Perfil/DetallePedido.aspx?pedido={numeroPedido}");
        }

        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            // Aumenta el número de pedidos visibles
            LimitePedidos += 3;
            // Vuelve a cargar, esta vez leerá de la Sesión y aplicará el nuevo límite
            CargarPedidos();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}