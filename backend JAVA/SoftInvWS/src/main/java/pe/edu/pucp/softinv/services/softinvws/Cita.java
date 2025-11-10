package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.sql.Time;
import java.sql.Date;
import pe.edu.pucp.softinv.business.BO.Impl.CitaBO;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;

/**
 *
 * @author softinv
 */
@WebService(serviceName = "Cita")
public class Cita {
    
    private CitaBO citaBO;
    
    public Cita() {
        citaBO = new CitaBO();
    }
    
    @WebMethod(operationName = "InsertarCitaPorPartes")
    public Integer insertarCita(
            @WebParam(name = "empleadoId") Integer empleadoId,
            @WebParam(name = "clienteId") Integer clienteId,
            @WebParam(name = "servicioId") Integer servicioId,
            @WebParam(name = "fecha") Date fecha,
            @WebParam(name = "horaIni") Time horaIni,
            @WebParam(name = "horaFin") Time horaFin,
            @WebParam(name = "igv") Double igv,
            @WebParam(name = "activo") Integer activo,
            @WebParam(name = "codTransacc") String codTransacc) {
        
        return citaBO.insertar(empleadoId, clienteId, servicioId, fecha, horaIni, horaFin, igv, activo, codTransacc);
    }
    
    @WebMethod(operationName = "ModificarCita")
    public Integer modificarCita(
            @WebParam(name = "cita") CitaDTO cita) {
        return citaBO.modificar(cita);
    }
    
    @WebMethod(operationName = "EliminarCita")
    public Integer eliminarCita(
            @WebParam(name = "cita") CitaDTO cita) {
        return citaBO.eliminar(cita);
    }
    
    @WebMethod(operationName = "ObtenerCitaPorId")
    public CitaDTO obtenerCitaPorId(
            @WebParam(name = "citaBusqueda") CitaDTO citaBusqueda) {
        return citaBO.obtenerPorId(citaBusqueda);
    }
}