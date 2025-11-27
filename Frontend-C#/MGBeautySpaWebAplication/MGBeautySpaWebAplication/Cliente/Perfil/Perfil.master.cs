using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class PerfilMaster : System.Web.UI.MasterPage
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario != null)
                {
                    litSidebarUserName.Text = usuario.nombre;
                    imgProfileSidebar.Src = usuario.urlFotoPerfil;
                }

                SetActiveNavigation();
            }
        }

        private void SetActiveNavigation()
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);

            navPerfil.Attributes["class"] = "nav-item-link";
            navPassword.Attributes["class"] = "nav-item-link";
            navPedidos.Attributes["class"] = "nav-item-link";
            navReservas.Attributes["class"] = "nav-item-link";

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