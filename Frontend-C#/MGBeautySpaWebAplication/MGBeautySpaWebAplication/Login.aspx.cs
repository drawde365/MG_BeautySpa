using SoftInvBusiness;
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
                // Limpiar sesión previa (por si el usuario viene de Cerrar Sesión)
                Session.Clear();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            UsuarioBO user = new UsuarioBO();

            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            // Validar usuario
            var usuario = user.IniciarSesion(correo, contrasena);

            if (usuario.idUsuario != 0)
            {
                Session["UsuarioActual"] = usuario;

                // Redirigir según rol
                switch (usuario.rol)
                {
                    case 3:
                        Response.Redirect("~/Admin/PanelDeControl.aspx");
                        break;
                    case 2:
                        Response.Redirect("~/Empleado/InicioEmpleado.aspx");
                        break;
                    case 1:
                        // Si venía redirigido desde otra página (por ejemplo, ReturnUrl del carrito)
                        if (Request.QueryString["ReturnUrl"] != null)
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"]);
                        }
                        else Response.Redirect("~/Cliente/InicioCliente.aspx");
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
            // Crear sesión temporal como invitado
            Session["UsuarioId"] = null;
            Session["Nombre"] = "Invitado";
            Session["Rol"] = "Invitado";

            // Redirige al catálogo o página principal del cliente
            Response.Redirect("~/Cliente/InicioCliente.aspx");
        }
    }
}
