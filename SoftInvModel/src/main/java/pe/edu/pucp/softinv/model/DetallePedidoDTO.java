package pe.edu.pucp.softinv.model;

public class DetallePedidoDTO {
    private ProductoDTO producto;
    private Integer cantidad;
    private Double subtotal;

    public ProductoDTO getProducto() {
        return producto;
    }

    public void setProducto(ProductoDTO producto) {
        this.producto = producto;
    }

    public Integer getCantidad() {
        return cantidad;
    }

    public void setCantidad(Integer cantidad) {
        this.cantidad = cantidad;
    }

    public Double getSubtotal() {
        return subtotal;
    }

    public void setSubtotal(Double subtotal) {
        this.subtotal = subtotal;
    }

    public DetallePedidoDTO() {
        producto = null;
        cantidad = null;
        subtotal = null;
    }

    public DetallePedidoDTO(ProductoDTO producto, Integer cantidad, Double subtotal) {
        this.producto = producto;
        this.cantidad = cantidad;
        this.subtotal = subtotal;
    }
}
