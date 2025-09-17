package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.util.ArrayList;

public interface PedidoDAO {
    public Integer insertar(PedidoDTO pedido);
    public PedidoDTO optenerPorId(Integer id);
    public Integer modificar(PedidoDTO pedido);
    public Integer eliminar(PedidoDTO pedido);
    public ArrayList<PedidoDTO> listarPedidos(Integer id);
}
