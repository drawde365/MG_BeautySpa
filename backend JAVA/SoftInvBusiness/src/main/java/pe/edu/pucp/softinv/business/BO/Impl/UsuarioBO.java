package pe.edu.pucp.softinv.business.BO.Impl;

import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Util.Cifrado;
import static pe.edu.pucp.softinv.business.BO.Util.Cifrado.cifrarMD5;
import static pe.edu.pucp.softinv.business.BO.Util.Cifrado.descifrarMD5;
import pe.edu.pucp.softinv.dao.TokensDAO;
import pe.edu.pucp.softinv.dao.UsuarioDAO;
import pe.edu.pucp.softinv.daoImp.TokensDAOImpl;
import pe.edu.pucp.softinv.daoImp.UsuarioDAOImpl;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;
import pe.edu.pucp.softinv.model.Token.ContrasenhaTokenDTO;

/**
 *
 * @author Alvaro
 */
public class UsuarioBO {

    private UsuarioDAO usuario;
    private final String temp = "NO_ENCONTRADO";
    private TokensDAO token;
    
    public UsuarioBO() {
        usuario = new UsuarioDAOImpl();
        token = new TokensDAOImpl();
    }

    public UsuarioDTO inicioSesion(String correoElectronico, String contrasenha) {
        UsuarioDTO u = usuario.busquedaPorCorreo(correoElectronico);
        String pswCifrado = cifrarMD5(contrasenha);
        if (u == null || !(u.getContrasenha().equals(pswCifrado))) {
             u = new UsuarioDTO(temp, temp, temp, temp, temp, temp, temp, 0,0);
        }
        return u;
    }
    
    public UsuarioDTO buscarUsuarioCorreo(String correoElectronico){
        return usuario.busquedaPorCorreo(correoElectronico);
    }
    
    public Integer actualizarContrasenha(Integer usuarioId,String nuevaPassword){
        return usuario.actualizarContrasenha(usuarioId, nuevaPassword);
    }
    
    public Integer insertarTokenRecuperacion(Integer usuarioId,String token){
        return this.token.insertarTokenRecuperacion(usuarioId, token);
    }
    
    public ContrasenhaTokenDTO obtenerToken(String token){
        return this.token.obtenerToken(token);
    }
    
    public Integer marcarTokenUsado(ContrasenhaTokenDTO token){
        return this.token.marcarTokenUsado(token);
    }
    
    public ArrayList<UsuarioDTO> obtenerUsuarios(){
        return this.usuario.obtenerUsuarios();
    }
    
    public Integer activoUsuario(Integer idUsuario,Integer activo){
        return this.usuario.modificarActivoCliente(idUsuario, activo);
    }
}
