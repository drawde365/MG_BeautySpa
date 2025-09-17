package pe.edu.pucp.softinv;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
class   EmpleadoDAOImplTest {

    private static EmpleadoDAO empleadoDAO;
    private static int empleadoIdInsertado;

    @BeforeAll
    static void setUp() {
        empleadoDAO = new EmpleadoDAOImpl();
    }

    @Test
    void testInsertarEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PREZ");
        emp.setSegundoapellido("LOPEZ");
        emp.setNombre("JUAN");
        emp.setCorreoElectronico("juaasdn.perez@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("9998777");
        emp.setUrlFotoPerfil("peril.jpg");
        emp.setRol("Empleado");

        int result = empleadoDAO.insertar(emp);
        assertEquals(1, result, "El empleado debería insertarse correctamente");
        //Guardamos el ID para las siguientes pruebas (asumiendo autoincrement en BD)
        List<EmpleadoDTO> empleados = empleadoDAO.listarTodos();
        EmpleadoDTO ultimo = empleados.getLast();
        empleadoIdInsertado = ultimo.getIdUsuario();

        assertNotEquals(0, empleadoIdInsertado, "El empleado debe tener un ID asignado");
    }

    @Test
    void testObtenerEmpleadoPorId() {
        EmpleadoDTO emp = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        assertNotNull(emp, "El empleado debe existir en la BD");
        assertEquals("JUAN", emp.getNombre());

        // Servicios asociados (pueden estar vacíos si no insertaste en EMPLEADOS_SERVICIOS)
        ArrayList<ServicioDTO> servicios = emp.getServicios();
        assertNotNull(servicios, "La lista de servicios no debe ser null");
    }

    @Test
    void testModificarEmpleado() {
        EmpleadoDTO emp = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        emp.setNombre("JUAN MODIFICADO");

        int result = empleadoDAO.modificar(emp);
        assertEquals(1, result, "El empleado debería modificarse correctamente");

        EmpleadoDTO actualizado = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        assertEquals("JUAN MODIFICADO", actualizado.getNombre(), "El nombre debe haberse actualizado");
    }

    @Test
    void testEliminarEmpleado() {
        int result = empleadoDAO.eliminar(empleadoIdInsertado);
        assertEquals(1, result, "El empleado debería eliminarse correctamente");

        EmpleadoDTO eliminado = empleadoDAO.obtenerPorId(empleadoIdInsertado);
        assertNull(eliminado, "El empleado ya no debería existir");
    }
}
