/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.model.Token;

import java.util.Date;

/**
 *
 * @author Rodrigo Cahuana
 */
public class ContrasenhaTokenDTO {

    private Integer usuarioId;
    private Integer tokenId;
    private String token;
    private Date fecha_expiracion;
    private Integer usado;
    
    /**
     * @return the usuarioId
     */
    public Integer getUsuarioId() {
        return usuarioId;
    }

    /**
     * @param usuarioId the usuarioId to set
     */
    public void setUsuarioId(Integer usuarioId) {
        this.usuarioId = usuarioId;
    }

    /**
     * @return the tokenId
     */
    public Integer getTokenId() {
        return tokenId;
    }

    /**
     * @param tokenId the tokenId to set
     */
    public void setTokenId(Integer tokenId) {
        this.tokenId = tokenId;
    }

    /**
     * @return the token
     */
    public String getToken() {
        return token;
    }

    /**
     * @param token the token to set
     */
    public void setToken(String token) {
        this.token = token;
    }

    /**
     * @return the fecha_expiracion
     */
    public Date getFecha_expiracion() {
        return fecha_expiracion;
    }

    /**
     * @param fecha_expiracion the fecha_expiracion to set
     */
    public void setFecha_expiracion(Date fecha_expiracion) {
        this.fecha_expiracion = fecha_expiracion;
    }

    /**
     * @return the usado
     */
    public Integer getUsado() {
        return usado;
    }

    /**
     * @param usado the usado to set
     */
    public void setUsado(Integer usado) {
        this.usado = usado;
    }
    
    public ContrasenhaTokenDTO(){
        usuarioId = null;
        token = null;
        tokenId = null;
        fecha_expiracion = null;
        usado = null;
    }
    
    public ContrasenhaTokenDTO(Integer usuarioId,Integer tokenId,String token,
            Date fecha_expiracion,Integer usado){
        this.usuarioId=usuarioId;
        this.tokenId = tokenId;
        this.token = token;
        this.fecha_expiracion = fecha_expiracion;
        this.usado = usado;
    }
}
