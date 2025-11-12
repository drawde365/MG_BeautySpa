package pe.edu.pucp.softpub;

import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.*;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.daoImp.ComentarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

class ComentarioDAOImplTest {
    private ComentarioDAO comentarioDAO;
    private ClienteDAO clienteDAO;
    private ProductoDAO productoDAO;
    private ServicioDAO servicioDAO;
    private ArrayList<ComentarioDTO> comentarios = new ArrayList<>();
    private Integer idProd;

    public ComentarioDAOImplTest() {
        comentarioDAO = new ComentarioDAOImpl();
        clienteDAO = new ClienteDAOimpl();
        productoDAO = new ProductoDAOimpl();
        servicioDAO = new ServicioDAOImpl();
    }

    @Test
    public void testInsertar() {
        System.out.println("insertar");
        ArrayList<Integer> listaComentarios = new ArrayList<>();
        insertarComentarios(listaComentarios);
        eliminarTodos();
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

    private ProductoDTO definirProducto() {
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
        ArrayList<ProductoTipoDTO> productos = new ArrayList<>();
        productos.add(productoTipo);
        productos.add(productoTipo2);
        producto.setProductosTipos(productos);
        Integer result = productoDAO.insertar(producto);
        producto.setIdProducto(result);
        assertNotEquals(0, result, "El producto debería insertarse correctamente");
        return producto;
    }

    private ServicioDTO definirServicio() {
        ServicioDTO servicio = new ServicioDTO();
        servicio.setDuracionHora(1);
        servicio.setUrlImagen("kdjacsn.jpg");
        servicio.setNombre("Tratamiento facial");
        servicio.setDescripcion("Te deja la cara muy suave conn buenas mascarillas");
        servicio.setTipo("FACIAL");
        servicio.setPrecio(100.01);
        servicio.setPromedioValoracion(2.3);
        servicio.setActivo(1);
        return servicio;
    }

    public void insertarComentarios(ArrayList<Integer> listaComentarios) {
        //Producto
        ComentarioDTO comentarioProducto = new ComentarioDTO();
        ProductoDTO producto = definirProducto();
        comentarioProducto.setComentario("El producto estaba muy bueno");
        comentarioProducto.setValoracion(5);
        ClienteDTO cliente = definirCliente();
        int idUser = clienteDAO.insertar(cliente);
        assertTrue(idUser != 0);

        cliente.setIdUsuario(idUser);
        comentarioProducto.setCliente(cliente);
        int idProducto = productoDAO.insertar(producto);
        this.idProd = idProducto;
        assertTrue(idProducto != 0);

        producto.setIdProducto(idProducto);
        comentarioProducto.setProducto(producto);

        int idComen = comentarioDAO.insertar(comentarioProducto);

        comentarioProducto.setIdComentario(idComen);
        listaComentarios.add(idComen);
        comentarios.add(comentarioProducto);
        ComentarioDTO comen2 = new ComentarioDTO();

        comen2.setComentario("YOLOOOOO");
        comen2.setValoracion(2);
        comen2.setCliente(cliente);
        comen2.setProducto(producto);
        int idComen2 = comentarioDAO.insertar(comentarioProducto);
        comen2.setIdComentario(idComen2);
        listaComentarios.add(idComen2);
        comentarios.add(comen2);

        //Servicio
        ComentarioDTO comentarioServicio = new ComentarioDTO();
        comentarioServicio.setComentario("El servicio fue malisimo");
        comentarioServicio.setValoracion(2);
        comentarioServicio.setCliente(cliente);

        ServicioDTO servicio = definirServicio();
        int idSer = servicioDAO.insertar(servicio);
        assertTrue(idSer != 0);
        servicio.setIdServicio(idSer);
        comentarioServicio.setServicio(servicio);
        idComen = comentarioDAO.insertar(comentarioServicio);
        listaComentarios.add(idComen);
        comentarioServicio.setIdComentario(idComen);
        comentarios.add(comentarioServicio);
    }

    private void eliminarTodos() {
        int id_Cliente=0;
        for (ComentarioDTO comentario : comentarios) {
            Integer resul = comentarioDAO.eliminar(comentario);
            id_Cliente = comentario.getCliente().getIdUsuario();
            assertTrue(resul != 0);
            ComentarioDTO com = comentarioDAO.obtenerPorId(comentario.getIdComentario());
            assertNull(com);
        }
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(id_Cliente);
        clienteDAO.eliminar(cliente);
        productoDAO.eliminar(comentarios.get(1).getProducto());
        servicioDAO.eliminar(comentarios.get(2).getServicio());
        comentarios.clear();
    }

    @Test
    public void testModificar() {
        System.out.println("modificar");
        ArrayList<Integer> listaComentariosId = new ArrayList<>();
        insertarComentarios(listaComentariosId);
        ComentarioDTO com = comentarioDAO.obtenerPorId(listaComentariosId.get(0));
        com.setComentario("RETRACTO MI COMENTARIOOOO!!! YOLOO");
        int res = comentarioDAO.modificar(com);
        assertTrue(res!=0);
        eliminarTodos();
    }

    @Test
    public void testObtenerComentarioPorId() {
        System.out.println("Obtener Comentario por ID");
        ArrayList<Integer> listaComentariosId = new ArrayList<>();
        insertarComentarios(listaComentariosId);
        ComentarioDTO com = comentarioDAO.obtenerPorId(listaComentariosId.get(0));
        assertNotNull(com);
        eliminarTodos();
    }

    @Test
    public void testProcedureObtenerComentariosProducto(){
        System.out.println("SP obtener comentarios por producto");
        ArrayList<Integer> listaComentarios = new ArrayList<>();
        insertarComentarios(listaComentarios);
        ArrayList<ComentarioDTO> listaCom = comentarioDAO.obtenerComentariosPorProducto(this.idProd);
        for (ComentarioDTO comentario : listaCom) {
            System.out.printf("%d   %s    %s   %d    %d\n",comentario.getIdComentario(),
                    comentario.getCliente().getNombre(),comentario.getCliente().getPrimerapellido(),comentario.getProducto().getIdProducto(),comentario.getServicio().getIdServicio());
        }
        eliminarTodos();
    }
}
