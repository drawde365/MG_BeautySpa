package pe.edu.pucp.softinv.dao;
import java.util.ArrayList;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;



public interface HorarioTrabajoDAO {

    Integer insertar(HorarioTrabajoDTO horarioTrabajo);

    Integer modificar(HorarioTrabajoDTO horarioTrabajo);

    Integer eliminar(HorarioTrabajoDTO horarioTrabajo);

    ArrayList<HorarioTrabajoDTO> obtenerPorEmpleadoYFecha(Integer empleadoId, Integer diaSemana);
    
    HorarioTrabajoDTO obtenerPorId(Integer horarioId);
    
    ArrayList<HorarioTrabajoDTO> listarPorEmpleado(Integer idEmpleado);

}