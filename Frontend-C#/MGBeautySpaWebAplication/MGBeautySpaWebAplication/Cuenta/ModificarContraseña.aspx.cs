using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cuenta
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;

        public WebForm2()
        {
            usuarioBO = new UsuarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // IMPORTANTE: Esto habilita la validación en tiempo real (mensajes rojos dinámicos)
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];

                // Si no hay token o es inválido en la BD
                if (string.IsNullOrEmpty(token))
                {
                    MostrarModalBootstrap("modalError");
                    return;
                }

                var infoT = usuarioBO.recuperarToken(token);

                if (infoT == null || infoT.usado == 1 || infoT.fecha_expiracion < DateTime.Now)
                {
                    MostrarModalBootstrap("modalError");
                }
                else
                {
                    // Token válido
                    Session["ResetUserId"] = infoT.usuarioId;
                    Session["ResetToken"] = infoT;
                }
            }
        }

        protected void btnModificaContraseña_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Verificamos que la sesión siga viva
            if (Session["ResetUserId"] == null || Session["ResetToken"] == null)
            {
                MostrarModalBootstrap("modalError");
                return;
            }

            try
            {
                int userId = (int)Session["ResetUserId"];
                contrasenhaTokenDTO token = (contrasenhaTokenDTO)Session["ResetToken"];
                string nuevaPass = txtPassword.Text.Trim();

                // Marcar token como usado
                token.fecha_expiracionSpecified = true;
                token.usadoSpecified = true;
                token.usado = 1;
                token.usuarioIdSpecified = true;
                token.tokenIdSpecified = true;

                usuarioBO.tokenUsado(token);

                // Actualizar contraseña
                if (usuarioBO.actualizarContraseña(userId, nuevaPass) == 1)
                {
                    // Limpiar sesión por seguridad
                    Session.Remove("ResetUserId");
                    Session.Remove("ResetToken");

                    // Mostrar éxito
                    MostrarModalBootstrap("modalExito");
                }
                else
                {
                    // Si falla la BD
                    // Podrías crear un 'modalFalloBD' o reutilizar error
                    MostrarModalBootstrap("modalError");
                }
            }
            catch
            {
                MostrarModalBootstrap("modalError");
            }
        }

        // Método auxiliar para abrir los modales de Bootstrap 5 correctamente
        // Método auxiliar corregido
        private void MostrarModalBootstrap(string modalId)
        {
            // Envolvemos el código en un evento 'load' para asegurarnos 
            // de que la librería de Bootstrap ya se cargó antes de ejecutarlo.
            string script = $@"
            window.addEventListener('load', function() {{
                var modalElement = document.getElementById('{modalId}');
                if (modalElement) {{
                    var myModal = new bootstrap.Modal(modalElement);
                    myModal.show();
                }}
            }});";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", script, true);
        }
    }
}