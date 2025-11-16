package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.ClienteDAOimpl;
import pe.edu.pucp.softinv.daoImp.PedidoDAOimpl;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.util.ArrayList;

public class ClienteBO {
    private PedidoDAO pedidoDAO;
    private ClienteDAO clienteDAO;

    public ClienteBO(){
        clienteDAO = new ClienteDAOimpl();
        pedidoDAO = new PedidoDAOimpl();
    }

    public Integer insertar(String nombre, String Primerapellido, String Segundoapellido,String correoElectronico, String contrasenha,
                            String celular, String urlFotoPerfil){
        ClienteDTO clienteDTO = new ClienteDTO();
        clienteDTO.setNombre(nombre);
        clienteDTO.setPrimerapellido(Primerapellido);
        clienteDTO.setSegundoapellido(Segundoapellido);
        clienteDTO.setCorreoElectronico(correoElectronico);
        clienteDTO.setContrasenha(contrasenha);
        clienteDTO.setCelular(celular);
        clienteDTO.setUrlFotoPerfil(urlFotoPerfil);
        clienteDTO.setRol(1);
        clienteDTO.setActivo(1);
        return clienteDAO.insertar(clienteDTO);
    }

    public Integer insertar(ClienteDTO cliente) {
        return clienteDAO.insertar(cliente);
    }

    public Integer modificar(Integer idUsuario, String nombre, String Primerapellido, String Segundoapellido,String correoElectronico,
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
        clienteDTO.setRol(1);
        clienteDTO.setActivo(1);
        return clienteDAO.modificar(clienteDTO);
    }

    public Integer modificar(ClienteDTO cliente) {
        return clienteDAO.modificar(cliente);
    }

    public Integer eliminar(Integer idUsuario){
        ClienteDTO clienteDTO = clienteDAO.obtenerPorId(idUsuario);
        clienteDTO.setActivo(0);
        return  clienteDAO.modificar(clienteDTO);
    }

    public Integer eliminar(ClienteDTO cliente){
        cliente.setActivo(0);
        return  clienteDAO.modificar(cliente);
    }

    public ClienteDTO obtenerPorId(Integer idUsuario){
        return  clienteDAO.obtenerPorId(idUsuario);
    }

    public ArrayList<PedidoDTO> listarPedidosDeCliente(Integer idCliente) {
        return pedidoDAO.listarPedidos(idCliente);
    }
}
