package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
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

    public Integer insertar(String nombre, String descripcion, Double precio, Double tamanho, String urlImagen,
                            ArrayList<String> tipos, ArrayList<String> ingredientes, ArrayList<Integer> stock_fisico) {

//considerando que desde el frontend se puede convertir BindingList de c# a ArrayList de Java

        ArrayList<Integer> stock_despacho;
        ProductoDTO productoDTO = new ProductoDTO();
        productoDTO.setNombre(nombre);
        productoDTO.setActivo(1);
        productoDTO.setDescripcion(descripcion);
        productoDTO.setPrecio(precio);
        productoDTO.setTamanho(tamanho);
        productoDTO.setUrlImagen(urlImagen);
        ArrayList<ProductoTipoDTO> productosTipos = new ArrayList<>();

        for (int i = 0; i < tipos.size(); i++) {
            ProductoTipoDTO productoTipoDTO = new ProductoTipoDTO();
            productoTipoDTO.setTipo(tipos.get(i));
            productoTipoDTO.setIngredientes(ingredientes.get(i));
            productoTipoDTO.setStock_fisico(stock_fisico.get(i));
            productoTipoDTO.setStock_despacho(0);
            productosTipos.add(productoTipoDTO);
        }
        productoDTO.setProductosTipos(productosTipos);
        return productoDAO.insertar(productoDTO);
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

            Map<String, ProductoTipoDTO> mapaNuevos = new HashMap<>();
            for (ProductoTipoDTO tipoNuevo : listaNueva) {
                mapaNuevos.put(tipoNuevo.getTipo(), tipoNuevo);
            }

            for (ProductoTipoDTO tipoAntiguo : listaAntigua) {
                ProductoTipoDTO tipoNuevo = mapaNuevos.get(tipoAntiguo.getTipo());

                if (tipoNuevo != null) {
                    tipoAntiguo.setActivo(1);
                    tipoAntiguo.setStock_fisico(tipoNuevo.getStock_fisico());
                    tipoAntiguo.setIngredientes(tipoNuevo.getIngredientes());
                    productoTipoDAO.modificar(tipoAntiguo);
                    mapaNuevos.remove(tipoAntiguo.getTipo());
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

//cambia el estado a inactivo, primero desde el front llamo a obtener por id
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

//lo borra de la BD, primero desde el front llamo a obtener por id
/*
    public Integer eliminarBorrandolo(ProductoDTO producto) {
        return productoDAO.eliminar(producto);
    }
*/
