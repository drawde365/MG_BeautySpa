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

        public int InsertarDetalle(detallePedidoDTO detalle, int idPedido)
        {
            return pedidoSOAP.InsertarDetallePedido(detalle, idPedido);
        }

        public detallePedidoDTO ObtenerDetalle(int idPedido, int idProducto, int id_tipoProducto)
        {
            return pedidoSOAP.ObtenerDetallePedido(idPedido, idProducto, id_tipoProducto);
        }

        public int ModificarDetalle(detallePedidoDTO detalle, int idPedido)
        {
            return pedidoSOAP.ModificarDetallePedido(detalle, idPedido);
        }

        public int EliminarDetalle(detallePedidoDTO detalle, int idPedidp)
        {
            return pedidoSOAP.EliminarDetallePedido(detalle, idPedidp);
        }

        public IList<detallePedidoDTO> ObtenerDetallesPorPedido(int idPedido)
        {
            return pedidoSOAP.ObtenerDetallesPorPedidoId(idPedido);
        }
    }
}
