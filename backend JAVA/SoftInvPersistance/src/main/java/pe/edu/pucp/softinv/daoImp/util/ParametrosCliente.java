/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp.util;

/**
 *
 * @author alulab14
 */
public class ParametrosCliente {
    private String nombre;
    private String pApe;
    private String sApe;
    private String correo;
    private String celular;

    public ParametrosCliente(String nombre,String pApe,String sApe,String correo,String celular){
        this.nombre = nombre;
        this.pApe = pApe;
        this.sApe = sApe;
        this.correo = correo;
        this.celular = celular;
    }
    
    /**
     * @return the nombre
     */
    public String getNombre() {
        return nombre;
    }

    /**
     * @param nombre the nombre to set
     */
    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    /**
     * @return the pApe
     */
    public String getpApe() {
        return pApe;
    }

    /**
     * @param pApe the pApe to set
     */
    public void setpApe(String pApe) {
        this.pApe = pApe;
    }

    /**
     * @return the sApe
     */
    public String getsApe() {
        return sApe;
    }

    /**
     * @param sApe the sApe to set
     */
    public void setsApe(String sApe) {
        this.sApe = sApe;
    }

    /**
     * @return the correo
     */
    public String getCorreo() {
        return correo;
    }

    /**
     * @param correo the correo to set
     */
    public void setCorreo(String correo) {
        this.correo = correo;
    }

    /**
     * @return the celular
     */
    public String getCelular() {
        return celular;
    }

    /**
     * @param celular the celular to set
     */
    public void setCelular(String celular) {
        this.celular = celular;
    }
    
    
}
