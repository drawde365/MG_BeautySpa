package pe.edu.pucp.softinv.model.Pedido;

import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

public class DetallePedidoDTO {
    private ProductoTipoDTO productoTipo;
    private PedidoDTO pedido;
    private Integer cantidad;
    private Double subtotal;

    public PedidoDTO getPedido() {
        return pedido;
    }

    public void setPedido(PedidoDTO pedido) {
        this.pedido = pedido;
    }

    public ProductoTipoDTO getProducto() {
        return productoTipo;
    }

    public void setProducto(ProductoTipoDTO producto) {
        this.productoTipo = producto;
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
        productoTipo = null;
        cantidad = null;
        subtotal = null;
        pedido = null;
    }

    public DetallePedidoDTO(ProductoTipoDTO producto, PedidoDTO pedido, Integer cantidad, Double subtotal) {
        this.productoTipo = producto;
        this.cantidad = cantidad;
        this.subtotal = subtotal;
        this.pedido = pedido;
    }
}
