using MGBeautySpaWebAplication.Util;
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
            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            // Validar usuario
            var usuario = UsuarioDAO.ValidarLogin(correo, contrasena);

            if (usuario != null)
            {
                Session["UsuarioId"] = usuario.Id;
                Session["Nombre"] = usuario.Nombre;
                Session["Rol"] = usuario.Rol;

                // Si venía redirigido desde otra página (por ejemplo, ReturnUrl del carrito)
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    Response.Redirect(Request.QueryString["ReturnUrl"]);
                }
                else
                {
                    // Redirigir según rol
                    switch (usuario.Rol)
                    {
                        case "Administrador":
                            Response.Redirect("~/Admin/PanelDeControl.aspx");
                            break;
                        case "Empleado":
                            Response.Redirect("~/Empleado/InicioEmpleado.aspx");
                            break;
                        default:
                            Response.Redirect("~/Cliente/InicioCliente.aspx");
                            break;
                    }
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
