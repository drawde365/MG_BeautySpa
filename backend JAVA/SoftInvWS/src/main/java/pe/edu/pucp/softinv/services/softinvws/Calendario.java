package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.xml.bind.annotation.adapters.XmlJavaTypeAdapter;
import java.util.Date;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;
import pe.edu.pucp.softinv.business.BO.Impl.CalendarioBO;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.util.LocalTimeAdapter;

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
    @WebMethod(operationName = "CalcularBloquesDisponibles")
    @XmlJavaTypeAdapter(LocalTimeAdapter.class)
    public List<LocalTime> calcularBloquesDisponibles(
            @WebParam(name = "empleadoId") Integer empleadoId,
            @WebParam(name = "fecha") Date fecha,
            @WebParam(name = "duracionServicioMinutos") int duracionServicioMinutos) {
        return calendarioBO.calcularBloquesDisponibles(empleadoId, fecha, duracionServicioMinutos);
    }
    
    @WebMethod(operationName = "ReservarBloqueYCita")
    public Integer reservarBloqueYCita(
            @WebParam(name = "cita") CitaDTO cita) {
        return calendarioBO.reservarBloqueYCita(cita);
    }
    @WebMethod(operationName = "InsertarCalendario")
    public Integer insertarCalendario(
            @WebParam(name = "calendario") CalendarioDTO calendario) {
        return calendarioBO.insertar(calendario);
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