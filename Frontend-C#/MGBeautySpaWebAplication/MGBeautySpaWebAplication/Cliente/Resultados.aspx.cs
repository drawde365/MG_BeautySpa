using SoftInvBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Resultados : Page
    {
        private ProductoBO productoBO;

        public Resultados()
        {
            productoBO = new ProductoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string q = (Request.QueryString["q"] ?? "").Trim();
            litQuery.Text = q;

            Literal1.Text = q;
            Literal1.DataBind();

            // Filtrado simple: contiene (case-insensitive)
            var productos = productoBO.buscarPorNombre(q);

            if (productos == null || productos.Count == 0)
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
