package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ServicioEmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class ServicioEmpleadoDAOImpl extends DAOImplBase implements ServicioEmpleadoDAO {
    private ServicioDTO servicio;
    private EmpleadoDTO empleado;
    private int EXS;
    private int activo;

    public ServicioEmpleadoDAOImpl() {
        super("EMPLEADOS_SERVICIOS");
        servicio=null;
        empleado=null;
        EXS=0;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("SERVICIO_ID", true, false));
        this.listaColumnas.add(new Columna("EMPLEADO_ID", true, false));
        this.listaColumnas.add(new Columna("ACTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1, servicio.getIdServicio());
        this.statement.setInt(2, empleado.getIdUsuario());
        this.statement.setInt(3, activo);
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, servicio.getIdServicio());
        this.statement.setInt(2, empleado.getIdUsuario());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException{
        this.statement.setInt(1,activo);
        this.statement.setInt(2, servicio.getIdServicio());
        this.statement.setInt(3, empleado.getIdUsuario());
    }
    
    @Override
    public Integer insertar(Integer empleadoId, Integer servicioId) {
        this.empleado = new EmpleadoDTO();
        this.activo=1;
        empleado.setIdUsuario(empleadoId);
        this.servicio = new ServicioDTO();
        servicio.setIdServicio(servicioId);
        return super.insertar();
    }
    
    @Override
    public Integer eliminar(Integer empleadoId, Integer servicioId) {
        this.empleado = new EmpleadoDTO();
        this.activo=0;
        empleado.setIdUsuario(empleadoId);
        this.servicio = new ServicioDTO();
        servicio.setIdServicio(servicioId);
        return super.eliminar();
    }

    @Override
    public Integer eliminarLogico(Integer empleadoId, Integer servicioId){
        this.empleado = new EmpleadoDTO();
        this.activo=0;
        empleado.setIdUsuario(empleadoId);
        this.servicio = new ServicioDTO();
        servicio.setIdServicio(servicioId);
        return super.modificar();
    }
    
    private void agregarServicio(List lista) throws SQLException {
        ServicioDTO servicio =new ServicioDTO();
        servicio.setIdServicio(resultSet.getInt("SERVICIO_ID"));
        servicio.setNombre(resultSet.getString("NOMBRE"));
        servicio.setTipo(resultSet.getString("TIPO"));
        servicio.setPrecio(resultSet.getDouble("PRECIO"));
        servicio.setDescripcion(resultSet.getString("DESCRIPCION"));
        servicio.setPromedioValoracion(resultSet.getDouble("PROM_VALORACIONES"));
        servicio.setUrlImagen(resultSet.getString("URL_IMAGEN"));
        servicio.setActivo(resultSet.getInt("ACTIVO"));
        lista.add(servicio);
    }
    private void agregarEmpleado(List lista) throws SQLException {
        EmpleadoDTO empleado =new EmpleadoDTO();
        empleado.setIdUsuario(resultSet.getInt("USUARIO_ID"));
        empleado.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
        empleado.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
        empleado.setNombre(resultSet.getString("NOMBRE"));
        empleado.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
        empleado.setContrasenha(resultSet.getString("CONTRASENHA"));
        empleado.setCelular(resultSet.getString("CELULAR"));
        empleado.setRol(resultSet.getInt("ROL_ID"));
        empleado.setUrlFotoPerfil(resultSet.getString("URL_IMAGEN"));
        empleado.setActivo(resultSet.getInt("ACTIVO")); // <-- CORREGIDO
        lista.add(empleado);
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        if(EXS==0) {
            agregarServicio(lista);
        } else {
            agregarEmpleado(lista);
        }
    }
    @Override
    public ArrayList<ServicioDTO> listarServiciosDeEmpleado(Integer empleadoId) {
        String sql = "SELECT s.SERVICIO_ID, s.NOMBRE, s.TIPO, s.PRECIO, s.DESCRIPCION," +
                " s.PROM_VALORACIONES, s.URL_IMAGEN, s.DURACION_HORAS, s.ACTIVO" +
                " FROM USUARIOS AS u JOIN EMPLEADOS_SERVICIOS AS es ON es.EMPLEADO_ID = u.USUARIO_ID" +
                " JOIN SERVICIOS AS s ON s.SERVICIO_ID = es.SERVICIO_ID WHERE u.USUARIO_ID = ? AND es.ACTIVO=1";
        EXS=0;
        return (ArrayList<ServicioDTO>)listarTodos(sql, this::incluirId, empleadoId);
    }

    private void incluirId(Object objetoParametros){
        Integer id = (Integer) objetoParametros;
        try {
            this.statement.setInt(1,id);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    @Override
    public ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(Integer servicioId){
        String sql="SELECT u.USUARIO_ID, u.PRIMER_APELLIDO, u.SEGUNDO_APELLIDO, u.NOMBRE, u.CORREO_ELECTRONICO, u.CONTRASENHA, u.CELULAR, u.ROL_ID, u.URL_IMAGEN, u.ACTIVO \n" +
"                    	FROM USUARIOS AS u JOIN EMPLEADOS_SERVICIOS AS es ON es.EMPLEADO_ID = u.USUARIO_ID JOIN SERVICIOS AS s ON s.SERVICIO_ID = es.SERVICIO_ID WHERE s.SERVICIO_ID = ? AND es.ACTIVO=1";
        EXS=1;
        return (ArrayList<EmpleadoDTO>)listarTodos(sql, this::incluirId, servicioId);
    }
}