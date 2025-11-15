package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;

import java.util.ArrayList;

public interface DetallePedidoDAO {
    public Integer insertar(DetallePedidoDTO detallePedido);
    Integer insertar(DetallePedidoDTO detallePedidoDTO,boolean dejarConexionAbierta, boolean transaccionInciada);
    public DetallePedidoDTO obtener(Integer idPedido, Integer idProducto, Integer tipoId) ;
    public Integer modificar(DetallePedidoDTO detallePedido);
    public Integer eliminar(DetallePedidoDTO detallePedido);
    Integer eliminar(DetallePedidoDTO detallePedido,boolean dejarConexionAbierta, boolean transaccionInciada);
    public ArrayList<DetallePedidoDTO> obtenerDetallesPedidosId(Integer idPedido);
}
