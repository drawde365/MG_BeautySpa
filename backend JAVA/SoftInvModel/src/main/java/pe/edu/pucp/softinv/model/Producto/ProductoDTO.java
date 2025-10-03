package pe.edu.pucp.softinv.model.Producto;

import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

import java.util.ArrayList;

public class ProductoDTO {
    private Integer idProducto;
    private String nombre;
    private String descripcion;
    private Double precio;
    private String modoUso;
    private String urlImagen;
    private ArrayList<ComentarioDTO> comentarios;
    private Double promedioValoracion;
    private ArrayList<ProductoTipoDTO> productosTipos;
    private Integer activo;

    public Integer getActivo() {
        return activo;
    }

    public void setActivo(Integer activo) {
        this.activo = activo;
    }

    public ArrayList<ProductoTipoDTO> getProductosTipos() {
        return productosTipos;
    }

    public void setProductosTipos(ArrayList<ProductoTipoDTO> productosTipos) {
        this.productosTipos = productosTipos;
    }

    public Integer getIdProducto() {
        return idProducto;
    }

    public void setIdProducto(Integer idProducto) {
        this.idProducto = idProducto;
    }

    public Double getPromedioValoracion() {
        return promedioValoracion;
    }

    public void setPromedioValoracion(Double promedioValoracion) {
        this.promedioValoracion = promedioValoracion;
    }

    public ArrayList<ComentarioDTO> getComentarios() {
        return comentarios;
    }

    public void setComentarios(ArrayList<ComentarioDTO> comentarios) {
        this.comentarios = comentarios;
    }

    public String getUrlImagen() {
        return urlImagen;
    }

    public void setUrlImagen(String urlImagen) {
        this.urlImagen = urlImagen;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public Double getPrecio() {
        return precio;
    }

    public void setPrecio(Double precio) {
        this.precio = precio;
    }

    public String getModoUso() {
        return modoUso;
    }

    public void setModoUso(String modoUso) {
        this.modoUso = modoUso;
    }

    public ProductoDTO() {
        idProducto=null;
        nombre = null;
        descripcion = null;
        precio = null;
        modoUso = null;
        urlImagen=null;
        comentarios = null;
        promedioValoracion=null;
        productosTipos = null;
        activo = null;
    }

    public ProductoDTO(Integer idProducto,String nombre,String descripcion,Double precio,
                       ArrayList<ComentarioDTO> comentarios,String modoUso,
                       String urlImagen,Double promedioValoracion,ArrayList<ProductoTipoDTO> productosTipos, Integer activo) {
        this.idProducto=idProducto;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
        this.comentarios = comentarios;
        this.modoUso = modoUso;
        this.urlImagen = urlImagen;
        this.promedioValoracion = promedioValoracion;
        this.productosTipos = productosTipos;
        this.activo = activo;
    }
}
