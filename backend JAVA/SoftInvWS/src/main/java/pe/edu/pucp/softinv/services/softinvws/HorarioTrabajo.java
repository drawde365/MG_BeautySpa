package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.sql.Time;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.HorarioTrabajoBO;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;

@WebService(serviceName = "HorarioTrabajo")
public class HorarioTrabajo {
    
    private HorarioTrabajoBO horarioTrabajoBO;
    
    public HorarioTrabajo() {
        horarioTrabajoBO = new HorarioTrabajoBO();
    }
    
    @WebMethod(operationName = "InsertarHorarioTrabajo")
    public Integer insertarHorarioTrabajo(
            @WebParam(name = "horarioTrabajo") HorarioTrabajoDTO horarioTrabajo) {
        return horarioTrabajoBO.insertar(horarioTrabajo);
    }
    
    @WebMethod(operationName = "ObtenerHorariosPorEmpleadoYFecha")
    public ArrayList<HorarioTrabajoDTO> obtenerHorariosPorEmpleadoYFecha(
            @WebParam(name = "empleadoId") Integer empleadoId,
            @WebParam(name = "diaSemana") Integer diaSemana) {
        return horarioTrabajoBO.obtenerPorEmpleadoYFecha(empleadoId, diaSemana);
    }
    
    @WebMethod(operationName = "ObtenerHorarioTrabajoPorId")
    public HorarioTrabajoDTO obtenerHorarioTrabajoPorId(
            @WebParam(name = "horarioId") Integer horarioId) {
        return horarioTrabajoBO.obtenerPorId(horarioId);
    }
    
    @WebMethod(operationName = "ModificarHorarioTrabajo")
    public Integer modificarHorarioTrabajo(
            @WebParam(name = "horarioTrabajo") HorarioTrabajoDTO horarioTrabajo) {
        return horarioTrabajoBO.modificar(horarioTrabajo);
    }
    
    @WebMethod(operationName = "EliminarHorarioTrabajo")
    public Integer eliminarHorarioTrabajo(
            @WebParam(name = "horarioTrabajo") HorarioTrabajoDTO horarioTrabajo) {
        return horarioTrabajoBO.eliminar(horarioTrabajo);
    }
    
    @WebMethod(operationName = "ListarPorEmpleado")
    public ArrayList<HorarioTrabajoDTO> listarPorEmpleado(
            @WebParam(name = "empleadoId") Integer empleadoId) {
        return horarioTrabajoBO.listarPorEmpleado(empleadoId);
    }
}