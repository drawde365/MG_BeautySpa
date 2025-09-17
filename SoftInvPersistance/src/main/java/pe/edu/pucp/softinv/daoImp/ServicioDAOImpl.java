//package pe.edu.pucp.softinv.daoImp;
//
//import pe.edu.pucp.softinv.dao.ServicioDAO;
//import pe.edu.pucp.softinv.daoImp.util.Columna;
//import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;
//import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
//
//import java.sql.Date;
//import java.sql.SQLException;
//import java.util.ArrayList;
//
//public class ServicioDAOImpl extends DAOImplBase implements ServicioDAO {
//
//    private ServicioDTO servicio;
//
//    public ServicioDAOImpl() {
//        super("PEDIDOS");
//        this.servicio = null;
//    }
//
//    @Override
//    protected void configurarListaDeColumnas() {
//        this.listaColumnas.add(new Columna("CLIENTE_ID", true, true));
//        this.listaColumnas.add(new Columna("TOTAL", false, false));
//    }
//
//
//    @Override
//    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
//        this.statement.setInt(1, servicio.getIdServicio());
//        this.statement.setString(2, servicio.getNombre());
//        this.statement.setString(3, servicio.getTipo());
//
//    }
//
//    @Override
//    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
//        //this.statement = this.conexion.prepareCall();
//        this.statement.setInt(1, servicio.getIdServicio());
//        this.statement.setString(2, servicio.getNombre());
//    }
//
//    @Override
//    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
//        this.statement.setInt(1, servicio.getIdServicio());
//    }
//
//    @Override
//    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
//        this.statement.setInt(1, this.servicio.getIdServicio());
//    }
//
//    @Override
//    protected void instanciarObjetoDelResultSet() throws SQLException {
//        int Producto_ID = this.resultSet.getInt("PRODUCTO_ID");
//        //MODIFICAR
//    }
//
//    @Override
//    protected void limpiarObjetoDelResultSet() {
//        this.servicio = null;
//    }
//
//    public Integer insertar(ServicioDTO servicio){
//        this.servicio = servicio;
//        return super.insertar();
//    }
//
//    public ServicioDTO obtenerPorId(Integer idServicio){
//        this.servicio = new ServicioDTO();
//        this.servicio.setIdServicio(idServicio);
//        super.obtenerPorId();
//        return this.servicio;
//    }
//    public Integer modificar(ServicioDTO servicio){
//        this.servicio=servicio;
//        return super.modificar();
//    }
//    public Integer eliminar(ServicioDTO servicio){
//        this.servicio=servicio;
//        return super.eliminar();
//    }
//    ArrayList<ServicioDTO> listarServiciosDeEmpleado(Integer empleadoID){
//
//    }
//}
