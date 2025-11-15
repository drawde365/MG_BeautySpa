package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.DetallePedidoDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.DetallePedidoDAOImpl;
import pe.edu.pucp.softinv.daoImp.PedidoDAOimpl;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.util.ArrayList;

public class PedidoBO {

    private PedidoDAO pedidoDAO;
    private DetallePedidoDAO detallePedidoDAO;

    public PedidoBO() {
        pedidoDAO = new PedidoDAOimpl();
        detallePedidoDAO = new DetallePedidoDAOImpl();
    }

    public Integer insertar(PedidoDTO pedido) {
        return pedidoDAO.insertar(pedido);
    }
    public PedidoDTO obtenerPorId(Integer idPedido) {
        return pedidoDAO.obtenerPorId(idPedido);
    }
    public Integer modificar(PedidoDTO pedido) {
        return pedidoDAO.modificar(pedido);
    }
    public Integer eliminar(PedidoDTO pedido) {
        pedido.setEstadoPedido(EstadoPedido.ELIMINADO);
        return pedidoDAO.modificar(pedido);
    }
    public ArrayList<PedidoDTO> listarPedidosDeCliente(Integer idCliente) {
        return pedidoDAO.listarPedidos(idCliente);
    }
    public PedidoDTO obtenerCarritoPorCliente(Integer idCliente) {
        return pedidoDAO.obtenerCarritoPorCliente(idCliente);
    }

    public Integer insertarDetalle(DetallePedidoDTO detallePedido) {
        PedidoDTO pedido = pedidoDAO.obtenerPorId(detallePedido.getPedido().getIdPedido());
        pedido.setTotal(pedido.getTotal()+detallePedido.getSubtotal());
        pedidoDAO.modificar(pedido);
        return detallePedidoDAO.insertar(detallePedido);
    }

    public DetallePedidoDTO obtenerDetalle(Integer idPedido, Integer idProducto, Integer tipoId) {
        return detallePedidoDAO.obtener(idPedido, idProducto, tipoId);
    }
    
    public Integer modificarDetalle(DetallePedidoDTO detallePedido) {
        DetallePedidoDTO detallle = detallePedidoDAO.obtener(detallePedido.getPedido().getIdPedido(),
                detallePedido.getProducto().getProducto().getIdProducto(), 
                detallePedido.getProducto().getTipo().getId());
                
        if(!detallle.getSubtotal().equals(detallePedido.getSubtotal())) {
            PedidoDTO pedido = pedidoDAO.obtenerPorId(detallePedido.getPedido().getIdPedido());
            double nuevoTotal = pedido.getTotal() - detallle.getSubtotal() + detallePedido.getSubtotal();
            pedido.setTotal(nuevoTotal);
            pedidoDAO.modificar(pedido);
        }
        return detallePedidoDAO.modificar(detallePedido);

    }
    public Integer eliminarDetalle(DetallePedidoDTO detallePedido) {
        PedidoDTO pedido = pedidoDAO.obtenerPorId(detallePedido.getPedido().getIdPedido());
        pedido.setTotal(pedido.getTotal()-  detallePedido.getSubtotal());
        pedidoDAO.modificar(pedido);
        return detallePedidoDAO.eliminar(detallePedido);
    }
    public ArrayList<DetallePedidoDTO> obtenerDetallesPedidosId(Integer idPedido) {
        return detallePedidoDAO.obtenerDetallesPedidosId(idPedido);
    }
}