using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SoftInvBusiness.SoftInvWSProductos;
using System.Web.UI;
using SoftInvBusiness;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Productos : System.Web.UI.Page
    {
        ProductoBO productoBO;
        public Productos()
        {
            productoBO = new ProductoBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CategoriaActiva"] = "facial";
                LoadProductsAndStyleTabs((string)ViewState["CategoriaActiva"]);
            }
        }

        protected void FilterProducts_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string selectedCategory = clickedButton.CommandArgument;

            ViewState["CategoriaActiva"] = selectedCategory;

            LoadProductsAndStyleTabs(selectedCategory);
        }

        private void LoadProductsAndStyleTabs(string activeCategory)
        {
            btnFaciales.CssClass = "tab-button";
            btnCorporales.CssClass = "tab-button";


            IList<productoDTO> listaProductos;
            if (activeCategory == "corporal")
            {
                btnCorporales.CssClass += " active";
                listaProductos = productoBO.filtroCorporal();
            }
            else
            {
                btnFaciales.CssClass += " active";
                listaProductos = productoBO.filtroFacial();
            }


            rpProductos.DataSource = listaProductos;
            rpProductos.DataBind();
        }
    }
}