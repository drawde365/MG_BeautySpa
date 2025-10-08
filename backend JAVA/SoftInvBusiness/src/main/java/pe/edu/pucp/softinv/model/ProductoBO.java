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

    public Integer insertar(String nombre, String descripcion, Double precio, Double tamanho, String urlImagen,
                            ArrayList<String> tipos, ArrayList<String> ingredientes, ArrayList<Integer> stock_fisico) {

//considerando que desde el frontend se puede convertir BindingList de c# a ArrayList de Java

        ArrayList<Integer> stock_despacho;
        ProductoDTO productoDTO = new ProductoDTO();
        productoDTO.setNombre(nombre);
        productoDTO.setActivo(1);
        productoDTO.setDescripcion(descripcion);
        productoDTO.setPrecio(precio);
        //productoDTO.setTamanho(tamanho);
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

    public Integer modificar(ProductoDTO productoDTO) {
        return productoDAO.modificar(productoDTO);
    }

    public Integer modificar(String nombre, String descripcion, Double prom_valoraciones, Double precio,
                             Double tamanho, String urlImagen, String modoDeUso, Integer activo) {

        ProductoDTO producto = new ProductoDTO();
        producto.setNombre(nombre);
        producto.setDescripcion(descripcion);
        producto.setPromedioValoracion();
    }

    public ArrayList<ProductoTipoDTO> devolverTiposProductos(String nombre) {


    }
}