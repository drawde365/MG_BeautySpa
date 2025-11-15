package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;

public class HorarioTrabajoDAOImpl extends DAOImplBase implements HorarioTrabajoDAO {
    HorarioTrabajoDTO horarioTrabajo;

    public HorarioTrabajoDAOImpl() {
        super("HORARIO_TRABAJO");
        this.retornarLlavePrimaria = true;
        this.horarioTrabajo = null;
    }
    
    public HorarioTrabajoDAOImpl(Connection conexion) {
        super("HORARIO_TRABAJO", conexion);
        this.horarioTrabajo = null;
        this.retornarLlavePrimaria = false;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("HORARIO_ID", true, true));
        this.listaColumnas.add(new Columna("EMPLEADO_ID", false, false));
        this.listaColumnas.add(new Columna("DIA_SEMANA", false, false));
        this.listaColumnas.add(new Columna("HORA_INICIO", false, false));
        this.listaColumnas.add(new Columna("HORA_FIN", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(2, horarioTrabajo.getDiaSemana());
        this.statement.setTimestamp(3, new Timestamp (horarioTrabajo.getHoraInicio().getTime()));
        this.statement.setTimestamp(4, new Timestamp (horarioTrabajo.getHoraFin().getTime()));
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(2, horarioTrabajo.getDiaSemana());
        this.statement.setTimestamp(3, new Timestamp (horarioTrabajo.getHoraInicio().getTime()));
        this.statement.setTimestamp(4, new Timestamp (horarioTrabajo.getHoraFin().getTime()));
        this.statement.setInt(5, horarioTrabajo.getId());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getId());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getId());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.horarioTrabajo = new HorarioTrabajoDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(this.resultSet.getInt("EMPLEADO_ID"));
        this.horarioTrabajo.setEmpleado(empleado);
        this.horarioTrabajo.setDiaSemana(this.resultSet.getInt("DIA_SEMANA"));
        this.horarioTrabajo.setHoraInicio(this.resultSet.getTime("HORA_INICIO"));
        this.horarioTrabajo.setHoraFin(this.resultSet.getTime("HORA_FIN"));
        this.horarioTrabajo.setId(this.resultSet.getInt("HORARIO_ID"));
    }
    
    @Override
    protected void limpiarObjetoDelResultSet() {
        this.horarioTrabajo = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(horarioTrabajo);
    }

    @Override
    public Integer insertar(HorarioTrabajoDTO horarioTrabajo) {
        this.horarioTrabajo = horarioTrabajo;
        return super.insertar();
    }

    @Override
    public ArrayList<HorarioTrabajoDTO> obtenerPorEmpleadoYFecha(Integer empleadoId, Integer diaSemana) {
        this.horarioTrabajo = new HorarioTrabajoDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);
        this.horarioTrabajo.setEmpleado(empleado);
        this.horarioTrabajo.setDiaSemana(diaSemana);
        
        String sql = "SELECT * FROM HORARIO_TRABAJO WHERE EMPLEADO_ID = ? AND DIA_SEMANA = ?";
        
        return (ArrayList<HorarioTrabajoDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaObtenerEmpleadoYFecha,null);
    }
    
    void incluirValoresDeParametrosParaObtenerEmpleadoYFecha(Object objetoParametros) {
        try {
            this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
            this.statement.setInt(2, horarioTrabajo.getDiaSemana());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Integer modificar(HorarioTrabajoDTO horarioTrabajo){
        this.horarioTrabajo = horarioTrabajo;
        return super.modificar();
    }

    @Override
    public Integer eliminar(HorarioTrabajoDTO horarioTrabajo){
        this.horarioTrabajo = horarioTrabajo;
        return super.eliminar();
    }
    
    private void incluirValoresDeParametrosParaListar(Object objetoParametros){
        Integer idEmpleado = (Integer) objetoParametros;
        try {
            this.statement.setInt(1, idEmpleado);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    public ArrayList<HorarioTrabajoDTO> listarPorEmpleado(Integer idEmpleado){
        String sql = "SELECT * FROM HORARIO_TRABAJO WHERE EMPLEADO_ID = ?";
        return (ArrayList<HorarioTrabajoDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListar,idEmpleado);
    }
    
    @Override
    public HorarioTrabajoDTO obtenerPorId(Integer horarioId) {
        this.horarioTrabajo = new HorarioTrabajoDTO();
        this.horarioTrabajo.setId(horarioId);
        super.obtenerPorId();
        return this.horarioTrabajo;
    }
}