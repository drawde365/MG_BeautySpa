package pe.edu.pucp.softpub;
import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.CalendarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Date;
import java.time.LocalDate;

import static org.junit.jupiter.api.Assertions.*;

@Tag("integration")
@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class CalendarioDAOImplTest {
    private EmpleadoDAO empleadoDAO;
    private CalendarioDAOImpl calendarioDAO;
    CalendarioDTO calendario;

    CalendarioDAOImplTest (){
        calendarioDAO = new CalendarioDAOImpl();
        empleadoDAO = new EmpleadoDAOImpl();
        calendario = null;
    }

    private EmpleadoDTO creaEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PREZ");
        emp.setSegundoapellido("LOPEZ");
        emp.setNombre("JUAN");
        emp.setCorreoElectronico("jUANhbsafaf@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("9998777");
        emp.setUrlFotoPerfil("peril.jpg");
        emp.setActivo(1);
        emp.setAdmin(true);
        emp.setRol(2);
        emp.setIdUsuario(empleadoDAO.insertar(emp));
        return emp;
    }

    public CalendarioDTO insertar() {
        // Creamos un empleado simulado
        EmpleadoDTO empleado = this.creaEmpleado();
        CalendarioDTO calendario;
        // Creamos el calendario de prueba
        calendario = new CalendarioDTO();
        calendario.setEmpleado(empleado);
        calendario.setFecha(Date.valueOf(LocalDate.of(2025, 10, 8)));
        calendario.setCantLibre(2);
        calendario.setMotivo(null);
        Integer resultado = calendarioDAO.insertar(calendario);
        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "El resultado debe indicar éxito");
        this.calendario=calendario;
        return calendario;
    }

    public void eliminar() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp=this.calendario.getEmpleado();
        Integer resultado = calendarioDAO.eliminar(this.calendario);
        empleadoDAO.eliminar(emp.getIdUsuario());
        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "Debe indicar éxito");
    }

    @Test
    @Order(1)
    @DisplayName("Insertar un calendario")
    void testInsertarCalendario() {
        CalendarioDTO calendario=insertar();
        eliminar();
    }

    @Test
    @Order(2)
    @DisplayName("Obtener un calendario por ID")
    void testObtenerPorId() {
        CalendarioDTO calendario=insertar();
        CalendarioDTO obtenido = calendarioDAO.obtenerPorId(
                calendario.getEmpleado().getIdUsuario(),
                calendario.getFecha()
        );

        eliminar();
    }

    @Test
    @Order(3)
    @DisplayName("Modificar un calendario")
    void testModificarCalendario() {
        CalendarioDTO calendario = insertar();
        calendario.setCantLibre(1);
        calendario.setMotivo("Cambio de turno");

        Integer resultado = calendarioDAO.modificar(calendario);
        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "Debe indicar éxito");

        CalendarioDTO actualizado = calendarioDAO.obtenerPorId(
                calendario.getEmpleado().getIdUsuario(),
                calendario.getFecha()
        );

        assertEquals("Cambio de turno", actualizado.getMotivo());
        assertEquals(1, actualizado.getCantLibre());
        eliminar();
    }

}
