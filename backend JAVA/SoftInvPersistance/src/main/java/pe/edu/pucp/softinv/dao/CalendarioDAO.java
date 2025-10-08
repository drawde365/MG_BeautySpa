package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;

import java.util.ArrayList;
import java.util.Date;

public interface CalendarioDAO {
    Integer insertar(CalendarioDTO calendario);

    Integer modificar(CalendarioDTO calendario);

    Integer eliminar(CalendarioDTO calendario);

    CalendarioDTO obtenerPorId(Integer empleadoId, Date fecha);

    ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(Integer empleadoId);

}
