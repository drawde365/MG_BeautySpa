package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.UsuarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class EmpleadoDAOImpl extends DAOImplBase implements UsuarioDAO {
    private Connection conexion;
    private ResultSet resultSet;
    private CallableStatement statement;
    private UsuarioDTO usuario;

    public EmpleadoDAOImpl() {
        super("USUARIOS");
        usuario = null;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("USUARIO_ID", true, true));
        this.listaColumnas.add(new Columna("PRIMER_APELLIDO", false, false));
        this.listaColumnas.add(new Columna("SEGUNDO_APELLIDO", false, false));
        this.listaColumnas.add(new Columna("NOMBRE", false, false));
        this.listaColumnas.add(new Columna("CORREO_ELECTRONICO", false, false));
        this.listaColumnas.add(new Columna("CONTRASENHA", false, false));
        this.listaColumnas.add(new Columna("CELULAR", false, false));
        this.listaColumnas.add(new Columna("ROL", false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN", false, false));

    }

    @Override
    public Integer insertar(UsuarioDTO usuario) {
        this.usuario = usuario;
        return super.insertar();
    }
    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        statement.setString(1, usuario.getPrimerapellido());
        statement.setString(2, usuario.getSegundoapellido());
        statement.setString(3, usuario.getNombre());
        statement.setString(4, usuario.getCorreoElectronico());
        statement.setString(5, usuario.getContrasenha());
        statement.setString(6, usuario.getCelular());
        statement.setString(7, usuario.getRol());
        statement.setString(8, String.valueOf(usuario.getIdUsuario()));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        statement.setString(1, usuario.getPrimerapellido());
        statement.setString(2, usuario.getSegundoapellido());
        statement.setString(3, usuario.getNombre());
        statement.setString(4, usuario.getCorreoElectronico());
        statement.setString(5, usuario.getContrasenha());
        statement.setString(6, usuario.getCelular());
        statement.setString(7, usuario.getRol());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        statement.setString(1, String.valueOf(usuario.getIdUsuario()));
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        statement.setString(1, String.valueOf(usuario.getIdUsuario()));
    }

    @Override
    public Integer modificar(UsuarioDTO usuario){
        this.usuario = usuario;
        return super.modificar();
    }

    @Override
    public Integer eliminar(UsuarioDTO usuario){
        this.usuario = usuario;
        return super.insertar();
    }

    @Override
    public UsuarioDTO obtenerPorId(Integer almacenId) {
        this.usuario = new EmpleadoDTO();
        this.usuario.setIdUsuario(almacenId);
        super.obtenerPorId();
        return this.usuario;
    }


