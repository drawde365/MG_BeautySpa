package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

public interface ProductoDAO {
    public Integer insertar(ProductoDTO producto);
    public ProductoDTO optenerPorId(Integer id);
    public Integer modificar(ProductoDTO producto);
    public Integer eliminar(Integer id);
}