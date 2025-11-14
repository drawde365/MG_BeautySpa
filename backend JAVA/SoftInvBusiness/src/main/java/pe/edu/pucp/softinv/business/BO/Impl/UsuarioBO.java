package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.daoImp.UsuarioDAOImpl;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author Alvaro
 */
public class UsuarioBO {

    private UsuarioDAOImpl usuario;
    private final String temp = "NO_ENCONTRADO";

    public UsuarioBO() {
        usuario = new UsuarioDAOImpl();
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
}
