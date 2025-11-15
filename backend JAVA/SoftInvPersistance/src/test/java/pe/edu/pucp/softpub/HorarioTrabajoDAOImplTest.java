package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.HorarioTrabajoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;
import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@Tag("integration")
@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class HorarioTrabajoDAOImplTest {

    private EmpleadoDAO empleadoDAO;
    private HorarioTrabajoDAO horarioTrabajoDAO;
    private ArrayList<HorarioTrabajoDTO> horariosInsertados;
    private EmpleadoDTO empleadoGlobal;

    public HorarioTrabajoDAOImplTest() {
        empleadoDAO = new EmpleadoDAOImpl();
        horarioTrabajoDAO = new HorarioTrabajoDAOImpl();
        horariosInsertados = new ArrayList<>();
        empleadoGlobal = null;
    }

    private EmpleadoDTO creaEmpleado() {
        if (empleadoGlobal != null) {
            return empleadoGlobal;
        }
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
        emp.setRol(3);
        emp.setIdUsuario(empleadoDAO.insertar(emp));

        assertTrue(emp.getIdUsuario() > 0, "El empleado no se insertó correctamente");
        empleadoGlobal = emp;
        return emp;
    }

    public void insertarMultiplesHorarios() {
        EmpleadoDTO empleado = creaEmpleado();

        HorarioTrabajoDTO horario1 = new HorarioTrabajoDTO();
        horario1.setEmpleado(empleado);
        horario1.setDiaSemana(2); 
        horario1.setHoraInicio(Time.valueOf("08:00:00"));
        horario1.setHoraFin(Time.valueOf("12:00:00"));
        int id1 = horarioTrabajoDAO.insertar(horario1);
        horario1.setId(id1);
        horariosInsertados.add(horario1);

        HorarioTrabajoDTO horario2 = new HorarioTrabajoDTO();
        horario2.setEmpleado(empleado);
        horario2.setDiaSemana(2); 
        horario2.setHoraInicio(Time.valueOf("14:00:00"));
        horario2.setHoraFin(Time.valueOf("18:00:00"));
        int id2 = horarioTrabajoDAO.insertar(horario2);
        horario2.setId(id2);
        horariosInsertados.add(horario2);
    }

    public void eliminar() {
        for (HorarioTrabajoDTO horario : horariosInsertados) {
            Integer resultado = horarioTrabajoDAO.eliminar(horario);
            assertNotNull(resultado, "El resultado no debe ser nulo");
            assertTrue(resultado >= 0, "Debe indicar éxito");
        }
        horariosInsertados.clear();
        
        if (empleadoGlobal != null) {
            empleadoDAO.eliminar(empleadoGlobal.getIdUsuario());
            empleadoGlobal = null;
        }
    }

    @Test
    @DisplayName("Insertar un horario de trabajo")
    void testInsertarHorarioTrabajo() {
        insertarMultiplesHorarios();
        eliminar();
    }

    @Test
    @DisplayName("Obtener horarios por Empleado y Día")
    void testObtenerPorEmpleadoYFecha() {
        insertarMultiplesHorarios();
        
        HorarioTrabajoDTO primerHorario = horariosInsertados.get(0);

        ArrayList<HorarioTrabajoDTO> obtenidos = horarioTrabajoDAO.obtenerPorEmpleadoYFecha(
                primerHorario.getEmpleado().getIdUsuario(),
                primerHorario.getDiaSemana()
        );

        assertNotNull(obtenidos, "La lista no debe ser nula");
        assertEquals(2, obtenidos.size(), "Debe retornar 2 horarios para el Martes");
        assertEquals(primerHorario.getEmpleado().getIdUsuario(), obtenidos.get(0).getEmpleado().getIdUsuario());
        assertEquals(primerHorario.getDiaSemana(), obtenidos.get(0).getDiaSemana());

        eliminar();
    }

    @Test
    @DisplayName("Modificar un horario de trabajo")
    void testModificarHorarioTrabajo() {
        insertarMultiplesHorarios();
        
        HorarioTrabajoDTO horarioAModificar = horariosInsertados.get(0);
        
        Time nuevaHoraInicio = Time.valueOf("09:00:00");
        Time nuevaHoraFin = Time.valueOf("13:00:00");
        
        horarioAModificar.setHoraInicio(nuevaHoraInicio);
        horarioAModificar.setHoraFin(nuevaHoraFin);

        Integer resultado = horarioTrabajoDAO.modificar(horarioAModificar);
        assertNotNull(resultado, "El resultado no debe ser nulo");
        assertTrue(resultado >= 0, "Debe indicar éxito");

        HorarioTrabajoDTO actualizado = horarioTrabajoDAO.obtenerPorId(horarioAModificar.getId());

        assertEquals(nuevaHoraInicio, actualizado.getHoraInicio());
        assertEquals(nuevaHoraFin, actualizado.getHoraFin());

        eliminar();
    }

    @Test
    @DisplayName("Listar todos los horarios de un empleado")
    void testListarPorEmpleado() {
        insertarMultiplesHorarios();
        
        ArrayList<HorarioTrabajoDTO> lista = horarioTrabajoDAO.listarPorEmpleado(empleadoGlobal.getIdUsuario());
        
        assertNotNull(lista, "La lista no debe ser nula");
        assertEquals(2, lista.size(), "Debe devolver los 2 horarios insertados");
        
        eliminar();
    }
}