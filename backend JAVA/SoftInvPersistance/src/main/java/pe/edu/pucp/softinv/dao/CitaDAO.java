package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

import java.util.ArrayList;
import java.util.Date;

public interface CitaDAO {
    Integer insertar(CitaDTO cita);
    CitaDTO obtenerPorId(CitaDTO idCita);
    Integer modificar(CitaDTO cita);
    Integer eliminar(CitaDTO cita);
    ArrayList<CitaDTO> listarCitasPorUsuario(UsuarioDTO usuario);
    ArrayList<CitaDTO> listarTodos();
    ArrayList<CitaDTO> listarCitasPorEmpleadoYFecha(Integer empleadoId, Date fecha);
}
