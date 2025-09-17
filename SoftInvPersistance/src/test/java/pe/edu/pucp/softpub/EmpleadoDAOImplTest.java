package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

//TEST FUNCIONAL!!

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class EmpleadoDAOImplTest {

    private static EmpleadoDAO empleadoDAO;
    private static int empleadoIdInsertado;

    @BeforeAll
    static void setUp() {
        empleadoDAO = new EmpleadoDAOImpl();
    }

    @Test
    @Order(1)
    void testInsertarEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PREZ");
        emp.setSegundoapellido("LOPEZ");
        emp.setNombre("JUAN");
        emp.setCorreoElectronico("jUANhbsafaf@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("9998777");
        emp.setUrlFotoPerfil("peril.jpg");
        emp.setAdmin(true);
        emp.setRol();

        empleadoIdInsertado = empleadoDAO.insertar(emp);
        assertNotEquals(0, empleadoIdInsertado, "El empleado debe tener un ID asignado");
    }

    @Test
    @Order(2)
    void testObtenerEmpleadoPorId() {
        System.out.println("obtenerPorId");

        EmpleadoDTO nuevo = new EmpleadoDTO();
        nuevo.setNombre("Ana");
        nuevo.setPrimerapellido("Ramirez");
        nuevo.setSegundoapellido("Flores");
        nuevo.setCorreoElectronico("ana.ramirez@empra.com");
        nuevo.setCelular("999111222");
        nuevo.setContrasenha("clave123");
        nuevo.setRol("EMPLEADO");
        nuevo.setUrlFotoPerfil("fotoAna.jpg");

        Integer idGenerado = empleadoDAO.insertar(nuevo);
        assertNotEquals(0, idGenerado, "El ID generado no debe ser 0");
        nuevo.setIdUsuario(idGenerado);

        EmpleadoDTO emp = empleadoDAO.obtenerPorId(idGenerado);
        assertNotNull(emp, "El empleado debe existir en la BD");

        assertEquals("Ana", emp.getNombre());
        assertEquals("Ramirez", emp.getPrimerapellido());
        assertEquals("Flores", emp.getSegundoapellido());
        assertEquals("ana.ramirez@empra.com", emp.getCorreoElectronico());
        assertEquals("999111222", emp.getCelular());
    }


    @Test
    @Order(3)
    void testListarTodo() {
        ArrayList<EmpleadoDTO> empleados = empleadoDAO.listarTodos();
        assertNotNull(empleados, "La lista no debe ser null");
        assertFalse(empleados.isEmpty(), "La lista no debe estar vacía");

        empleados.forEach(e ->
                System.out.println(e.getIdUsuario() + " - " + e.getNombre() + " " + e.getPrimerapellido())
        );
    }

    @Test
    @Order(4)
    void testModificarEmpleado() {
        EmpleadoDTO emp = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        emp.setNombre("JUAN MODIFICADO");

        int result = empleadoDAO.modificar(emp);

        EmpleadoDTO actualizado = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        assertEquals("JUAN MODIFICADO", actualizado.getNombre(), "El nombre debe haberse actualizado");
    }

    @Test
    @Order(5)
    void testEliminarEmpleado() {
        int result = empleadoDAO.eliminar(empleadoIdInsertado);
        //assertEquals(1, result, "El empleado debería eliminarse correctamente");

        EmpleadoDTO eliminado = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        assertNull(eliminado, "El empleado ya no debería existir");
    }
}
