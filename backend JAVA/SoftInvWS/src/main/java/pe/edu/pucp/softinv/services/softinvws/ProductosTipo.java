package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.ProductoTipoBO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

/**
 *
 * @author Flavio
 */
@WebService(serviceName = "ProductoTipo")
public class ProductosTipo {
    private ProductoTipoBO productoTipoBO;
    
    public ProductosTipo() {
        productoTipoBO = new ProductoTipoBO ();
    }
    
    @WebMethod(operationName = "ObtenerProductosTiposId")
    public ArrayList<ProductoTipoDTO> obtenerProductoIdProductoTipo(@WebParam(name = "idProducto") Integer idProducto) {
        return productoTipoBO.obtenerProductoId(idProducto);
    }
    @WebMethod(operationName = "ObtenerProductosTiposIdActivo")
    public ArrayList<ProductoTipoDTO> obtenerProductoIdActivosProductoTipo(@WebParam(name = "idProducto") Integer idProducto) {
        return productoTipoBO.obtenerProductoIdActivo(idProducto);
    }
    @WebMethod(operationName = "Insertar")
    public Integer insertarProductoTipo(@WebParam(name = "productoTipo")ProductoTipoDTO productoTipo) {
        return productoTipoBO.insertar(productoTipo);   
    }
    @WebMethod(operationName = "Modificar")
    public Integer modificarProductoTipo(@WebParam(name = "productoTipo")ProductoTipoDTO productoTipo){
        return productoTipoBO.modificar(productoTipo);
    }
    @WebMethod(operationName = "Eliminar")
    public Integer eliminarProductoTipo(@WebParam(name = "productoTipo")ProductoTipoDTO productoTipo){
        return productoTipoBO.eliminar(productoTipo);
    }
}
