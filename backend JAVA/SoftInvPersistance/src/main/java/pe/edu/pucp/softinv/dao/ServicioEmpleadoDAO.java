package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public interface ServicioEmpleadoDAO {
    public Integer insertar(Integer empleadoId, Integer servicioId);
    public Integer eliminar(Integer empleadoId, Integer servicioId);
    public Integer eliminarLogico(Integer empleadoId, Integer servicioId);
    public ArrayList<ServicioDTO> listarServiciosDeEmpleado(Integer empleadoId);
    public ArrayList<ServicioDTO> listarServiciosNoBrindadosEmpleado(Integer empleadoId);
    public ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(Integer servicioId);
}
