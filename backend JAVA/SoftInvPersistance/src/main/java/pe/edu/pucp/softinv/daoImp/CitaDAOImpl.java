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
    public ArrayList<CitaDTO> listarCitasPorUsuario(UsuarioDTO usuario){
        String filtro = "";
        if(usuario.getRol()==1) filtro="CLIENTE_ID";
        else if(usuario.getRol()==3 || usuario.getRol()==2) filtro="EMPLEADO_ID";
        String sql = """
                SELECT
                    c.CITA_ID,
                
                    cli.USUARIO_ID AS Cliente_ID,
                    cli.NOMBRE AS Cliente_Nombre,
                    cli.PRIMER_APELLIDO AS Cliente_Primer_Apellido,
                    cli.SEGUNDO_APELLIDO AS Cliente_Segundo_Apellido,
                    cli.CORREO_ELECTRONICO AS Cliente_Correo,
                    cli.CELULAR AS Cliente_Celular,
                
                    emp.USUARIO_ID AS Empleado_ID,
                    emp.NOMBRE AS Empleado_Nombre,
                    emp.PRIMER_APELLIDO AS Empleado_Primer_Apellido,
                    emp.SEGUNDO_APELLIDO AS Empleado_Segundo_Apellido,
                    emp.CORREO_ELECTRONICO AS Empleado_Correo,
                    emp.CELULAR AS Empleado_Celular,
                
                    s.SERVICIO_ID AS Servicio_ID,
                    s.NOMBRE AS Servicio_Nombre,
                    s.TIPO AS Servicio_Tipo,
                    s.PRECIO AS Servicio_Precio,
                    s.DESCRIPCION AS Servicio_Descripcion,
                    s.DURACION_HORAS AS Servicio_Duracion,
                
                    c.FECHA,
                    c.HORA_INICIO,
                    c.HORA_FIN,
                    c.IGV,
                    c.ACTIVO,
                    c.CODTR
                
                FROM CITAS c
                INNER JOIN USUARIOS cli ON c.CLIENTE_ID = cli.USUARIO_ID
                INNER JOIN USUARIOS emp ON c.EMPLEADO_ID = emp.USUARIO_ID
                INNER JOIN SERVICIOS s ON c.SERVICIO_ID = s.SERVICIO_ID
                WHERE c.%s = ?
                  AND c.FECHA >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)
                ORDER BY c.FECHA DESC;
        """.formatted(filtro);
        return (ArrayList<CitaDTO>) super.listarTodos(sql,this::incluirParametrosParaListarPorUsuario, usuario.getIdUsuario());
    }

    private void incluirParametrosParaListarPorUsuario(Object objetoParametros) {
        try {
            this.statement.setInt(1,(Integer) objetoParametros);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.cita = new CitaDTO();
        EmpleadoDTO empleado = this.instanciarEmpleadoDelResultSet();
        ClienteDTO cliente = this.instanciarClienteDelResultSet();
        ServicioDTO servicio = new ServicioDTO();

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
        empleado.setIdUsuario(resultSet.getInt("Empleado_ID"));
        empleado.setNombre(resultSet.getString("Empleado_Nombre"));
        empleado.setPrimerapellido(resultSet.getString("Empleado_Primer_Apellido"));
        empleado.setSegundoapellido(resultSet.getString("Empleado_Segundo_Apellido"));
        empleado.setCorreoElectronico(resultSet.getString("Empleado_Correo"));
        empleado.setCelular(resultSet.getString("Empleado_Celular"));
        return empleado;
    }

    private ClienteDTO instanciarClienteDelResultSet() throws SQLException {
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(resultSet.getInt("Cliente_ID"));
        cliente.setNombre(resultSet.getString("Cliente_Nombre"));
        cliente.setPrimerapellido(resultSet.getString("Cliente_Primer_Apellido"));
        cliente.setSegundoapellido(resultSet.getString("Cliente_Segundo_Apellido"));
        cliente.setCorreoElectronico(resultSet.getString("Cliente_Correo"));
        cliente.setCelular(resultSet.getString("Cliente_Celular"));
        return cliente;
    }

    private ServicioDTO instanciarServicioDelResultSet() throws SQLException {
        ServicioDTO servicio = new ServicioDTO();
        servicio.setIdServicio(resultSet.getInt("Servicio_ID"));
        servicio.setNombre(resultSet.getString("Servicio_Nombre"));
        servicio.setTipo(resultSet.getString("Servicio_Tipo"));
        servicio.setDescripcion(resultSet.getString("Servicio_Descripcion"));
        servicio.setPrecio(resultSet.getDouble("Servicio_Precio"));
        servicio.setDuracionHora(resultSet.getInt("Servicio_Duracion"));
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
        return null;
    }
}
