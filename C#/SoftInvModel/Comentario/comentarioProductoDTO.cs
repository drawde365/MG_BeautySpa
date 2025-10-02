using System;

namespace softinv.model
{
    public class ComentarioProductoDTO: ComentarioDTO
        {
        private ProductoDTO producto;

        public ProductoDTO Producto { get => producto; set => producto = value; }

        public ComentarioProductoDTO(): base()
        {
            Producto = null;
        }

        public ComentarioProductoDTO(int idComentario, ClienteDTO cliente, String comentario, int valoracion, ProductoDTO producto): base(idComentario, cliente, comentario, valoracion)
        {
            this.Producto = producto;
        }

        public int? IdProducto { get => producto.IdProducto; set => producto.IdProducto = value; }
    }
}
