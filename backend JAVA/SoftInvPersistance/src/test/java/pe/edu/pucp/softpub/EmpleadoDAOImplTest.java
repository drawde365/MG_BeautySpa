package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class EmpleadoDAOImplTest {

    private EmpleadoDAOImpl empleadoDAO;
    private EmpleadoDTO empleado;

    public EmpleadoDAOImplTest() {
        empleadoDAO = new EmpleadoDAOImpl();
        empleado = null;
    }

    private EmpleadoDTO crearEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("SALAZAR");
        emp.setSegundoapellido("ROJAS");
        emp.setNombre("PEDRO");
        emp.setCorreoElectronico("pedro_" + System.currentTimeMillis() + "@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("999888777");
        emp.setUrlFotoPerfil("perfil.jpg");
        emp.setActivo(1);
        emp.setAdmin(false);
        emp.setRol(2); // ajusta automáticamente el rol según admin
        emp.setIdUsuario(empleadoDAO.insertar(emp));
        assertTrue(emp.getIdUsuario() > 0, "Empleado no se insertó correctamente.");
        return emp;
    }

    public EmpleadoDTO insertar() {
        EmpleadoDTO e = crearEmpleado();
        this.empleado = e;
        return e;
    }

    public void eliminar() {
        Integer resultado = empleadoDAO.eliminar(this.empleado.getIdUsuario());
        assertNotNull(resultado, "El resultado de eliminación no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito al eliminar.");
    }

    @Test
    @Order(1)
    @DisplayName("Insertar un empleado")
    void testInsertarEmpleado() {
        EmpleadoDTO emp = insertar();
        assertNotNull(emp.getIdUsuario(), "Debe asignarse un ID al insertar.");
        eliminar();
    }

    @Test
    @Order(2)
    @DisplayName("Modificar un empleado existente")
    void testModificarEmpleado() {
        EmpleadoDTO emp = insertar();

        emp.setNombre("PEDRO LUIS");
        emp.setCorreoElectronico("pedroluis_" + System.currentTimeMillis() + "@test.com");
        emp.setCelular("988777666");
        emp.setActivo(0);

        Integer resultado = empleadoDAO.modificar(emp);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito al modificar.");

        EmpleadoDTO actualizado = empleadoDAO.obtenerPorId(emp.getIdUsuario());
        assertEquals("PEDRO LUIS", actualizado.getNombre());
        assertEquals(0, actualizado.getActivo());

        eliminar();
    }

    @Test
    @Order(3)
    @DisplayName("Obtener empleado por ID")
    void testObtenerPorId() {
        EmpleadoDTO emp = insertar();

        EmpleadoDTO obtenido = empleadoDAO.obtenerPorId(emp.getIdUsuario());
        assertNotNull(obtenido, "Debe retornar un empleado válido.");
        assertEquals(emp.getIdUsuario(), obtenido.getIdUsuario());
        assertEquals(emp.getCorreoElectronico(), obtenido.getCorreoElectronico());

        eliminar();
    }

    @Test
    @Order(4)
    @DisplayName("Listar todos los empleados")
    void testListarTodos() {
        EmpleadoDTO emp = insertar();

        ArrayList<EmpleadoDTO> lista = empleadoDAO.listarTodos();
        assertNotNull(lista, "La lista no debe ser nula.");
        assertFalse(lista.isEmpty(), "Debe contener al menos un empleado.");
        assertTrue(lista.stream().anyMatch(e -> e.getIdUsuario().equals(emp.getIdUsuario())),
                "El empleado insertado debe estar en la lista.");

        eliminar();
    }

    @Test
    @Order(5)
    @DisplayName("Eliminar empleado")
    void testEliminarEmpleado() {
        EmpleadoDTO emp = insertar();
        Integer resultado = empleadoDAO.eliminar(emp.getIdUsuario());
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito.");
    }
}
