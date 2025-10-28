using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class CambiarPassword : Page
    {
        // Simulamos que la contraseña actual es "123"
        private const string ContraseñaActual = "123";

        protected void Page_Load(object sender, EventArgs e)
        {
            // No necesitamos lógica en la carga inicial.
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string antigua = txtAntigua.Text.Trim();
            string nueva = txtNueva.Text.Trim();
            string verificar = txtVerificar.Text.Trim();

            // 1️⃣ Validar contraseña actual
            if (antigua != ContraseñaActual)
            {
                lblInfo.Text = "⚠️ La contraseña actual no es correcta.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 2️⃣ Validar que las nuevas contraseñas coincidan
            if (nueva != verificar)
            {
                lblInfo.Text = "⚠️ Las nuevas contraseñas no coinciden.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 3️⃣ Simular guardado de la nueva contraseña
            // (En tu proyecto real aquí iría la lógica para actualizar la contraseña en base de datos)
            // GuardarContraseñaUsuario(usuarioId, nueva);

            // 4️⃣ Mostrar mensaje y redirigir
            Session["NuevaContraseña"] = nueva; // Opcional: ejemplo
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Redirige de vuelta al perfil del usuario
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}