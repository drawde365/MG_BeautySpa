package pe.edu.pucp.softpub;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.dao.ServicioEmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.daoImp.ServicioEmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Tag;

@Tag("integration")
public class ServicioDAOImplTest {
    private static ServicioDAO servicioDAO;
    private static EmpleadoDAO empleadoDAO;
    private static ServicioEmpleadoDAO servicioEmpleadoDAO;
    private EmpleadoDTO empleadoTest;

    @BeforeAll
    static void setUp() {
        servicioDAO = new ServicioDAOImpl();
        empleadoDAO = new EmpleadoDAOImpl();
        servicioEmpleadoDAO = new ServicioEmpleadoDAOImpl();
    }

    void llenarDatos(ServicioDTO servicio){
        servicio.setNombre("ServiceSample");
        servicio.setTipo(TipoServicio.CORPORAL);
        servicio.setPrecio(99.90);
        servicio.setDescripcion("Este es una descripción de prueba. El servicio no es real, mas sí adquirible");
        servicio.setUrlImagen("ejemploURL.com");
        servicio.setDuracionHora(2);
        servicio.setPromedioValoracion(3.4);
        servicio.setActivo(1);
    }
    
    // Helper para crear un empleado y devolverlo
    private EmpleadoDTO creaEmpleadoTest() {
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setPrimerapellido("TEST");
        empleado.setSegundoapellido("EMPLEADO");
        empleado.setNombre("TESTER");
        empleado.setCorreoElectronico("test_empleado_" + System.currentTimeMillis() + "@test.com");
        empleado.setContrasenha("pass123");
        empleado.setCelular("999999999");
        empleado.setRol(2); // Rol de empleado
        empleado.setActivo(1);
        int id = empleadoDAO.insertar(empleado);
        empleado.setIdUsuario(id);
        assertTrue(id > 0);
        empleadoTest = empleado;
        return empleado;
    }

    @Test
    void testInsertarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int id_servicio_insertado = servicioDAO.insertar(servicio);
        servicio.setIdServicio(id_servicio_insertado);
        System.out.println(id_servicio_insertado);
        assertTrue(id_servicio_insertado>0, "El servicio debería insertarse correctamente");
        servicioDAO.eliminar(servicio);
    }

    @Test
    void testObtenerServicioPorId(){
        ServicioDTO servicio = new ServicioDTO();
        ServicioDTO servicioID = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicioID = servicioDAO.obtenerPorId(servicio.getIdServicio());
        assertNotNull(servicioID, "El servicio debe existir en la BD");
        assertEquals("ServiceSample", servicioID.getNombre(), "El nombre debe coincidir");
        servicioDAO.eliminar(servicio);
    }
    
    @Test
    void testModificarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicio.setNombre("ServiceModificado");
        servicio.setPrecio(130.99);
        int result = servicioDAO.modificar(servicio);
        assertTrue(result!=0, "La modificación debería ser exitosa");
        ServicioDTO actualizado = servicioDAO.obtenerPorId(servicio.getIdServicio());
        assertEquals("ServiceModificado", actualizado.getNombre(), "El nombre debe haberse actualizado");
        assertEquals(130.99, actualizado.getPrecio(), 0.001, "El precio debe haberse actualizado");
        servicioDAO.eliminar(servicio);
    }

    @Test
    void testEliminarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicioDAO.eliminar(servicio);
        servicio = servicioDAO.obtenerPorId(resul);
        assertNull(servicio, "El servicio ya no debería existir");
    }
    
    @Test
    void testListarTodos(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        
        ArrayList<ServicioDTO> lista = servicioDAO.listarTodos();
        assertNotNull(lista, "La lista no debe ser nula");
        assertTrue(lista.size()>0, "La lista debe contener elementos");
        
        servicioDAO.eliminar(servicio);
    }
    
    @Test
    void testListarEmpleadosPorServicio(){
        // SETUP: Insertar un servicio y un empleado
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int idServicio = servicioDAO.insertar(servicio);
        servicio.setIdServicio(idServicio);
        
        EmpleadoDTO empleado = creaEmpleadoTest();
        int idEmpleado = empleado.getIdUsuario();

        // ACCIÓN 1: Enlazar el empleado al servicio
        Integer res = servicioEmpleadoDAO.insertar(idEmpleado, idServicio);
        assertTrue(res > 0, "Debe insertarse la relación Empleado-Servicio");
        
        // ACCIÓN 2: Listar empleados para ese servicio
        ArrayList<EmpleadoDTO> lista = servicioEmpleadoDAO.listarEmpleadosDeServicio(idServicio);
        
        // ASSERT: Verificar que el empleado esté en la lista y sea el correcto
        assertNotNull(lista, "La lista no debe ser nula");
        assertEquals(1, lista.size(), "Debe retornar 1 empleado para el servicio insertado");
        assertEquals(idEmpleado, lista.get(0).getIdUsuario(), "El ID del empleado en la lista debe coincidir");
        
        // CLEANUP: Eliminar relación, servicio y empleado
        servicioEmpleadoDAO.eliminar(idEmpleado, idServicio);
        servicioDAO.eliminar(servicio);
        empleadoDAO.eliminar(idEmpleado);
    }
}