package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Disponibilidad.DisponibilidadDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;



public interface DisponibilidadDAO {

    Integer insertar(DisponibilidadDTO disponibilidad);

    Integer modificar(DisponibilidadDTO disponibilidad);

    Integer eliminar(DisponibilidadDTO disponibilidad);

    DisponibilidadDTO obtenerPorId(Integer disponibilidadId);

}