using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado.Perfil
{
    public partial class Perfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session and redirect to the login page
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        protected void btnEditarPerfil(object sender, EventArgs e)
        {
            // Redirect to the Edit Profile page

            //CAMBIAR!!!! NO HAY PANTALLA DE EDITAR PERFIL
            Response.Redirect("~/Cuenta/Registro.aspx");
        }
    }
}