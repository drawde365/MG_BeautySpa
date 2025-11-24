package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;
import static pe.edu.pucp.softinv.daoImp.util.Cifrado.cifrarMD5;

/**
 * Implementación de EmpleadoDAO basada en DAOImplBase y su generador de SQL.
 * Tabla: USUARIOS (PK: USUARIO_ID). Se fuerza ROL='EMPLEADO' para insertar/modificar.
 */
public class EmpleadoDAOImpl extends DAOImplBase implements EmpleadoDAO {

    private EmpleadoDTO empleado;
    private final int rol=2;
    public EmpleadoDAOImpl() {
        super("USUARIOS");
        this.empleado = null;
        this.retornarLlavePrimaria = true;
    }

    @Override
    public int insertar(EmpleadoDTO empleado) {
        this.empleado = empleado;
        return super.insertar();
    }

    @Override
    public int modificar(EmpleadoDTO empleado) {
        this.empleado = empleado;
        return super.modificar();
    }

    @Override
    public int eliminar(int empleadoId) {
        this.empleado = new EmpleadoDTO();
        this.empleado.setIdUsuario(empleadoId);
        return super.eliminar();
    }

    @Override
    public EmpleadoDTO obtenerPorId(int empleadoId) {
        this.empleado = new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);
        super.obtenerPorId(); // poblará this.empleado vía instanciarObjetoDelResultSet()
        return this.empleado;
    }

    @Override
    public ArrayList<EmpleadoDTO> listarTodos() {
        List lista = new ArrayList<>();
        try {
            this.abrirConexion();
            String sql = this.generarSQLParaListarTodos();
            sql += " WHERE ROL_ID=2 OR ROL_ID = 3";
            this.colocarSQLEnStatement(sql);
            this.ejecutarSelectEnDB();
            while (this.resultSet.next()) {
                this.agregarObjetoALaLista(lista);
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
        return (ArrayList<EmpleadoDTO>)lista;
    }

    @Override
    protected void configurarListaDeColumnas() {
        // Columna(nombre, esLlavePrimaria, esAutoGenerado)
        this.listaColumnas.add(new Columna("USUARIO_ID",       true,  true));
        this.listaColumnas.add(new Columna("PRIMER_APELLIDO",  false, false));
        this.listaColumnas.add(new Columna("SEGUNDO_APELLIDO", false, false));
        this.listaColumnas.add(new Columna("NOMBRE",           false, false));
        this.listaColumnas.add(new Columna("CORREO_ELECTRONICO", false, false));
        this.listaColumnas.add(new Columna("CONTRASENHA",       false, false));
        this.listaColumnas.add(new Columna("CELULAR",          false, false));
        this.listaColumnas.add(new Columna("ROL_ID",              false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN",       false, false));
        this.listaColumnas.add(new Columna("ACTIVO",       false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        // Orden = columnas SIN autogenerados (como los construye DAOImplBase)
        int i = 1;
        this.statement.setString(i++, empleado.getPrimerapellido());
        this.statement.setString(i++, empleado.getSegundoapellido());
        this.statement.setString(i++, empleado.getNombre());
        this.statement.setString(i++, empleado.getCorreoElectronico());
        this.statement.setString(i++, cifrarMD5(empleado.getContrasenha()));
        this.statement.setString(i++, empleado.getCelular());
        this.statement.setInt(i++, empleado.getRol());
        this.statement.setString(i++, empleado.getUrlFotoPerfil());
        this.statement.setInt(i, empleado.getActivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        // SET (todas excepto PK) en el mismo orden, luego WHERE (PK) en su orden
        int i = 1;
        this.statement.setString(i++, empleado.getPrimerapellido());
        this.statement.setString(i++, empleado.getSegundoapellido());
        this.statement.setString(i++, empleado.getNombre());
        this.statement.setString(i++, empleado.getCorreoElectronico());
        this.statement.setString(i++, cifrarMD5(empleado.getContrasenha()));
        this.statement.setString(i++, empleado.getCelular());
        this.statement.setInt(i++, empleado.getRol());
        this.statement.setString(i++, empleado.getUrlFotoPerfil());
        this.statement.setInt(i++, empleado.getActivo());
        // WHERE PK
        this.statement.setInt(i, empleado.getIdUsuario());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, empleado.getIdUsuario());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        // WHERE PK
        this.statement.setInt(1, empleado.getIdUsuario());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.empleado = mapRow(this.resultSet);
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.empleado = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        lista.add(mapRow(this.resultSet));
    }

    private EmpleadoDTO mapRow(ResultSet rs) throws SQLException {
        EmpleadoDTO e = new EmpleadoDTO();
        e.setIdUsuario(rs.getInt("USUARIO_ID"));
        e.setPrimerapellido(rs.getString("PRIMER_APELLIDO"));
        e.setSegundoapellido(rs.getString("SEGUNDO_APELLIDO"));
        e.setNombre(rs.getString("NOMBRE"));
        e.setCorreoElectronico(rs.getString("CORREO_ELECTRONICO"));
        e.setContrasenha(rs.getString("CONTRASENHA"));
        e.setCelular(rs.getString("CELULAR"));
        e.setRol(rs.getInt("ROL_ID"));
        e.setUrlFotoPerfil(rs.getString("URL_IMAGEN"));
        e.setActivo(rs.getInt("ACTIVO"));
        // Derivar admin/rol a partir de la columna ROL
        String rolDb = rs.getString("ROL_ID");
        boolean isAdmin = rolDb != null && rolDb.equalsIgnoreCase("ADMIN");
        e.setAdmin(isAdmin);

        return e;
    }
}
