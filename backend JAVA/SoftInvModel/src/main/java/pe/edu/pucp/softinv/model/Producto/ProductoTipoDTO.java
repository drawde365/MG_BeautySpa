package pe.edu.pucp.softinv.model.Producto;

public class ProductoTipoDTO {
    private ProductoDTO producto;
    private Integer stock_fisico;
    private Integer stock_despacho;
    private String Ingredientes;
    private String tipo;
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
    }

    public ProductoTipoDTO(ProductoDTO producto, String tipo, Integer stock_fisico, Integer stock_despacho, String ingredientes, Integer activo) {
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

    public String getTipo() {
        return tipo;
    }

    public void setTipo(String tipo) {
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
