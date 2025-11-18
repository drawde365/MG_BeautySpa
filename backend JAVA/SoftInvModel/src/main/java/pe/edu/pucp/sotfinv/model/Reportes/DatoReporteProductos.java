/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.sotfinv.model.Reportes;

import java.text.SimpleDateFormat;
import java.util.Date;
import pe.edu.pucp.sotfinv.model.Reportes.Interfaces.Reportable;

/**
 *
 * @author Usuario
 */
public class DatoReporteProductos implements Reportable{

    private Integer pedido_id;
    private String estado;
    private String nombreProducto;
    private String tipo;
    private Integer tamanho;
    private Double precioUnitario;
    private Integer cantidad;
    private Double subtotal;
    private Date fecha_pago;
    private Date fecha_recojo;
    
    public DatoReporteProductos(){
        pedido_id = null;
        estado = null;
        nombreProducto = null;
        tipo = null;
        tamanho = null;
        precioUnitario = null;
        cantidad = null;
        subtotal = null;
        fecha_pago = null;
        fecha_recojo = null;
    }
    
    public DatoReporteProductos(Integer pedido_id,String estado,String nombreProducto,String tipo,Integer tamanho,
            Double precioUnitario,Integer cantidad,Double subtotal,Date fecha_pago,Date fecha_recojo){
        this.pedido_id = pedido_id;
        this.estado = estado;
        this.nombreProducto = nombreProducto;
        this.tipo = tipo;
        this.tamanho = tamanho;
        this.precioUnitario = precioUnitario;
        this.cantidad = cantidad;
        this.subtotal = subtotal;
        this.fecha_pago = fecha_pago;
        this.fecha_recojo = fecha_recojo;
    }
    
    public DatoReporteProductos(DatoReporteProductos copia){
        this.pedido_id = copia.pedido_id;
        this.estado = copia.estado;
        this.nombreProducto = copia.nombreProducto;
        this.tipo = copia.tipo;
        this.tamanho = copia.tamanho;
        this.precioUnitario = copia.precioUnitario;
        this.cantidad = copia.cantidad;
        this.subtotal = copia.subtotal;
        this.fecha_pago = copia.fecha_pago;
        this.fecha_recojo = copia.fecha_recojo;
    }
    
    /**
     * @return the pedido_id
     */
    public Integer getPedido_id() {
        return pedido_id;
    }

    /**
     * @param pedido_id the pedido_id to set
     */
    public void setPedido_id(Integer pedido_id) {
        this.pedido_id = pedido_id;
    }

    /**
     * @return the estado
     */
    public String getEstado() {
        return estado;
    }

    /**
     * @param estado the estado to set
     */
    public void setEstado(String estado) {
        this.estado = estado;
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
     * @return the tipo
     */
    public String getTipo() {
        return tipo;
    }

    /**
     * @param tipo the tipo to set
     */
    public void setTipo(String tipo) {
        this.tipo = tipo;
    }

    /**
     * @return the tamanho
     */
    public Integer getTamanho() {
        return tamanho;
    }

    /**
     * @param tamanho the tamanho to set
     */
    public void setTamanho(Integer tamanho) {
        this.tamanho = tamanho;
    }

    /**
     * @return the precioUnitario
     */
    public Double getPrecioUnitario() {
        return precioUnitario;
    }

    /**
     * @param precioUnitario the precioUnitario to set
     */
    public void setPrecioUnitario(Double precioUnitario) {
        this.precioUnitario = precioUnitario;
    }

    /**
     * @return the cantidad
     */
    public Integer getCantidad() {
        return cantidad;
    }

    /**
     * @param cantidad the cantidad to set
     */
    public void setCantidad(Integer cantidad) {
        this.cantidad = cantidad;
    }

    /**
     * @return the subtotal
     */
    public Double getSubtotal() {
        return subtotal;
    }

    /**
     * @param subtotal the subtotal to set
     */
    public void setSubtotal(Double subtotal) {
        this.subtotal = subtotal;
    }

    /**
     * @return the fecha_pago
     */
    public Date getFecha_pago() {
        return fecha_pago;
    }

    /**
     * @param fecha_pago the fecha_pago to set
     */
    public void setFecha_pago(Date fecha_pago) {
        this.fecha_pago = fecha_pago;
    }

    /**
     * @return the fecha_recojo
     */
    public Date getFecha_recojo() {
        return fecha_recojo;
    }

    /**
     * @param fecha_recojo the fecha_recojo to set
     */
    public void setFecha_recojo(Date fecha_recojo) {
        this.fecha_recojo = fecha_recojo;
    }

    @Override
    public String[] getTitulosColumnas() {
        return new String[] {"Id. Pedido", "Estado del Pedido","Fecha de pago", "Fecha de recojo", 
            "Nombre del Producto", "Tipo", "Tamaño (ml)","Precio Unitario", "Cantidad", "Subtotal",};
    }
    
    @Override
    public String[] getDatosFila() {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy");
        return new String[] {
            this.pedido_id.toString(),
            this.estado,
            (this.fecha_pago!=null) ? sdf.format(this.fecha_pago) : "-",
            (this.fecha_recojo!=null) ? sdf.format(this.fecha_recojo) : "-",
            this.nombreProducto,
            this.tipo,
            this.tamanho.toString(),
            String.format("S/.%.2f", this.precioUnitario),
            this.cantidad.toString(),
            String.format("S/.%.2f", this.subtotal),
        };
    }

    @Override
    public float[] getAnchosColumnas() {
        return new float[] {1f, 1.5f, 1.5f, 1.5f, 3f, 1f, 1.5f, 1.5f, 1.5f, 1.5f};
    }

    @Override
    public double getMontoTotal() {
        return this.getSubtotal();
    }
    
}
