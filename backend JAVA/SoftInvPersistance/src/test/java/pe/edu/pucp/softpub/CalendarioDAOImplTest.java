package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.CalendarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.HorarioTrabajoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Date;
import java.sql.Time;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@Tag("integration")
@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class CalendarioDAOImplTest {
    
    private CalendarioDAOImpl calendarioDAO;
    private EmpleadoDAO empleadoDAO;
    private HorarioTrabajoDAOImpl horarioDAO; // Necesario para configurar el escenario

    public CalendarioDAOImplTest (){
        calendarioDAO = new CalendarioDAOImpl();
        empleadoDAO = new EmpleadoDAOImpl();
        horarioDAO = new HorarioTrabajoDAOImpl();
    }

    // Helper para crear el Empleado necesario para la prueba
    private EmpleadoDTO creaEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PEREZ");
        emp.setSegundoapellido("LOPEZ");
        emp.setNombre("JUAN");
        // Asegura que el correo sea único
        emp.setCorreoElectronico("JUAN" + System.currentTimeMillis() + "@test.com"); 
        emp.setContrasenha("12345");
        emp.setCelular("9998777");
        emp.setUrlFotoPerfil("perfil.jpg");
        emp.setActivo(1);
        emp.setAdmin(true);
        emp.setRol(2);
        int id = empleadoDAO.insertar(emp);
        emp.setIdUsuario(id);
        return emp;
    }

    // Helper para crear un horario de trabajo ficticio (Lunes a Domingo)
    private void crearHorarioTrabajo(EmpleadoDTO empleado) {
        // Creamos horario para los 7 días de la semana para asegurar que se inserten los 30 días
        for (int i = 1; i <= 7; i++) {
            HorarioTrabajoDTO horario = new HorarioTrabajoDTO();
            horario.setEmpleado(empleado); // Asumiendo que el DTO tiene este método
            // horario.setEmpleadoId(empleado.getIdUsuario()); // Alternativa si usa ID directo
            horario.setDiaSemana(i);
            horario.setHoraInicio(Time.valueOf("09:00:00"));
            horario.setHoraFin(Time.valueOf("18:00:00"));
            horario.setNumIntervalo(9); // 8 horas libres
            
            horarioDAO.insertar(horario);
        }
    }

    // Helper para eliminar horarios del empleado (limpieza)
    private void eliminarHorarioTrabajo(Integer empleadoId) {
        ArrayList<HorarioTrabajoDTO> horarios = horarioDAO.listarPorEmpleado(empleadoId);
        for (HorarioTrabajoDTO h : horarios) {
            horarioDAO.eliminar(h);
        }
    }

    // Helper para crear e insertar el registro de calendario
    private CalendarioDTO insertarRegistro() {
        EmpleadoDTO empleado = this.creaEmpleado();
        
        CalendarioDTO calendario = new CalendarioDTO();
        calendario.setEmpleado(empleado);
        calendario.setFecha(Date.valueOf(LocalDate.now().plusDays(1)));
        calendario.setCantLibre(2);
        calendario.setMotivo(null);
        
        Integer resultado = calendarioDAO.insertar(calendario);
        assertTrue(resultado >= 0, "La inserción del calendario debe indicar éxito");
        return calendario;
    }

    // Helper para limpiar el registro insertado y el empleado asociado
    private void eliminarRegistro(CalendarioDTO calendario) {
        if (calendario == null) return;
        
        // 1. Elimina el registro del calendario
        Integer resultado = calendarioDAO.eliminar(calendario);
        
        // 2. Elimina el empleado asociado
        empleadoDAO.eliminar(calendario.getEmpleado().getIdUsuario());
        
        assertTrue(resultado >= 0, "La eliminación del calendario debe indicar éxito");
    }

    @Test
    @Order(1)
    @DisplayName("Insertar un calendario")
    void testInsertarCalendario() {
        CalendarioDTO calendario = insertarRegistro();
        // Verificar inserción antes de eliminar
        CalendarioDTO obtenido = calendarioDAO.obtenerPorId(calendario.getEmpleado().getIdUsuario(), calendario.getFecha());
        assertNotNull(obtenido, "El registro debe poder obtenerse después de la inserción.");
        eliminarRegistro(calendario);
    }

    @Test
    @Order(2)
    @DisplayName("Obtener un calendario por ID (Clave compuesta)")
    void testObtenerPorId() {
        CalendarioDTO esperado = insertarRegistro();
        CalendarioDTO obtenido = calendarioDAO.obtenerPorId(
                esperado.getEmpleado().getIdUsuario(),
                esperado.getFecha()
        );

        assertNotNull(obtenido, "Debe retornar el objeto Calendario.");
        
        assertEquals(esperado.getEmpleado().getIdUsuario(), obtenido.getEmpleado().getIdUsuario(), "El ID de empleado debe coincidir.");
        assertEquals(esperado.getFecha().getTime(), obtenido.getFecha().getTime(), "La fecha debe coincidir.");
        assertEquals(esperado.getCantLibre(), obtenido.getCantLibre(), "La cantidad libre debe coincidir.");
        
        eliminarRegistro(esperado);
    }

    @Test
    @Order(3)
    @DisplayName("Modificar un calendario")
    void testModificarCalendario() {
        CalendarioDTO calendario = insertarRegistro();
        calendario.setCantLibre(1);
        calendario.setMotivo("Cambio de turno");

        Integer resultado = calendarioDAO.modificar(calendario);
        assertTrue(resultado >= 0, "Debe indicar éxito");

        CalendarioDTO actualizado = calendarioDAO.obtenerPorId(
                calendario.getEmpleado().getIdUsuario(),
                calendario.getFecha()
        );

        assertEquals("Cambio de turno", actualizado.getMotivo());
        assertEquals(1, actualizado.getCantLibre());
        
        eliminarRegistro(calendario);
    }
    
    @Test
    @Order(4)
    @DisplayName("Listar calendario de un empleado (Rango de 30 días)")
    void testListarCalendarioDeEmpleado() {
        CalendarioDTO calendario = insertarRegistro(); 
        
        ArrayList<CalendarioDTO> lista = calendarioDAO.listarCalendarioDeEmpleado(calendario.getEmpleado().getIdUsuario());
        
        assertNotNull(lista, "La lista no debe ser nula.");
        assertTrue(lista.size() >= 1, "Debe contener al menos el día insertado.");
        
        // Verificación de la fecha (usando el valor long)
        long fechaEsperadaMillis = calendario.getFecha().getTime();
        boolean fechaEncontrada = lista.stream()
            .anyMatch(c -> c.getFecha().getTime() == fechaEsperadaMillis);
            
        assertTrue(fechaEncontrada, "La fecha insertada debe encontrarse dentro del rango de 30 días.");
        
        eliminarRegistro(calendario);
    }

    @Test
    @Order(5)
    @DisplayName("Eliminar un calendario y verificar que no exista")
    void testEliminarCalendario() {
        CalendarioDTO calendario = insertarRegistro();
        
        Integer resultado = calendarioDAO.eliminar(calendario);
        assertTrue(resultado >= 0, "Debe indicar éxito");

        // Verificación final (el registro debe ser null en la BD)
        CalendarioDTO eliminado = calendarioDAO.obtenerPorId(
                calendario.getEmpleado().getIdUsuario(),
                calendario.getFecha()
        );
        assertNull(eliminado, "El registro debe ser nulo después de la eliminación.");
        
        // Limpiar el empleado
        empleadoDAO.eliminar(calendario.getEmpleado().getIdUsuario());
    }

    @Test
    @Order(6)
    @DisplayName("Insertar y Eliminar disponibilidad de 30 días futuros")
    void testInsertarYEliminar30DiasFuturos() {
        // 1. Crear un empleado nuevo para la prueba
        EmpleadoDTO emp = creaEmpleado();
        
        // 2. Asignar Horario de Trabajo (Necesario porque insertar30DiasFuturos lee de aquí)
        // Si no tiene horario, insertaría 0 registros.
        crearHorarioTrabajo(emp);

        // 3. Ejecutar inserción masiva
        Integer insertados = calendarioDAO.insertar30DiasFuturos(emp.getIdUsuario());
        
        // Verificación: Como asignamos horario para los 7 días, debe haber insertado 30 registros
        assertEquals(30, insertados, "Se deberían haber insertado 30 días de disponibilidad.");
        
        // Verificar que realmente existan en BD
        ArrayList<CalendarioDTO> lista = calendarioDAO.listarCalendarioDeEmpleado(emp.getIdUsuario());
        assertEquals(30, lista.size(), "La lista de calendario debe contener 30 registros.");

        // 4. Ejecutar eliminación masiva
        Integer eliminados = calendarioDAO.eliminar30DiasFuturos(emp.getIdUsuario());
        
        // Verificación de eliminación
        assertEquals(30, eliminados, "Se deberían haber eliminado los 30 días insertados.");
        
        // Verificar que ya no existan
        lista = calendarioDAO.listarCalendarioDeEmpleado(emp.getIdUsuario());
        assertEquals(0, lista.size(), "La lista de calendario debería estar vacía tras la eliminación.");

        // 5. Limpieza final (Horarios y Empleado)
        eliminarHorarioTrabajo(emp.getIdUsuario());
        empleadoDAO.eliminar(emp.getIdUsuario());
    }
}