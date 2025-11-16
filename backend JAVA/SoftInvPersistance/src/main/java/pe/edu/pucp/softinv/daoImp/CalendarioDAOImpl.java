package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class CalendarioDAOImpl extends DAOImplBase implements CalendarioDAO {
    CalendarioDTO calendario;

    public CalendarioDAOImpl() {
        super("CALENDARIOS_EMPLEADOS");
        this.retornarLlavePrimaria = false;
        this.calendario = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("EMPLEADO_ID", true, false));
        this.listaColumnas.add(new Columna("FECHA", true, false));
        this.listaColumnas.add(new Columna("CANT_LIBRE", false, false));
        this.listaColumnas.add(new Columna("MOTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
        this.statement.setInt(3, calendario.getCantLibre());
        this.statement.setString(4,calendario.getMotivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, calendario.getCantLibre());
        this.statement.setString(2,calendario.getMotivo());
        this.statement.setInt(3, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(4, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setTimestamp(2, new Timestamp(calendario.getFecha().getTime()));
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.calendario = new CalendarioDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(this.resultSet.getInt("EMPLEADO_ID"));

        this.calendario.setEmpleado(empleado);
        this.calendario.setCantLibre(this.resultSet.getInt("CANT_LIBRE"));
        this.calendario.setFecha(new Timestamp (this.resultSet.getTimestamp("FECHA").getTime()));
        this.calendario.setMotivo(this.resultSet.getString("MOTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.calendario = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(calendario);
    }

    @Override
    public Integer insertar(CalendarioDTO calendario) {
        this.calendario = calendario;
        return super.insertar();
    }

    @Override
    public CalendarioDTO obtenerPorId(Integer empleadoId, Date fecha) {
        this.calendario = new CalendarioDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);
        this.calendario.setEmpleado(empleado);
        this.calendario.setFecha(new java.sql.Date(fecha.getTime()));
        super.obtenerPorId();
        return this.calendario;
    }

    @Override
    public Integer modificar(CalendarioDTO calendario){
        this.calendario = calendario;
        return super.modificar();
    }

    @Override
    public Integer eliminar(CalendarioDTO calendario){
        this.calendario = calendario;
        return super.eliminar();
    }
    @Override
    public ArrayList<CalendarioDTO> listarCalendarioEnRango(Integer empleadoId, Date fechaInicio, Date fechaFin) {
        String sql = "SELECT EMPLEADO_ID, FECHA, CANT_LIBRE, MOTIVO FROM CALENDARIOS_EMPLEADOS WHERE EMPLEADO_ID = ? AND FECHA BETWEEN ? AND ?";
        
        return (ArrayList<CalendarioDTO>)listarTodos(sql, this::incluirTresParametros, new Object[]{empleadoId, fechaInicio, fechaFin});
    }
    
    @Override
    public ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(Integer empleadoId) {
        java.sql.Date hoy = new java.sql.Date(System.currentTimeMillis());
        java.sql.Date fechaFin = new java.sql.Date(System.currentTimeMillis() + (30L * 24L * 60L * 60L * 1000L * 30L)); // Aproximadamente 30 días
        
        return listarCalendarioEnRango(empleadoId, hoy, fechaFin);
    }

    private void incluirTresParametros(Object objetoParametros) {
        Object[] params = (Object[]) objetoParametros;
        try {
            this.statement.setInt(1, (Integer) params[0]);
            this.statement.setTimestamp(2, new Timestamp(((Date) params[1]).getTime()));
            this.statement.setTimestamp(3, new Timestamp(((Date) params[2]).getTime()));
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    private void incluirId(Object objetoParametros){
        Integer id = (Integer) objetoParametros;
        try {
            this.statement.setInt(1,id);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}