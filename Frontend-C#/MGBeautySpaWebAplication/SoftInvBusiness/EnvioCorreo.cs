using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class EnvioCorreo
    {
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";
        private const int protocolo = 587;
        private const string smtpProtocolo = "smtp.gmail.com";

        public EnvioCorreo() { 
        
        }

        public async Task enviarCorreo(string correoDestino,string asunto,string cuerpo, byte[] pdfBytes) {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(correoDestino);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.IsBodyHtml = false;

            if (pdfBytes != null)
            {
                mensaje.Attachments.Add(new Attachment(
                new MemoryStream(pdfBytes),
                "ComprobanteCompra.pdf",
                "application/pdf"
                ));
            }

            SmtpClient smtp = new SmtpClient(smtpProtocolo, protocolo);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mensaje);

        }


    }
}
