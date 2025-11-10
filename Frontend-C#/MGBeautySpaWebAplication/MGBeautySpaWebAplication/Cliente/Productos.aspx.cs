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
    {   /*
        // 1. DTO SIMULADO (Debe coincidir con lo que esperas de Java)
        public class ProductoDTO
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string DescripcionCorta { get; set; }
            public string ImagenUrl { get; set; }
        }

        // 2. CATÁLOGO SIMULADO (MOCK DATA)
        private List<ProductoDTO> GetFullCatalog()
        {
            return new List<ProductoDTO>
            {
                new ProductoDTO { Id = 1, Nombre = "Loción Tónica", Categoria = "facial", DescripcionCorta = "Refresca y equilibra el pH.", ImagenUrl = ResolveUrl("~/Content/images/product_face_1.jpg") },
                new ProductoDTO { Id = 2, Nombre = "Mascarilla Correctiva", Categoria = "facial", DescripcionCorta = "Purifica la piel y controla la grasa.", ImagenUrl = ResolveUrl("~/Content/images/product_face_2.jpg") },
                new ProductoDTO { Id = 3, Nombre = "Crema Hidratante", Categoria = "facial", DescripcionCorta = "Nutre profundamente la piel seca.", ImagenUrl = ResolveUrl("~/Content/images/product_face_3.jpg") },

                new ProductoDTO { Id = 4, Nombre = "Exfoliante Corporal", Categoria = "corporal", DescripcionCorta = "Suaviza la piel al instante.", ImagenUrl = ResolveUrl("~/Content/images/product_body_1.jpg") },
                new ProductoDTO { Id = 5, Nombre = "Aceite de Masaje", Categoria = "corporal", DescripcionCorta = "Relajante y nutritivo para el cuerpo.", ImagenUrl = ResolveUrl("~/Content/images/product_body_2.jpg") },
            };
        }

        */

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

            //var todosProductos = GetFullCatalog();
            var listaProductos = productoBO.pagina(1);

            /*
            if (activeCategory == "corporal")
            {
                btnCorporales.CssClass += " active";
                productosFiltrados = todosProductos.Where(p => p.Categoria == "corporal").ToList();
            }
            else // 'facial' por defecto
            {
                btnFaciales.CssClass += " active";
                productosFiltrados = todosProductos.Where(p => p.Categoria == "facial").ToList();
            }
            */

            // Enlazar datos al Repeater (asumo que se llama rpProductos)
            rpProductos.DataSource = listaProductos;
            rpProductos.DataBind();
        }
    }
}