package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.daoImp.UsuarioDAOImpl;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author Alvaro
 */
public class UsuarioBO {

    private UsuarioDAOImpl usuario;
    private String temp = "NO_ENCONTRADO";

    public UsuarioBO() {
        usuario = new UsuarioDAOImpl();
    }

    public UsuarioDTO inicioSesion(String correoElectronico, String contrasenha) {
        UsuarioDTO u = usuario.busquedaPorCorreo(correoElectronico);
        if (u == null) {
             u = new UsuarioDTO(temp, temp, temp, temp, temp, temp, temp, 0);
        }
        return u;
    }

}
