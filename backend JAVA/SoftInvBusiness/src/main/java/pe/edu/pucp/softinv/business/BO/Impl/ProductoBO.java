package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProdDTO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;

public class ProductoBO {

    private ProductoDAO productoDAO;
    private ProductoTipoDAO productoTipoDAO;

    public ProductoBO() {
        this.productoTipoDAO = new ProductoTipoDAOImpl();
        this.productoDAO = new ProductoDAOimpl();
    }

    public Integer obtenerCantPaginas(){
        return productoDAO.obtenerCantPaginas();
    }

    public Integer insertar(ProductoDTO producto){
        return productoDAO.insertar(producto);
    }

    public ProductoDTO obtenerPorId(Integer idProd) {
        return productoDAO.obtenerPorId(idProd);
    }

    public Integer modificar(ProductoDTO producto) {
        ArrayList<ProductoTipoDTO> listaAntigua = productoTipoDAO.obtenerProductoId(producto.getIdProducto());
        ArrayList<ProductoTipoDTO> listaNueva = producto.getProductosTipos();

        Map<Integer, ProductoTipoDTO> mapaNuevos = new HashMap<>();
        if(listaNueva != null) {
            for (ProductoTipoDTO tipoNuevo : listaNueva) {
                if(tipoNuevo.getTipo() != null) {
                    mapaNuevos.put(tipoNuevo.getTipo().getId(), tipoNuevo);
                }
            }
        }

        for (ProductoTipoDTO tipoAntiguo : listaAntigua) {
            ProductoTipoDTO tipoNuevo = mapaNuevos.get(tipoAntiguo.getTipo().getId());

            if (tipoNuevo != null) {
                tipoAntiguo.setActivo(1);
                tipoAntiguo.setStock_fisico(tipoNuevo.getStock_fisico());
                tipoAntiguo.setIngredientes(tipoNuevo.getIngredientes());
                productoTipoDAO.modificar(tipoAntiguo);
                mapaNuevos.remove(tipoAntiguo.getTipo().getId());
            } else {
                tipoAntiguo.setActivo(0);
                productoTipoDAO.modificar(tipoAntiguo);
            }
        }

        for (ProductoTipoDTO tipoParaInsertar : mapaNuevos.values()) {
            tipoParaInsertar.setProducto(producto);
            tipoParaInsertar.setActivo(1);
            tipoParaInsertar.setStock_despacho(0);
            productoTipoDAO.insertar(tipoParaInsertar);
        }

        return productoDAO.modificar(producto);
    }

    public Integer eliminar(ProductoDTO producto) {
        producto.setActivo(0);
        return productoDAO.modificar(producto);
    }

    public ArrayList<ProductoDTO> buscarPorNombre (String nombre){
        return productoDAO.obtenerPorNombre(nombre);
    }

    public ArrayList<ProductoDTO> obtenerPagina (Integer pagina){
        return productoDAO.obtenerPorPagina(pagina);
    }

    public Integer cantidadPaginas() {
        return productoDAO.obtenerCantPaginas();
    }

    public ArrayList<ProductoDTO> obtenerCorporales (){
        return productoDAO.obtenerPorFiltro("Corporal");
    } 
    public ArrayList<ProductoDTO> obtenerFaciales (){
        return productoDAO.obtenerPorFiltro("Facial");
    } 
    public ArrayList<ProductoDTO> listarTodos (){
        return productoDAO.listarTodos();
    } 
    public ArrayList<ProductoDTO> listarTodosActivos (){
        return productoDAO.listarTodosActivos();
    } 
}