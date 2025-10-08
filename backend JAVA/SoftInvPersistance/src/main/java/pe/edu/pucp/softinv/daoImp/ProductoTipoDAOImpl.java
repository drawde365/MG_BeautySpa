package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ProductoTipoDAO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;
import pe.edu.pucp.softinv.daoImp.util.Columna;

import java.sql.Connection;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class ProductoTipoDAOImpl extends DAOImplBase implements ProductoTipoDAO {

    ProductoTipoDTO productoTipo;

    public ProductoTipoDAOImpl() {
        super("PRODUCTOS_TIPOS");
        this.productoTipo = null;
    }

    public ProductoTipoDAOImpl(Connection conexion) {
        super("PRODUCTOS_TIPOS",conexion);
        this.productoTipo = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PRODUCTO_ID", true, false));
        this.listaColumnas.add(new Columna("TIPO_PRODUCTO", true, false));
        this.listaColumnas.add(new Columna("STOCK_FISICO", false, false));
        this.listaColumnas.add(new Columna("STOCK_DESPACHO", false, false));
        this.listaColumnas.add(new Columna("INGREDIENTES", false, false));
        this.listaColumnas.add(new Columna("ACTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, productoTipo.getProducto().getIdProducto());
        this.statement.setString(2, productoTipo.getTipo());
        this.statement.setInt(3, productoTipo.getStock_fisico());
        this.statement.setInt(4, productoTipo.getStock_despacho());
        this.statement.setString(5, productoTipo.getIngredientes());
        this.statement.setInt(6, productoTipo.getActivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, productoTipo.getStock_fisico());
        this.statement.setInt(2, productoTipo.getStock_despacho());
        this.statement.setString(3, productoTipo.getIngredientes());
        this.statement.setInt(4, productoTipo.getActivo());
        this.statement.setInt(5, productoTipo.getProducto().getIdProducto());
        this.statement.setString(6, productoTipo.getTipo());

    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, productoTipo.getProducto().getIdProducto());
        this.statement.setString(2, productoTipo.getTipo());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, productoTipo.getProducto().getIdProducto());
        this.statement.setString(2, productoTipo.getTipo());
    }

    private void incluirValoresDeParametrosParaListarPorProducto(Object objetoParametros){
        ProductoDTO producto = (ProductoDTO) objetoParametros;
        try {
            this.statement.setInt(1,producto.getIdProducto());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.productoTipo = new ProductoTipoDTO();
        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(resultSet.getInt("PRODUCTO_ID"));
        this.productoTipo.setTipo(this.resultSet.getString("TIPO_PRODUCTO"));
        this.productoTipo.setProducto(producto);
        this.productoTipo.setIngredientes(this.resultSet.getString("INGREDIENTES"));
        this.productoTipo.setStock_fisico(this.resultSet.getInt("STOCK_FISICO"));
        this.productoTipo.setStock_despacho(this.resultSet.getInt("STOCK_DESPACHO"));
        this.productoTipo.setActivo(this.resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.productoTipo = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(productoTipo);
    }

    @Override
    public Integer insertar(ProductoTipoDTO productoTipo) {
        this.productoTipo = productoTipo;
        return super.insertar();
    }

    @Override
    public Integer insertar(ProductoTipoDTO productoTipo, boolean dejarConexionAbierta, boolean transaccionInciada) {
        this.productoTipo = productoTipo;
        return super.insertar(dejarConexionAbierta, transaccionInciada);
    }
    
    @Override
    public ProductoTipoDTO obtener(Integer id, String tipo){
        this.productoTipo = new ProductoTipoDTO();
        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(id);
        this.productoTipo.setProducto(producto);
        this.productoTipo.setTipo(tipo);
        super.obtenerPorId();
        return this.productoTipo;
    }
    
    @Override
    public Integer modificar(ProductoTipoDTO productoTipo){
        this.productoTipo = productoTipo;
        return super.modificar();
    }
    
    @Override
    public Integer eliminar(ProductoTipoDTO productoTipo){
        this.productoTipo = productoTipo;
        return super.eliminar();
    }

    @Override
    public Integer eliminar(ProductoTipoDTO productoTipo, boolean dejarConexionAbierta, boolean transaccionInciada){
        this.productoTipo = productoTipo;
        return super.eliminar(dejarConexionAbierta, transaccionInciada);
    }

    @Override
    public ArrayList<ProductoTipoDTO> obtenerProductoId(Integer idProducto) {
        String sql = "SELECT * FROM PRODUCTOS_TIPOS WHERE PRODUCTO_ID = ?";
        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(idProducto);
        return (ArrayList<ProductoTipoDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPorProducto,producto);
    }

}
