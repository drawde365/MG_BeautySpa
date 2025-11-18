package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Calendar;

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
        this.statement.setTimestamp(4, new java.sql.Timestamp(cita.getFecha().getTime()));
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
        this.statement.setTimestamp(4, new java.sql.Timestamp(cita.getFecha().getTime()));
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
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.cita = new CitaDTO();
        
        EmpleadoDTO empleado = this.instanciarEmpleadoDelResultSet();
        ClienteDTO cliente = this.instanciarClienteDelResultSet();
        ServicioDTO servicio = this.instanciarServicioDelResultSet();

        cita.setId(resultSet.getInt("CITA_ID"));
        
        cita.setEmpleado(empleado);
        cita.setCliente(cliente);
        cita.setServicio(servicio);

        cita.setFecha(resultSet.getDate("FECHA"));
        cita.setHoraIni(resultSet.getTime("HORA_INICIO"));
        cita.setHoraFin(resultSet.getTime("HORA_FIN"));
        cita.setActivo(resultSet.getInt("ACTIVO"));
        cita.setIgv(resultSet.getDouble("IGV"));
        cita.setCodigoTransaccion(resultSet.getString("CODTR"));
    }

    private EmpleadoDTO instanciarEmpleadoDelResultSet() throws SQLException {
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(resultSet.getInt("EMPLEADO_ID"));
        empleado.setNombre(resultSet.getString("EMPLEADO_NOMBRE")); 
        empleado.setPrimerapellido(resultSet.getString("EMPLEADO_PRIMER_APELLIDO"));
        empleado.setSegundoapellido(resultSet.getString("EMPLEADO_SEGUNDO_APELLIDO"));
        empleado.setCorreoElectronico(resultSet.getString("EMPLEADO_CORREO"));
        empleado.setCelular(resultSet.getString("EMPLEADO_CELULAR"));
        return empleado;
    }

    private ClienteDTO instanciarClienteDelResultSet() throws SQLException {
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(resultSet.getInt("CLIENTE_ID"));
        cliente.setNombre(resultSet.getString("CLIENTE_NOMBRE"));
        cliente.setPrimerapellido(resultSet.getString("CLIENTE_PRIMER_APELLIDO"));
        cliente.setSegundoapellido(resultSet.getString("CLIENTE_SEGUNDO_APELLIDO"));
        cliente.setCorreoElectronico(resultSet.getString("CLIENTE_CORREO"));
        cliente.setCelular(resultSet.getString("CLIENTE_CELULAR"));
        return cliente;
    }

    private ServicioDTO instanciarServicioDelResultSet() throws SQLException {
        ServicioDTO servicio = new ServicioDTO();
        servicio.setIdServicio(resultSet.getInt("SERVICIO_ID"));
        servicio.setNombre(resultSet.getString("SERVICIO_NOMBRE"));
        servicio.setTipo(resultSet.getString("SERVICIO_TIPO"));
        servicio.setDescripcion(resultSet.getString("SERVICIO_DESCRIPCION"));
        servicio.setPrecio(resultSet.getDouble("SERVICIO_PRECIO"));
        servicio.setDuracionHora(resultSet.getInt("SERVICIO_DURACION"));
        return servicio;
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
        this.cita = idCita;
        
        String sql = ObtenerQueryBaseCita() + " WHERE c.CITA_ID = ?";
        
        ArrayList<CitaDTO> lista = (ArrayList<CitaDTO>) super.listarTodos(sql,this::incluirParametrosParaObtenerPorId, idCita.getId());
        
        return lista.isEmpty() ? null : lista.get(0);
    }
    
    // --- QUERY BASE COMPLETA CON ALIAS EN MAYÚSCULAS ---
    private String ObtenerQueryBaseCita() {
        return """
            SELECT
                c.CITA_ID, c.FECHA, c.HORA_INICIO, c.HORA_FIN, c.IGV, c.ACTIVO, c.CODTR,
                
                cli.USUARIO_ID AS CLIENTE_ID, cli.NOMBRE AS CLIENTE_NOMBRE, 
                cli.PRIMER_APELLIDO AS CLIENTE_PRIMER_APELLIDO, cli.SEGUNDO_APELLIDO AS CLIENTE_SEGUNDO_APELLIDO, 
                cli.CORREO_ELECTRONICO AS CLIENTE_CORREO, cli.CELULAR AS CLIENTE_CELULAR,
                
                emp.USUARIO_ID AS EMPLEADO_ID, emp.NOMBRE AS EMPLEADO_NOMBRE, 
                emp.PRIMER_APELLIDO AS EMPLEADO_PRIMER_APELLIDO, emp.SEGUNDO_APELLIDO AS EMPLEADO_SEGUNDO_APELLIDO, 
                emp.CORREO_ELECTRONICO AS EMPLEADO_CORREO, emp.CELULAR AS EMPLEADO_CELULAR,
                
                s.SERVICIO_ID AS SERVICIO_ID, s.NOMBRE AS SERVICIO_NOMBRE, s.TIPO AS SERVICIO_TIPO, 
                s.PRECIO AS SERVICIO_PRECIO, s.DESCRIPCION AS SERVICIO_DESCRIPCION, s.DURACION_HORAS AS SERVICIO_DURACION
                
            FROM CITAS c
            INNER JOIN USUARIOS cli ON c.CLIENTE_ID = cli.USUARIO_ID
            INNER JOIN USUARIOS emp ON c.EMPLEADO_ID = emp.USUARIO_ID
            INNER JOIN SERVICIOS s ON c.SERVICIO_ID = s.SERVICIO_ID
        """;
    }

    @Override
    public ArrayList<CitaDTO> listarCitasPorUsuario(UsuarioDTO usuario) {
        // 1. Cálculo de fecha de inicio (6 meses atrás) en Java
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.MONTH, -6);
        java.sql.Date fechaInicio = new java.sql.Date(cal.getTimeInMillis());
        
        String filtroColumna = "";
        if(usuario.getRol()==1) filtroColumna="CLIENTE_ID";
        else if(usuario.getRol()==3 || usuario.getRol()==2) filtroColumna="EMPLEADO_ID";
        
        String sql = ObtenerQueryBaseCita() + """
             WHERE c.%s = ? 
               AND c.FECHA >= ?
             ORDER BY c.FECHA DESC;
         """.formatted(filtroColumna);
        
        // Parámetros: [UserID, FechaInicio]
        Object[] params = new Object[]{usuario.getIdUsuario(), fechaInicio};
        
        return (ArrayList<CitaDTO>) super.listarTodos(sql,this::incluirIdYFechaInicio, params);
    }
    
    // Handler para 2 parámetros: ID y Fecha
    private void incluirIdYFechaInicio(Object objetoParametros) {
        Object[] params = (Object[]) objetoParametros;
        try {
            this.statement.setInt(1,(Integer) params[0]);
            this.statement.setDate(2, (java.sql.Date) params[1]);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }


    private void incluirParametrosParaListarPorUsuario(Object objetoParametros) {
        try {
            this.statement.setInt(1,(Integer) objetoParametros);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private void incluirParametrosParaObtenerPorId(Object objetoParametros) {
        try {
            this.statement.setInt(1,(Integer) objetoParametros);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    @Override
    public ArrayList<CitaDTO> listarTodos() {
        return (ArrayList<CitaDTO>) super.listarTodos();
    }

    @Override
    public ArrayList<CitaDTO> listarCitasPorEmpleadoYFecha(Integer empleadoId, Date fecha) {
        String sql = ObtenerQueryBaseCita() + """
             WHERE c.EMPLEADO_ID = ?
               AND c.FECHA = ?
               AND c.ACTIVO = 1
             ORDER BY c.HORA_INICIO ASC;
         """;
        
        // Creamos un array de objetos para pasar los dos parámetros
        Object[] params = new Object[]{empleadoId, new java.sql.Date(fecha.getTime())};
        
        return (ArrayList<CitaDTO>)super.listarTodos(sql, this::incluirParametrosParaListarPorEmpleadoYFecha, params);
    }
    
    private void incluirParametrosParaListarPorEmpleadoYFecha(Object objetoParametros) {
        Object[] params = (Object[]) objetoParametros;
        try {
            this.statement.setInt(1, (Integer) params[0]);
            this.statement.setDate(2, (java.sql.Date) params[1]); 
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    @Override
    public void agregarObjetoALaLista(List lista) throws SQLException { // Changed from protected to public (if possible)
        this.instanciarObjetoDelResultSet();
        lista.add(cita);
    }
}