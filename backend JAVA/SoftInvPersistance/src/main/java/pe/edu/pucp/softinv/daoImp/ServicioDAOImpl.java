package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;

public class ServicioDAOImpl extends DAOImplBase implements ServicioDAO {

    private ServicioDTO servicio;

    public ServicioDAOImpl() {
        super("SERVICIOS");
        this.servicio = null;
        this.retornarLlavePrimaria=true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("SERVICIO_ID", true, true));
        this.listaColumnas.add(new Columna("NOMBRE", false, false));
        this.listaColumnas.add(new Columna("TIPO", false, false));
        this.listaColumnas.add(new Columna("PRECIO", false, false));
        this.listaColumnas.add(new Columna("DESCRIPCION", false, false));
        this.listaColumnas.add(new Columna("PROM_VALORACIONES", false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN", false, false));
        this.listaColumnas.add(new Columna("DURACION_HORAS", false, false));
        this.listaColumnas.add(new Columna("ACTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setString(1, servicio.getNombre());
        this.statement.setString(2, servicio.getTipo());
        this.statement.setDouble(3, servicio.getPrecio());
        this.statement.setString(4, servicio.getDescripcion());
        this.statement.setDouble(5, servicio.getPromedioValoracion());
        this.statement.setString(6, servicio.getUrlImagen());
        this.statement.setInt(7, servicio.getDuracionHora());
        this.statement.setInt(8, servicio.getActivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setString(1, servicio.getNombre());
        this.statement.setString(2, servicio.getTipo());
        this.statement.setDouble(3, servicio.getPrecio());
        this.statement.setString(4, servicio.getDescripcion());
        this.statement.setDouble(5, servicio.getPromedioValoracion());
        this.statement.setString(6, servicio.getUrlImagen());
        this.statement.setInt(7, servicio.getDuracionHora());
        this.statement.setInt(8, servicio.getActivo());
        this.statement.setInt(9, servicio.getIdServicio());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, servicio.getIdServicio());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.servicio.getIdServicio());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.servicio = new ServicioDTO();
        this.servicio.setIdServicio(this.resultSet.getInt("SERVICIO_ID"));
        this.servicio.setNombre(this.resultSet.getString("NOMBRE"));
        this.servicio.setPrecio(this.resultSet.getDouble("PRECIO"));
        this.servicio.setDescripcion(this.resultSet.getString("DESCRIPCION"));
        this.servicio.setPromedioValoracion(this.resultSet.getDouble("PROM_VALORACIONES"));
        this.servicio.setUrlImagen(this.resultSet.getString("URL_IMAGEN"));
        this.servicio.setDuracionHora(this.resultSet.getInt("DURACION_HORAS"));
        this.servicio.setActivo(this.resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.servicio = null;
    }

    @Override
    public Integer insertar(ServicioDTO servicio){
        this.servicio = servicio;
        return super.insertar();
    }

    @Override
    public ServicioDTO obtenerPorId(Integer idServicio){
        this.servicio = new ServicioDTO();
        this.servicio.setIdServicio(idServicio);
        super.obtenerPorId();
        return this.servicio;
    }

    @Override
    public Integer modificar(ServicioDTO servicio){
        this.servicio=servicio;
        return super.modificar();
    }

    @Override
    public Integer eliminar(ServicioDTO servicio){
        this.servicio=servicio;
        return super.eliminar();
    }

    @Override
    public ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(Integer servicioID){
        ArrayList<EmpleadoDTO>empleados=new ArrayList<>();
        try {
            String sql="SELECT u.USUARIO_ID, u.PRIMER_APELLIDO, u.SEGUNDO_APELLIDO, u.NOMBRE, u.CORREO_ELECTRONICO, u.CONTRASENHA, u.CELULAR, u.ROL, u.URL_IMAGEN u.ACTIVO FROM USUARIOS AS u JOIN EMPLEADOS_SERVICIOS AS es ON es.EMPLEADO_ID = u.USUARIO_ID JOIN SERVICIOS AS s ON s.SERVICIO_ID = es.SERVICIO_ID WHERE s.SERVICIO_ID = ? AND u.ROL = 'EMPLEADO'";
            this.abrirConexion();
            this.colocarSQLEnStatement(sql);
            this.ejecutarSelectEnDB();
            while (this.resultSet.next()) {
                EmpleadoDTO empleado =new EmpleadoDTO();
                empleado.setIdUsuario(resultSet.getInt("USUARIO_ID"));
                empleado.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
                empleado.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
                empleado.setNombre(resultSet.getString("NOMBRE"));
                empleado.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
                empleado.setContrasenha(resultSet.getString("CONTRASENHA"));
                empleado.setCelular(resultSet.getString("CELULAR"));
                empleado.setRol(resultSet.getInt("ROL"));
                empleado.setUrlFotoPerfil(resultSet.getString("URL_IMAGEN"));
                empleado.setUrlFotoPerfil(resultSet.getString("ACTIVO"));
                empleados.add(empleado);
            }
        } catch (SQLException ex) {
            System.err.println("Error al intentar listarTodos - " + ex);
        } finally {
            try {
                this.cerrarConexion();
            } catch (SQLException ex) {
                System.err.println("Error al cerrar la conexión - " + ex);
            }
        }
        return empleados;
    }
}
