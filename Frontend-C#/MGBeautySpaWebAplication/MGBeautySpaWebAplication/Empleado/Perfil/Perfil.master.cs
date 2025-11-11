using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class PerfilMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // 1. Limpiar la sesión
            Session.Clear();
            Session.Abandon();

            // 2. Cerrar la autenticación (si estás usando Forms Authentication)
            FormsAuthentication.SignOut();

            // 3. Redirigir al usuario a la página de Login o Inicio
            // Asegúrate de usar la ruta correcta a tu página de Login o de Inicio
            Response.Redirect(ResolveUrl("~/Login.aspx"));
        }
    }
}