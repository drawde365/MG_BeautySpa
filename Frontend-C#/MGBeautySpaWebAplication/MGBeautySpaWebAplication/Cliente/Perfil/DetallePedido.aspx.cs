using System;
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
    public partial class DetallePedido : Page
    {
        private PedidoBO pedidoBO;

        public DetallePedido()
        {
            pedidoBO = new PedidoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDetalle();
            }
        }

        private void CargarDetalle()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            if (!int.TryParse(Request.QueryString["pedido"], out int pedidoId))
            {
                Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
                return;
            }

            try
            {
                pedidoDTO pedido = pedidoBO.ObtenerPorId(pedidoId);

                if (pedido == null || pedido.cliente.idUsuario != usuario.idUsuario)
                {
                    Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
                    return;
                }

                litNumeroPedido.Text = pedido.idPedido.ToString("D4");
                litFecha.Text = pedido.fechaPagoSpecified ? pedido.fechaPago.ToString("dd/MM/yyyy") : "Pendiente de Pago";
                litTotal.Text = pedido.totalSpecified ? pedido.total.ToString("N2", new CultureInfo("es-PE")) : "0.00";

                if (pedido.detallesPedido != null)
                {
                    var productos = pedido.detallesPedido.Select(d => new
                    {
                        Imagen = ResolveUrl(d.producto.producto.urlImagen ?? "~/Content/images/placeholder.png"),
                        Nombre = d.producto.producto.nombre ?? "Producto no disponible",
                        Descripcion = d.producto.tipo.nombre,
                        Cantidad = d.cantidadSpecified ? d.cantidad : 0,
                        Subtotal = d.subtotalSpecified ? d.subtotal.ToString("N2", new CultureInfo("es-PE")) : "0.00"
                    }).ToList();

                    rptProductos.DataSource = productos;
                    rptProductos.DataBind();
                }
                else
                {
                    rptProductos.DataSource = null;
                    rptProductos.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
        }
    }
}