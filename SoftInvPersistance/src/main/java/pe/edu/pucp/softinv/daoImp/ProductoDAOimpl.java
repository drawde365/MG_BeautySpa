package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProducto;

import java.sql.*;
import java.util.List;

public class ProductoDAOimpl extends DAOImplBase implements ProductoDAO {

    private ProductoDTO producto;

    public ProductoDAOimpl() {
        super("PRODUCTOS");
        this.producto = null;
        this.retornarLlavePrimaria = true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PRODUCTO_ID", true, true));
        this.listaColumnas.add(new Columna("NOMBRE", false, false));
        this.listaColumnas.add(new Columna("TIPO_PRODUCTO", false, false));
        this.listaColumnas.add(new Columna("PRECIO", false, false));
        this.listaColumnas.add(new Columna("DESCRIPCION", false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN", false, false));
        this.listaColumnas.add(new Columna("STOCK", false, false));
        this.listaColumnas.add(new Columna("MODO_DE_USO", false, false));
        this.listaColumnas.add(new Columna("PROM_VALORACIONES", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setString(1, producto.getNombre());
        this.statement.setString(2, producto.getTipoProductoS());
        this.statement.setDouble(3, producto.getPrecio());
        this.statement.setString(4, producto.getDescripcion());
        this.statement.setString(5, producto.getUrlImagen());
        this.statement.setInt(6, producto.getStock());
        this.statement.setString(7, producto.getModoUso());
        this.statement.setDouble(8, producto.getPromedioValoracion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setString(1, producto.getNombre());
        this.statement.setString(2, producto.getTipoProductoS());
        this.statement.setDouble(3, producto.getPrecio());
        this.statement.setString(4, producto.getDescripcion());
        this.statement.setString(5, producto.getUrlImagen());
        this.statement.setInt(6, producto.getStock());
        this.statement.setString(7, producto.getModoUso());
        this.statement.setDouble(8, producto.getPromedioValoracion());
        this.statement.setInt(9, producto.getIdProducto());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, producto.getIdProducto());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.producto.getIdProducto());
    }

    private TipoProducto EscogerTipoProducto(String tipo) {
        if (tipo == TipoProducto.CORPORAL.name())
            return TipoProducto.CORPORAL;
        else if (tipo == TipoProducto.FACIAL.name())
            return TipoProducto.FACIAL;
        else return null;
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.producto = new ProductoDTO();
        this.producto.setIdProducto(this.resultSet.getInt("PRODUCTO_ID"));
        this.producto.setNombre(this.resultSet.getString("NOMBRE"));
        String tipo = this.resultSet.getString("TIPO_PRODUCTO");
        TipoProducto type = EscogerTipoProducto(tipo);
        this.producto.setTipoProducto(type);
        this.producto.setPrecio(this.resultSet.getDouble("PRECIO"));
        this.producto.setDescripcion(this.resultSet.getString("DESCRIPCION"));
        this.producto.setUrlImagen(this.resultSet.getString("URL_IMAGEN"));
        this.producto.setStock(this.resultSet.getInt("STOCK"));
        this.producto.setModoUso(this.resultSet.getString("MODO_DE_USO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.producto = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(this.producto);
    }

    @Override
    public Integer insertar(ProductoDTO producto) {
        this.producto = producto;
        return super.insertar();
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
        return super.eliminar();
    }

}
