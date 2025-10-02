package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.dao.ProductoDAO;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioProductoDTO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioServicioDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

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
        this.statement.setObject(1,comentario.getIdServicio(),java.sql.Types.INTEGER);
        this.statement.setObject(2,comentario.getIdProducto(),java.sql.Types.INTEGER);
        this.statement.setInt(3,comentario.getCliente().getIdUsuario());
        this.statement.setString(4,comentario.getComentario());
        this.statement.setInt(5,comentario.getValoracion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        // SET ( =?)
        this.statement.setObject(1,comentario.getIdServicio(),java.sql.Types.INTEGER);
        this.statement.setObject(2,comentario.getIdProducto(),java.sql.Types.INTEGER);
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
            ProductoDAO productoDAO = new ProductoDAOimpl();
            ProductoDTO producto = productoDAO.obtenerPorId(Producto_ID);
            comentarioProducto.setProducto(producto);
        }else{
            if (Producto_ID == null && Servicio_ID>0){
                this.comentario = new ComentarioServicioDTO();
                ComentarioServicioDTO comentarioServicioDTO = (ComentarioServicioDTO) this.comentario;
                ServicioDAO servicioDAO = new ServicioDAOImpl();
                ServicioDTO servicio = servicioDAO.obtenerPorId(Servicio_ID);
                comentarioServicioDTO.setServicio(servicio);
            }else
                this.comentario = null;
        }
        Integer cliente_ID = this.resultSet.getInt("CLIENTE_ID");
        ClienteDTO cliente = null;
        if (cliente_ID > 0) {
            ClienteDAO clienteDAO = new ClienteDAOimpl();
            cliente = clienteDAO.obtenerPorId(cliente_ID);
            this.comentario.setCliente(cliente);
        }
        this.comentario.setIdComentario(this.resultSet.getInt("COMENTARIO_ID"));
        this.comentario.setComentario(this.resultSet.getString("DESCRIPCION"));
        this.comentario.setValoracion(this.resultSet.getInt("VALORACION"));

    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.comentario = null;
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
