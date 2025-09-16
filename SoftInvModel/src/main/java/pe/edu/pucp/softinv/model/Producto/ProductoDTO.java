package pe.edu.pucp.softinv.model.Producto;

import pe.edu.pucp.softinv.model.Comentario.ComentarioProductoDTO;

import java.util.ArrayList;

public class ProductoDTO {
    private Integer idProducto;
    private String nombre;
    private String descripcion;
    private Double precio;
    private ArrayList<String> ingredientes;
    private String modoUso;
    private TipoProducto tipoProducto;
    private ArrayList<String>tamanios;
    private ArrayList<TipoPiel> tipoPiel;
    private Integer stock;
    private String urlImagen;
    private ArrayList<ComentarioProductoDTO> comentarios;
    private Double promedioValoracion;

    public ArrayList<String> getTamanios() {
        return tamanios;
    }

    public void setTamanios(ArrayList<String> tamanios) {
        this.tamanios = tamanios;
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

    public ArrayList<ComentarioProductoDTO> getComentarios() {
        return comentarios;
    }

    public void setComentarios(ArrayList<ComentarioProductoDTO> comentarios) {
        this.comentarios = comentarios;
    }

    public String getUrlImagen() {
        return urlImagen;
    }

    public void setUrlImagen(String urlImagen) {
        this.urlImagen = urlImagen;
    }

    public Integer getStock() {
        return stock;
    }

    public void setStock(Integer stock) {
        this.stock = stock;
    }

    public ArrayList<TipoPiel> getTipoPiel() {
        return tipoPiel;
    }

    public void setTipoPiel(ArrayList<TipoPiel> tipoPiel) {
        this.tipoPiel = tipoPiel;
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

    public ArrayList<String> getIngredientes() {
        return ingredientes;
    }

    public void setIngredientes(ArrayList<String> ingredientes) {
        this.ingredientes = ingredientes;
    }

    public String getModoUso() {
        return modoUso;
    }

    public void setModoUso(String modoUso) {
        this.modoUso = modoUso;
    }

    public TipoProducto getTipoProducto() {
        return tipoProducto;
    }

    public void setTipoProducto(TipoProducto tipoProducto) {
        this.tipoProducto = tipoProducto;
    }

    public ProductoDTO() {
        idProducto=null;
        tipoProducto = null;
        tipoPiel = null;
        nombre = null;
        descripcion = null;
        precio = null;
        ingredientes = null;
        modoUso = null;
        stock=null;
        urlImagen=null;
        comentarios = null;
        promedioValoracion=null;
        tamanios=null;
    }

    public ProductoDTO(Integer idProducto,String nombre,String descripcion,Double precio,
                       ArrayList<ComentarioProductoDTO> comentarios,ArrayList<String> ingredientes,String modoUso,
                       TipoProducto tipoProducto,ArrayList<String> tamanios,ArrayList<TipoPiel> tipoPiel,
                       Integer stock,String urlImagen,Double promedioValoracion) {
        this.idProducto=idProducto;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
        this.comentarios = comentarios;
        this.ingredientes = ingredientes;
        this.modoUso = modoUso;
        this.tipoProducto = tipoProducto;
        this.tamanios = tamanios;
        this.tipoPiel = tipoPiel;
        this.stock = stock;
        this.urlImagen = urlImagen;
        this.promedioValoracion = promedioValoracion;
    }
}
