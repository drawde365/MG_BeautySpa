package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.util.List;

public class HorarioTrabajoDAOImpl extends DAOImplBase implements HorarioTrabajoDAO {
    HorarioTrabajoDTO horarioTrabajo;

    public HorarioTrabajoDAOImpl() {
        super("HORARIO_TRABAJO");
        this.retornarLlavePrimaria = false;
        this.horarioTrabajo = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("EMPLEADO_ID", true, false));
        this.listaColumnas.add(new Columna("DIA_SEMANA", true, false));
        this.listaColumnas.add(new Columna("HORA_INICIO", false, false));
        this.listaColumnas.add(new Columna("HORA_FIN", false, false));
        this.listaColumnas.add(new Columna("NUM_INTERVALOS", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(2, horarioTrabajo.getDiaSemana());
        this.statement.setTime(4, horarioTrabajo.getHoraFin());
        this.statement.setTime(3,horarioTrabajo.getHoraInicio());
        this.statement.setInt(5, horarioTrabajo.getIntervalos());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setTime(2, horarioTrabajo.getHoraFin());
        this.statement.setTime(1,horarioTrabajo.getHoraInicio());
        this.statement.setInt(3, horarioTrabajo.getIntervalos());
        this.statement.setInt(4, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(5, horarioTrabajo.getDiaSemana());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(2, horarioTrabajo.getDiaSemana());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, horarioTrabajo.getEmpleado().getIdUsuario());
        this.statement.setInt(2, horarioTrabajo.getDiaSemana());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.horarioTrabajo = new HorarioTrabajoDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(this.resultSet.getInt("EMPLEADO_ID"));

        this.horarioTrabajo.setEmpleado(empleado);
        this.horarioTrabajo.setDiaSemana(this.resultSet.getInt("DIA_SEMANA"));
        this.horarioTrabajo.setHoraFin(this.resultSet.getTime("HORA_FIN"));
        this.horarioTrabajo.setHoraInicio(this.resultSet.getTime("HORA_INICIO"));
        this.horarioTrabajo.setIntervalos(this.resultSet.getInt("NUM_INTERVALOS"));
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
    public HorarioTrabajoDTO obtenerPorId(Integer empleadoId, Integer diaSemana) {
        this.horarioTrabajo = new HorarioTrabajoDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);
        this.horarioTrabajo.setEmpleado(empleado);
        this.horarioTrabajo.setDiaSemana(diaSemana);
        super.obtenerPorId();
        return this.horarioTrabajo;
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

}
