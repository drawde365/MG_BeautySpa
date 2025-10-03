package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;

public interface ProductoTipoDAO {
    public Integer insertar(ProductoTipoDTO productoTipo);
    public ProductoTipoDTO obtener(Integer id, String tipo);
    public Integer modificar(ProductoTipoDTO productoTipo);
    public Integer eliminar(ProductoTipoDTO productoTipo);
    public ArrayList<ProductoTipoDTO> obtenerProductoId(Integer idProducto);
}