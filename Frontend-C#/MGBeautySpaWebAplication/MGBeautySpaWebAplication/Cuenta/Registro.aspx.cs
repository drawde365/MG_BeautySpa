using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCliente;
using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cuenta
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private ClienteBO clienteBO;
        private const string DEFAULT_IMAGE_PATH = "~/Content/images/blank-photo.jpg";

        public WebForm1()
        {
            clienteBO = new ClienteBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string urlFoto = DEFAULT_IMAGE_PATH;

            if (fileUpload.HasFile)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (Array.IndexOf(allowedExtensions, fileExtension) > -1)
                {
                    try
                    {
                        string fileName = Guid.NewGuid().ToString() + fileExtension;
                        string folderPath = Server.MapPath("~/Content/images/Cliente/");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string filePath = Path.Combine(folderPath, fileName);
                        fileUpload.SaveAs(filePath);
                        urlFoto = "~/Content/images/Cliente/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al subir imagen: " + ex.Message);
                        urlFoto = DEFAULT_IMAGE_PATH;
                    }
                }
            }

            var cliente = new clienteDTO
            {
                nombre = txtNombre.Text.Trim(),
                primerapellido = txtApellidoP.Text.Trim(),
                segundoapellido = txtApellidoM.Text.Trim(),
                correoElectronico = txtEmail.Text.Trim(),
                celular = txtCelular.Text.Trim(),
                contrasenha = txtPassword.Text,
                rol = 1,
                rolSpecified = true,
                activo = 1,
                activoSpecified = true,
                urlFotoPerfil = urlFoto
            };

            int seCreo = clienteBO.CrearCliente(cliente);

            if (seCreo > 0)
            {
                string script = @"
                    window.addEventListener('load', function() {
                        showSuccessModal();
                    });";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessModal", script, true);
            }
            else
            {
                string script = "showErrorModal('No se pudo crear la cuenta. Verifique si el correo ya existe.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorModal", script, true);
            }
        }
    }
}