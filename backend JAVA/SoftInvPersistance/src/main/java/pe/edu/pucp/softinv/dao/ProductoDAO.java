package pe.edu.pucp.softinv.dao;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

import java.sql.SQLException;
import java.util.ArrayList;

public interface ProductoDAO {
    public Integer insertar(ProductoDTO producto);
    public ProductoDTO obtenerPorId(Integer id);
    public Integer modificar(ProductoDTO producto);
    public Integer eliminar(ProductoDTO idProducto);
    ArrayList<ProductoDTO> listarTodos();
    public ArrayList<ProductoDTO> obtenerPorNombre(String nombre);
    public ArrayList<ProductoDTO> obtenerPorPagina(Integer pag);
    public Integer obtenerCantPaginas();
    public ArrayList<ProductoDTO> obtenerPorFiltro(String filtro);
}