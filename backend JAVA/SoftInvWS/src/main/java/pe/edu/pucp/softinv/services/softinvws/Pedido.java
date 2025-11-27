package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.PedidoBO;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

@WebService(serviceName = "Pedido")
public class Pedido {

    private PedidoBO pedidoBO;

    public Pedido() {
        pedidoBO = new PedidoBO();
    }

    @WebMethod(operationName = "InsertarPedido")
    public Integer insertarPedido(
            @WebParam(name = "pedido") PedidoDTO pedido) {
        return pedidoBO.insertar(pedido);
    }

    @WebMethod(operationName = "ObtenerPedidoPorId")
    public PedidoDTO obtenerPedidoPorId(
            @WebParam(name = "idPedido") Integer idPedido) {
        return pedidoBO.obtenerPorId(idPedido);
    }

    @WebMethod(operationName = "ModificarPedido")
    public Integer modificarPedido(
            @WebParam(name = "pedido") PedidoDTO pedido) {
        return pedidoBO.modificar(pedido);
    }

    @WebMethod(operationName = "EliminarPedido")
    public Integer eliminarPedido(
            @WebParam(name = "pedido") PedidoDTO pedido) {
        return pedidoBO.eliminar(pedido);
    }
    
    @WebMethod(operationName = "ListarPedidosNOCARRITO")
    public ArrayList<PedidoDTO> listarTodoPedidos() {
        return pedidoBO.listarTodoPedidos();
    }

    @WebMethod(operationName = "ListarPedidosDeCliente")
    public ArrayList<PedidoDTO> listarPedidosDeCliente(
            @WebParam(name = "idCliente") Integer idCliente) {
        return pedidoBO.listarPedidosDeCliente(idCliente);
    }

    @WebMethod(operationName = "ObtenerCarritoPorCliente")
    public PedidoDTO obtenerCarritoPorCliente(
            @WebParam(name = "idCliente") Integer idCliente) {
        return pedidoBO.obtenerCarritoPorCliente(idCliente);
    }

    @WebMethod(operationName = "InsertarDetallePedido")
    public Integer insertarDetallePedido(
            @WebParam(name = "detallePedido") DetallePedidoDTO detallePedido,
            @WebParam(name = "idPedido") Integer idPedido) { // <-- AÑADE ESTE PARÁMETRO
        return pedidoBO.insertarDetalle(detallePedido, idPedido);
    }

    @WebMethod(operationName = "ObtenerDetallePedido")
    public DetallePedidoDTO obtenerDetallePedido(
            @WebParam(name = "idPedido") Integer idPedido,
            @WebParam(name = "idProducto") Integer idProducto,
            @WebParam(name = "tipoId") Integer tipoId) {
        return pedidoBO.obtenerDetalle(idPedido, idProducto, tipoId);
    }

    @WebMethod(operationName = "ModificarDetallePedido")
    public Integer modificarDetallePedido(
            @WebParam(name = "detallePedido") DetallePedidoDTO detallePedido,
            @WebParam(name = "idPedido") Integer idPedido) { // <-- AÑADE EL PARÁMETRO
        return pedidoBO.modificarDetalle(detallePedido, idPedido);
    }

    @WebMethod(operationName = "EliminarDetallePedido")
    public Integer eliminarDetallePedido(
            @WebParam(name = "detallePedido") DetallePedidoDTO detallePedido, @WebParam(name = "idPedido") Integer idPedido) {
        return pedidoBO.eliminarDetalle(detallePedido, idPedido);
    }

    @WebMethod(operationName = "ObtenerDetallesPorPedidoId")
    public ArrayList<DetallePedidoDTO> obtenerDetallesPorPedidoId(
            @WebParam(name = "idPedido") Integer idPedido) {
        return pedidoBO.obtenerDetallesPedidosId(idPedido);
    }
    @WebMethod(operationName = "ComprobarDetallesPedidos")
    public ArrayList<Integer> comprobarDetallePedido(
            @WebParam(name = "idPedido")Integer idPedido) {
        return pedidoBO.comprobarDetallePedido(idPedido);
    }
    @WebMethod(operationName = "AceptarRecojo")
    public Integer aceptarRecojo(
            @WebParam(name = "pedido")PedidoDTO pedido) {
        return pedidoBO.aceptarRecojo(pedido);
    }
    @WebMethod(operationName = "RechazarRecojo")
    public Integer rechazarRecojo(
            @WebParam(name = "pedido")PedidoDTO pedido) {
        return pedidoBO.rechazarRecojo(pedido);
    }
    @WebMethod(operationName = "modificarProductoTipo")
    public Integer modificarProductoTipoPedido(ProductoTipoDTO productoTipo) {
        return pedidoBO.modificar(productoTipo);
    }
    @WebMethod(operationName = "listarTodoPedidosPaginado")
    public ArrayList<PedidoDTO> listarTodoPedidosPaginado(@WebParam(name = "numeroPagina")Integer pagina) {
        return pedidoBO.listarTodoPedidosPaginado(pagina);
    }
    @WebMethod(operationName = "obtenerPaginasPedido")
    public Integer obtenerPaginasPedido() {
        return pedidoBO.obtenerPaginas();
    }
}