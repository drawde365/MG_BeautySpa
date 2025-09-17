package pe.edu.pucp.softpub;

import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.daoImp.ComentarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioProductoDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioServicioDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProducto;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

class ComentarioDAOImplTest {
    private ComentarioDAO comentarioDAO;
    private ClienteDAO clienteDAO;
    private ProductoDAO productoDAO;
    private ServicioDAO servicioDAO;
    private ArrayList<ComentarioDTO> comentarios = new ArrayList<>();

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
        cliente.setRol();
        cliente.setUrlFotoPerfil("dsajdjalds");
        return cliente;
    }

    private ProductoDTO definirProducto() {
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        producto.setTipoProducto(TipoProducto.CORPORAL);
        producto.setPrecio(50.25);
        producto.setModoUso("Producto que se usa asi");
        producto.setStock(20);
        producto.setPromedioValoracion(4.2);
        producto.setUrlImagen("hola.jpg");
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
        return servicio;
    }

    private void insertarComentarios(ArrayList<Integer> listaComentarios) {
        ComentarioProductoDTO comentarioDTO = new ComentarioProductoDTO();
        comentarioDTO.setComentario("El producto estaba muy bueno");
        comentarioDTO.setValoracion(5);
        ClienteDTO cliente = definirCliente();
        int idUser = clienteDAO.insertar(cliente);
        assertTrue(idUser != 0);
        comentarioDTO.setCliente(cliente);
        cliente.setIdUsuario(idUser);
        ProductoDTO producto = definirProducto();
        int idProducto = productoDAO.insertar(producto);
        assertTrue(idProducto != 0);
        producto.setIdProducto(idProducto);
        comentarioDTO.setProducto(producto);
        int idComen = comentarioDAO.insertar(comentarioDTO);
        comentarioDTO.setIdComentario(idComen);
        listaComentarios.add(idComen);
        comentarios.add(comentarioDTO);

        ComentarioServicioDTO comentarioServicioDTO = new ComentarioServicioDTO();
        comentarioServicioDTO.setComentario("El servicio fue malisimo");
        comentarioServicioDTO.setValoracion(2);
        comentarioServicioDTO.setCliente(cliente);

        ServicioDTO servicio = definirServicio();
        int idSer = servicioDAO.insertar(servicio);
        assertTrue(idSer != 0);
        servicio.setIdServicio(idSer);
        comentarioServicioDTO.setServicio(servicio);
        idComen = comentarioDAO.insertar(comentarioServicioDTO);
        listaComentarios.add(idComen);
        comentarioServicioDTO.setIdComentario(idComen);
        comentarios.add(comentarioServicioDTO);
    }

    private void eliminarTodos() {
        for (ComentarioDTO comentario : comentarios) {
            Integer resul = comentarioDAO.eliminar(comentario);
            assertTrue(resul != 0);
            ComentarioDTO com = comentarioDAO.obtenerPorId(comentario.getIdComentario());
            assertNull(com);
        }
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

//    @Test
//    public void testObtenerComentarioPorId(){
//      int
//    }
}