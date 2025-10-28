package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public interface ServicioDAO {
    Integer insertar(ServicioDTO servicio);
    ServicioDTO obtenerPorId(Integer id);
    Integer modificar(ServicioDTO servicio);
    Integer eliminar(ServicioDTO servicio);
    ArrayList<ServicioDTO> obtenerPorNombre(String nombre);
    public ArrayList<ServicioDTO> obtenerPorPagina(Integer pag);
    Integer obtenerCantPaginas();
    ArrayList<ServicioDTO> listarTodos();
}
