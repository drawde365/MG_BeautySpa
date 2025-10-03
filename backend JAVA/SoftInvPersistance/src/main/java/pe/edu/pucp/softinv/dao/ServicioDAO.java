package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public interface ServicioDAO {
    public Integer insertar(ServicioDTO servicio);
    public ServicioDTO obtenerPorId(Integer id);
    public Integer modificar(ServicioDTO servicio);
    public Integer eliminar(ServicioDTO servicio);
    ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(Integer servicioID);
}
