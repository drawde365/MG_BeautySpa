/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.ClienteBO;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

/**
 *
 * @author 00123
 */
@WebService(serviceName = "Cliente")
public class Cliente {
    private ClienteBO clienteBO;
    
    public Cliente(){
        clienteBO = new ClienteBO();
    }
    /**
     * This is a sample web service operation
     */
    @WebMethod(operationName = "crearCliente")
    public Integer insertarCliente(@WebParam(name = "Cliente") ClienteDTO cliente) {
        return clienteBO.insertar(cliente);
    }
    
    @WebMethod(operationName = "modificarDatosCliente")
    public Integer modificarCliente(@WebParam(name="Cliente") ClienteDTO cliente){
        return clienteBO.modificar(cliente);
    }
    
    @WebMethod(operationName = "eliminarCuenta")
    public Integer eliminarCliente(ClienteDTO cliente){
        return clienteBO.eliminar(cliente);
    }
}
