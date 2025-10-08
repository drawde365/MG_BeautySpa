package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class ProductoDAOimpl extends DAOImplBase implements ProductoDAO {

    private ProductoDTO producto;

    public ProductoDAOimpl() {
        super("PRODUCTOS");
        this.producto = null;
        this.retornarLlavePrimaria = true;
    }
//Activo
    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PRODUCTO_ID", true, true));
        this.listaColumnas.add(new Columna("NOMBRE", false, false));
        this.listaColumnas.add(new Columna("PRECIO", false, false));
        this.listaColumnas.add(new Columna("DESCRIPCION", false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN", false, false));
        this.listaColumnas.add(new Columna("MODO_DE_USO", false, false));
        this.listaColumnas.add(new Columna("PROM_VALORACIONES", false, false));
        this.listaColumnas.add(new Columna("ACTIVO", false, false));
        this.listaColumnas.add(new Columna("TAMANHO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setString(1, producto.getNombre());
        this.statement.setDouble(2, producto.getPrecio());
        this.statement.setString(3, producto.getDescripcion());
        this.statement.setString(4, producto.getUrlImagen());
        this.statement.setString(5, producto.getModoUso());
        this.statement.setDouble(6, producto.getPromedioValoracion());
        this.statement.setInt(7,producto.getActivo());
        this.statement.setDouble(8, producto.getTamanho());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setString(1, producto.getNombre());
        this.statement.setDouble(2, producto.getPrecio());
        this.statement.setString(3, producto.getDescripcion());
        this.statement.setString(4, producto.getUrlImagen());
        this.statement.setString(5, producto.getModoUso());
        this.statement.setDouble(6, producto.getPromedioValoracion());
        this.statement.setInt(7,producto.getActivo());
        this.statement.setDouble(9, producto.getTamanho());
        this.statement.setInt(10, producto.getIdProducto());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, producto.getIdProducto());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.producto.getIdProducto());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.producto = new ProductoDTO();
        this.producto.setIdProducto(this.resultSet.getInt("PRODUCTO_ID"));
        this.producto.setNombre(this.resultSet.getString("NOMBRE"));
        this.producto.setPrecio(this.resultSet.getDouble("PRECIO"));
        this.producto.setDescripcion(this.resultSet.getString("DESCRIPCION"));
        this.producto.setUrlImagen(this.resultSet.getString("URL_IMAGEN"));
        this.producto.setModoUso(this.resultSet.getString("MODO_DE_USO"));
        this.producto.setActivo(this.resultSet.getInt("ACTIVO"));
        this.producto.setTamanho(this.resultSet.getDouble("TAMANHO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.producto = null;
    }

    @Override
    public Integer insertar(ProductoDTO producto) {
        this.producto = producto;
        Integer resultado = super.insertar(true,false);
        this.producto.setIdProducto(resultado);
        ProductoTipoDAO productoTipoDAO = new ProductoTipoDAOImpl(this.conexion);
        ArrayList<ProductoTipoDTO> productos = this.producto.getProductosTipos();
        for(ProductoTipoDTO prod : productos){
            prod.setProducto(this.producto);
            productoTipoDAO.insertar(prod,true,true);
        }
        try {
            this.cerrarConexion();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
        return resultado;
    }

    @Override
    public ProductoDTO obtenerPorId(Integer idProd) {
        this.producto = new ProductoDTO();
        this.producto.setIdProducto(idProd);
        super.obtenerPorId();
        return this.producto;
    }

    @Override
    public Integer modificar(ProductoDTO producto) {
        this.producto = producto;
        return super.modificar();
    }

    @Override
    public Integer eliminar(ProductoDTO producto) {
        this.producto = producto;
        try {
            this.iniciarTransaccion();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
        ProductoTipoDAO productoTipoDAO = new ProductoTipoDAOImpl(this.conexion);
        ArrayList<ProductoTipoDTO> productos = producto.getProductosTipos();
        for(ProductoTipoDTO prod : productos)
            productoTipoDAO.eliminar(prod, true, true);

        return super.eliminar(false, true);
    }

}
