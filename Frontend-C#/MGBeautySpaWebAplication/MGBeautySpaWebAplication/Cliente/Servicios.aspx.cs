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
        // 1. DTO SIMULADO para servicios
        /*public class ServicioDTO 
        { 
            public int Id { get; set; } 
            public string Nombre { get; set; } 
            public string Categoria { get; set; } 
            public string DescripcionCorta { get; set; } 
            public string ImagenUrl { get; set; } 
        }
        */

        // 2. CATÁLOGO SIMULADO (MOCK DATA)
        /*
        private IList<ServicioDTO> GetFullServiceCatalog()
        {
            return new List<ServicioDTO>
            {
                new ServicioDTO { Id = 101, Nombre = "Limpieza Facial Profunda", Categoria = "facial", DescripcionCorta = "Elimina impurezas y revitaliza tu piel.", ImagenUrl = ResolveUrl("~/Content/images/s1.jpg") },
                new ServicioDTO { Id = 102, Nombre = "Tratamiento Anti-Edad", Categoria = "facial", DescripcionCorta = "Reduce líneas de expresión y mejora la elasticidad.", ImagenUrl = ResolveUrl("~/Content/images/s2.jpg") },
                
                new ServicioDTO { Id = 201, Nombre = "Masaje Relajante", Categoria = "corporal", DescripcionCorta = "Alivia la tensión muscular y promueve el descanso.", ImagenUrl = ResolveUrl("~/Content/images/s3.jpg") },
                
                new ServicioDTO { Id = 301, Nombre = "Aromaterapia", Categoria = "terapias", DescripcionCorta = "Terapia con aceites esenciales.", ImagenUrl = ResolveUrl("~/Content/images/s4.jpg") },
            };
        }
        */
        ServicioBO servicioBO;
        public Servicios()
        {
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establece la categoría inicial por defecto
                ViewState["CategoriaActiva"] = "facial";
                LoadServicesAndStyleTabs((string)ViewState["CategoriaActiva"]);
            }
        }
        
        // 3. Manejador de Clic Único
        protected void FilterServices_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string selectedCategory = clickedButton.CommandArgument;
            
            ViewState["CategoriaActiva"] = selectedCategory;
            
            LoadServicesAndStyleTabs(selectedCategory);
        }

        private void LoadServicesAndStyleTabs(string activeCategory)
        {
            // Resetear estilos de todos los botones (debes declararlos)
            btnFaciales.CssClass = "tab-button";
            btnCorporales.CssClass = "tab-button";
            btnTerapias.CssClass = "tab-button"; // ¡Asegúrate de declarar este ID en el diseñador!
            
            IList<servicioDTO> serviciosFiltrados;

            // 4. Lógica de filtrado y activación de estilos
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
            else // 'facial' por defecto
            {
                btnFaciales.CssClass += " active";
                serviciosFiltrados = servicioBO.ListarFiltro("Facial");
            }

            // Enlazar datos al Repeater (asumo que se llama rpServicios en tu ASPX)
            rpServicios.DataSource = serviciosFiltrados;
            rpServicios.DataBind();
        }
    }
}