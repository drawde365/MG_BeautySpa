package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class PedidoDAOimpl extends DAOImplBase implements PedidoDAO {

    private PedidoDTO pedido;

    public PedidoDAOimpl() {
        super("PEDIDOS");
        this.pedido = null;
        this.retornarLlavePrimaria = true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("CLIENTE_ID", true, true));
        this.listaColumnas.add(new Columna("TOTAL", false, false));
        this.listaColumnas.add(new Columna("ESTADO", false, false));
        this.listaColumnas.add(new Columna("FECHA_PAGO", false, false));
        this.listaColumnas.add(new Columna("FECHA_LISTA_PARA_RECOGER", false, false));
        this.listaColumnas.add(new Columna("FECHA_RECOJO", false, false));
        this.listaColumnas.add(new Columna("IGV", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, pedido.getIDCliente());
        this.statement.setDouble(2, pedido.getTotal());
        this.statement.setString(3, pedido.getEstadoPedidoS());
        this.statement.setDate(4, (Date) pedido.getFechaPago());
        this.statement.setDate(5, (Date) pedido.getFechaListaParaRecojo());
        this.statement.setDate(6, (Date) pedido.getFechaRecojo());
        this.statement.setDouble(7, pedido.getIGV());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, pedido.getIDCliente());
        this.statement.setDouble(2, pedido.getTotal());
        this.statement.setString(3, pedido.getEstadoPedidoS());
        this.statement.setDate(4, (Date) pedido.getFechaPago());
        this.statement.setDate(5, (Date) pedido.getFechaListaParaRecojo());
        this.statement.setDate(6, (Date) pedido.getFechaRecojo());
        this.statement.setDouble(7, pedido.getIGV());
        this.statement.setInt(8,pedido.getIdPedido());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, pedido.getIdPedido());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.pedido.getIdPedido());
    }

    private EstadoPedido DefinirEstado(String estado){
        if (estado == EstadoPedido.CANCELADO.name())
            return EstadoPedido.CANCELADO;
        else if (estado == EstadoPedido.CONFIRMADO.name())
            return EstadoPedido.CONFIRMADO;
        else if (estado == EstadoPedido.LISTO_PARA_RECOGER.name())
            return EstadoPedido.LISTO_PARA_RECOGER;
        else if (estado == EstadoPedido.NO_RECOGIDO.name())
            return EstadoPedido.NO_RECOGIDO;
        else if (estado == EstadoPedido.RECOGIDO.name())
            return EstadoPedido.RECOGIDO;
        return null;
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        Integer cliente_ID = this.resultSet.getInt("CLIENTE_ID");
        ClienteDTO cliente = null;
        if (cliente_ID > 0) {
            ClienteDAO clienteDAO = new ClienteDAOimpl();
            cliente = clienteDAO.obtenerPorId(cliente_ID);
        }
        pedido = new PedidoDTO();
        this.pedido.setCliente(cliente);
        this.pedido.setTotal(this.resultSet.getDouble("TOTAL"));
        String estado = this.resultSet.getString("ESTADO");
        EstadoPedido state = DefinirEstado(estado);
        this.pedido.setEstadoPedido(state);
        this.pedido.setFechaPago(this.resultSet.getDate("FECHA_PAGO"));
        this.pedido.setFechaListaParaRecojo(this.resultSet.getDate("FECHA_LISTA_PARA_RECOGO"));
        this.pedido.setFechaRecojo(this.resultSet.getDate("FECHA_RECOGO"));
        this.pedido.setIGV(this.resultSet.getDouble("IGV"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.pedido = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(this.pedido);
    }

    @Override
    public Integer insertar(PedidoDTO pedido) {
        try {
            this.pedido = pedido;
            for (DetallePedidoDTO detalle : pedido.getDetallesPedido()) {
                String sql = "INSERT INTO DETALLES_PEDIDOS (PEDIDO_ID, PRODUCTO_ID, CANTIDAD, SUBTOTAL) " +
                        "VALUES (?,?,?,?)";
                this.statement = this.conexion.prepareCall(sql);
                this.statement.setInt(1, detalle.getPedido().getIdPedido());
                this.statement.setInt(2, detalle.getProducto().getIdProducto());
                this.statement.setInt(3, detalle.getCantidad());
                this.statement.setDouble(4, detalle.getSubtotal());
                this.statement.executeUpdate();
            }
            return super.insertar();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public PedidoDTO obtenerPorId(Integer idPedido) {
        this.pedido = new PedidoDTO();
        this.pedido.setIdPedido(idPedido);
        super.obtenerPorId();
        return this.pedido;
    }

    @Override
    public Integer modificar(PedidoDTO pedido) {
        this.pedido = pedido;
        return super.modificar();
    }

    @Override
    public Integer eliminar(PedidoDTO pedido) {
        this.pedido = pedido;
        return super.eliminar();
    }

    @Override
    public ArrayList<PedidoDTO> listarPedidos(Integer idCliente) {
        ArrayList<PedidoDTO> lista = new ArrayList<>();
        try {
            this.abrirConexion();
            String sql = this.generarSQLParaListarTodos();
            sql+=" WHERE CLIENTE_ID";
            this.colocarSQLEnStatement(sql);
            this.ejecutarSelectEnDB();
            while (this.resultSet.next()) {
                this.agregarObjetoALaLista(lista);
            }
        } catch (SQLException ex) {
            System.err.println("Error al intentar listarTodos - " + ex);
        } finally {
            try {
                this.cerrarConexion();
            } catch (SQLException ex) {
                System.err.println("Error al cerrar la conexión - " + ex);
            }
        }
        return lista;
    }

}