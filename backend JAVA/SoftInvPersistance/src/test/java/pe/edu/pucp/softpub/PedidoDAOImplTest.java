package pe.edu.pucp.softpub;

import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.daoImp.PedidoDAOimpl;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.sql.Date;
import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Tag;

@Tag("integration")
public class PedidoDAOImplTest {
    private PedidoDAO pedidoDAO;
    private ClienteDAO clienteDAO;
    private ProductoDAO productoDAO;
    private ProductoTipoDAO productoTipoDAO;
    private ArrayList<PedidoDTO> pedidos;
    private int idProducto;
    private ArrayList<ProductoTipoDTO> productos;

    public PedidoDAOImplTest(){
        pedidoDAO=new PedidoDAOimpl();
        clienteDAO = new ClienteDAOimpl();
        productoDAO = new ProductoDAOimpl();
        productoTipoDAO = new ProductoTipoDAOImpl();
        pedidos = new ArrayList<>();
    }

    public ProductoDTO insertar() {
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        producto.setPrecio(50.25);
        producto.setModoUso("Producto que se usa asi");
        producto.setUrlImagen("hola.jpg");
        producto.setPromedioValoracion(5.0);
        producto.setActivo(1);
        producto.setTamanho(2.5);

        ProductoTipoDTO productoTipo = new ProductoTipoDTO();
        productoTipo.setTipo("Grasa");
        productoTipo.setStock_despacho(12);
        productoTipo.setStock_fisico(120481038);
        productoTipo.setIngredientes("Hola mundo");
        productoTipo.setActivo(1);
        ProductoTipoDTO productoTipo2 = new ProductoTipoDTO();
        productoTipo2.setTipo("Suave");
        productoTipo2.setStock_despacho(10);
        productoTipo2.setStock_fisico(3543);
        productoTipo2.setIngredientes("Adios mundo");
        productoTipo2.setActivo(1);
        productos = new ArrayList<>();
        productos.add(productoTipo);
        productos.add(productoTipo2);
        producto.setProductosTipos(productos);
        idProducto = productoDAO.insertar(producto);
        producto.setIdProducto(idProducto);
        assertNotEquals(0, idProducto, "El producto debería insertarse correctamente");
        return producto;
    }

    private ClienteDTO definirCliente() {
        ClienteDTO cliente = new ClienteDTO();
        cliente.setPrimerapellido("Guanira");
        cliente.setSegundoapellido("Erasmo");
        cliente.setNombre("Juan");
        cliente.setCorreoElectronico("a20834215@mail.com");
        cliente.setContrasenha("1234");
        cliente.setCelular("999888777");
        cliente.setRol(1);
        cliente.setUrlFotoPerfil("dsajdjalds");
        cliente.setActivo(1);
        return cliente;
    }

    private ArrayList<DetallePedidoDTO> definirDetalles(ArrayList<ProductoTipoDTO> listaProductos){
        ArrayList<DetallePedidoDTO> listaDevolver = new ArrayList<>();
        DetallePedidoDTO detalle1 = new DetallePedidoDTO();
        detalle1.setProducto(listaProductos.get(0));
        detalle1.setCantidad(1);
        detalle1.setSubtotal(50.00);
        listaDevolver.add(detalle1);
        detalle1 = new DetallePedidoDTO();
        detalle1.setProducto(listaProductos.get(1));
        detalle1.setCantidad(1);
        detalle1.setSubtotal(50.00);
        listaDevolver.add(detalle1);
        return listaDevolver;
    }

    private PedidoDTO definirPedido(ClienteDTO cliente,ProductoDTO productoDTO) {
        PedidoDTO pedido = new PedidoDTO();
        pedido.setCliente(cliente);
        ArrayList<ProductoTipoDTO> listaProductos = productoTipoDAO.obtenerProductoId(productoDTO.getIdProducto());
        assertNotNull(listaProductos.get(0),"DEBERIA HABER PRODUCTOS");
        ArrayList<DetallePedidoDTO> detalles = definirDetalles(listaProductos);
        pedido.setDetallesPedido(detalles);
        pedido.setEstadoPedido(EstadoPedido.CONFIRMADO);
        pedido.setTotal(100.00);
        pedido.setIGV(18.00);
        pedido.setCodigoTransaccion("enigma");
        Date fecha = Date.valueOf("2025-10-08");
        pedido.setFechaPago(fecha);
        pedido.setFechaRecojo(null);
        pedido.setFechaListaParaRecojo(null);
        return pedido;
    }

    @Test
    void testInsertarPedido(){
        ProductoDTO productoDTO = insertar();
        ClienteDTO cliente = definirCliente();
        int idUser = clienteDAO.insertar(cliente);
        assertTrue(idUser != 0);
        cliente.setIdUsuario(idUser);
        PedidoDTO pedido = this.definirPedido(cliente,productoDTO);
        int id = pedidoDAO.insertar(pedido);
        assertNotEquals(0,id,"DEBERIA HABER PEDIDO");
        pedido.setIdPedido(id);
        pedidos.add(pedido);
        listarPedido();
        eliminarPedido();
    }

    private void listarPedido(){
        ArrayList<PedidoDTO> listaDevolver = pedidoDAO.listarPedidos(pedidos.get(0).getCliente().getIdUsuario());
        for(PedidoDTO pedido : listaDevolver){
            System.out.printf("Pedido: %d  %.2f %s  %s %5$td/%5$tm/%5$tY%n",pedido.getIdPedido(),pedido.getTotal(),pedido.getEstadoPedidoS()
            ,pedido.getCliente().getNombre(),pedido.getFechaPago());
            for(DetallePedidoDTO detalle : pedido.getDetallesPedido()){
                System.out.printf("Detalle: %d  %.2f  %s   %s\n",detalle.getCantidad(),detalle.getSubtotal(),
                        detalle.getProducto().getProducto().getNombre(),detalle.getProducto().getTipo());
            }
        }
    }

    private void eliminarPedido(){
        int idCliente=0;
        for (PedidoDTO pedido : pedidos) {
            Integer resultado = pedidoDAO.eliminar(pedido);
            idCliente = pedido.getCliente().getIdUsuario();
            assertTrue(resultado != 0);
            PedidoDTO ped = pedidoDAO.obtenerPorId(pedido.getIdPedido());
            assertNull(ped);
        }
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(idCliente);
        clienteDAO.eliminar(cliente);
        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(idProducto);
        producto.setProductosTipos(productos);
        productoDAO.eliminar(producto);
    }
}
