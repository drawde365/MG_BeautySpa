package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.dao.DetallePedidoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class DetallePedidoDAOImpl extends DAOImplBase implements DetallePedidoDAO {

    DetallePedidoDTO detallePedido;

    public DetallePedidoDAOImpl() {
        super("DETALLES_PEDIDOS");
        this.detallePedido = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PEDIDO_ID", true, false));
        this.listaColumnas.add(new Columna("PRODUCTO_ID", true, false));
        this.listaColumnas.add(new Columna("TIPO_PRODUCTO", true, false));
        this.listaColumnas.add(new Columna("CANTIDAD", false, false));
        this.listaColumnas.add(new Columna("SUBTOTAL", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setString(3, detallePedido.getProducto().getTipo());
        this.statement.setInt(4, detallePedido.getCantidad());
        this.statement.setDouble(5, detallePedido.getSubtotal());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setString(3, detallePedido.getProducto().getTipo());
        this.statement.setInt(4, detallePedido.getCantidad());
        this.statement.setDouble(5, detallePedido.getSubtotal());

    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setString(3, detallePedido.getProducto().getTipo());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, detallePedido.getPedido().getIdPedido());
        this.statement.setInt(2, detallePedido.getProducto().getProducto().getIdProducto());
        this.statement.setString(3, detallePedido.getProducto().getTipo());
    }

    private void incluirValoresDeParametrosParaListarPorPedido(Object objetoParametros){
        ProductoDTO producto = (ProductoDTO) objetoParametros;
        try {
            this.statement.setInt(1,producto.getIdProducto());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public ArrayList<ProductoTipoDTO> obtenerDetallesPedidosId(Integer idPedido) {
        String sql = "SELECT * FROM DETALLES_PEDIDOS WHERE PEDIDO_ID = ?";
        DetallePedidoDTO detallePedido = new DetallePedidoDTO();
        detallePedido.getPedido().setIdPedido(idPedido);
        return (ArrayList<ProductoTipoDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPorPedido,detallePedido);
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.detallePedido = new DetallePedidoDTO();
        ProductoTipoDTO productoTipo = new ProductoTipoDTO();
        ProductoDTO producto = new ProductoDTO();
        PedidoDTO pedido = new PedidoDTO();

        producto.setIdProducto(resultSet.getInt("PRODUCTO_ID"));
        productoTipo.setProducto(producto);
        productoTipo.setTipo(this.resultSet.getString("TIPO_PRODUCTO"));
        this.detallePedido.setProducto(productoTipo);
        this.detallePedido.setProducto(productoTipo);
        pedido.setIdPedido(resultSet.getInt("PEDIDO_ID"));

        this.detallePedido.setCantidad(resultSet.getInt("CANTIDAD"));
        this.detallePedido.setSubtotal(resultSet.getDouble("SUBTOTAL"));
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

    }
    @Override
    public DetallePedidoDTO obtener(Integer idPedido, Integer idProducto, String tipoProducto){

    }
    @Override
    public Integer modificar(DetallePedidoDTO detallePedido){

    }
    @Override
    public Integer eliminar(DetallePedidoDTO detallePedido){

    }
    @Override
    public ArrayList<DetallePedidoDTO> obtenerProductoId(Integer idPedido, Integer idProducto, String tipoProducto){

    }

}
