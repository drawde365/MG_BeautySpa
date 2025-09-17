package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ProductoDAOimpl implements ProductoDAO {
    private Connection conexion;
    private CallableStatement statement;
    protected ResultSet resultSet;

    @Override
    public Integer insertar(ProductoDTO producto){
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "INSERT INTO PRODUCTOS (PRODUCTO_ID, NOMBRE, TIPO_PRODUCTO, PRECIO, " +
                    "DESCRIPCION, URL_IMAGEN, STOCK, MODO_DE_USO) VALUES (?,?,?,?,?,?,?,?)";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setInt(1 , producto.getIdProducto());
            this.statement.setString(2, producto.getNombre());
            this.statement.setString(3, producto.getTipoProductoS());
            this.statement.setDouble(4, producto.getPrecio());
            this.statement.setString(5, producto.getDescripcion());
            this.statement.setString(6, producto.getUrlImagen());
            this.statement.setInt(7, producto.getStock());
            this.statement.setString(8, producto.getModoUso());
            resultado = this.statement.executeUpdate();
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

    @Override
    public Integer eliminar(Integer id){
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "DELETE FROM PRODUCTOS WHERE TIPO_DOCUMENTO_ID=?";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setInt(1, id);
            resultado = this.statement.executeUpdate();
            this.conexion.commit();
        } catch (SQLException ex) {
            System.err.println("Error al intentar eliminar - " + ex);
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

    @Override
    public ProductoDTO optenerPorId(Integer id){
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

    @Override
    public Integer modificar(ProductoDTO producto){
        int resultado = 0;
        try {
            this.conexion = DBManager.getInstance().getConnection();
            this.conexion.setAutoCommit(false);
            String sql = "UPDATE PRODUCTOS SET NOMBRE=?, TIPO_PRODUCTO=?, PRECIO=?, DESCRIPCION=?, URL_IMAGE=?, STOCK=?, MODO_DE_USO=? WHERE PRODUCTO_ID=?";
            this.statement = this.conexion.prepareCall(sql);
            this.statement.setString(1, producto.getNombre());
            this.statement.setString(2, producto.getTipoProductoS());
            this.statement.setDouble(3, producto.getPrecio());
            this.statement.setString(4, producto.getDescripcion());
            this.statement.setString(5, producto.getUrlImagen());
            this.statement.setInt(6, producto.getStock());
            this.statement.setString(7, producto.getModoUso());
            this.statement.setInt(8, producto.getIdProducto());
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

}
