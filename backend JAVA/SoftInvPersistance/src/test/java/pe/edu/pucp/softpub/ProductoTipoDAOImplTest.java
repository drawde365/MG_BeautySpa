package pe.edu.pucp.softpub;

import org.junit.jupiter.api.*;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;

@Tag("integration")
@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class ProductoTipoDAOImplTest {

    private ProductoDAO productoDAO;
    private ProductoTipoDAO productoTipoDAO;
    private ProductoDTO producto;
    private ProductoTipoDTO productoTipo;

    public ProductoTipoDAOImplTest() {
        productoDAO = new ProductoDAOimpl();
        productoTipoDAO = new ProductoTipoDAOImpl();
        producto = null;
        productoTipo = null;
    }

    public void insertar() {
        ProductoDTO prod = new ProductoDTO();
        prod.setNombre("Shampoo Anticaspa");
        prod.setDescripcion("Shampoo para cabello graso con efecto refrescante.");
        prod.setPrecio(15.50);
        prod.setUrlImagen("shampoo.png");
        prod.setModoUso("Aplicar en cabello húmedo, dejar actuar 2 minutos y enjuagar.");
        prod.setPromedioValoracion(4.6);
        prod.setActivo(1);
        prod.setTamanho(250.0);
        prod.setProductosTipos(new ArrayList<>());

        productoTipo = new ProductoTipoDTO();
        productoTipo.setProducto(producto);
        productoTipo.setTipo("Grasa");
        productoTipo.setStock_fisico(50);
        productoTipo.setStock_despacho(20);
        productoTipo.setIngredientes("Extracto de menta, sulfato, agua");
        productoTipo.setActivo(1);

        prod.getProductosTipos().add(productoTipo);

        Integer resultado = productoDAO.insertar(prod);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "El resultado debe indicar éxito.");
        prod.setIdProducto(resultado);
        this.producto=prod;

        this.productoTipo = new ProductoTipoDTO();
        this.productoTipo.setProducto(this.producto);
        this.productoTipo.setTipo("Seca");
        this.productoTipo.setStock_fisico(70);
        this.productoTipo.setStock_despacho(45);
        this.productoTipo.setIngredientes("Extracto de vainilla, sulfato, agua");
        this.productoTipo.setActivo(1);

        resultado = productoTipoDAO.insertar(this.productoTipo);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "El resultado debe indicar éxito.");
        this.producto.getProductosTipos().add(this.productoTipo);
    }

    public void eliminar() {
        Integer resultadoProducto = productoDAO.eliminar(producto);
        assertNotNull(resultadoProducto, "Debe eliminar el producto correctamente.");
    }

    @Test
    @Order(1)
    @DisplayName("Insertar un ProductoTipo")
    void testInsertarProductoTipo() {
        insertar();
        eliminar();
    }

    @Test
    @Order(2)
    @DisplayName("Obtener un ProductoTipo por ID (Singular)")
    void testObtenerProductoTipo() {
        insertar();
        ProductoTipoDTO obtenido = productoTipoDAO.obtener(
                productoTipo.getProducto().getIdProducto(),
                productoTipo.getTipo()
        );

        assertNotNull(obtenido, "Debe retornar un objeto ProductoTipoDTO.");
        assertEquals(productoTipo.getProducto().getIdProducto(), obtenido.getProducto().getIdProducto());
        assertEquals(productoTipo.getTipo(), obtenido.getTipo());
        eliminar();
    }

    @Test
    @Order(3)
    @DisplayName("Modificar un ProductoTipo existente")
    void testModificarProductoTipo() {
        insertar();
        productoTipo.setStock_fisico(80);
        productoTipo.setStock_despacho(60);
        productoTipo.setIngredientes("Extracto de romero, sin sulfatos");
        productoTipo.setActivo(0);

        Integer resultado = productoTipoDAO.modificar(productoTipo);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito.");

        ProductoTipoDTO actualizado = productoTipoDAO.obtener(
                productoTipo.getProducto().getIdProducto(),
                productoTipo.getTipo()
        );

        assertEquals(80, actualizado.getStock_fisico());
        assertEquals(60, actualizado.getStock_despacho());
        assertEquals("Extracto de romero, sin sulfatos", actualizado.getIngredientes());
        assertEquals(0, actualizado.getActivo());
        eliminar();
    }

    @Test
    @Order(4)
    @DisplayName("Listar tipos de producto por ID de producto (Lista)")
    void testObtenerProductoId() {
        insertar();

        ArrayList<ProductoTipoDTO> lista = productoTipoDAO.obtenerProductoId(productoTipo.getProducto().getIdProducto());
        assertNotNull(lista, "La lista no debe ser nula.");
        assertFalse(lista.isEmpty(), "Debe contener al menos un tipo.");
        assertEquals(productoTipo.getProducto().getIdProducto(), lista.get(0).getProducto().getIdProducto());

        eliminar();
    }

    @Test
    @Order(5)
    @DisplayName("Eliminar un ProductoTipo")
    void testEliminarProductoTipo() {
        insertar();
        Integer resultado = productoTipoDAO.eliminar(productoTipo);
        assertNotNull(resultado, "El resultado no debe ser nulo.");
        assertTrue(resultado >= 0, "Debe indicar éxito.");

        eliminar();
    }
}
