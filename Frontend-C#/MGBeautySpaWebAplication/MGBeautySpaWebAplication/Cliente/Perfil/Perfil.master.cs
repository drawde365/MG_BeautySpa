using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO; // Necesario para Path

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class PerfilMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Cargar datos del usuario (ejemplo, deberías obtenerlo de la sesión)
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario != null)
                {
                    litSidebarUserName.Text = usuario.nombre;
                    // imgProfileSidebar.ImageUrl = ResolveUrl(usuario.urlFotoPerfil ?? "~/Content/default_profile.png");
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

            // Resetea todas las clases (por si acaso)
            navPerfil.Attributes["class"] = "nav-item-link";
            navPassword.Attributes["class"] = "nav-item-link";
            navPedidos.Attributes["class"] = "nav-item-link";
            navReservas.Attributes["class"] = "nav-item-link";

            // Añade la clase 'active' a la página correspondiente
            switch (currentPage.ToLower())
            {
                case "perfilusuario.aspx":
                    navPerfil.Attributes["class"] = "nav-item-link active";
                    break;
                case "cambiarpassword.aspx":
                    navPassword.Attributes["class"] = "nav-item-link active";
                    break;
                case "pedidos.aspx":
                    navPedidos.Attributes["class"] = "nav-item-link active";
                    break;
                case "reservas.aspx":
                    navReservas.Attributes["class"] = "nav-item-link active";
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect(ResolveUrl("~/Login.aspx"));
        }
    }
}