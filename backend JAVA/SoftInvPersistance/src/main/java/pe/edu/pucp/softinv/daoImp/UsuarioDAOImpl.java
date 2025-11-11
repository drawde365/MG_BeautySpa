package pe.edu.pucp.softinv.daoImp;

import java.sql.SQLException;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

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

    public UsuarioDTO busquedaPorCorreo(String correo) {
        usuario = new UsuarioDTO();
        usuario.setCorreoElectronico(correo);
        try {
            this.abrirConexion();
            intercambiarPK();
            String sql = this.generarSQLParaObtenerPorId();
            this.colocarSQLEnStatement(sql);
            this.incluirValorDeParametrosParaBuscarPorCorreo();
            this.ejecutarSelectEnDB();
            if (this.resultSet.next()) {
                this.instanciarObjetoDelResultSet();
                //no se si esto es lo más eficiente
                if(usuario.getRol()==1){
                    ClienteDTO cliente = new ClienteDTO(usuario.getNombre(), usuario.getPrimerapellido(),
                    usuario.getSegundoapellido(), usuario.getCorreoElectronico(), usuario.getContrasenha(),
                    usuario.getCelular(), usuario.getIdUsuario(), null, usuario.getUrlFotoPerfil(), usuario.getActivo());
                    usuario=cliente;
                }
                else{
                    boolean esAdmin=false;
                    if(usuario.getRol()==3){
                        esAdmin=true;
                    }
                    EmpleadoDTO empleado = new EmpleadoDTO(usuario.getNombre(), usuario.getPrimerapellido(),
                    usuario.getSegundoapellido(), usuario.getCorreoElectronico(), usuario.getContrasenha(),
                    usuario.getCelular(), usuario.getIdUsuario(), esAdmin, usuario.getUrlFotoPerfil(), null);
                    usuario=empleado;
                }
            } else {
                this.limpiarObjetoDelResultSet();
            }
        } catch (SQLException ex) {
            System.err.println("Error al intentar buscar por correo - " + ex);
        } finally {
            try {
                intercambiarPK();
                this.cerrarConexion();
            } catch (SQLException ex) {
                System.err.println("Error al cerrar la conexión - " + ex);
            }
        }
        return usuario;
    }

    void intercambiarPK() {
        for (Columna c : this.listaColumnas) {
            if (c.getNombre().equals("USUARIO_ID")) {
                if (c.getEsLlavePrimaria()) {
                    c.setEsLlavePrimaria(false);
                } else {
                    c.setEsLlavePrimaria(true);
                }
            }
            if (c.getNombre().equals("CORREO_ELECTRONICO")) {
                if (c.getEsLlavePrimaria()) {
                    c.setEsLlavePrimaria(false);
                } else {
                    c.setEsLlavePrimaria(true);
                }
            }
        }
    }

    protected void incluirValorDeParametrosParaBuscarPorCorreo() throws SQLException {
        statement.setString(1, usuario.getCorreoElectronico());
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
