package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.HorarioTrabajoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class HorarioTrabajoDAOImplTest {

    private EmpleadoDAO empleadoDAO;
    private HorarioTrabajoDAOImpl horarioTrabajoDAO;
    private HorarioTrabajoDTO horarioTrabajo;

    public HorarioTrabajoDAOImplTest() {
        empleadoDAO = new EmpleadoDAOImpl();
        horarioTrabajoDAO = new HorarioTrabajoDAOImpl();
        horarioTrabajo = null;
    }

    private EmpleadoDTO creaEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PEREZ");
        emp.setSegundoapellido("LOPEZ");
        emp.setNombre("MARIO");
        emp.setCorreoElectronico("mario_" + System.currentTimeMillis() + "@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("987654321");
        emp.setUrlFotoPerfil("perfil.jpg");
        emp.setActivo(1);
        emp.setAdmin(true);
        emp.setRol();
        emp.setIdUsuario(empleadoDAO.insertar(emp));

        assertTrue(emp.getIdUsuario() > 0, "El empleado no se insertó correctamente");
        return emp;
    }

    public HorarioTrabajoDTO insertar() {
        EmpleadoDTO empleado = creaEmpleado();

        HorarioTrabajoDTO horario = new HorarioTrabajoDTO();
        horario.setEmpleado(empleado);
        horario.setDiaSemana(2); // Lunes = 1, Martes = 2
        horario.setHoraInicio(Time.valueOf("08:00:00"));
        horario.setHoraFin(Time.valueOf("17:00:00"));
        horario.setIntervalos(2);

        horarioTrabajoDAO.insertar(horario);
        this.horarioTrabajo = horario;
        return horario;
    }

    public void eliminar() {
        EmpleadoDTO emp = this.horarioTrabajo.getEmpleado();
        Integer resultado = horarioTrabajoDAO.eliminar(this.horarioTrabajo);
        empleadoDAO.eliminar(emp.getIdUsuario());

        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "Debe indicar éxito");
    }

    @Test
    @Order(1)
    @DisplayName("Insertar un horario de trabajo")
    void testInsertarHorarioTrabajo() {
        HorarioTrabajoDTO horario = insertar();
        eliminar();
    }

    @Test
    @Order(2)
    @DisplayName("Obtener un horario de trabajo por ID")
    void testObtenerPorId() {
        HorarioTrabajoDTO horario = insertar();

        HorarioTrabajoDTO obtenido = horarioTrabajoDAO.obtenerPorId(
                horario.getEmpleado().getIdUsuario(),
                horario.getDiaSemana()
        );

        assertNotNull(obtenido, "Debe retornar un objeto horario");
        assertEquals(horario.getEmpleado().getIdUsuario(), obtenido.getEmpleado().getIdUsuario());
        assertEquals(horario.getDiaSemana(), obtenido.getDiaSemana());

        eliminar();
    }

    @Test
    @Order(3)
    @DisplayName("Modificar un horario de trabajo")
    void testModificarHorarioTrabajo() {
        HorarioTrabajoDTO horario = insertar();
        horario.setHoraInicio(Time.valueOf("09:00:00"));
        horario.setHoraFin(Time.valueOf("18:00:00"));
        horario.setIntervalos(3);

        Integer resultado = horarioTrabajoDAO.modificar(horario);
        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "Debe indicar éxito");

        HorarioTrabajoDTO actualizado = horarioTrabajoDAO.obtenerPorId(horario.getEmpleado().getIdUsuario(),
                horario.getDiaSemana()
        );

        assertEquals(Time.valueOf("09:00:00"), actualizado.getHoraInicio());
        assertEquals(Time.valueOf("18:00:00"), actualizado.getHoraFin());
        assertEquals(3, actualizado.getIntervalos());

        eliminar();
    }
}
