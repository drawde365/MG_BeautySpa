using SoftInvBusiness.SoftInvWSCliente;
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class PerfilUsuario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfil();
            }
        }

        private void CargarPerfil()
        {
            // Recuperar el usuario actual desde la sesión
            var usuario = Session["UsuarioActual"] as usuarioDTO;

            if (usuario == null)
            {
                // Si no hay sesión, redirigir o mostrar mensaje
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Asignar los datos del usuario a los controles ASP.NET Literal
            litUserNameGreeting.Text = usuario.nombre;
            litNombres.Text = usuario.nombre;
            litApellidos.Text = $"{usuario.primerapellido} {usuario.segundoapellido}";
            litEmail.Text = usuario.correoElectronico;
            litTelefono.Text = usuario.celular;
        }
    }
}