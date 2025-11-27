using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSServicio;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Servicios : System.Web.UI.Page
    {
        ServicioBO servicioBO;
        public Servicios()
        {
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CategoriaActiva"] = "facial";
                LoadServicesAndStyleTabs((string)ViewState["CategoriaActiva"]);
            }
        }

        protected void FilterServices_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string selectedCategory = clickedButton.CommandArgument;

            ViewState["CategoriaActiva"] = selectedCategory;

            LoadServicesAndStyleTabs(selectedCategory);
        }

        private void LoadServicesAndStyleTabs(string activeCategory)
        {
            btnFaciales.CssClass = "tab-button";
            btnCorporales.CssClass = "tab-button";
            btnTerapias.CssClass = "tab-button";

            IList<servicioDTO> serviciosFiltrados;

            if (activeCategory == "corporal")
            {
                btnCorporales.CssClass += " active";
                serviciosFiltrados = servicioBO.ListarFiltro("Corporal");
            }
            else if (activeCategory == "terapias")
            {
                btnTerapias.CssClass += " active";
                serviciosFiltrados = servicioBO.ListarFiltro("Terapia Complementaria");
            }
            else
            {
                btnFaciales.CssClass += " active";
                serviciosFiltrados = servicioBO.ListarFiltro("Facial");
            }

            rpServicios.DataSource = serviciosFiltrados;
            rpServicios.DataBind();
        }
    }
}