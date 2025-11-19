using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ⚠️ NO usar Session.Clear() porque borra el comentario pendiente
                // Solo limpiar usuario si existe
                Session.Remove("UsuarioActual");
                Session.Remove("Carrito");
                Session.Remove("CartCount");

                // Mantener intactos: ComentarioPendiente y ReturnUrl
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            UsuarioBO user = new UsuarioBO();

            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = user.IniciarSesion(correo, contrasena);

            if (usuario.idUsuario != 0)
            {
                Session["UsuarioActual"] = usuario;

                switch (usuario.rol)
                {
                    case 3:
                        Response.Redirect("~/Admin/PanelDeControl.aspx");
                        break;
                    case 2:
                        Response.Redirect("~/Empleado/InicioEmpleado.aspx");
                        break;
                    case 1:
                        PedidoBO pedidoBO = new PedidoBO();
                        pedidoDTO carrito = pedidoBO.ObtenerCarritoPorCliente(usuario.idUsuario);
                        if (carrito == null)
                        {
                            pedidoDTO carro = new pedidoDTO();
                            carro.cliente = new clienteDTO();
                            carro.cliente.idUsuario = usuario.idUsuario;
                            carro.cliente.idUsuarioSpecified = true;
                            carro.total = 0;
                            carro.totalSpecified = true;
                            carro.estadoPedido = estadoPedido.EnCarrito;
                            carro.estadoPedidoSpecified = true;
                            carro.detallesPedido = new detallePedidoDTO[0];
                            carro.idPedido = pedidoBO.Insertar(carro);
                            carrito = carro;
                        }
                        Session["Carrito"] = carrito;
                        int cartCount = 0;
                        if (carrito.detallesPedido != null)
                        {
                            cartCount = carrito.detallesPedido.Sum(d => d.cantidad);
                        }
                        Session["CartCount"] = cartCount;

                        // ✅ PRIORIZAR ReturnUrl de Session sobre QueryString
                        string returnUrl = Session["ReturnUrl"] as string;

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            // Limpiar ReturnUrl de sesión (pero mantener ComentarioPendiente)
                            Session.Remove("ReturnUrl");
                            Response.Redirect(returnUrl);
                        }
                        else if (Request.QueryString["ReturnUrl"] != null)
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"]);
                        }
                        else
                        {
                            Response.Redirect("~/Cliente/InicioCliente.aspx");
                        }
                        break;
                }
            }
            else
            {
                lblError.Text = "Correo o contraseña incorrectos.";
            }
        }

        protected void btnInvitado_Click(object sender, EventArgs e)
        {
            Session.Remove("ComentarioPendiente");

            string returnUrl = Session["ReturnUrl"] as string;
            Session.Remove("ReturnUrl");

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(returnUrl);
            }
            else
            {
                Response.Redirect("~/Cliente/InicioCliente.aspx");
            }
        }
    }
}