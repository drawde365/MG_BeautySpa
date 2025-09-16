package pe.edu.pucp.softinv.model;

import java.util.ArrayList;

public class EmpleadoDTO extends UsuarioDTO{
    private Boolean admin;
    private Double sueldo;
    private ArrayList<ServicioDTO> servicios;
    private ArrayList<CitaDTO> citas;

    public ArrayList<CitaDTO> getCitas() {
        return citas;
    }

    public void setCitas(ArrayList<CitaDTO> citas) {
        this.citas = citas;
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
        sueldo=null;
        servicios=null;
    }

    public EmpleadoDTO(String nombre, String PrimerApellido, String SegundoApellido, String correoElectronico,
                       String contrasenha, String celular, Boolean admin, Double sueldo,String urlFotoPerfil,ArrayList<ServicioDTO> servicios) {
        super(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular, urlFotoPerfil);
        this.admin=admin;
        this.sueldo=sueldo;
        this.servicios=servicios;
    }

    public Boolean isAdmin() {
        return admin;
    }

    public void setAdmin(Boolean admin) {
        this.admin = admin;
    }

    public Double getSueldo() {
        return sueldo;
    }

    public void setSueldo(Double sueldo) {
        this.sueldo = sueldo;
    }

}
