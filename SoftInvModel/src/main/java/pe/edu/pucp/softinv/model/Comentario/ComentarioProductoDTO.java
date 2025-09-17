package pe.edu.pucp.softinv.model.Comentario;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

public class ComentarioProductoDTO extends ComentarioDTO{
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

    public ComentarioProductoDTO(Integer idComentario, ClienteDTO cliente, String comentario, Integer valoracion, ProductoDTO producto) {
        super(idComentario,cliente,comentario,valoracion);
        this.producto = producto;
    }
}
