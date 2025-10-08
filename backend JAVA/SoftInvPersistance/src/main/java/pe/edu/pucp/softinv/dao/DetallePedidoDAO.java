package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;

import java.util.ArrayList;

public interface DetallePedidoDAO {
    public Integer insertar(DetallePedidoDTO detallePedido);
    public DetallePedidoDTO obtener(Integer idPedido, Integer idProducto, String tipoProducto);
    public Integer modificar(DetallePedidoDTO detallePedido);
    public Integer eliminar(DetallePedidoDTO detallePedido);
    public ArrayList<DetallePedidoDTO> obtenerProductoId(Integer idPedido, Integer idProducto, String tipoProducto);
}
