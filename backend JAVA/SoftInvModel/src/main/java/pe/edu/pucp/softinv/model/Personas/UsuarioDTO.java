package pe.edu.pucp.softinv.model.Personas;

import pe.edu.pucp.softinv.model.Cita.CitaDTO;

import java.util.ArrayList;

/**
 *
 * @author Alvaro
 */

public class UsuarioDTO {
    private String nombre;
    private String Primerapellido;
    private String Segundoapellido;
    private String correoElectronico;
    private String contrasenha;
    private String celular;
    private String urlFotoPerfil;
    private Integer rol;
    private ArrayList<CitaDTO> citas;
    private Integer idUsuario;
    private Integer activo;

    public Integer getActivo() {
        return activo;
    }

    public void setActivo(Integer activo) {
        this.activo = activo;
    }

    public void setRol(int rol){
        this.rol=rol;
    }

    public Integer getRol(){
        return rol;
    }

    public ArrayList<CitaDTO> getCitas() {
        return citas;
    }

    public Integer getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Integer idUsuario) {
        this.idUsuario = idUsuario;
    }

    public String getUrlFotoPerfil() {
        return urlFotoPerfil;
    }

    public void setUrlFotoPerfil(String urlFotoPerfil) {
        this.urlFotoPerfil = urlFotoPerfil;
    }

    public UsuarioDTO() {
        nombre = null;
        Primerapellido = null;
        Segundoapellido = null;
        rol=null;
        contrasenha = null;
        correoElectronico = null;
        celular = null;
        urlFotoPerfil = null;
        citas=null;
        idUsuario=null;
        activo=null;
    }

    public UsuarioDTO(String nombre,String PrimerApellido, String SegundoApellido, String correoElectronico, String contrasenha, String celular, String urlFotoPerfil,Integer idUsuario) {
        this.nombre = nombre;
        this.Primerapellido = PrimerApellido;
        this.Segundoapellido = SegundoApellido;
        this.correoElectronico = correoElectronico;
        this.contrasenha = contrasenha;
        this.celular = celular;
        this.urlFotoPerfil = urlFotoPerfil;
        this.idUsuario = idUsuario;
        this.activo = 1;
    }

    public String getPrimerapellido() {
        return Primerapellido;
    }

    public void setPrimerapellido(String primerapellido) {
        Primerapellido = primerapellido;
    }

    public String getSegundoapellido() {
        return Segundoapellido;
    }

    public void setSegundoapellido(String segundoapellido) {
        Segundoapellido = segundoapellido;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getContrasenha() {
        return contrasenha;
    }

    public void setContrasenha(String contrasenha) {
        this.contrasenha = contrasenha;
    }

    public String getCelular() {
        return celular;
    }

    public void setCelular(String celular) {
        this.celular = celular;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

}