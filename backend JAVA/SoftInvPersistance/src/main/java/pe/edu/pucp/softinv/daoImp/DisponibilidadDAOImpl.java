package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.DisponibilidadDAO;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.daoImp.util.Tipo_Operacion;
import pe.edu.pucp.softinv.model.Disponibilidad.DisponibilidadDTO;
import pe.edu.pucp.softinv.model.Pedido.EstadoPedido;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.util.List;

public class DisponibilidadDAOImpl extends DAOImplBase implements DisponibilidadDAO  {

    private DisponibilidadDTO disponibilidad;

    public DisponibilidadDAOImpl() {
        super("DISPONIBILIDAD");
        this.disponibilidad = null;
    }

    @Override
    protected void configurarListaDeColumnas(){
        listaColumnas.add(new Columna("DISPONIBILIDAD_ID",true,true));
        listaColumnas.add(new Columna("EMPLEADO_ID",false,false));
        listaColumnas.add(new Columna("DIA_DE_SEMANA",false,false));
        listaColumnas.add(new Columna("HORA_INICIO",false,false));
        listaColumnas.add(new Columna("HORA_FIN",false,false));
    }

    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        statement.setInt(1,disponibilidad.getEmpleado().getIdUsuario());
        statement.setInt(2,disponibilidad.getDiaSemana());
        statement.setTime(3,disponibilidad.getHoraInicio());
        statement.setTime(4,disponibilidad.getHoraFin());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        // SET ( =?)
        statement.setInt(1,disponibilidad.getEmpleado().getIdUsuario());
        statement.setInt(2,disponibilidad.getDiaSemana());
        statement.setTime(3,disponibilidad.getHoraInicio());
        statement.setTime(4,disponibilidad.getHoraFin());
        //WHERE = ?
        statement.setInt(5,disponibilidad.getDisponibilidadId());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException{
        this.statement.setInt(1,disponibilidad.getDisponibilidadId());
    }

    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.disponibilidad.getDisponibilidadId());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.disponibilidad = new DisponibilidadDTO();
        this.disponibilidad.setDisponibilidadId(this.resultSet.getInt("DISPONIBILIDAD_ID"));
        this.disponibilidad.setDiaSemana(this.resultSet.getInt("DIA_SEMANA"));
        this.disponibilidad.setHoraInicio(this.resultSet.getTime("HORA_INICIO"));
        this.disponibilidad.setHoraFin(this.resultSet.getTime("HORA_FIN"));
        int empleadoId = this.resultSet.getInt("EMPLEADO_ID");
        if (empleadoId > 0) {
            EmpleadoDAOImpl empleadoDAO = new EmpleadoDAOImpl();
            EmpleadoDTO empleado = empleadoDAO.obtenerPorId(empleadoId);
            this.disponibilidad.setEmpleado(empleado);
        }

    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.disponibilidad = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException{
        this.instanciarObjetoDelResultSet();
        lista.add(this.disponibilidad);
    }
    @Override
    public DisponibilidadDTO obtenerPorId(Integer disponibilidadId) {
        this.disponibilidad = new DisponibilidadDTO();
        this.disponibilidad.setDisponibilidadId(disponibilidadId);
        super.obtenerPorId();
        return this.disponibilidad;
    }

    @Override
    public Integer insertar(DisponibilidadDTO disponibilidad){
        this.disponibilidad = disponibilidad;
        return super.insertar();
    }

    @Override
    public Integer modificar(DisponibilidadDTO disponibilidad){
        this.disponibilidad = disponibilidad;
        return super.modificar();
    }

    @Override
    public Integer eliminar(DisponibilidadDTO disponibilidad){
        this.disponibilidad = disponibilidad;
        return super.eliminar();
    }



}
