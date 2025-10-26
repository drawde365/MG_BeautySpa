using MGBeautySpaWebAplication.Util;
using System;
using System.Linq;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Resultados : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string q = (Request.QueryString["q"] ?? "").Trim();
            litQuery.Text = q;

            var productos = ProductoDAO.BuscarPorNombre(q); // fake DAO

            if (productos == null || !productos.Any())
            {
                pnlNoResults.Visible = true;
                litCount.Text = "0 resultados";
                return;
            }

            rptProductos.DataSource = productos;
            rptProductos.DataBind();
            litCount.Text = productos.Count + " resultado(s)";
        }
    }
}
