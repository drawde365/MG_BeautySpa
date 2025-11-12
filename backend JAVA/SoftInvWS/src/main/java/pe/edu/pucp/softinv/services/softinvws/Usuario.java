package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebService;
import pe.edu.pucp.softinv.business.BO.Impl.UsuarioBO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

@WebService(serviceName = "Usuario")
public class Usuario {

    private UsuarioBO usuario;

	public Usuario() {
        usuario = new UsuarioBO();
    }

    @WebMethod(operationName = "IniciarSesion")
    public boolean inicioSesion(@WebParam(name="correoElectronico") String correoElectronico, @WebParam(name="contrasenha") String contrasenha) {
        return usuario.inicioSesion(correoElectronico, contrasenha);
    }

}
    