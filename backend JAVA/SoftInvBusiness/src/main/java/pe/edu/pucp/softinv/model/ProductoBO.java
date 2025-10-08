package pe.edu.pucp.softinv.model;

import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoDAOimpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class ProductoBO {

    private ProductoDAO productoDAO;

    public ProductoBO() {

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

//desde el front se llama a obtener por id, se modifica el campo que se quiere y se llama a este metodo
    public Integer modificar(ProductoDTO producto) {
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

}

//lo borra de la BD, primero desde el front llamo a obtener por id
/*
    public Integer eliminarBorrandolo(ProductoDTO producto) {
        return productoDAO.eliminar(producto);
    }
*/
