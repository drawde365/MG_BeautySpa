using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCliente;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cuenta
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private ClienteBO clienteBO;
        private const string DEFAULT_IMAGE_PATH = "~/Content/images/Cliente/blank-photo.jpg";

        public WebForm1()
        {
            clienteBO = new ClienteBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {

            string urlFoto = DEFAULT_IMAGE_PATH; // ruta de la foto por defecto

            if (fileUpload.HasFile)
            {
                
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png"};
                string fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (Array.IndexOf(allowedExtensions, fileExtension) > -1)
                {
                    try
                    {
                        // Crea un nombre de archivo único
                        string fileName = Guid.NewGuid().ToString() + fileExtension;

                        // Define la ruta física donde se guardará
                        string folderPath = Server.MapPath("~/Content/images/Cliente/Perfiles/");

                        // Crea el directorio si no existe
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string filePath = Path.Combine(folderPath, fileName);

                        // Guarda el archivo en el servidor
                        fileUpload.SaveAs(filePath);

                        // Guarda la ruta virtual relativa a la aplicación
                        urlFoto = "~/Content/images/Cliente/Perfiles/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores de subida
                        // Podrías registrar el error y mostrar un mensaje
                        System.Diagnostics.Debug.WriteLine("Error al subir el archivo: " + ex.Message);
                        // Opcional: mostrar un mensaje de error específico al usuario si falla la subida.
                        // Por ahora, usaremos la imagen por defecto si la subida falla.
                        urlFoto = DEFAULT_IMAGE_PATH;
                    }
                }
                else
                {
                    // La extensión no es válida.
                    // Podemos mostrar un mensaje de error o usar la foto por defecto.
                    // Para Web Forms, una buena práctica es usar un CustomValidator para el control.
                    // En este ejemplo, simplemente usaremos la ruta por defecto.
                    // Para una mejor UX, deberías usar un CustomValidator con ServerValidate.
                }
            }

            var cliente = new clienteDTO
            {
                nombre = txtNombre.Text.Trim(),
                primerapellido = txtApellidoP.Text,
                segundoapellido = txtApellidoM.Text,
                correoElectronico = txtEmail.Text,
                celular = txtCelular.Text,
                contrasenha = txtPassword.Text,
                rol = 1,
                rolSpecified = true,
                activo = 1,
                activoSpecified = true,
                urlFotoPerfil = urlFoto
            };

            int seCreo = clienteBO.CrearCliente(cliente);
            if (seCreo>0)
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
                string script = @"document.getElementById('modalError').style.display = 'flex';";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "MostrarModalError",
                    script,
                    true
                );
            }
        }
    }
}