package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioProductoDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioServicioDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;

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
        // SET ( =?)
        this.statement.setInt(1,comentario.getIdServicio());
        this.statement.setInt(2,comentario.getIdProducto());
        this.statement.setInt(3,comentario.getCliente().getIdUsuario());
        this.statement.setString(4,comentario.getComentario());
        this.statement.setInt(5,comentario.getValoracion());
        //WHERE = ?
        this.statement.setInt(6,comentario.getIdComentario());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException{
        this.statement.setInt(1,comentario.getIdComentario());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.comentario.getIdComentario());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        Integer Producto_ID = this.resultSet.getInt("PRODUCTO_ID");
        Integer Servicio_ID = this.resultSet.getInt("SERVICIO_ID");
        if (Producto_ID>0  && Servicio_ID == null) {
            this.comentario = new ComentarioProductoDTO();
            ComentarioProductoDTO comentarioProducto = (ComentarioProductoDTO) this.comentario;
            ProductoDAOimpl productoDAO = new ProductoDAOimpl();
            ProductoDTO producto = productoDAO.obtenerPorId(Producto_ID);
            
        }else{
            if (Producto_ID == null && Servicio_ID>0){
                this.comentario = new ComentarioServicioDTO();
            }else
                this.comentario = null;
        }
        this.comentario.setIdComentario(this.resultSet.getInt("COMENTARIO_ID"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.comentario = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException{
        this.instanciarObjetoDelResultSet();
        lista.add(this.comentario);
    }

    @Override
    public Integer insertar(ComentarioDTO comentario)  {
        this.comentario = comentario;
        return super.insertar();
    }

    @Override
    public ComentarioDTO obtenerPorId(Integer idComentario){
        this.comentario = new ComentarioDTO();
        this.comentario.setIdComentario(idComentario);
        super.obtenerPorId();
        return this.comentario;
    }

    @Override
    public Integer modificar(ComentarioDTO comentario) {
        this.comentario = comentario;
        return super.modificar();
    }

    @Override
    public Integer eliminar(ComentarioDTO comentario) {
        this.comentario = comentario;
        return super.eliminar();
    }

}
