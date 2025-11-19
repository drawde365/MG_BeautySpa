package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.EmpleadoBO;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

/**
 *
 * @author softinv
 */
@WebService(serviceName = "Empleado")
public class Empleado {
    
    private EmpleadoBO empleadoBO;
    
    public Empleado() {
        empleadoBO = new EmpleadoBO();
    }
    
    @WebMethod(operationName = "InsertarEmpleadoPorPartes")
    public Integer insertarEmpleadoPorPartes(
            @WebParam(name = "nombre") String nombre,
            @WebParam(name = "Primerapellido") String Primerapellido,
            @WebParam(name = "Segundoapellido") String Segundoapellido,
            @WebParam(name = "correoElectronico") String correoElectronico,
            @WebParam(name = "contrasenha") String contrasenha,
            @WebParam(name = "celular") String celular,
            @WebParam(name = "urlFotoPerfil") String urlFotoPerfil,
            @WebParam(name = "admin") Boolean admin) {
        
        return empleadoBO.insertar(nombre, Primerapellido, Segundoapellido, correoElectronico, contrasenha, celular, urlFotoPerfil, admin,2);
    }
    
    @WebMethod(operationName = "InsertarEmpleado")
    public Integer insertarEmpleado(
            @WebParam(name = "empleado") EmpleadoDTO empleado) {
        return empleadoBO.insertar(empleado);
    }
    
    @WebMethod(operationName = "ModificarEmpleado")
    public Integer modificarEmpleado(
            @WebParam(name = "empleado") EmpleadoDTO empleado) {
        return empleadoBO.modificar(empleado);
    }
    
    @WebMethod(operationName = "EliminarEmpleado")
    public Integer eliminarEmpleado(
            @WebParam(name = "empleado") EmpleadoDTO empleado) {
        return empleadoBO.eliminar(empleado);
    }
    
    @WebMethod(operationName = "ObtenerEmpleadoPorId")
    public EmpleadoDTO ObtenerEmpleadoPorId(
            @WebParam(name = "idUsuario") Integer idUsuario) {
        return empleadoBO.ObtenerPorId(idUsuario);
    }
    
    @WebMethod(operationName = "ListarTodosEmpleados")
    public ArrayList<EmpleadoDTO> ListarTodosEmpleados() {
        return empleadoBO.ListarTodos();
    }
    
    @WebMethod(operationName = "AgregarServicioAEmpleado")
    public void agregarServicioAEmpleado(
            @WebParam(name = "empleadoId") Integer empleadoId,
            @WebParam(name = "servicioId") Integer servicioId) {
        empleadoBO.agregarServicio(empleadoId, servicioId);
    }
    
    @WebMethod(operationName = "QuitarServicioAEmpleado")
    public void eliminarServicioEmpleado(@WebParam(name = "empleadoId") Integer empleadoId,
            @WebParam(name = "servicioId") Integer servicioId){
        empleadoBO.eliminarServicio(empleadoId, servicioId);
    }
    
    @WebMethod(operationName = "ListarServiciosDeEmpleado")
    public ArrayList<ServicioDTO> listarServiciosDeEmpleado(
            @WebParam(name = "empleadoId") Integer empleadoId) {
        return empleadoBO.listarServicios(empleadoId);
    }
    
    @WebMethod(operationName = "ListarCitasDeEmpleado")
    public ArrayList<CitaDTO> listarCitasDeEmpleado(
            @WebParam(name = "empleadoId") Integer empleadoId) {
        return empleadoBO.listarCitas(empleadoId);
    }
}