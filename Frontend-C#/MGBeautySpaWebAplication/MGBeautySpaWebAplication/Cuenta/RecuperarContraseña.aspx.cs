using SoftInvBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string correo = txtEmail.Text.Trim();
            var usuario = usuarioBO.ObtenerUsuarioPorCorreo(correo);
            if (usuario == null)
            {
                lblMensaje.Text = "No existe una cuenta asociada a este correo.";
                lblMensaje.CssClass = "text-danger";
                return;
            }

            // Crear un token único para el enlace (por ejemplo un GUID)
            string token = Guid.NewGuid().ToString();

            // Guardarlo en BD asociado al usuario (con fecha de expiración)
            usuarioBO.GuardarTokenRecuperacion(usuario.idUsuario, token);

            // Construir URL para resetear contraseña
            string url = Request.Url.GetLeftPart(UriPartial.Authority) +
                         "/Cuenta/ModificarContraseña.aspx?token=" + token;

            // Crear correo
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(correo);
            mensaje.Subject = "Recuperación de contraseña | MG Beauty SPA";
            mensaje.Body = "¡Hola, "+usuario.nombre+"!\n\n"+"Haz clic en el siguiente enlace para restablecer tu contraseña:\n\n" + url;
            mensaje.IsBodyHtml = false;

            // 2. Configurar SMTP (Ejemplo para Gmail)
            SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;
            // 3. Enviar
            smtp.Send(mensaje);

            //Mensaje de exito
            lblMensaje.Text = "Se ha enviado un enlace a tu correo.";
            lblMensaje.CssClass = "text-success";
        }
    }
}