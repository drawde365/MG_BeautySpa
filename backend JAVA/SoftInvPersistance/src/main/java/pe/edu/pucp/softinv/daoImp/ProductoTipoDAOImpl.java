package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.DetallePedidoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProdDTO;

import java.sql.Connection;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class DetallePedidoDAOImpl extends DAOImplBase implements DetallePedidoDAO {

    DetallePedidoDTO detallePedido;

    public DetallePedidoDAOImpl() {
        super("DETALLES_PEDIDOS");
        this.detallePedido = null;
        this.retornarLlavePrimaria = false;
    }

    public DetallePedidoDAOImpl(Connection c) {
        super("DETALLES_PEDIDOS", c);
        this.detallePedido = null;
        this.retornarLlavePrimaria = false;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PEDIDO_ID", true, false));
        this.listaColumnas.add(new Columna("PRODUCTO_ID", true, false));
        this.listaColumnas.add(new Columna("TIPO_ID", true, false));
        this.listaColumnas.add(new Columna("CANTIDAD", false, false));
        this.listaColumnas.add(new Columna("SUBTOTAL", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setInt(3, detallePedido.getProducto().getTipo().getId());
        this.statement.setInt(4, detallePedido.getCantidad());
        this.statement.setDouble(5, detallePedido.getSubtotal());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, detallePedido.getCantidad());
        this.statement.setDouble(2, detallePedido.getSubtotal());
        
        this.statement.setInt(3, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(4, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setInt(5, detallePedido.getProducto().getTipo().getId());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setInt(3, detallePedido.getProducto().getTipo().getId());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setInt(3, detallePedido.getProducto().getTipo().getId());
    }

    private void incluirValoresDeParametrosParaListarPorPedido(Object objetoParametros) {
        PedidoDTO pedido = (PedidoDTO) objetoParametros;
        try {
            this.statement.setInt(1, pedido.getIdPedido());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public ArrayList<DetallePedidoDTO> obtenerDetallesPedidosId(Integer idPedido) {
        // Se agrega el JOIN a PRODUCTOS_TIPOS para obtener el stock y datos extra
        String sql = "SELECT dp.*, tp.NOMBRE AS NOMBRE_TIPO, " +
                     "pt.STOCK_FISICO, pt.STOCK_DESPACHO, pt.INGREDIENTES, pt.ACTIVO AS ACTIVO_TIPO " +
                     "FROM DETALLES_PEDIDOS dp " +
                     "JOIN TIPOS_PRODS tp ON dp.TIPO_ID = tp.TIPO_ID " +
                     "JOIN PRODUCTOS_TIPOS pt ON dp.PRODUCTO_ID = pt.PRODUCTO_ID AND dp.TIPO_ID = pt.TIPO_ID " +
                     "WHERE dp.PEDIDO_ID = ?";
                     
        PedidoDTO pedido = new PedidoDTO();
        pedido.setIdPedido(idPedido);
        return (ArrayList<DetallePedidoDTO>) super.listarTodos(sql, this::incluirValoresDeParametrosParaListarPorPedido, pedido);
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.detallePedido = new DetallePedidoDTO();
        ProductoTipoDTO productoTipo = new ProductoTipoDTO();
        ProductoDTO producto = new ProductoDTO();
        PedidoDTO pedido = new PedidoDTO();
        
        TipoProdDTO tipoProd = new TipoProdDTO(); 
        
        // Mapeo de IDs básicos
        tipoProd.setId(this.resultSet.getInt("TIPO_ID"));
        producto.setIdProducto(resultSet.getInt("PRODUCTO_ID"));
        pedido.setIdPedido(resultSet.getInt("PEDIDO_ID"));

        // Mapeo de datos obtenidos via JOIN (Nombre del Tipo)
        if(columnaExiste(resultSet, "NOMBRE_TIPO")){
            tipoProd.setNombre(this.resultSet.getString("NOMBRE_TIPO"));
        }

        // Mapeo de datos extendidos de ProductoTipo (Stock, Ingredientes, etc.)
        // Se usa columnaExiste para evitar errores si se usa el metodo obtener() basico sin el JOIN
        if(columnaExiste(resultSet, "STOCK_FISICO")) {
            productoTipo.setStock_fisico(resultSet.getInt("STOCK_FISICO"));
        }
        if(columnaExiste(resultSet, "STOCK_DESPACHO")) {
            productoTipo.setStock_despacho(resultSet.getInt("STOCK_DESPACHO"));
        }
        if(columnaExiste(resultSet, "INGREDIENTES")) {
            productoTipo.setIngredientes(resultSet.getString("INGREDIENTES"));
        }
        if(columnaExiste(resultSet, "ACTIVO_TIPO")) {
            productoTipo.setActivo(resultSet.getInt("ACTIVO_TIPO"));
        }

        // Armado de objetos
        productoTipo.setProducto(producto);
        productoTipo.setTipo(tipoProd);

        this.detallePedido.setProducto(productoTipo);
        this.detallePedido.setPedido(pedido);
        this.detallePedido.setCantidad(resultSet.getInt("CANTIDAD"));
        this.detallePedido.setSubtotal(resultSet.getDouble("SUBTOTAL"));
    }
    
    private boolean columnaExiste(java.sql.ResultSet rs, String nombreColumna) throws SQLException {
        java.sql.ResultSetMetaData metaData = rs.getMetaData();
        int count = metaData.getColumnCount();
        for (int i = 1; i <= count; i++) {
            if (metaData.getColumnLabel(i).equalsIgnoreCase(nombreColumna)) {
                return true;
            }
        }
        return false;
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.detallePedido = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(detallePedido);
    }

    @Override
    public Integer insertar(DetallePedidoDTO detallePedido) {
        this.detallePedido = detallePedido;
        return super.insertar();
    }

    @Override
    public Integer insertar(DetallePedidoDTO detallePedido, boolean dejarConexionAbierta, boolean transaccionInciada) {
        this.detallePedido = detallePedido;
        return super.insertar(dejarConexionAbierta, transaccionInciada);
    }

    @Override
    public DetallePedidoDTO obtener(Integer idPedido, Integer idProducto, Integer tipoId) {
        PedidoDTO pedido = new PedidoDTO();
        pedido.setIdPedido(idPedido);
        
        ProductoTipoDTO productoTipo = new ProductoTipoDTO();
        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(idProducto);
        
        TipoProdDTO tipoProd = new TipoProdDTO();
        tipoProd.setId(tipoId);

        productoTipo.setProducto(producto);
        productoTipo.setTipo(tipoProd);
        
        if(this.detallePedido == null) this.detallePedido = new DetallePedidoDTO(); 
        
        this.detallePedido.setPedido(pedido);
        this.detallePedido.setProducto(productoTipo);
        
        super.obtenerPorId();
        return detallePedido;
    }

    @Override
    public Integer modificar(DetallePedidoDTO detallePedido) {
        this.detallePedido = detallePedido;
        return super.modificar();
    }

    @Override
    public Integer eliminar(DetallePedidoDTO detallePedido) {
        this.detallePedido = detallePedido;
        return super.eliminar();
    }

    @Override
    public Integer eliminar(DetallePedidoDTO detallePedido, boolean dejarConexionAbierta, boolean transaccionInciada) {
        this.detallePedido = detallePedido;
        return super.eliminar(dejarConexionAbierta, transaccionInciada);
    }
}