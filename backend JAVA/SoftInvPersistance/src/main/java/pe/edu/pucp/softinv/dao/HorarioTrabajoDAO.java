package pe.edu.pucp.softinv.dao;
import java.util.ArrayList;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;



public interface HorarioTrabajoDAO {

    Integer insertar(HorarioTrabajoDTO horarioTrabajo);

    Integer modificar(HorarioTrabajoDTO horarioTrabajo);

    Integer eliminar(HorarioTrabajoDTO horarioTrabajo);

    HorarioTrabajoDTO obtenerPorId(Integer empleadoId, Integer diaSemana);
    
    ArrayList<HorarioTrabajoDTO> listarPorEmpleado(Integer idEmpleado);

}