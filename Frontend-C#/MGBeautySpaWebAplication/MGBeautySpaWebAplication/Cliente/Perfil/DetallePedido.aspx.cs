using System;
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
            // 1. Validar Usuario y Pedido ID
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            if (!int.TryParse(Request.QueryString["pedido"], out int pedidoId))
            {
                // Si no hay ID, redirigir de vuelta a la lista de pedidos
                Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
                return;
            }

            try
            {
                // 2. Obtener el Pedido Completo desde el BO
                pedidoDTO pedido = pedidoBO.ObtenerPorId(pedidoId);

                // (Opcional: Validar que el pedido pertenezca al cliente)
                if (pedido == null || pedido.cliente.idUsuario != usuario.idUsuario)
                {
                    Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
                    return;
                }

                // 3. Poblar los datos del resumen del pedido
                litNumeroPedido.Text = pedido.idPedido.ToString("D4"); // Formato "0001"
                litFecha.Text = pedido.fechaPagoSpecified ? pedido.fechaPago.ToString("dd/MM/yyyy") : "Pendiente de Pago";
                litTotal.Text = pedido.totalSpecified ? pedido.total.ToString("N2", new CultureInfo("es-PE")) : "0.00";

                // 4. Mapear los detalles del pedido para el Repeater
                if (pedido.detallesPedido != null)
                {
                    var productos = pedido.detallesPedido.Select(d => new
                    {
                        // Asumiendo que el DTO del producto está anidado
                        Imagen = ResolveUrl(d.producto.producto.urlImagen ?? "~/Content/images/placeholder.png"),
                        Nombre = d.producto.producto.nombre ?? "Producto no disponible",
                        Descripcion = d.producto.tipo.nombre, // Usamos el tipo de piel como descripción
                        Cantidad = d.cantidadSpecified ? d.cantidad : 0,
                        Subtotal = d.subtotalSpecified ? d.subtotal.ToString("N2", new CultureInfo("es-PE")) : "0.00"
                    }).ToList();

                    rptProductos.DataSource = productos;
                    rptProductos.DataBind();
                }
                else
                {
                    // Manejar caso de pedido sin detalles (aunque no debería ocurrir si está bien gestionado)
                    rptProductos.DataSource = null;
                    rptProductos.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Manejar error si el Web Service falla
                // (Podrías mostrar un mensaje de error en la página)
                Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
        }
    }
}