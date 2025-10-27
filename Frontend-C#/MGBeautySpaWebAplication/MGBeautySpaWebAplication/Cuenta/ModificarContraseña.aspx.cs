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
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnModificaContraseña_Click(object sender, EventArgs e)
        {
            // 1. Aquí va tu lógica para guardar en la base de datos...
            

            bool registroExitoso = true; // Asumimos que la modificación es correcta

            if (registroExitoso)
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