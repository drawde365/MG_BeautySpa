package pe.edu.pucp.softinv.model;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

public class ClienteBO {

    private ClienteDAO clienteDAO;

    public ClienteBO(){
        clienteDAO = new ClienteDAOimpl();
    }

    public Integer Insertar(String nombre, String Primerapellido, String Segundoapellido,String correoElectronico, String contrasenha,
                            String celular, String urlFotoPerfil){
        ClienteDTO clienteDTO = new ClienteDTO();
        clienteDTO.setNombre(nombre);
        clienteDTO.setPrimerapellido(Primerapellido);
        clienteDTO.setSegundoapellido(Segundoapellido);
        clienteDTO.setCorreoElectronico(correoElectronico);
        clienteDTO.setContrasenha(contrasenha);
        clienteDTO.setCelular(celular);
        clienteDTO.setUrlFotoPerfil(urlFotoPerfil);
        clienteDTO.setRol();
        clienteDTO.setActivo(1);
        return clienteDAO.insertar(clienteDTO);
    }

    public Integer Modificar(Integer idUsuario, String nombre, String Primerapellido, String Segundoapellido,String correoElectronico,
                             String contrasenha, String celular, String urlFotoPerfil){
        ClienteDTO clienteDTO = new ClienteDTO();
        clienteDTO.setIdUsuario(idUsuario);
        clienteDTO.setNombre(nombre);
        clienteDTO.setPrimerapellido(Primerapellido);
        clienteDTO.setSegundoapellido(Segundoapellido);
        clienteDTO.setCorreoElectronico(correoElectronico);
        clienteDTO.setContrasenha(contrasenha);
        clienteDTO.setCelular(celular);
        clienteDTO.setUrlFotoPerfil(urlFotoPerfil);
        clienteDTO.setRol();
        clienteDTO.setActivo(1);
        return clienteDAO.modificar(clienteDTO);
    }

    public Integer Eliminar(Integer idUsuario){
        ClienteDTO clienteDTO = new ClienteDTO();
        clienteDTO.setIdUsuario(idUsuario);
        return  clienteDAO.eliminar(clienteDTO);
    }

    public ClienteDTO ObtenerPorId(Integer idUsuario){
        return  clienteDAO.obtenerPorId(idUsuario);
    }

}
