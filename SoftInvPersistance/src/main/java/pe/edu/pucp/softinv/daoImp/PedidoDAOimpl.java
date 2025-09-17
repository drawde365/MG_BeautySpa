package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Pedido.DetallePedidoDTO;
import pe.edu.pucp.softinv.model.Pedido.PedidoDTO;

import java.sql.*;
import java.util.ArrayList;

public class PedidoDAOimpl {

    private Connection conexion;
    private ResultSet resultSet;
    private CallableStatement statement;

    public Integer insertar(PedidoDTO pedido) {
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "INSERT INTO PEDIDOS (CLIENTE_ID, TOTAL, ESTADO, FECHA_PAGO, FECHA_LISTA_PARA_RECOGER, FECHA_RECOJO, IGV) " +
                    "VALUES (?,?,?,?,?,?,?)";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setInt(1, pedido.getIDCliente());
            this.statement.setDouble(2, pedido.getTotal());
            this.statement.setString(3, pedido.getEstadoPedidoS());
            this.statement.setDate(4, (Date) pedido.getFechaPago());
            this.statement.setDate(5, (Date) pedido.getFechaListaParaRecojo());
            this.statement.setDate(6, (Date) pedido.getFechaRecojo());
            this.statement.setDouble(7, pedido.getIGV());
            resultado = this.statement.executeUpdate();
            for (DetallePedidoDTO detalle : pedido.getDetallesPedido()) {
                sql = "INSERT INTO DETALLES_PEDIDOS (PEDIDO_ID, PRODUCTO_ID, CANTIDAD, SUBTOTAL) " +
                        "VALUES (?,?,?,?)";
                this.statement = this.conexion.prepareCall(sql);
                this.statement.setInt(1, pedido.getIdPedido());
                this.statement.setInt(2, detalle.getProducto().getIdProducto());
                this.statement.setInt(3, detalle.getCantidad());
                this.statement.setDouble(4, detalle.getSubtotal());
                resultado = this.statement.executeUpdate();
            }
            this.conexion.commit();
        } catch (SQLException ex) {
            System.err.println("Error al intentar insertar - " + ex);
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

    public PedidoDTO optenerPorId(Integer id) {
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "SELECT * FROM PRODUCTOS WHERE PRODUCTO_ID=?";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setInt(1, id);
            resultado = this.statement.executeUpdate();
            this.conexion.commit();
        } catch (SQLException ex) {
            System.err.println("Error al obtener por id - " + ex);
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

    public Integer modificar(PedidoDTO pedido) {
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "UPDATE PEDIDOS SET ESTADO=?, FECHA_PAGO=?, FECHA_RECOJO=?, FECHA_LISTA_PARA_RECOGER=? WHERE PEDIDO_ID=?";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setString(1, pedido.getEstadoPedidoS());
            this.statement.setDate(2, (Date) pedido.getFechaPago());
            this.statement.setDate(3, (Date) pedido.getFechaRecojo());
            this.statement.setDate(4, (Date) pedido.getFechaListaParaRecojo());
            this.statement.setInt(5, pedido.getIdPedido());
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

    public Integer eliminar(Integer id) {

    }

    public ArrayList<PedidoDTO> listarPedidos(Integer id) {

    }
}
