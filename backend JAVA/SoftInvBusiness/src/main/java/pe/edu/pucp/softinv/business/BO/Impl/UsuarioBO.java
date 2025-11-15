package pe.edu.pucp.softinv.business.BO.Impl;

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
        if (u == null || !(contrasenha.equals(u.getContrasenha()))) {
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
}
