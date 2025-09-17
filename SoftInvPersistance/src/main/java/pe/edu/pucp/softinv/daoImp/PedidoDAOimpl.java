package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

public class PedidoDAOimpl {

    private Connection conexion;
    private ResultSet resultSet;
    private CallableStatement statement;

    public Integer insertar(PedidoDTO pedido){

    }

    public PedidoDTO optenerPorId(Integer id){

    }

    public Integer modificar(PedidoDTO pedido){
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "UPDATE PEDIDOS SET ESTADO=?, FECHA_PAGO=?, FECHA_RECOJO=? WHERE PEDIDO_ID=?";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setString(1, pedido.getEstadoPedidoS());
            this.statement.setString(2, pedido.getf());
            this.statement.setString(3, pedido.getUrlImagen());
            this.statement.setInt(4, pedido.getIdPedido());
            resultado = this.statement.executeUpdate();
            this.conexion.commit();
        } catch (SQLException ex) {
            System.err.println("Error al intentar modificar - " + ex);
            try {
                if (this.conexion != null) {
                    this.conexion.rollback();
                }
            } catch (SQLException ex1) {
                System.err.println("Error al hacer rollback - " + ex1);
            }
        } finally {
            try {
                if (this.conexion != null) {
                    this.conexion.close();
                }
            } catch (SQLException ex) {
                System.err.println("Error al cerrar la conexión - " + ex);
            }
        }
        return resultado;
    }

    public Integer eliminar(Integer id){

    }

    public ArrayList<PedidoDTO> listarPedidos(Integer id){

    }
}
