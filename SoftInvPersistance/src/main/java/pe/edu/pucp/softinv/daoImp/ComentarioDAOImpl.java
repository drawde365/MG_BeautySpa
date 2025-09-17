package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ComentarioDAOImpl extends DAOImplBase implements ComentarioDAO {

    private ComentarioDTO comentario;

    public ComentarioDAOImpl() {
        super("COMENTARIOS");
        this.comentario = null;
        this.retornarLlavePrimaria = true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("COMENTARIO_ID", true, true));
        this.listaColumnas.add(new Columna("SERVICIO_ID", false, false));
        this.listaColumnas.add(new Columna("PRODUCTO_ID", false, false));
        this.listaColumnas.add(new Columna("CLIENTE_ID", false, false));
        this.listaColumnas.add(new Columna("DESCRIPCION", false, false));
        this.listaColumnas.add(new Columna("VALORACION", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1,comentario.getIdServicio());
        this.statement.setInt(2,comentario.getIdProducto());
        this.statement.setInt(3,comentario.getCliente().getIdUsuario());
        this.statement.setString(4,comentario.getComentario());
        this.statement.setInt(5,comentario.getValoracion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1,comentario.getIdServicio());
        this.statement.setInt(2,comentario.getIdProducto());
        this.statement.setInt(3,comentario.getCliente().getIdUsuario());
        this.statement.setString(4,comentario.getComentario());
        this.statement.setInt(5,comentario.getValoracion());
        //MODIFICABLE
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.comentario.getIdComentario());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        int Producto_ID = this.resultSet.getInt("PRODUCTO_ID");
    }

    @Override
    public Integer insertar(ComentarioDTO comentario)  {
        Integer resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "INSERT INTO COMENTARIOS(SERVICIO_ID, PRODUCTO_ID, CLIENTE_ID, DESCRIPCION, VALORACION) VALUES (?,?,?,?,?)";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setInt(1,comentario.getIdServicio());
            this.statement.setInt(2,comentario.getIdProducto());
            this.statement.setInt(3,comentario.getCliente().getIdUsuario());
            this.statement.setString(4,comentario.getComentario());
            this.statement.setInt(5,comentario.getValoracion());
            this.statement.executeUpdate();
            resultado = this.retornarUltimoAutogenerado();
            comentario.setIdComentario(resultado);
            this.conexion.commit();
        } catch (SQLException ex) {
            try{
                if (this.conexion != null) {
                    this.conexion.rollback();
                }
            } catch (SQLException ex1){
                System.getLogger(ComentarioDAOImpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex1);
            }
            System.getLogger(ComentarioDAOImpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
        }
        finally {
            try {
                if (this.conexion != null) {
                    this.conexion.close();
                }
            } catch (SQLException ex) {
                System.getLogger(ComentarioDAOImpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
            }
            this.conexion = null;
        }
        return resultado;

    }

    private Integer retornarUltimoAutogenerado(){
        Integer resultado = null;
        try {
            String sql = "SELECT @@LAST_INSERT_ID AS ID";
            this.statement = this.conexion.prepareCall(sql);
            this.resultSet = this.statement.executeQuery();
            if (this.resultSet.next()) {
                resultado = this.resultSet.getInt("ID");
            }
        } catch (SQLException ex) {
            System.getLogger(ComentarioDAOImpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
        }
        return resultado;
    }

    @Override
    public ComentarioDTO obtenerPorId(Integer idComentario){

    }

    @Override
    public Integer modificar(ComentarioDTO comentario){

    }

    @Override
    public Integer eliminar(ComentarioDTO comentario){

    }

}
