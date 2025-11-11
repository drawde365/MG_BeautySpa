package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.daoImp.UsuarioDAOImpl;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

public class UsuarioBO {

    private UsuarioDAOImpl usuario;

    public UsuarioBO() {
        usuario = new UsuarioDAOImpl();
    }
    
    public boolean inicioSesion(String correoElectronico, String contrasenha){
        UsuarioDTO u = usuario.busquedaPorCorreo(correoElectronico);
        if(u!=null){
            //verificar contraseña
            if(u.getContrasenha().equals(contrasenha)){
                return true;
            }
        }
        return false;
    }

}
