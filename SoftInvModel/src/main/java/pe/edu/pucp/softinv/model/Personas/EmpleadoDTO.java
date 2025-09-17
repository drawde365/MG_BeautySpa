package pe.edu.pucp.softinv.model.Personas;

import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public class EmpleadoDTO extends UsuarioDTO{
    private Boolean admin;
    private ArrayList<ServicioDTO> servicios;

    @Override
    public void setRol(){
        if(admin) rol="Admin";
        else rol="Empleado";
    }

    public void setRol(String rol){
        this.rol=rol;
    }

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
        super(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular, urlFotoPerfil,idUsuario);
        this.admin=admin;
        this.servicios=servicios;
        setRol();
    }

    public Boolean isAdmin() {
        return admin;
    }

    public void setAdmin(Boolean admin) {
        this.admin = admin;
    }

}
