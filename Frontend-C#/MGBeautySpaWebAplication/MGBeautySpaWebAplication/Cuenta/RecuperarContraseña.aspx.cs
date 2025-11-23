using SoftInvBusiness;
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
            // ESTA LÍNEA ES CRUCIAL: Habilita la validación dinámica antigua (JavaScript puro)
            // que hace que los mensajes desaparezcan al escribir.
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            // 1. Validar que la página sea válida según los Validators del ASPX
            if (!Page.IsValid) return;

            // 2. Limpiar mensajes anteriores
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

                // Crear token y guardar
                string token = Guid.NewGuid().ToString();
                usuarioBO.GuardarTokenRecuperacion(usuario.idUsuario, token);

                // Construir URL
                string url = Request.Url.GetLeftPart(UriPartial.Authority) +
                             "/Cuenta/ModificarContraseña.aspx?token=" + token;

                // Configurar correo
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

                // Éxito
                lblMensaje.Text = "Se ha enviado un enlace a tu correo.";
                lblMensaje.CssClass = "text-success small fw-bold";

                // Opcional: Limpiar el campo para que no envíen dos veces
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