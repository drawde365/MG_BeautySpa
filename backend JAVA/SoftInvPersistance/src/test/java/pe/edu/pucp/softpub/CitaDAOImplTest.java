package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.*;
import pe.edu.pucp.softinv.daoImp.*;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.Date;
import java.sql.Time;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@Tag("integration")
@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class CitaDAOImplTest {

    private CitaDAO citaDAO;
    private EmpleadoDAO empleadoDAO;
    private ClienteDAO clienteDAO;
    private ServicioDAO servicioDAO;
    private ServicioEmpleadoDAO servicioEmpleadoDAO;

    private CitaDTO cita;

    public CitaDAOImplTest() {
        citaDAO = new CitaDAOImpl();
        empleadoDAO = new EmpleadoDAOImpl();
        clienteDAO = new ClienteDAOimpl();
        servicioDAO = new ServicioDAOImpl();
        servicioEmpleadoDAO = new ServicioEmpleadoDAOImpl();
        cita = null;
    }

    private EmpleadoDTO crearEmpleado() {
        EmpleadoDTO emp = new EmpleadoDTO();
        emp.setPrimerapellido("PEREZ");
        emp.setSegundoapellido("RODRIGUEZ");
        emp.setNombre("CARLOS");
        emp.setCorreoElectronico("carlos_" + System.currentTimeMillis() + "@test.com");
        emp.setContrasenha("12345");
        emp.setCelular("999888777");
        emp.setUrlFotoPerfil("perfil.jpg");
        emp.setActivo(1);
        emp.setAdmin(true);
        emp.setRol(2);
        emp.setIdUsuario(empleadoDAO.insertar(emp));
        assertTrue(emp.getIdUsuario() > 0, "Empleado no se insertó correctamente.");
        return emp;
    }

    private ClienteDTO crearCliente() {
        ClienteDTO cli = new ClienteDTO();
        cli.setPrimerapellido("RAMIREZ");
        cli.setSegundoapellido("DIAZ");
        cli.setNombre("LUISA");
        cli.setCorreoElectronico("luisa_" + System.currentTimeMillis() + "@test.com");
        cli.setContrasenha("12345");
        cli.setCelular("987654321");
        cli.setUrlFotoPerfil("perfil2.jpg");
        cli.setActivo(1);
        cli.setRol(1);
        cli.setIdUsuario(clienteDAO.insertar(cli));
        assertTrue(cli.getIdUsuario() > 0, "Cliente no se insertó correctamente.");
        return cli;
    }

    private ServicioDTO crearServicio() {
        ServicioDTO s = new ServicioDTO();
        s.setNombre("Limpieza facial");
        s.setDescripcion("Limpieza profunda con productos naturales");
        s.setDuracionHora(1);
        s.setPromedioValoracion(4.1);
        s.setUrlImagen("imagen.jpg");
        s.setTipo("Facial");
        s.setPrecio(80.00);
        s.setActivo(1);
        s.setIdServicio(servicioDAO.insertar(s));
        assertTrue(s.getIdServicio() > 0, "Servicio no se insertó correctamente.");
        return s;
    }

    public CitaDTO insertar() {
        EmpleadoDTO empleado = crearEmpleado();
        ClienteDTO cliente = crearCliente();
        ServicioDTO servicio = crearServicio();
        servicioEmpleadoDAO.insertar(empleado.getIdUsuario(),servicio.getIdServicio());

        CitaDTO c = new CitaDTO();
        c.setEmpleado(empleado);
        c.setCliente(cliente);
        c.setServicio(servicio);
        c.setFecha(Date.valueOf(LocalDate.now()));
        c.setHoraIni(Time.valueOf(LocalTime.of(10, 0)));
        c.setHoraFin(Time.valueOf(LocalTime.of(11, 0)));
        c.setIgv(18.0);
        c.setActivo(1);
        c.setCodigoTransaccion("TX-" + System.currentTimeMillis());

        Integer resultado = citaDAO.insertar(c);
        assertTrue(resultado >= 0, "El resultado debe indicar éxito.");
        c.setId(resultado);
        this.cita = c;
        return c;
    }

    public void eliminar() {
        Integer resultado = citaDAO.eliminar(this.cita);
        assertTrue(resultado >= 0, "Debe indicar éxito.");

        // Limpieza de datos asociados
        servicioEmpleadoDAO.eliminar(this.cita.getEmpleado().getIdUsuario(),this.cita.getServicio().getIdServicio());
        servicioDAO.eliminar(this.cita.getServicio());
        clienteDAO.eliminar(this.cita.getCliente());
        empleadoDAO.eliminar(this.cita.getEmpleado().getIdUsuario());
    }

    @Test
    @Order(1)
    @DisplayName("Insertar una cita")
    void testInsertarCita() {
        CitaDTO c = insertar();
        eliminar();
    }

    @Test
    @Order(2)
    @DisplayName("Modificar una cita")
    void testModificarCita() {
        CitaDTO c = insertar();
        c.setIgv(15.0);
        c.setCodigoTransaccion("TX-MODIFICADA-" + System.currentTimeMillis());
        c.setActivo(0);

        Integer resultado = citaDAO.modificar(c);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito.");
        eliminar();
    }

    @Test
    @Order(3)
    @DisplayName("Listar citas por usuario (empleado o cliente)")
    void testListarCitasPorUsuario() {
        CitaDTO c = insertar();

        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(c.getEmpleado().getIdUsuario());
        empleado.setAdmin(false);
        empleado.setRol(2); // Empleado

        ArrayList<CitaDTO> listaEmpleado = citaDAO.listarCitasPorUsuario(empleado);
        assertNotNull(listaEmpleado, "La lista no debe ser nula.");
        assertFalse(listaEmpleado.isEmpty(), "Debe contener al menos una cita E.");

        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(c.getCliente().getIdUsuario());
        cliente.setRol(1); // Cliente

        ArrayList<CitaDTO> listaCliente = citaDAO.listarCitasPorUsuario(cliente);
        assertNotNull(listaCliente, "La lista no debe ser nula.");
        assertFalse(listaCliente.isEmpty(), "Debe contener al menos una cita C.");

        eliminar();
    }
}
