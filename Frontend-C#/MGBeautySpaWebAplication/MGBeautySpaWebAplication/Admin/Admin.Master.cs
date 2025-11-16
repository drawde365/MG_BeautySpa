using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls; // <-- Asegúrate que esté
using System.IO;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        // El designer.cs ahora generará estas variables 
        // (Asegúrate de que tu designer.cs se actualice)
        protected HyperLink navPanelDeControl;
        protected HyperLink navAdmProductos;
        protected HyperLink navAdmServicios;
        protected HyperLink navReportes;
        protected HyperLink navBuscarUsuarios;
        protected HyperLink navAgregarEmpleado;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData();
                SetActiveNavigation(); // <-- Ahora esto funcionará
            }
        }

        private void LoadUserData()
        {
            string nombre = (Session["UserName"] as string) ?? "Invitado";
            string fotoUrl = (Session["UserPhotoUrl"] as string) ?? "~/Content/default_profile.png";

            litUserName.Text = nombre;
            // imgProfile.ImageUrl = ResolveUrl(fotoUrl);
        }

        private void SetActiveNavigation()
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            // Resetea todos (ahora usando .CssClass)
            navPanelDeControl.CssClass = "nav-link";
            navAdmProductos.CssClass = "nav-link";
            navAdmServicios.CssClass = "nav-link";
            navReportes.CssClass = "nav-link";
            navBuscarUsuarios.CssClass = "nav-link";
            navAgregarEmpleado.CssClass = "nav-link";

            // Aplica 'active' al link actual
            switch (currentPage)
            {
                case "paneldecontrol.aspx":
                    navPanelDeControl.CssClass = "nav-link active";
                    break;
                case "admproductos.aspx":
                case "insertarproducto.aspx":
                    navAdmProductos.CssClass = "nav-link active";
                    break;
                case "admservicios.aspx":
                case "insertarservicio.aspx":
                    navAdmServicios.CssClass = "nav-link active";
                    break;
                case "reportes.aspx":
                    navReportes.CssClass = "nav-link active";
                    break;
                case "buscarusuarios.aspx":
                    navBuscarUsuarios.CssClass = "nav-link active";
                    break;
                case "agregarempleado.aspx":
                    navAgregarEmpleado.CssClass = "nav-link active";
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }
    }
}