/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.sotfinv.model.Reportes;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 *
 * @author Usuario
 */
public class DatoReporteServicios {

    private String nombreCliente;
    private String nombreServicio;
    private String tipoServicio;
    private Date fecha;
    private Double precio;
    private String nombreEmpleado;
    
    public DatoReporteServicios(){
        nombreCliente = null;
        nombreServicio = null;
        tipoServicio = null;
        fecha = null;
        precio = null;
        nombreEmpleado = null;
    }
    
    public DatoReporteServicios(String nombreCliente,String nombreServicio,String tipoServicio,
            Date fecha,Double precio,String nombreEmpleado){
        this.nombreCliente = nombreCliente;
        this.nombreServicio = nombreServicio;
        this.tipoServicio = tipoServicio;
        this.fecha = fecha;
        this.precio = precio;
        this.nombreEmpleado = nombreEmpleado;
    }
    
    public DatoReporteServicios(DatoReporteServicios copia){
        this.nombreCliente = copia.nombreCliente;
        this.nombreServicio = copia.nombreServicio;
        this.tipoServicio = copia.tipoServicio;
        this.fecha = copia.fecha;
        this.precio = copia.precio;
        this.nombreEmpleado = copia.nombreEmpleado;
    }
    
    /**
     * @return the nombreCliente
     */
    public String getNombreCliente() {
        return nombreCliente;
    }

    /**
     * @param nombreCliente the nombreCliente to set
     */
    public void setNombreCliente(String nombreCliente) {
        this.nombreCliente = nombreCliente;
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
     * @return the fecha
     */
    public Date getFecha() {
        return fecha;
    }

    /**
     * @param fecha the fecha to set
     */
    public void setFecha(Date fecha) {
        this.fecha = fecha;
    }

    /**
     * @return the precio
     */
    public Double getPrecio() {
        return precio;
    }

    /**
     * @param precio the precio to set
     */
    public void setPrecio(Double precio) {
        this.precio = precio;
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
    
    
}
