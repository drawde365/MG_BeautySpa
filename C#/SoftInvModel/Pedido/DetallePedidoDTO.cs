using System;

namespace softinv.model
{
    public class DetallePedidoDTO
    {
        private ProductoDTO producto;
        private PedidoDTO pedido;
        private int? cantidad;
        private double? subtotal;


        public DetallePedidoDTO()
        {
            Producto = null;
            Cantidad = null;
            Subtotal = null;
            Pedido = null;
        }

        public DetallePedidoDTO(ProductoDTO producto, PedidoDTO pedido, int cantidad, double subtotal)
        {
            this.Producto = producto;
            this.Cantidad = cantidad;
            this.Subtotal = subtotal;
            this.Pedido = pedido;
            //COMPLETAR POR FAVOR
            //this.
        }

        public ProductoDTO Producto { get => producto; set => producto = value; }
        public PedidoDTO Pedido { get => pedido; set => pedido = value; }
        public int? Cantidad { get => cantidad; set => cantidad = value; }
        public double? Subtotal { get => subtotal; set => subtotal = value; }
    }
}
