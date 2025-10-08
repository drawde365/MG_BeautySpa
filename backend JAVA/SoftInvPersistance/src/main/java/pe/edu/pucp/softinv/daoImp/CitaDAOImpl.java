package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class CitaDAOImpl extends DAOImplBase implements CitaDAO {
    private CitaDTO cita;

    public CitaDAOImpl() {
        super("CITAS");
        cita = null;
        retornarLlavePrimaria=true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        listaColumnas.add(new Columna("CITA_ID", true, true));
        listaColumnas.add(new Columna("EMPLEADO_ID", false, false));
        listaColumnas.add(new Columna("CLIENTE_ID", false, false));
        listaColumnas.add(new Columna("SERVICIO_ID", false, false));
        listaColumnas.add(new Columna("FECHA", false, false));
        listaColumnas.add(new Columna("HORA_INICIO", false, false));
        listaColumnas.add(new Columna("HORA_FIN", false, false));
        listaColumnas.add(new Columna("IGV", false, false));
        listaColumnas.add(new Columna("ACTIVO", false, false));
        listaColumnas.add(new Columna("CODTR", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, cita.getEmpleado().getIdUsuario());
        this.statement.setInt(2, cita.getCliente().getIdUsuario());
        this.statement.setInt(3, cita.getServicio().getIdServicio());
        this.statement.setDate(4, cita.getFecha());
        this.statement.setTime(5, cita.getHoraIni());
        this.statement.setTime(6, cita.getHoraFin());
        this.statement.setDouble(7, cita.getIgv());
        this.statement.setInt(8, cita.getActivo());
        this.statement.setString(9, cita.getCodigoTransaccion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, cita.getEmpleado().getIdUsuario());
        this.statement.setInt(2, cita.getCliente().getIdUsuario());
        this.statement.setInt(3, cita.getServicio().getIdServicio());
        this.statement.setDate(4, cita.getFecha());
        this.statement.setTime(5, cita.getHoraIni());
        this.statement.setTime(6, cita.getHoraFin());
        this.statement.setDouble(7, cita.getIgv());
        this.statement.setInt(8, cita.getActivo());
        this.statement.setString(9, cita.getCodigoTransaccion());
        this.statement.setInt(10, cita.getId());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, cita.getId());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, cita.getId());
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(cita);
    }

    @Override
    public ArrayList<CitaDTO> listarCitasPorPeriodo(Date inicio, Date fin) {
        String sql = "SELECT * FROM CITAS WHERE FECHA >= ? AND FECHA <= ?";
        ArrayList<Date> lista = new ArrayList<>();
        lista.add(inicio);
        lista.add(fin);
        return (ArrayList<CitaDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPorFechas,lista);
    }

    //public ArrayList<CitaDTO> listarCitas;

    private void incluirValoresDeParametrosParaListarPorFechas(Object objetoParametros){
        ArrayList<Date> lista = (ArrayList<Date>) objetoParametros;
        try {
            this.statement.setDate(1, (java.sql.Date) lista.get(0));
            this.statement.setDate(2, (java.sql.Date) lista.get(1));
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.cita = new CitaDTO();
        CitaDTO cita = new CitaDTO();
        EmpleadoDTO empleado = new EmpleadoDTO();
        ClienteDTO cliente = new ClienteDTO();
        ServicioDTO servicio = new ServicioDTO();

        cita.setId(resultSet.getInt("CITA_ID"));
        empleado.setIdUsuario(resultSet.getInt("EMPLEADO_ID")); ;
        cita.setEmpleado(empleado);
        cliente.setIdUsuario(resultSet.getInt("CLIENTE_ID"));
        cita.setCliente(cliente);
        servicio.setIdServicio(resultSet.getInt("SERVICIO_ID"));
        cita.setServicio(servicio);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Se necesita el contenido servicio y empleado
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        cita.setFecha(resultSet.getDate("FECHA"));
        cita.setHoraIni(resultSet.getTime("HORA_INICIO"));
        cita.setHoraFin(resultSet.getTime("HORA_FIN"));
        cita.setActivo(resultSet.getInt("ACTIVO"));
        cita.setIgv(resultSet.getDouble("IGV"));
        cita.setCodigoTransaccion(resultSet.getString("CODTR"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.cita = null;
    }

    @Override
    public Integer insertar(CitaDTO cita) {
        this.cita = cita;
        return super.insertar();
    }

    @Override
    public Integer modificar(CitaDTO cita) {
        this.cita = cita;
        return super.modificar();
    }

    @Override
    public Integer eliminar(CitaDTO cita) {
        this.cita = cita;
        return super.eliminar();
    }

    @Override
    public CitaDTO obtenerPorId(CitaDTO idCita) {
        return null;
    }
}
