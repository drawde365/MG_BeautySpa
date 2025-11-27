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
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];

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
                    Session["ResetUserId"] = infoT.usuarioId;
                    Session["ResetToken"] = infoT;
                }
            }
        }

        protected void btnModificaContraseña_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

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

                token.fecha_expiracionSpecified = true;
                token.usadoSpecified = true;
                token.usado = 1;
                token.usuarioIdSpecified = true;
                token.tokenIdSpecified = true;

                usuarioBO.tokenUsado(token);

                if (usuarioBO.actualizarContraseña(userId, nuevaPass) == 1)
                {
                    Session.Remove("ResetUserId");
                    Session.Remove("ResetToken");

                    MostrarModalBootstrap("modalExito");
                }
                else
                {
                    MostrarModalBootstrap("modalError");
                }
            }
            catch
            {
                MostrarModalBootstrap("modalError");
            }
        }

        private void MostrarModalBootstrap(string modalId)
        {
            string script = $@"
                function openModal() {{
                    var modalElement = document.getElementById('{modalId}');
                    if (modalElement && typeof bootstrap !== 'undefined') {{
                        var myModal = new bootstrap.Modal(modalElement);
                        myModal.show();
                    }}
                }}
        
                if (document.readyState === 'complete' || document.readyState === 'interactive') {{
                    openModal();
                }} else {{
                    window.addEventListener('DOMContentLoaded', openModal);
                }}
            ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModalScript", script, true);
        }
    }
}