using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGBeautySpaWebAplication.Util
{
        

    public static class ProductoDAO
    {
        private static readonly List<ProductoDTO> _fake = new List<ProductoDTO>
        {
            new ProductoDTO { Id = 1, Nombre = "Crema hidratante",  Precio = 49.90m, ImagenUrl = "/Content/images/CremaHidratante.jpg" },
            new ProductoDTO { Id = 2, Nombre = "Crema reafirmante", Precio = 59.90m, ImagenUrl = "/Content/images/CremaReafirmante.jpg" },
            new ProductoDTO { Id = 3, Nombre = "Crema reductora",   Precio = 54.90m, ImagenUrl = "/Content/images/CremaReductora.jpg" },
            new ProductoDTO { Id = 4, Nombre = "Crema termogénica", Precio = 62.00m, ImagenUrl = "/Content/images/CremaTermogenica.jpg" },
        };

        public static List<ProductoDTO> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return new List<ProductoDTO>();
            nombre = nombre.ToLower();

            // “coincidencia por contiene” simple
            return _fake.Where(p => p.Nombre.ToLower().Contains(nombre)).ToList();
        }
    }
}