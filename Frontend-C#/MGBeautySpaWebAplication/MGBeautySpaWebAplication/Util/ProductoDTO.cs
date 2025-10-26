using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGBeautySpaWebAplication.Util
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
    }
}