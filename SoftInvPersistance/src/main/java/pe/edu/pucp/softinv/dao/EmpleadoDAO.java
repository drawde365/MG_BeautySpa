package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.List;

public interface EmpleadoDAO {
    int insertar(EmpleadoDTO empleado);
    int modificar(EmpleadoDTO empleado);
    int eliminar(int empleadoId);
    EmpleadoDTO obtenerPorId(int empleadoId);
    List<EmpleadoDTO> listarTodos();
}