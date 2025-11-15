package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
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
        this.statement.setDate(2, (java.sql.Date) calendario.getFecha());
        this.statement.setInt(3, calendario.getCantLibre());
        this.statement.setString(4,calendario.getMotivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, calendario.getCantLibre());
        this.statement.setString(2,calendario.getMotivo());
        this.statement.setInt(3, calendario.getEmpleado().getIdUsuario());
        this.statement.setDate(4, (java.sql.Date) calendario.getFecha());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setDate(2, (java.sql.Date) calendario.getFecha());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, calendario.getEmpleado().getIdUsuario());
        this.statement.setDate(2, (java.sql.Date) calendario.getFecha());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.calendario = new CalendarioDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(this.resultSet.getInt("EMPLEADO_ID"));

        this.calendario.setEmpleado(empleado);
        this.calendario.setCantLibre(this.resultSet.getInt("CANT_LIBRE"));
        this.calendario.setFecha(this.resultSet.getDate("FECHA"));
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
        this.calendario.setFecha((java.sql.Date) fecha);
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
    public ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(Integer empleadoId) {
        String sql = "SELECT EMPLEADO_ID, FECHA, CANT_LIBRE, MOTIVO FROM CALENDARIOS_EMPLEADOS WHERE EMPLEADO_ID = ?" +
                "  AND FECHA BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 30 DAY)";
        return (ArrayList<CalendarioDTO>)listarTodos(sql, this::incluirId, empleadoId);
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
