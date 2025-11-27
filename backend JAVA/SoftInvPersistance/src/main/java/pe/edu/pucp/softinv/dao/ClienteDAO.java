package pe.edu.pucp.softinv.dao;

import java.util.ArrayList;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

public interface ClienteDAO {
    Integer insertar(ClienteDTO usuario);
    ClienteDTO obtenerPorId(Integer id);
    Integer modificar(ClienteDTO usuario);
    Integer eliminar(ClienteDTO cliente);
    ArrayList<ClienteDTO> buscarClienteAdmin(String nombre,String pApe,String sApe,String correo,String celular);
}
