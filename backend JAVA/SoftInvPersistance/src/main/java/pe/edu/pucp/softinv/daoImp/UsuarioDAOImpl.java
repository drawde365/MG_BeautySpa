package pe.edu.pucp.softinv.daoImp;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import pe.edu.pucp.softinv.dao.UsuarioDAO;
import static pe.edu.pucp.softinv.daoImp.util.Cifrado.cifrarMD5;
import static pe.edu.pucp.softinv.daoImp.util.Cifrado.descifrarMD5;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author Alvaro
 */
public class UsuarioDAOImpl extends DAOImplBase implements UsuarioDAO{

    private UsuarioDTO usuario;

    public UsuarioDAOImpl() {
        super("USUARIOS");
        usuario = null;
        retornarLlavePrimaria = true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        listaColumnas.add(new Columna("USUARIO_ID", true, true));
        listaColumnas.add(new Columna("PRIMER_APELLIDO", false, false));
        listaColumnas.add(new Columna("SEGUNDO_APELLIDO", false, false));
        listaColumnas.add(new Columna("NOMBRE", false, false));
        listaColumnas.add(new Columna("CORREO_ELECTRONICO", false, false));
        listaColumnas.add(new Columna("CONTRASENHA", false, false));
        listaColumnas.add(new Columna("CELULAR", false, false));
        listaColumnas.add(new Columna("ROL_ID", false, false));
        listaColumnas.add(new Columna("URL_IMAGEN", false, false));
        listaColumnas.add(new Columna("ACTIVO", false, false));
    }

    @Override
    public UsuarioDTO busquedaPorCorreo(String correo) {
        usuario = new UsuarioDTO();
        usuario.setCorreoElectronico(correo);
        String sql = "SELECT USUARIO_ID, PRIMER_APELLIDO, SEGUNDO_APELLIDO,"
                + "NOMBRE, CORREO_ELECTRONICO, CONTRASENHA, CELULAR,"
                + " ROL_ID, URL_IMAGEN, ACTIVO FROM USUARIOS WHERE CORREO_ELECTRONICO = ?";
        super.obtenerPorId(sql);
        return usuario;
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        String as = "";
        as += usuario.getCorreoElectronico();
        statement.setString(1, as);
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.usuario = new UsuarioDTO();
        usuario.setIdUsuario(resultSet.getInt("USUARIO_ID"));
        usuario.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
        usuario.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
        usuario.setNombre(resultSet.getString("NOMBRE"));
        usuario.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
        String contrasenhaCifrado=resultSet.getString("CONTRASENHA");
        usuario.setContrasenha(descifrarMD5(contrasenhaCifrado));
        usuario.setCelular(resultSet.getString("CELULAR"));
        usuario.setRol(resultSet.getInt("ROL_ID"));
        usuario.setUrlFotoPerfil(resultSet.getString("URL_IMAGEN"));
        usuario.setActivo(resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        usuario = null;
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException{
        if(usuario.getContrasenha()!=null) statement.setString(1,usuario.getContrasenha());
        else statement.setInt(1,usuario.getActivo());
        statement.setInt(2,usuario.getIdUsuario());
    }
    
    @Override
    public Integer actualizarContrasenha(Integer usuarioId,String nuevaContrasenha){
        String sql = "UPDATE USUARIOS SET CONTRASENHA=? WHERE USUARIO_ID=?";
        usuario = new UsuarioDTO();
        usuario.setIdUsuario(usuarioId);
        usuario.setContrasenha(cifrarMD5(nuevaContrasenha));
        return super.modificar(sql);
    }
    
    @Override
    public ArrayList<UsuarioDTO> obtenerUsuarios(){
        String sql = "SELECT \n" +"    U.USUARIO_ID,\n"
                + "    U.NOMBRE,\n"
                + "    U.PRIMER_APELLIDO,\n"
                + "    U.SEGUNDO_APELLIDO,\n"
                + "    U.CORREO_ELECTRONICO,\n"
                + "    U.CELULAR,\n"
                + "    U.ROL_ID,\n"
                + "    U.ACTIVO,\n"
                + "    COUNT(ES.SERVICIO_ID) AS CANT_SERVICIOS\n"
                + "FROM \n"
                + "    USUARIOS U\n"
                + "LEFT JOIN \n"
                + "    EMPLEADOS_SERVICIOS ES \n"
                + "        ON ES.EMPLEADO_ID = U.USUARIO_ID\n"
                + "WHERE\n"
                + "    U.ROL_ID!=3\n"
                + "GROUP BY \n"
                + "    U.USUARIO_ID,\n"
                + "    U.NOMBRE,\n"
                + "    U.PRIMER_APELLIDO,\n"
                + "    U.SEGUNDO_APELLIDO,\n"
                + "    U.CORREO_ELECTRONICO,\n"
                + "    U.CELULAR,\n"
                + "    U.ROL_ID, \n"
                + "    U.ACTIVO;";
        return (ArrayList<UsuarioDTO>)super.listarTodos(sql, null, null);
    }
    
    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoListaUsuarios();
        lista.add(this.usuario);
    }

    private void instanciarObjetoListaUsuarios() throws SQLException {
        this.usuario = new UsuarioDTO();
        usuario.setIdUsuario(resultSet.getInt("USUARIO_ID"));
        usuario.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
        usuario.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
        usuario.setNombre(resultSet.getString("NOMBRE"));
        usuario.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
        usuario.setCelular(resultSet.getString("CELULAR"));
        usuario.setRol(resultSet.getInt("ROL_ID"));
        usuario.setActivo(resultSet.getInt("ACTIVO"));
        usuario.setCantidadServicios(resultSet.getInt("CANT_SERVICIOS"));
    }
    
    @Override
    public Integer modificarActivoCliente(Integer usuarioId,Integer activo){
        String sql = "UPDATE USUARIOS SET ACTIVO=? WHERE USUARIO_ID=?";
        usuario = new UsuarioDTO();
        usuario.setIdUsuario(usuarioId);
        usuario.setActivo(activo);
        return super.modificar(sql);
    }
}
