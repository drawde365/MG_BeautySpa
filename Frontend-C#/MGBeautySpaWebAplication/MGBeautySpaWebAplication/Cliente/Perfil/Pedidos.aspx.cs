using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSUsuario;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Pedidos : Page
    {
        private PedidoBO pedidoBO;

        private int LimitePedidos
        {
            get { return (int)(ViewState["LimitePedidos"] ?? 3); }
            set { ViewState["LimitePedidos"] = value; }
        }

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

            if (ListaCompletaPedidos == null)
            {
                if (ListaCompletaPedidos == null)
                {
                    var pedidos = pedidoBO.ListarPorCliente(usuario.idUsuario);
                    ListaCompletaPedidos = (pedidos != null) ? pedidos.ToList() : new List<pedidoDTO>();
                }
            }

            var listaCompleta = ListaCompletaPedidos;

            if (listaCompleta == null || !listaCompleta.Any())
            {
                rptPedidos.Visible = false;
                btnVerMas.Visible = false;
                pnlNoPedidos.Visible = true;
            }
            else
            {
                rptPedidos.Visible = true;
                pnlNoPedidos.Visible = false;
                var listaMapeada = listaCompleta.Select(p => new
                {
                    NumeroPedido = p.idPedido,
                    FechaCompra = p.fechaPagoSpecified ? p.fechaPago.ToString("dd/MM/yyyy") : "Pendiente",
                    Subtotal = p.total.ToString("C", new CultureInfo("es-PE"))
                });

                var listaLimitada = listaMapeada.Take(LimitePedidos).ToList();

                rptPedidos.DataSource = listaLimitada;
                rptPedidos.DataBind();

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
            LimitePedidos += 3;
            CargarPedidos();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}