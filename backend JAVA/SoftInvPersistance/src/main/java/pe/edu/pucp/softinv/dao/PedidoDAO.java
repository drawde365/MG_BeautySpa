package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.util.ArrayList;

public interface PedidoDAO {
    Integer insertar(PedidoDTO pedido);
    PedidoDTO obtenerPorId(Integer idPedido);
    Integer modificar(PedidoDTO pedido);
    Integer eliminar(PedidoDTO pedido);
    ArrayList<PedidoDTO> listarPedidos(Integer idCliente);
    PedidoDTO obtenerCarritoPorCliente(Integer idCliente);
    ArrayList<PedidoDTO> listarTodoPedidos();
    ArrayList<PedidoDTO> listarTodoPedidosPaginado(Integer pagina);
    Integer contarPedidos();
}
