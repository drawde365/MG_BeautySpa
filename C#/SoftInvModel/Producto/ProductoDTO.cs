using System;
using System.ComponentModel;

namespace softinv.model
{
    public class ProductoDTO
    {
        private int? idProducto;
        private string nombre;
        private string descripcion;
        private double? precio;
        private BindingList<string> ingredientes;
        private string modoUso;
        private TipoProducto? tipoProducto;
        private BindingList<TipoPiel> tipoPiel;
        private int? stock;
        private string urlImagen;
        private BindingList<ComentarioProductoDTO> comentarios;
        private double? promedioValoracion;

        public ProductoDTO()
        {
            IdProducto = null;
            TipoProducto = null;
            TipoPiel = null;
            Nombre = null;
            Descripcion = null;
            Precio = null;
            Ingredientes = null;
            ModoUso = null;
            Stock = null;
            UrlImagen = null;
            Comentarios = null;
            PromedioValoracion = null;
        }

        public ProductoDTO(int idProducto, string nombre, string descripcion, Double precio,
                           BindingList<ComentarioProductoDTO> comentarios, BindingList<string> ingredientes, string modoUso,
                           TipoProducto tipoProducto, BindingList<string> tamanios, BindingList<TipoPiel> tipoPiel,
                           int stock, string urlImagen, Double promedioValoracion)
        {
            this.IdProducto = idProducto;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Precio = precio;
            this.Comentarios = comentarios;
            this.Ingredientes = ingredientes;
            this.ModoUso = modoUso;
            this.TipoProducto = tipoProducto;
            this.TipoPiel = tipoPiel;
            this.Stock = stock;
            this.UrlImagen = urlImagen;
            this.PromedioValoracion = promedioValoracion;
        }

        public int? IdProducto { get => idProducto; set => idProducto = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public double? Precio { get => precio; set => precio = value; }
        public BindingList<string> Ingredientes { get => ingredientes; set => ingredientes = value; }
        public string ModoUso { get => modoUso; set => modoUso = value; }
        public TipoProducto? TipoProducto { get => tipoProducto; set => tipoProducto = value; }
        public BindingList<TipoPiel> TipoPiel { get => tipoPiel; set => tipoPiel = value; }
        public int? Stock { get => stock; set => stock = value; }
        public string UrlImagen { get => urlImagen; set => urlImagen = value; }
        public BindingList<ComentarioProductoDTO> Comentarios { get => comentarios; set => comentarios = value; }
        public double? PromedioValoracion { get => promedioValoracion; set => promedioValoracion = value; }
    }

}
