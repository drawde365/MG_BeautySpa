package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Cita.CitaDTO;

import java.util.ArrayList;
import java.util.Date;

public interface CitaDAO {
    public Integer insertar(CitaDTO cita);
    public CitaDTO obtenerPorId(CitaDTO idCita);
    public Integer modificar(CitaDTO cita);
    public Integer eliminar(CitaDTO cita);
    ArrayList<CitaDTO> listarCitasPorPeriodo(Date  inicio, Date fin);
}
