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

    public Integer insertar(String nombre,String descripcion,Double precio,Double tamaño,String ingredientes,
                            String urlImagen,) {


        private Integer idProducto;
        private String nombre;
        private String descripcion;
        private Double precio;
        private String modoUso;
        private String urlImagen;
        private ArrayList<ComentarioDTO> comentarios;
        private Double promedioValoracion;
        private ArrayList<ProductoTipoDTO> productosTipos;
        private Integer activo;

    }