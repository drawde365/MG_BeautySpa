using SoftInvBusiness; // Para UsuarioBO
using SoftInvBusiness.SoftInvWSUsuario; // Para usuarioDTO
using System;
using System.Web.UI;

// ADAPTADO: Namespace de Empleado
namespace MGBeautySpaWebAplication.Empleado.Perfil
{
    public partial class CambiarPassword : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // No necesitamos lógica en la carga inicial.
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string antigua = txtAntigua.Text.Trim();
            string nueva = txtNueva.Text.Trim();
            string verificar = txtVerificar.Text.Trim();

            usuarioDTO usuario = Session["UsuarioActual"] as usuarioDTO;
            if (usuario == null)
            {
                // ADAPTADO: Redirigir al login (general o de empleado)
                Response.Redirect(ResolveUrl("~/Login.aspx"));
                return;
            }

            // 1. Validar contraseña actual (usando la de la sesión)
            if (antigua != usuario.contrasenha)
            {
                lblInfo.Text = "⚠️ La contraseña actual no es correcta.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 2. Validar que las nuevas contraseñas coincidan
            if (nueva != verificar)
            {
                lblInfo.Text = "⚠️ Las nuevas contraseñas no coinciden.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 3. Validar que la contraseña no esté vacía
            if (string.IsNullOrEmpty(nueva))
            {
                lblInfo.Text = "⚠️ La nueva contraseña no puede estar vacía.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                // 4. Actualizar el DTO y llamar al BO
                usuario.contrasenha = nueva; // Actualiza la contraseña en el objeto

                // ADAPTADO: Usar UsuarioBO
                UsuarioBO usuarioBO = new UsuarioBO();
                // Asumimos que ModificarDatos actualiza todos los campos, incluida la contraseña
                usuarioBO.actualizarContraseña(usuario.idUsuario, usuario.contrasenha);

                // 5. Actualizar la sesión
                Session["UsuarioActual"] = usuario;

                // 6. Redirigir al perfil de Empleado
                Response.Redirect("~/Empleado/Perfil/PerfilUsuario.aspx");
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al actualizar la contraseña: " + ex.Message;
                lblInfo.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // ADAPTADO: Redirige de vuelta al perfil del empleado
            Response.Redirect("~/Empleado/Perfil/PerfilUsuario.aspx");
        }
    }
}