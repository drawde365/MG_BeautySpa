package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.DetallePedidoDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.DetallePedidoDAOImpl;
import pe.edu.pucp.softinv.daoImp.PedidoDAOimpl;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.util.ArrayList;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

public class PedidoBO {

    private PedidoDAO pedidoDAO;
    private DetallePedidoDAO detallePedidoDAO;
    private ProductoTipoDAO productoDAO; 

    public PedidoBO() {
        pedidoDAO = new PedidoDAOimpl();
        detallePedidoDAO = new DetallePedidoDAOImpl();
        productoDAO = new ProductoTipoDAOImpl();
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

    public Integer insertarDetalle(DetallePedidoDTO detallePedido, Integer idPedido) {
        PedidoDTO pedido = pedidoDAO.obtenerPorId(idPedido); // <-- Usa el ID del parámetro
        pedido.setTotal(pedido.getTotal() + detallePedido.getSubtotal());
        pedidoDAO.modificar(pedido);

        // Asigna manualmente la referencia del pedido ANTES de insertar el detalle
        PedidoDTO pedidoRef = new PedidoDTO();
        pedidoRef.setIdPedido(idPedido);
        detallePedido.setPedido(pedidoRef); 

        return detallePedidoDAO.insertar(detallePedido);
    }

    public DetallePedidoDTO obtenerDetalle(Integer idPedido, Integer idProducto, Integer tipoId) {
        return detallePedidoDAO.obtener(idPedido, idProducto, tipoId);
    }
    
    public Integer modificarDetalle(DetallePedidoDTO detallePedido, Integer idPedido) {
        // 1. Usa el idPedido del parámetro para la consulta
        DetallePedidoDTO detallle = detallePedidoDAO.obtener(
            idPedido, 
            detallePedido.getProducto().getProducto().getIdProducto(), 
            detallePedido.getProducto().getTipo().getId()
        );

        // 2. Actualiza el total del pedido padre
        if(!detallle.getSubtotal().equals(detallePedido.getSubtotal())) {
            PedidoDTO pedido = pedidoDAO.obtenerPorId(idPedido); // <-- Usa el ID
            double nuevoTotal = pedido.getTotal() - detallle.getSubtotal() + detallePedido.getSubtotal();
            pedido.setTotal(nuevoTotal);
            pedidoDAO.modificar(pedido);
        }

        // 3. Asigna la referencia del pedido al detalle ANTES de modificarlo
        PedidoDTO pedidoRef = new PedidoDTO();
        pedidoRef.setIdPedido(idPedido);
        detallePedido.setPedido(pedidoRef); 

        return detallePedidoDAO.modificar(detallePedido);
    }
    public Integer eliminarDetalle(DetallePedidoDTO detallePedido, Integer idPedido ) {
        PedidoDTO pedido = pedidoDAO.obtenerPorId(idPedido);
        pedido.setTotal(pedido.getTotal()-  detallePedido.getSubtotal());
        pedidoDAO.modificar(pedido);
        detallePedido.setPedido(pedido);
        return detallePedidoDAO.eliminar(detallePedido);
    }
    public ArrayList<DetallePedidoDTO> obtenerDetallesPedidosId(Integer idPedido) {
        return detallePedidoDAO.obtenerDetallesPedidosId(idPedido);
    }
    
    public ArrayList<Integer> comprobarDetallePedido(Integer idPedido) {
        ArrayList<Integer> listaValida = new ArrayList<Integer>();
        PedidoDTO pedido = pedidoDAO.obtenerPorId(idPedido);
        for (DetallePedidoDTO detallePedido : pedido.getDetallesPedido()) {
            ProductoTipoDTO productoTipo = productoDAO.obtener(
                    detallePedido.getProducto().getTipo().getId(), 
                    detallePedido.getProducto().getProducto().getIdProducto());
            if(productoTipo.getStock_despacho()+detallePedido.getCantidad()>productoTipo.getStock_fisico())
                listaValida.add(0);
            else
                listaValida.add(1);
        }
        return listaValida;
    }
    
    public Integer aceptarRecojo(PedidoDTO pedido) {
        for (DetallePedidoDTO detallePedido : pedido.getDetallesPedido()) {
            ProductoTipoDTO productoTipo = productoDAO.obtener(
                    detallePedido.getProducto().getTipo().getId(), 
                    detallePedido.getProducto().getProducto().getIdProducto());
            productoTipo.setStock_fisico(productoTipo.getStock_fisico()-detallePedido.getCantidad());
            productoTipo.setStock_despacho(productoTipo.getStock_despacho()-detallePedido.getCantidad());
        }
        pedido.setEstadoPedido(EstadoPedido.RECOGIDO);
        return pedidoDAO.modificar(pedido);
    }
}