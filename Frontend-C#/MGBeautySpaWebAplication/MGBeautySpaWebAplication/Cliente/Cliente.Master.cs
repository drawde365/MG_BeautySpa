using System;
using System.Web;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Cliente : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Nombre del usuario
                var nombre = (Session["Nombre"] as string) ?? "Invitado";
                litUserName.Text = nombre;

                // Contador del carrito
                int count = 0;
                if (Session["CartCount"] != null)
                {
                    int.TryParse(Session["CartCount"].ToString(), out count);
                }
                litCartCount.Text = count.ToString();

                var fotoUrl = Session["FotoPerfilUrl"] as string;

                if (!string.IsNullOrEmpty(fotoUrl))
                {
                    // Si hay una URL de foto en Session, la usamos.
                    imgProfile.Src = fotoUrl;
                }
                else
                {
                    // Si no hay foto, usamos la foto por defecto que ya pusimos en el HTML.
                    // imgProfile.Src = "~/Content/default_profile.png"; 
                }
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
        /*
        protected void btnDoSearch_Click(object sender, EventArgs e)
        {
            var q = (txtSearchModal.Text ?? "").Trim();
            var url = "~/Cliente/Resultados.aspx" + (string.IsNullOrEmpty(q) ? "" : ("?q=" + HttpUtility.UrlEncode(q)));
            Response.Redirect(ResolveUrl(url));
        }
        */
    }
}