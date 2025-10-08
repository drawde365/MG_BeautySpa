package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.PedidoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoTipoDTO;

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
        this.statement.setDate(4, (Date) pedido.getFechaPago());
        this.statement.setDate(5, (Date) pedido.getFechaListaParaRecojo());
        this.statement.setDate(6, (Date) pedido.getFechaRecojo());
        this.statement.setDouble(7, pedido.getIGV());
        this.statement.setString(8, pedido.getCodigoTransaccion());
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
        this.statement.setString(8, pedido.getCodigoTransaccion());
        this.statement.setInt(9,pedido.getIdPedido());
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
        // EnCarrito,ELIMINADO,CONFIRMADO,LISTO_PARA_RECOGER, RECOGIDO, NO_RECOGIDO
        if (estado == EstadoPedido.ELIMINADO.name())
            return EstadoPedido.ELIMINADO;
        else if (estado == EstadoPedido.CONFIRMADO.name())
            return EstadoPedido.CONFIRMADO;
        else if (estado == EstadoPedido.LISTO_PARA_RECOGER.name())
            return EstadoPedido.LISTO_PARA_RECOGER;
        else if (estado == EstadoPedido.NO_RECOGIDO.name())
            return EstadoPedido.NO_RECOGIDO;
        else if (estado == EstadoPedido.RECOGIDO.name())
            return EstadoPedido.RECOGIDO;
        else if (estado == EstadoPedido.EnCarrito.name())
            return EstadoPedido.EnCarrito;
        return null;
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(this.resultSet.getInt("CLIENTE_ID"));
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
        /*
            Falta incluir detalles pedidos
        */
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
        this.pedido = pedido;
        Integer resultado = super.insertar(true,false);
        this.pedido.setIdPedido(resultado);
        ArrayList<DetallePedidoDTO> detallesPedido = this.pedido.getDetallesPedido();
        DetallePedidoDAO detalleDAO = new DetallePedidoDAOImpl(this.conexion);
        for(DetallePedidoDTO detallePedido : detallesPedido){
            detallePedido.setPedido(this.pedido);
            detalleDAO.insertar(detallePedido,true,true);
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
        String sql = "SELECT * FROM PEDIDOS WHERE CLIENTE_ID = ?";
        return (ArrayList<PedidoDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPedido,idCliente);
    }

    private void incluirValoresDeParametrosParaListarPedido(Object objetoParametros){
        Integer idCliente = (Integer) objetoParametros;
        try {
            this.statement.setInt(1,idCliente);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

}