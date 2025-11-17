using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO; // Necesario para Path

// CORRECCIÓN: Namespace cambiado a 'Empleado'
namespace MGBeautySpaWebAplication.Empleado
{
    public partial class PerfilMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar datos del usuario (de la sesión)
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario != null)
                {
                    litSidebarUserName.Text = usuario.nombre;
                    // Descomenta si tienes la URL de la foto de perfil
                    // imgProfileSidebar.ImageUrl = ResolveUrl(usuario.urlFotoPerfil ?? "~/Content/default_profile.png");
                }
                else
                {
                    // Si no hay sesión, redirigir al login de empleado
                    Response.Redirect(ResolveUrl("~/Empleado/LoginEmpleado.aspx")); // Ajusta esta URL si es diferente
                }

                // Llamar al método para activar el enlace correcto
                SetActiveNavigation();
            }
        }

        /// <summary>
        /// Añade la clase 'active' al enlace de navegación que corresponde a la página actual.
        /// </summary>
        private void SetActiveNavigation()
        {
            // Obtiene el nombre del archivo de la página actual (ej: "PerfilUsuario.aspx")
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);

            // ADAPTACIÓN: Resetea solo las clases de los enlaces que existen en este master
            navPerfil.Attributes["class"] = "nav-item-link";
            navPassword.Attributes["class"] = "nav-item-link";

            // ADAPTACIÓN: Añade la clase 'active' solo a los casos que existen
            switch (currentPage.ToLower())
            {
                case "perfilusuario.aspx":
                    navPerfil.Attributes["class"] = "nav-item-link active";
                    break;
                case "cambiarpassword.aspx":
                    navPassword.Attributes["class"] = "nav-item-link active";
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect(ResolveUrl("~/Login.aspx")); // Redirige al login general
        }
    }
}