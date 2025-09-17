package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProducto;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)

public class ProductoDAOimplTest {
    private ProductoDAO productoDAO;

    public ProductoDAOimplTest() {
        productoDAO = new ProductoDAOimpl();
    }

    @Test
    void testInsertarProducto() {
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        producto.setTipoProducto(TipoProducto.CORPORAL);
        producto.setPrecio(50.25);
        producto.setModoUso("Producto que se usa asi");
        producto.setStock(20);
        producto.setUrlImagen("hola.jpg");
        Integer result = productoDAO.insertar(producto);
        producto.setIdProducto(result);
        assertNotEquals(0, result, "El producto debería insertarse correctamente");
        productoDAO.eliminar(producto);
    }

    @Test
    void testObtenerProductoPorId(){
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        producto.setTipoProducto(TipoProducto.CORPORAL);
        producto.setPrecio(50.25);
        producto.setModoUso("Producto que se usa asi");
        producto.setStock(20);
        producto.setUrlImagen("hola.jpg");
        Integer result = productoDAO.insertar(producto);
        producto.setIdProducto(result);
        ProductoDTO final_prod = productoDAO.obtenerPorId(result);
        assertNotNull(final_prod, "El producto debería ser encontrado correctamente");
        productoDAO.eliminar(producto);
    }

    @Test
    void testModificarProducto() {
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        int result = productoDAO.modificar(producto);
        assertEquals(1, result, "El producto debería fallar?");
    }

    @Test
    void testEliminarProducto() {
        ProductoDTO producto = new ProductoDTO();
        int result = productoDAO.eliminar(producto);
        assertEquals(1, result, "El producto debería eliminarse correctamente");
    }
}
