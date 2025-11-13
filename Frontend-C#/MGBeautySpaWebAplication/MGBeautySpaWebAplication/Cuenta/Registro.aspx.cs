using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCliente;

namespace MGBeautySpaWebAplication.Cuenta
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private ClienteBO clienteBO;

        public WebForm1()
        {
            clienteBO = new ClienteBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            // 1. Aquí va tu lógica para guardar en la base de datos...
            // string nombre = txtNombre.Text;
            // string email = txtEmail.Text;
            // ...
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
                urlFotoPerfil = "Hola.jpg"
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