using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cuenta
{
    public partial class RecuperarContraseña : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";

        public RecuperarContraseña()
        {
            usuarioBO = new UsuarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            lblMensaje.Text = "";
            lblMensaje.CssClass = "";

            string correo = txtEmail.Text.Trim();

            try
            {
                var usuario = usuarioBO.ObtenerUsuarioPorCorreo(correo);

                if (usuario == null)
                {
                    lblMensaje.Text = "No existe una cuenta asociada a este correo.";
                    lblMensaje.CssClass = "text-danger small fw-bold";
                    return;
                }

                string token = Guid.NewGuid().ToString();
                usuarioBO.GuardarTokenRecuperacion(usuario.idUsuario, token);

                string url = Request.Url.GetLeftPart(UriPartial.Authority) +
                             "/Cuenta/ModificarContraseña.aspx?token=" + token;

                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress(correoEmpresa);
                mensaje.To.Add(correo);
                mensaje.Subject = "Recuperación de contraseña | MG Beauty SPA";
                mensaje.Body = "¡Hola, " + usuario.nombre + "!\n\n" +
                               "Haz clic en el siguiente enlace para restablecer tu contraseña:\n\n" + url;
                mensaje.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
                smtp.EnableSsl = true;

                smtp.Send(mensaje);

                lblMensaje.Text = "Se ha enviado un enlace a tu correo.";
                lblMensaje.CssClass = "text-success small fw-bold";

                txtEmail.Text = "";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al enviar el correo: " + ex.Message;
                lblMensaje.CssClass = "text-danger small fw-bold";
            }
        }
    }
}