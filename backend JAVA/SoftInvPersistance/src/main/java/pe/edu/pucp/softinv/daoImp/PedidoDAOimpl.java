package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.DetallePedidoDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;
import pe.edu.pucp.softinv.model.Producto.TipoProdDTO;

import java.sql.*;
import java.util.Date;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

public class PedidoDAOimpl extends DAOImplBase implements PedidoDAO {

    private PedidoDTO pedido;
    private DetallePedidoDAO detallePedidoDAO;

    public PedidoDAOimpl() {
        super("PEDIDOS");
        this.pedido = null;
        this.retornarLlavePrimaria = true;
        detallePedidoDAO = new DetallePedidoDAOImpl();
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("PEDIDO_ID", true, true));
        this.listaColumnas.add(new Columna("CLIENTE_ID", false, false));
        this.listaColumnas.add(new Columna("TOTAL", false, false));
        this.listaColumnas.add(new Columna("ESTADO", false, false));
        this.listaColumnas.add(new Columna("FECHA_PAGO", false, false));
        this.listaColumnas.add(new Columna("FECHA_LISTA_PARA_RECOGER", false, false));
        this.listaColumnas.add(new Columna("FECHA_RECOJO", false, false));
        this.listaColumnas.add(new Columna("IGV", false, false));
        this.listaColumnas.add(new Columna("CODTR", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, pedido.getIDCliente());
        this.statement.setDouble(2, pedido.getTotal());
        this.statement.setString(3, pedido.getEstadoPedidoS());
        this.setFechaEnST(4, (Date) pedido.getFechaPago());
        this.setFechaEnST(5, (Date) pedido.getFechaListaParaRecojo());
        this.setFechaEnST(6, (Date) pedido.getFechaRecojo());
        this.setDoubleEnST(7, pedido.getIGV());
        this.setStringEnST(8, pedido.getCodigoTransaccion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, pedido.getIDCliente());
        this.statement.setDouble(2, pedido.getTotal());
        this.statement.setString(3, pedido.getEstadoPedidoS());
        this.setFechaEnST(4, (Date) pedido.getFechaPago());
        this.setFechaEnST(5, (Date) pedido.getFechaListaParaRecojo());
        this.setFechaEnST(6, (Date) pedido.getFechaRecojo());
        this.setDoubleEnST(7, pedido.getIGV());
        this.setStringEnST(8, pedido.getCodigoTransaccion());
        this.statement.setInt(9, pedido.getIdPedido());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, pedido.getIdPedido());
    }

    private void setFechaEnST(int indice, Date fecha) throws SQLException {
        if (fecha != null)
            this.statement.setTimestamp(indice, new Timestamp(fecha.getTime()));
        else
            this.statement.setNull(indice, java.sql.Types.DATE);
    }

    private void setDoubleEnST(int indice, Double valor) throws SQLException {
        if (valor != null) {
            this.statement.setDouble(indice, valor);
        } else {
            this.statement.setNull(indice, java.sql.Types.DOUBLE);
        }
    }

    private void setStringEnST(int indice, String valor) throws SQLException {
        if (valor != null) {
            this.statement.setString(indice, valor);
        } else {
            this.statement.setNull(indice, java.sql.Types.VARCHAR);
        }
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.pedido.getIdPedido());
    }

    private EstadoPedido DefinirEstado(String estado) {
        if (estado == null) return null;
        try {
            return EstadoPedido.valueOf(estado);
        } catch (IllegalArgumentException e) {
            if (estado.equals("EnCarrito")) return EstadoPedido.EnCarrito;
            return null;
        }
    }

    protected int instanciarObjetoDelResultSetEspecial() throws SQLException {
        int result = 0;
        int idPed = resultSet.getInt("PEDIDO_ID");
        if (this.pedido == null || !this.pedido.getIdPedido().equals(idPed) || this.pedido.getEstadoPedido() == null) {
            this.pedido = this.instanciarPedidoDelResultSet(idPed);
            result = 1;
        }
        DetallePedidoDTO detalle = this.instanciarDetallePedidoDelResultSet();
        
        if(detalle.getProducto() != null && detalle.getProducto().getProducto() != null && detalle.getProducto().getProducto().getIdProducto() != 0){
             this.pedido.agregarDetallesPedido(detalle);
        }
        
        return result;
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.pedido = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        int num = this.instanciarObjetoDelResultSetEspecial();
        if (num == 1)
            lista.add(this.pedido);
    }

    private PedidoDTO instanciarPedidoDelResultSet(Integer idPed) throws SQLException {
        PedidoDTO pedido = new PedidoDTO();
        pedido.setIdPedido(idPed);
        pedido.setTotal(this.resultSet.getDouble("TOTAL"));
        String estado = this.resultSet.getString("ESTADO");
        EstadoPedido state = DefinirEstado(estado);
        pedido.setEstadoPedido(state);
        pedido.setFechaPago(this.resultSet.getDate("FECHA_PAGO"));
        pedido.setFechaListaParaRecojo(this.resultSet.getDate("FECHA_LISTA_PARA_RECOGER"));
        pedido.setFechaRecojo(this.resultSet.getDate("FECHA_RECOJO"));
        pedido.setIGV(this.resultSet.getDouble("IGV"));
        pedido.setCodigoTransaccion(this.resultSet.getString("CODTR"));
        ArrayList<DetallePedidoDTO> detalles = new ArrayList<>();
        pedido.setDetallesPedido(detalles);

        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(this.resultSet.getInt("CLIENTE_ID"));
        cliente.setNombre(this.resultSet.getString("Cliente_Nombre"));
        cliente.setPrimerapellido(this.resultSet.getString("Cliente_Primer_Apellido"));
        cliente.setSegundoapellido(this.resultSet.getString("Cliente_Segundo_Apellido"));
        cliente.setCorreoElectronico(this.resultSet.getString("Cliente_Correo"));
        cliente.setCelular(this.resultSet.getString("Cliente_Celular"));
        pedido.setCliente(cliente);
        return pedido;
    }

    private DetallePedidoDTO instanciarDetallePedidoDelResultSet() throws SQLException {
        DetallePedidoDTO detalle = new DetallePedidoDTO();
        detalle.setPedido(this.pedido);
        
        int prodId = this.resultSet.getInt("PRODUCTO_ID");
        if(this.resultSet.wasNull()) return detalle;

        detalle.setCantidad(this.resultSet.getInt("CANTIDAD"));
        detalle.setSubtotal(this.resultSet.getDouble("SUBTOTAL"));

        TipoProdDTO tipoProd = new TipoProdDTO();
        tipoProd.setId(this.resultSet.getInt("TIPO_ID"));
        tipoProd.setNombre(this.resultSet.getString("TIPO_NOMBRE"));

        ProductoTipoDTO productoT = new ProductoTipoDTO();
        productoT.setTipo(tipoProd);
        
        productoT.setStock_fisico(this.resultSet.getInt("STOCK_FISICO"));
        productoT.setStock_despacho(this.resultSet.getInt("STOCK_DESPACHO"));
        productoT.setIngredientes(this.resultSet.getString("INGREDIENTES"));
        productoT.setActivo(this.resultSet.getInt("PT_ACTIVO"));

        ProductoDTO producto = new ProductoDTO();
        producto.setIdProducto(prodId);
        producto.setNombre(this.resultSet.getString("Producto_Nombre"));
        producto.setUrlImagen(this.resultSet.getString("Producto_Imagen"));
        producto.setPrecio(this.resultSet.getDouble("Producto_Precio"));
        producto.setTamanho(this.resultSet.getDouble("Producto_Tamanho"));
        productoT.setProducto(producto);

        detalle.setProducto(productoT);
        return detalle;
    }

    @Override
    public Integer insertar(PedidoDTO pedido) {
        Double total = 0.0;
        this.pedido = pedido;
        Integer resultado = super.insertar(true, false);
        this.pedido.setIdPedido(resultado);
        if (pedido.getDetallesPedido() != null) {
            ArrayList<DetallePedidoDTO> detallesPedido = this.pedido.getDetallesPedido();
            DetallePedidoDAO detalleDAO = new DetallePedidoDAOImpl(this.conexion);
            for (DetallePedidoDTO detallePedido : detallesPedido) {
                detallePedido.setPedido(this.pedido);
                total += detallePedido.getSubtotal();
                detalleDAO.insertar(detallePedido, true, true);
            }
        }
        this.pedido.setTotal(total);
        super.modificar(false, true);
        return resultado;
    }

    @Override
    public PedidoDTO obtenerPorId(Integer idPedido) {
        this.pedido = new PedidoDTO();
        this.pedido.setIdPedido(-1);
        String sql = this.ObtenerQueryPorId();
        ArrayList<PedidoDTO> pedidoLista = (ArrayList<PedidoDTO>) super.listarTodos(sql, this::incluirValoresDeParametrosSimpleId, idPedido);
        this.pedido = pedidoLista.isEmpty() ? null : pedidoLista.get(0);
        return this.pedido;
    }

    @Override
    public PedidoDTO obtenerCarritoPorCliente(Integer idCliente) {
        this.pedido = new PedidoDTO();
        this.pedido.setIdPedido(-1);
        String sql = this.ObtenerQueryCarrito();
        ArrayList<PedidoDTO> carritoLista = (ArrayList<PedidoDTO>) super.listarTodos(sql, this::incluirValoresDeParametrosSimpleId, idCliente);

        if (carritoLista.isEmpty()) {
            return null;
        } else {
            this.pedido = carritoLista.get(0);
            return this.pedido;
        }
    }

    private void incluirValoresDeParametrosSimpleId(Object objetoParametros) {
        Integer id = (Integer) objetoParametros;
        try {
            this.statement.setInt(1, id);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Integer modificar(PedidoDTO pedido) {
        this.pedido = pedido;
        return super.modificar();
    }

    @Override
    public Integer eliminar(PedidoDTO pedido) {
        this.pedido = pedido;
        try {
            this.iniciarTransaccion();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
        DetallePedidoDAO detallePedidoDAO = new DetallePedidoDAOImpl(this.conexion);
        ArrayList<DetallePedidoDTO> detallesPedido = this.pedido.getDetallesPedido();
        for (DetallePedidoDTO detalle : detallesPedido) {
            detallePedidoDAO.eliminar(detalle, true, true);
        }

        return super.eliminar(false, true);
    }

    @Override
    public ArrayList<PedidoDTO> listarPedidos(Integer idCliente) {
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.MONTH, -6);
        java.sql.Date fechaLimite = new java.sql.Date(cal.getTimeInMillis());

        Object[] parametros = new Object[]{idCliente, fechaLimite};

        String sql = this.ObtenerQueryPorCliente();
        this.pedido = new PedidoDTO();
        this.pedido.setIdPedido(-1);
        
        return (ArrayList<PedidoDTO>) super.listarTodos(sql, this::configurarParametrosHistorial, parametros);
    }

    private void configurarParametrosHistorial(Object objetoParametros) {
        Object[] params = (Object[]) objetoParametros;
        try {
            this.statement.setInt(1, (Integer) params[0]);
            this.statement.setDate(2, (java.sql.Date) params[1]);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    @Override
    public ArrayList<PedidoDTO> listarTodoPedidos() {
        String sql = this.ObtenerBaseQueryPedidos();
        sql += this.obtenerQuerySinCarrito();
        return (ArrayList<PedidoDTO>) super.listarTodos(sql, null, null);
    }
    
    private String obtenerQuerySinCarrito() {
        String sql = "AND p.ESTADO != 'EnCarrito'";
        return sql;
    }

    private String ObtenerBaseQueryPedidos() {
        String sql = """
        SELECT 
        p.PEDIDO_ID,
        p.CLIENTE_ID,

        cli.NOMBRE AS Cliente_Nombre,
        cli.PRIMER_APELLIDO AS Cliente_Primer_Apellido,
        cli.SEGUNDO_APELLIDO AS Cliente_Segundo_Apellido,
        cli.CORREO_ELECTRONICO AS Cliente_Correo,
        cli.CELULAR AS Cliente_Celular,

        p.FECHA_PAGO,
        p.FECHA_LISTA_PARA_RECOGER,
        p.FECHA_RECOJO,
        p.TOTAL,
        p.IGV,
        p.ESTADO,
        p.CODTR,

        dp.PRODUCTO_ID,
        dp.TIPO_ID,
        tp.NOMBRE AS TIPO_NOMBRE,
        pr.NOMBRE AS Producto_Nombre,
        pr.URL_IMAGEN AS Producto_Imagen,
        pr.PRECIO AS Producto_Precio, 
        pr.TAMANHO AS Producto_Tamanho,    
        dp.CANTIDAD,
        dp.SUBTOTAL,
        
        pt.STOCK_FISICO, 
        pt.STOCK_DESPACHO, 
        pt.INGREDIENTES, 
        pt.ACTIVO AS PT_ACTIVO

        FROM PEDIDOS p
        INNER JOIN USUARIOS cli ON p.CLIENTE_ID = cli.USUARIO_ID
        LEFT JOIN DETALLES_PEDIDOS dp ON p.PEDIDO_ID = dp.PEDIDO_ID
        LEFT JOIN PRODUCTOS pr ON dp.PRODUCTO_ID = pr.PRODUCTO_ID
        LEFT JOIN TIPOS_PRODS tp ON dp.TIPO_ID = tp.TIPO_ID
        LEFT JOIN PRODUCTOS_TIPOS pt ON dp.PRODUCTO_ID = pt.PRODUCTO_ID AND dp.TIPO_ID = pt.TIPO_ID
        """;
        return sql;
    }

    private String ObtenerQueryPorId() {
        return this.ObtenerBaseQueryPedidos() + " WHERE p.PEDIDO_ID = ?";
    }

    private String ObtenerQueryPorCliente() {
        return this.ObtenerBaseQueryPedidos() + """
        WHERE p.CLIENTE_ID = ?
          AND p.FECHA_PAGO >= ?
        ORDER BY p.FECHA_PAGO DESC
    """;
    }

    private String ObtenerQueryCarrito() {
        return this.ObtenerBaseQueryPedidos() + """
        WHERE p.CLIENTE_ID = ?
        AND p.ESTADO = 'EnCarrito'
    """;
    }
}