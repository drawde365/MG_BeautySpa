package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Disponibilidad.DisponibilidadDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;



public interface DisponibilidadDAO {

    public Integer insertar(DisponibilidadDTO disponibilidad);
    public Integer modificar(DisponibilidadDTO disponibilidad);
    public Integer eliminar(DisponibilidadDTO disponibilidad);
}
