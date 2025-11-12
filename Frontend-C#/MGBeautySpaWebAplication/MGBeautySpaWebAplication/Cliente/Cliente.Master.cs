using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Cliente : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData();
            }
            UpdateCartDisplay();
            VerificarSesion();
        }

        private void LoadUserData()
        {
            string nombre = (Session["UserName"] as string) ?? "Invitado";
            string fotoUrl = (Session["UserPhotoUrl"] as string) ?? "~/Content/default_profile.png";

            litUserName.Text = nombre;
        }

        private void VerificarSesion()
        {
            var usuario = Session["UsuarioActual"] as usuarioDTO;

            if (usuario != null)
            {
                divPerfil.Visible = true;
                divLogin.Visible = false;

                litUserName.Text = usuario.nombre;
            }
            else
            {
                divPerfil.Visible = false;
                divLogin.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect(ResolveUrl("~/Login.aspx"));
        }

        protected void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearchProduct.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Response.Redirect(ResolveUrl($"~/Cliente/Resultados.aspx?q={HttpUtility.UrlEncode(searchTerm)}"));
            }
        }

        public void UpdateCartDisplay()
        {
            int count = (Session["CartCount"] as int?) ?? 0;

            litCartCount.Text = count.ToString();

            HtmlGenericControl badge = (HtmlGenericControl)this.FindControl("cartCountBadge");
            badge.Visible = true;
            //if (badge != null)
            //{
            //    badge.Visible = count > 0;
            //}
        }
    }
}