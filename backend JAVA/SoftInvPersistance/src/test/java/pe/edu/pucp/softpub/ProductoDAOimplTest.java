package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)

public class ProductoDAOimplTest {
    private ProductoDAO productoDAO;
    private ArrayList<ProductoTipoDTO>lista; 
    public ProductoDAOimplTest() {
        productoDAO = new ProductoDAOimpl();
        lista = new ArrayList<ProductoTipoDTO>();
    }

    public ProductoDTO insertar() {
        ProductoDTO producto = new ProductoDTO();
        producto.setNombre("Crema1");
        producto.setDescripcion("Crema que funciona como bloqueador");
        producto.setPrecio(50.25);
        producto.setModoUso("Producto que se usa asi");
        producto.setUrlImagen("hola.jpg");
        producto.setPromedioValoracion(5.0);
        producto.setActivo(1);
        producto.setTamanho(2.5);

        ProductoTipoDTO productoTipo = new ProductoTipoDTO();
        productoTipo.setTipo("Grasa");
        productoTipo.setStock_despacho(12);
        productoTipo.setStock_fisico(120481038);
        productoTipo.setIngredientes("Hola mundo");
        productoTipo.setActivo(1);
        ProductoTipoDTO productoTipo2 = new ProductoTipoDTO();
        productoTipo2.setTipo("Suave");
        productoTipo2.setStock_despacho(10);
        productoTipo2.setStock_fisico(3543);
        productoTipo2.setIngredientes("Adios mundo");
        productoTipo2.setActivo(1);
        ArrayList<ProductoTipoDTO> productos = new ArrayList<>();
        productos.add(productoTipo);
        productos.add(productoTipo2);
        producto.setProductosTipos(productos);
        Integer result = productoDAO.insertar(producto);
        producto.setIdProducto(result);
        assertNotEquals(0, result, "El producto debería insertarse correctamente");
        return producto;
    }

    @Test
    void testInsertarProducto() {
        ProductoDTO producto = insertar();
        productoDAO.eliminar(producto);
    }

    @Test
    void testObtenerProductoPorId() {
        ProductoDTO producto = insertar();
        ProductoDTO final_prod = productoDAO.obtenerPorId(producto.getIdProducto());
        assertNotNull(final_prod, "El producto debería ser encontrado correctamente");
        productoDAO.eliminar(producto);
    }

    @Test
    void testModificarProducto() {
        ProductoDTO producto = insertar();
        producto.setNombre("Crema2");
        producto.setDescripcion("Crema que funciona como bloqueador");
        int result = productoDAO.modificar(producto);
        assertEquals(1, result, "El producto debería fallar?");
        productoDAO.eliminar(producto);
    }

    @Test
    void testEliminarProducto() {
        ProductoDTO producto = insertar();
        int result = productoDAO.eliminar(producto);
        assertEquals(1, result, "El producto debería eliminarse correctamente");
    }
}
