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
                            Response.Redirect("~/Admin/InicioAdmin.aspx");
                            break;
                        case "Empleado":
                            Response.Redirect("~/Empleado/MisCitas.aspx");
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

    // Clase modelo simple para prueba (luego la reemplazas por tu DAO real)
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
    }

    // Simulación de acceso a datos
    public static class UsuarioDAO
    {
        public static UsuarioDTO ValidarLogin(string correo, string contrasena)
        {
            // ⚠️ Ejemplo de prueba — luego conectarás con tu backend o base de datos
            if (correo == "admin@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 1, Nombre = "Mirelly Garcia", Rol = "Administrador" };

            if (correo == "empleado@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 2, Nombre = "Miguel Guanira", Rol = "Empleado" };

            if (correo == "cliente@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 3, Nombre = "Ronny Cueva", Rol = "Cliente" };

            return null;
        }
    }
}
