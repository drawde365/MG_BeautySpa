package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ComentarioDAOImpl implements ComentarioDAO {
    private Connection conexion;
    private ResultSet resultSet;
    private CallableStatement statement;

    @Override
    public Integer insertar(ComentarioDTO comentario)  {
        Integer resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "INSERT INTO COMENTARIOS(SERVICIO_ID, PRODUCTO_ID, CLIENTE_ID, DESCRIPCION, VALORACION) VALUES (?,?,?,?,?)";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setString(1,comentario.);
            
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

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
