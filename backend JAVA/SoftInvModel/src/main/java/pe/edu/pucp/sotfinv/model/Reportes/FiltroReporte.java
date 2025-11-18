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
public class FiltroReporte {

    //Implementación de un constructor Superset, que permita tener un solo builder,
    //en vez de uno por cada reporte
    
    //Datos comunes (Rango de fechas)
    private Date fechaInicio;
    private Date fechaFin;
    
    // Campos EXCLUSIVOS de Servicios
    private String nombreServicio;
    private String tipoServicio;
    //private Date fecha;
    private String nombreEmpleado;
    
    // Campos EXCLUSIVOS de Productos
    private String estadoPedido;
    private String nombreProducto;
    private String tipoProducto;
    //private Date fecha_pago;

    public FiltroReporte(){
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
    
}