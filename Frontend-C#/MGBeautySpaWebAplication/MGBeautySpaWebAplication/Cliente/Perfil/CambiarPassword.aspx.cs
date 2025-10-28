using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class CambiarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblInfo.Text = string.Empty;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string actual = txtAntigua.Text.Trim();
            string nueva = txtNueva.Text.Trim();
            string verificar = txtVerificar.Text.Trim();

            if (string.IsNullOrEmpty(actual) || string.IsNullOrEmpty(nueva) || string.IsNullOrEmpty(verificar))
            {
                lblInfo.CssClass = "text-danger";
                lblInfo.Text = "Por favor, completa todos los campos.";
                return;
            }

            if (nueva != verificar)
            {
                lblInfo.CssClass = "text-danger";
                lblInfo.Text = "Las contraseñas nuevas no coinciden.";
                return;
            }

            // Lógica simulada de cambio de contraseña
            bool cambioExitoso = true;

            if (cambioExitoso)
            {
                lblInfo.CssClass = "text-success";
                lblInfo.Text = "Tu contraseña ha sido actualizada correctamente.";
                txtAntigua.Text = txtNueva.Text = txtVerificar.Text = string.Empty;
            }
            else
            {
                lblInfo.CssClass = "text-danger";
                lblInfo.Text = "Ocurrió un error al cambiar la contraseña.";
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Redirige al perfil del usuario
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}