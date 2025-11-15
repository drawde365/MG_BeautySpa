package pe.edu.pucp.softinv.model.Producto;

public class ProductoTipoDTO {
    private ProductoDTO producto;
    private Integer stock_fisico;
    private Integer stock_despacho;
    private String Ingredientes;
    private TipoProdDTO tipo; // <-- CAMBIO 1: De String a TipoProdDTO
    private Integer activo;

    public Integer getActivo() {
        return activo;
    }

    public void setActivo(Integer activo) {
        this.activo = activo;
    }

    public ProductoTipoDTO() {
        producto = null;
        stock_fisico = null;
        stock_despacho = null;
        Ingredientes = null;
        tipo = null;
        activo = null; // Añadido para consistencia
    }

    // --- CAMBIO 2: Constructor actualizado ---
    public ProductoTipoDTO(ProductoDTO producto, TipoProdDTO tipo, Integer stock_fisico, Integer stock_despacho, String ingredientes, Integer activo) {
        this.producto = producto;
        this.tipo = tipo;
        this.stock_fisico = stock_fisico;
        this.stock_despacho = stock_despacho;
        this.Ingredientes = ingredientes;
        this.activo = activo;
    }

    public ProductoDTO getProducto() {
        return producto;
    }

    public void setProducto(ProductoDTO producto) {
        this.producto = producto;
    }

    // --- CAMBIO 3: Getters y Setters actualizados ---
    public TipoProdDTO getTipo() {
        return tipo;
    }

    public void setTipo(TipoProdDTO tipo) {
        this.tipo = tipo;
    }

    public Integer getStock_fisico() {
        return stock_fisico;
    }

    public void setStock_fisico(Integer stock_fisico) {
        this.stock_fisico = stock_fisico;
    }

    public Integer getStock_despacho() {
        return stock_despacho;
    }

    public void setStock_despacho(Integer stock_despacho) {
        this.stock_despacho = stock_despacho;
    }

    public String getIngredientes() {
        return Ingredientes;
    }

    public void setIngredientes(String ingredientes) {
        Ingredientes = ingredientes;
    }
}