using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class AgregarExcepcion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrarExcepcion_Click(object sender, EventArgs e)
        {
            //AQUI MODIFICAR PARA QUE PUEDA REGISTRAR EXCEPCIONES

            Session["FlashMessage"] = "¡Excepción registrada exitosamente!";

            Response.Redirect("MiHorario.aspx");
        }
    }
}