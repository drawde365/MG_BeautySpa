using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (string.IsNullOrEmpty(token))
                {
                    string script = @"document.getElementById('modalError').style.display = 'flex';";

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "MostrarModalError",
                        script,
                        true
                    );
                }

                var infoT = usuarioBO.recuperarToken(token);
                bool used = infoT.usado == 1 ? true : false;
                if (infoT == null || used || infoT.fecha_expiracion < DateTime.Now)
                {
                    string script = @"document.getElementById('modalError').style.display = 'flex';";

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "MostrarModalError",
                        script,
                        true
                    );
                }
                Session["ResetUserId"] = infoT.usuarioId;
                Session["ResetToken"] = infoT;
            }
        }

        protected void btnModificaContraseña_Click(object sender, EventArgs e)
        {
            // 1. Aquí va tu lógica para guardar en la base de datos...
            int userId = (int)Session["ResetUserId"];
            contrasenhaTokenDTO token = (contrasenhaTokenDTO)Session["ResetToken"];

            string nuevaPass = txtPassword.Text.Trim();

            token.fecha_expiracionSpecified = true;
            token.usadoSpecified = true;
            token.usado = 1;
            token.usuarioIdSpecified = true;
            token.tokenIdSpecified = true;
            usuarioBO.tokenUsado(token);

            if (usuarioBO.actualizarContraseña(userId, nuevaPass)==1)
            {
                // Preparamos el script de JavaScript
                string script = @"document.getElementById('modalExito').style.display = 'flex';";

                // Usamos ScriptManager para ejecutar el script en el navegador
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "MostrarModalExito",
                    script,
                    true
                    );
            }
            else
            {
                // Lógica si el registro falla (mostrar un mensaje de error, etc.)
            }
        }
    }
}