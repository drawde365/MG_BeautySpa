package pe.edu.pucp.softinv.model.Personas;

import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public class EmpleadoDTO extends UsuarioDTO{
    private Boolean admin;
    private ArrayList<ServicioDTO> servicios;

    public ArrayList<ServicioDTO> getServicios() {
        return servicios;
    }

    public void setServicios(ArrayList<ServicioDTO> servicios) {
        this.servicios = servicios;
    }

    public EmpleadoDTO() {
        super();
        admin=null;
        servicios=null;
    }

    public EmpleadoDTO(String nombre, String PrimerApellido, String SegundoApellido, String correoElectronico,
                       String contrasenha, String celular,Integer idUsuario, Boolean admin,String urlFotoPerfil,ArrayList<ServicioDTO> servicios) {
        super(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular, urlFotoPerfil, idUsuario);
        this.servicios=servicios;
    }

    public Boolean isAdmin() {
        return admin;
    }

    public void setAdmin(Boolean admin) {
        this.admin = admin;
    }

    public void setRol(){
        super.setRol(2);
    }
}
