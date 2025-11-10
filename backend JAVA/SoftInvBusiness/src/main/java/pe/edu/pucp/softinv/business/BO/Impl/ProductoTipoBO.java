package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.ProductoTipoDAOImpl;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.util.ArrayList;

public class ProductoTipoBO {
    private ProductoTipoDAO productoTipoDAO;

    public ProductoTipoBO() {
        productoTipoDAO=new ProductoTipoDAOImpl();
    }

    public ArrayList<ProductoTipoDTO> obtenerProductoId(Integer idProducto){
        return productoTipoDAO.obtenerProductoId(idProducto);
    }

    public Integer insertar(ProductoTipoDTO productoTipo) {
        return productoTipoDAO.insertar(productoTipo);
    }

    public Integer modificar(ProductoTipoDTO productoTipo){
        return productoTipoDAO.modificar(productoTipo);
    }

    public Integer eliminar(ProductoTipoDTO productoTipo){
        productoTipo.setActivo(0);
        return productoTipoDAO.modificar(productoTipo);
    }
}
