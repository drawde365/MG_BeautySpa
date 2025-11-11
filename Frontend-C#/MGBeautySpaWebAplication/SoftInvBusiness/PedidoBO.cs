using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class PedidoBO
    {
        private PedidoClient pedidoSOAP;

        public PedidoBO()
        {
            pedidoSOAP = new PedidoClient();
        }

        // ----- Pedidos -----

        public int Insertar(pedidoDTO pedido)
        {
            return pedidoSOAP.InsertarPedido(pedido);
        }

        public pedidoDTO ObtenerPorId(int idPedido)
        {
            return pedidoSOAP.ObtenerPedidoPorId(idPedido);
        }

        public int Modificar(pedidoDTO pedido)
        {
            return pedidoSOAP.ModificarPedido(pedido);
        }

        public int Eliminar(pedidoDTO pedido)
        {
            return pedidoSOAP.EliminarPedido(pedido);
        }

        public IList<pedidoDTO> ListarPorCliente(int idCliente)
        {
            return pedidoSOAP.ListarPedidosDeCliente(idCliente);
        }

        public pedidoDTO ObtenerCarritoPorCliente(int idCliente)
        {
            return pedidoSOAP.ObtenerCarritoPorCliente(idCliente);
        }

        // ----- Detalles de Pedido -----

        public int InsertarDetalle(detallePedidoDTO detalle)
        {
            return pedidoSOAP.InsertarDetallePedido(detalle);
        }

        public detallePedidoDTO ObtenerDetalle(int idPedido, int idProducto, string tipoProducto)
        {
            return pedidoSOAP.ObtenerDetallePedido(idPedido, idProducto, tipoProducto);
        }

        public int ModificarDetalle(detallePedidoDTO detalle)
        {
            return pedidoSOAP.ModificarDetallePedido(detalle);
        }

        public int EliminarDetalle(detallePedidoDTO detalle)
        {
            return pedidoSOAP.EliminarDetallePedido(detalle);
        }

        public IList<detallePedidoDTO> ObtenerDetallesPorPedido(int idPedido)
        {
            return pedidoSOAP.ObtenerDetallesPorPedidoId(idPedido);
        }
    }
}
