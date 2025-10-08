package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

//TEST FUNCIONAL!!

class ClienteDAOImplTest {
    private Integer ultimoId;
    private ClienteDAO clienteDAO;
    private ArrayList<ClienteDTO> listaClientesId = new ArrayList<>();

    public ClienteDAOImplTest() {
        clienteDAO = new ClienteDAOimpl();
    }

    @Test
    public void testInsertar(){
        System.out.println("insertar");
        insertarClientes();
        eliminarTodo();
    }

    private void insertarClientes(){
        ClienteDTO cliente = new ClienteDTO();
        cliente.setPrimerapellido("Perez");
        cliente.setSegundoapellido("Gomez");
        cliente.setNombre("Juan");
        cliente.setCorreoElectronico("juan@mail.com");
        cliente.setContrasenha("1234");
        cliente.setCelular("999888777");
        cliente.setRol();
        cliente.setUrlFotoPerfil("dsajdjalds");
        cliente.setActivo(1);
        Integer idGenerado = clienteDAO.insertar(cliente);
        cliente.setIdUsuario(idGenerado);
        assertTrue(idGenerado != 0);
        listaClientesId.add(cliente);

        cliente = new ClienteDTO();
        cliente.setPrimerapellido("Armas");
        cliente.setSegundoapellido("Agurto");
        cliente.setNombre("Rodrigo");
        cliente.setCorreoElectronico("rodrigo@mail.com");
        cliente.setContrasenha("14652");
        cliente.setCelular("920478163");
        cliente.setRol();
        cliente.setUrlFotoPerfil("dsajdjalds");
        cliente.setActivo(1);
        idGenerado = clienteDAO.insertar(cliente);
        cliente.setIdUsuario(idGenerado);
        assertTrue(idGenerado != 0);
        listaClientesId.add(cliente);

        cliente = new ClienteDTO();
        cliente.setPrimerapellido("Silva");
        cliente.setSegundoapellido("Yapo");
        cliente.setNombre("Alvaro");
        cliente.setCorreoElectronico("asyoso@mail.com");
        cliente.setContrasenha("8752");
        cliente.setCelular("924175268");
        cliente.setUrlFotoPerfil("dsajdjalds");
        cliente.setActivo(1);
        cliente.setRol();
        ultimoId = clienteDAO.insertar(cliente);
        cliente.setIdUsuario(ultimoId);
        assertTrue(ultimoId != 0);
        listaClientesId.add(cliente);
    }

    @Test
    void testObtenerPorId(){
        System.out.println("obtener por id");
        insertarClientes();
        ClienteDTO cliente = clienteDAO.obtenerPorId(ultimoId);
        assertNotNull(cliente);
        assertEquals(ultimoId, cliente.getIdUsuario());
        eliminarTodo();
    }

    @Test
    void testModificar() {
        System.out.println("modificar");
        insertarClientes();
        listaClientesId.get(2).setCorreoElectronico("holagerman@pucp.edu.pe");
        clienteDAO.modificar(listaClientesId.get(2));

        ClienteDTO cli = clienteDAO.obtenerPorId(ultimoId);
        assertEquals(listaClientesId.get(2).getCorreoElectronico(),cli.getCorreoElectronico());
        eliminarTodo();
    }

    @Test
    void testEliminar() { //Es igual a insertar
        System.out.println("eliminar");
        insertarClientes();
        eliminarTodo();
    }

    private void eliminarTodo() {
        for (ClienteDTO cliente : listaClientesId) {
            Integer resultado = clienteDAO.eliminar(cliente);
            assertNotEquals(0,resultado);
            ClienteDTO cli = clienteDAO.obtenerPorId(cliente.getIdUsuario());
            assertNull(cli);
        }
        listaClientesId.clear();
    }
}
