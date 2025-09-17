package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.UsuarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ClienteDAOimpl extends DAOImplBase implements UsuarioDAO {
    private ClienteDTO cliente;

    public ClienteDAOimpl() {
        super("USUARIOS");
        cliente = null;
        retornarLlavePrimaria=true;
    }

    @Override
    protected void configurarListaDeColumnas(){
        listaColumnas.add(new Columna("USUARIO_ID",true,true));
        listaColumnas.add(new Columna("PRIMER_APELLIDO",false,false));
        listaColumnas.add(new Columna("SEGUNDO_APELLIDO",false,false));
        listaColumnas.add(new Columna("NOMBRE",false,false));
        listaColumnas.add(new Columna("CORREO_ELECTRONICO",false,false));
        listaColumnas.add(new Columna("CONTRASENHA",false,false));
        listaColumnas.add(new Columna("CELULAR",false,false));
        listaColumnas.add(new Columna("ROL",false,false));
    }

    @Override
    public Integer insertar(UsuarioDTO cliente) {
        int resultado=0;
        try {
            conexion = DBManager.getInstance().getConnection();
            conexion.setAutoCommit(false);
            String sql = "INSERT INTO USUARIO (PRIMER_APELLIDO,SEGUNDO_APELLIDO,NOMBRE,CORREO_ELECTRONICO,CONTRASENHA,CELULAR,ROL)" +
                    " VALUES (?,?,?,?,?,?,?)";
            statement = conexion.prepareCall(sql);
            statement.setString(1,cliente.getPrimerapellido());
            statement.setString(2,cliente.getSegundoapellido());
            statement.setString(3,cliente.getNombre());
            statement.setString(4,cliente.getCorreoElectronico());
            statement.setString(5,cliente.getContrasenha());
            statement.setString(6,cliente.getCelular());
            statement.setString(7,cliente.getRol());
            statement.executeUpdate();
            resultado = retornarUltimoAutoGenerado();
            this.conexion.commit();
        } catch (SQLException ex) {
            try {
                if (this.conexion != null) {
                    this.conexion.rollback();
                }
            } catch (SQLException ex1) {
                System.getLogger(ClienteDAOimpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex1);
            }
            System.getLogger(ClienteDAOimpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
        } finally {
            try {
                if (this.conexion != null) {
                    this.conexion.close();
                }
            } catch (SQLException ex) {
                System.getLogger(ClienteDAOimpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
            }
        }
        return resultado;
    }

    public int retornarUltimoAutoGenerado() {
        Integer resu = null;
        try {
            String sql = "select @@last_insert_id as id";
            statement = conexion.prepareCall(sql);
            resultSet= statement.executeQuery();
            if(resultSet.next())
                resu = resultSet.getInt("id");
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
        return resu;
    }

    @Override
    public UsuarioDTO obtenerPorId(Integer id){
        ClienteDTO usuario = null;
        try {
            conexion = DBManager.getInstance().getConnection();
            String sql = "SELECT USUARIO_ID, PRIMER_APELLIDO, SEGUNDO_APELLIDO, NOMBRE, CORREO_ELECTRONICO,CONTRASENHA,CELULAR,ROL" +
                    " WHERE USUARIO_ID=?";
            statement = conexion.prepareCall(sql);
            statement.setInt(1,id);
            resultSet = statement.executeQuery();
            if(resultSet.next()){
                usuario = new ClienteDTO();
                usuario.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
                usuario.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));

            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        } finally{
            try{
                if(this.conexion!=null)
                    conexion.close();
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
        }
        return usuario;
    }

    @Override
    public Integer modificar(UsuarioDTO usuario) {
        int res = 0;
        try {
            conexion = DBManager.getInstance().getConnection();
        }
        return res;
    }

    @Override
    public Integer eliminar(Integer id){
        int res=0;

        return res;
    }
}
