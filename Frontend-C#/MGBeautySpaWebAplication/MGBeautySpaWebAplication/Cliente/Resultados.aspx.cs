using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Resultados : Page
    {
        // DTO exclusivo para la página de resultados
        private class ProductoDTOResultados
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public string ImagenUrl { get; set; }
        }

        // “Catálogo” local (mock) para esta página
        private static readonly List<ProductoDTOResultados> Catalogo = new List<ProductoDTOResultados>
        {
            new ProductoDTOResultados { Id = 1, Nombre = "Crema hidratante",  Precio = 49.90m, ImagenUrl = "/Content/images/CremaHidratante.jpg" },
            new ProductoDTOResultados { Id = 2, Nombre = "Crema reafirmante", Precio = 59.90m, ImagenUrl = "/Content/images/CremaReafirmante.jpg" },
            new ProductoDTOResultados { Id = 3, Nombre = "Crema reductora",   Precio = 54.90m, ImagenUrl = "/Content/images/CremaReductora.jpg" },
            new ProductoDTOResultados { Id = 4, Nombre = "Crema termogénica", Precio = 62.00m, ImagenUrl = "/Content/images/CremaTermogenica.jpg" },
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string q = (Request.QueryString["q"] ?? "").Trim();
            litQuery.Text = q;

            // Filtrado simple: contiene (case-insensitive)
            var productos = string.IsNullOrWhiteSpace(q)
                ? new List<ProductoDTOResultados>() // si no hay query, no mostramos nada
                : Catalogo
                    .Where(p => p.Nombre.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

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
