/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.sotfinv.model.Reportes;

import java.util.Date;

/**
 *
 * @author Usuario
 */
public class FiltroBuilder {

    private Date fechaInicio;
    private Date fechaFin;
    private String nombreServicio;
    private String tipoServicio;
    private String nombreEmpleado;
    private String estadoPedido;
    private String nombreProducto;
    private String tipoProducto;
    
    public FiltroBuilder(){
        fechaInicio = null;
        fechaFin = null;
        nombreServicio = null;
        tipoServicio = null;
        nombreEmpleado = null;
        estadoPedido = null;
        nombreProducto = null;
        tipoProducto = null;
    }
    
    
    /**
     * @return the fechaInicio
     */
    public Date getFechaInicio() {
        return fechaInicio;
    }

    /**
     * @param fechaInicio the fechaInicio to set
     */
    public void setFechaInicio(Date fechaInicio) {
        this.fechaInicio = fechaInicio;
    }

    /**
     * @return the fechaFin
     */
    public Date getFechaFin() {
        return fechaFin;
    }

    /**
     * @param fechaFin the fechaFin to set
     */
    public void setFechaFin(Date fechaFin) {
        this.fechaFin = fechaFin;
    }

    /**
     * @return the nombreServicio
     */
    public String getNombreServicio() {
        return nombreServicio;
    }

    /**
     * @param nombreServicio the nombreServicio to set
     */
    public void setNombreServicio(String nombreServicio) {
        this.nombreServicio = nombreServicio;
    }

    /**
     * @return the tipoServicio
     */
    public String getTipoServicio() {
        return tipoServicio;
    }

    /**
     * @param tipoServicio the tipoServicio to set
     */
    public void setTipoServicio(String tipoServicio) {
        this.tipoServicio = tipoServicio;
    }

    /**
     * @return the nombreEmpleado
     */
    public String getNombreEmpleado() {
        return nombreEmpleado;
    }

    /**
     * @param nombreEmpleado the nombreEmpleado to set
     */
    public void setNombreEmpleado(String nombreEmpleado) {
        this.nombreEmpleado = nombreEmpleado;
    }

    /**
     * @return the estadoPedido
     */
    public String getEstadoPedido() {
        return estadoPedido;
    }

    /**
     * @param estadoPedido the estadoPedido to set
     */
    public void setEstadoPedido(String estadoPedido) {
        this.estadoPedido = estadoPedido;
    }

    /**
     * @return the nombreProducto
     */
    public String getNombreProducto() {
        return nombreProducto;
    }

    /**
     * @param nombreProducto the nombreProducto to set
     */
    public void setNombreProducto(String nombreProducto) {
        this.nombreProducto = nombreProducto;
    }

    /**
     * @return the tipoProducto
     */
    public String getTipoProducto() {
        return tipoProducto;
    }

    /**
     * @param tipoProducto the tipoProducto to set
     */
    public void setTipoProducto(String tipoProducto) {
        this.tipoProducto = tipoProducto;
    }
    
    public FiltroBuilder filtrarFechaInicio(Date fechaInicio){
        this.fechaInicio = fechaInicio;
        return this;
    }
    
    public FiltroBuilder filtrarFechaFin(Date fechaFin){
        this.fechaFin = fechaFin;
        return this;
    }
    
    public FiltroBuilder filtrarServicioNombreServicio(String nombre){
        this.nombreServicio = nombre;
        return this;
    }
    
    public FiltroBuilder filtrarServicioTipoServicio(String tipo){
        this.tipoServicio = tipo;
        return this;
    }
    
    public FiltroBuilder filtrarServicioNombreEmpleado(String nombre){
        this.nombreEmpleado = nombre;
        return this;
    }
    
    public FiltroBuilder filtrarProductoEstadoPedido(String estado){
        this.estadoPedido = estado;
        return this;
    }
    
    public FiltroBuilder filtrarProductoNombreProducto(String nombre){
        this.nombreProducto = nombre;
        return this;
    }
    
    public FiltroBuilder filtrarProductoTipoProducto(String tipo){
        this.tipoProducto = tipo;
        return this;
    }
    
    public FiltroReporte buildFiltro(){
        FiltroReporte filtro = new FiltroReporte();
        filtro.setFechaInicio(this.fechaInicio);
        filtro.setFechaFin(this.fechaFin);
        filtro.setNombreServicio(this.nombreServicio);
        filtro.setTipoServicio(this.tipoServicio);
        filtro.setNombreEmpleado(this.nombreEmpleado);
        filtro.setNombreProducto(this.nombreProducto);
        filtro.setEstadoPedido(this.estadoPedido);
        filtro.setTipoProducto(this.tipoProducto);
        return filtro;
    }
    
}
