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
                // La categoría inicial al cargar la página por primera vez
                ViewState["CategoriaActiva"] = "facial";
                LoadProductsAndStyleTabs((string)ViewState["CategoriaActiva"]);
            }
        }

        // El método para filtrar y estilizar las pestañas
        protected void FilterProducts_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string selectedCategory = clickedButton.CommandArgument;

            ViewState["CategoriaActiva"] = selectedCategory;

            LoadProductsAndStyleTabs(selectedCategory);
        }

        private void LoadProductsAndStyleTabs(string activeCategory)
        {
            // Resetear estilos de todos los botones
            btnFaciales.CssClass = "tab-button";
            btnCorporales.CssClass = "tab-button";


            IList<productoDTO> listaProductos;
            if (activeCategory == "corporal")
            {
                btnCorporales.CssClass += " active";
                listaProductos = productoBO.filtroCorporal();
            }
            else // 'facial' por defecto
            {
                btnFaciales.CssClass += " active";
                listaProductos = productoBO.filtroFacial();
            }
            

            // Enlazar datos al Repeater (asumo que se llama rpProductos)
            rpProductos.DataSource = listaProductos;
            rpProductos.DataBind();
        }
    }
}