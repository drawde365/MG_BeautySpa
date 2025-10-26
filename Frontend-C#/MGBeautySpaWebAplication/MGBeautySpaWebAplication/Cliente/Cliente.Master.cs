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
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        protected void btnDoSearch_Click(object sender, EventArgs e)
        {
            var q = (txtSearchModal.Text ?? "").Trim();
            var url = "~/Cliente/Resultados.aspx" + (string.IsNullOrEmpty(q) ? "" : ("?q=" + HttpUtility.UrlEncode(q)));
            Response.Redirect(ResolveUrl(url));
        }

    }
}