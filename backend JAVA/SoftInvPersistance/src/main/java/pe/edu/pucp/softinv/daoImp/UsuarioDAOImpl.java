package pe.edu.pucp.softinv.daoImp;

import java.sql.SQLException;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author Alvaro
 */
public class UsuarioDAOImpl extends DAOImplBase {

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

    public UsuarioDTO busquedaPorCorreo(String correo,String contrasenha) {
        usuario = new UsuarioDTO();
        usuario.setCorreoElectronico(correo);
        usuario.setContrasenha(contrasenha);
        String sql = "SELECT USUARIO_ID, PRIMER_APELLIDO, SEGUNDO_APELLIDO,"
                + "NOMBRE, CORREO_ELECTRONICO, CONTRASENHA, CELULAR,"
                + " ROL_ID, URL_IMAGEN, ACTIVO FROM USUARIOS WHERE CORREO_ELECTRONICO = ? AND CONTRASENHA = ?";
        super.obtenerPorId(sql);
        return usuario;
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        String as = "";
        as += usuario.getCorreoElectronico();
        statement.setString(1, as);
        statement.setString(2, usuario.getContrasenha());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.usuario = new UsuarioDTO();
        usuario.setIdUsuario(resultSet.getInt("USUARIO_ID"));
        usuario.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
        usuario.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
        usuario.setNombre(resultSet.getString("NOMBRE"));
        usuario.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
        usuario.setContrasenha(resultSet.getString("CONTRASENHA"));
        usuario.setCelular(resultSet.getString("CELULAR"));
        usuario.setRol(resultSet.getInt("ROL_ID"));
        usuario.setUrlFotoPerfil(resultSet.getString("URL_IMAGEN"));
        usuario.setActivo(resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        usuario = null;
    }

}
