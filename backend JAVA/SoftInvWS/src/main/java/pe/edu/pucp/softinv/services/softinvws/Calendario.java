package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.sql.Date;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.CalendarioBO;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;

/**
 *
 * @author softinv
 */
@WebService(serviceName = "Calendario")
public class Calendario {
    
    private CalendarioBO calendarioBO;
    
    public Calendario() {
        calendarioBO = new CalendarioBO();
    }
    
    @WebMethod(operationName = "InsertarCalendarioPorPartes")
    public Integer insertarCalendarioPorPartes(
            @WebParam(name = "idEmpleado") Integer idEmpleado,
            @WebParam(name = "fecha") Date fecha,
            @WebParam(name = "cantLibre") Integer cantLibre,
            @WebParam(name = "motivo") String motivo) {
        return calendarioBO.insertar(idEmpleado, fecha, cantLibre, motivo);
    }
    
    @WebMethod(operationName = "InsertarCalendario")
    public Integer insertarCalendario(
            @WebParam(name = "calendario") CalendarioDTO calendario) {
        return calendarioBO.insertar(calendario);
    }
    
    @WebMethod(operationName = "ModificarCalendarioPorPartes")
    public Integer modificarCalendarioPorPartes(
            @WebParam(name = "idEmpleado") Integer idEmpleado,
            @WebParam(name = "fecha") Date fecha,
            @WebParam(name = "cantLibre") Integer cantLibre,
            @WebParam(name = "motivo") String motivo) {
        return calendarioBO.modificar(idEmpleado, fecha, cantLibre, motivo);
    }
    
    @WebMethod(operationName = "ModificarCalendario")
    public Integer modificarCalendario(
            @WebParam(name = "calendario") CalendarioDTO calendario) {
        return calendarioBO.modificar(calendario);
    }
    
    @WebMethod(operationName = "EliminarCalendario")
    public Integer eliminarCalendario(
            @WebParam(name = "calendario") CalendarioDTO calendario) {
        return calendarioBO.eliminar(calendario);
    }
    
    @WebMethod(operationName = "ListarCalendarioDeEmpleado")
    public ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(
            @WebParam(name = "empleadoId") Integer empleadoId) {
        return calendarioBO.listarCalendarioDeEmpleado(empleadoId);
    }
}