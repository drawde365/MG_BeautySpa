package pe.edu.pucp.softinv.model;

public class ComentarioProductoDTO extends ComenarioDTO{
    private ProductoDTO producto;

    public ProductoDTO getProducto() {
        return producto;
    }

    public void setProducto(ProductoDTO producto) {
        this.producto = producto;
    }

    public ComentarioProductoDTO() {
        super();
        producto = null;
    }

    public ComentarioProductoDTO(ClienteDTO cliente,String comentario,Integer valoracion,ProductoDTO producto) {
        super(cliente,comentario,valoracion);
        this.producto = producto;
    }
}
