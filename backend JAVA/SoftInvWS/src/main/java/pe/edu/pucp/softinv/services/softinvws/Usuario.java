package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebService;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.UsuarioBO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;
import pe.edu.pucp.softinv.model.Token.ContrasenhaTokenDTO;

/**
 *
 * @author Alvaro
 */

@WebService(serviceName = "Usuario")
public class Usuario {

    private UsuarioBO usuario;

    public Usuario() {
        usuario = new UsuarioBO();
    }

    @WebMethod(operationName = "IniciarSesion")
    public UsuarioDTO inicioSesion(@WebParam(name="correoElectronico") String correoElectronico, @WebParam(name="contrasenha") String contrasenha) {
        return usuario.inicioSesion(correoElectronico, contrasenha);
    }
    
    @WebMethod(operationName = "ObtenerUsuario")
    public UsuarioDTO buscarUsuarioCorreo(@WebParam(name = "correoElectronico") String correoElectronico){
        return usuario.buscarUsuarioCorreo(correoElectronico);
    }
    
    @WebMethod(operationName = "ModificarContrasenha")
    public Integer actualizarContrasenha(@WebParam(name = "usuarioId") Integer usuarioId,@WebParam(name = "nuevaContrasenha") String nuevaPassword){
        return usuario.actualizarContrasenha(usuarioId, nuevaPassword);
    }
    
    @WebMethod(operationName = "RegistrarToken")
    public Integer insertarTokenRecuperacion(@WebParam(name = "usuarioId") Integer usuarioId,@WebParam(name = "token") String token){
        return usuario.insertarTokenRecuperacion(usuarioId, token);
    }
    
    @WebMethod(operationName = "ObtenerTokenDelUsuario")
    public ContrasenhaTokenDTO obtenerToken(@WebParam(name = "token") String token){
        return usuario.obtenerToken(token);
    }
    
    @WebMethod(operationName = "MarcarTokenComoUsado")
    public Integer marcarTokenUsado(@WebParam(name = "token") ContrasenhaTokenDTO token){
        return usuario.marcarTokenUsado(token);
    }
    
    @WebMethod(operationName = "ListarUsuarios")
    public ArrayList<UsuarioDTO> obtenerUsuarios(){
        return usuario.obtenerUsuarios();
    }
    
    @WebMethod(operationName = "ActivoUsuario")
    public Integer activoUsuario(@WebParam(name = "userId") Integer idUsuario,@WebParam(name = "activoSN") Integer activo){
        return usuario.activoUsuario(idUsuario, activo);
    }
}
    