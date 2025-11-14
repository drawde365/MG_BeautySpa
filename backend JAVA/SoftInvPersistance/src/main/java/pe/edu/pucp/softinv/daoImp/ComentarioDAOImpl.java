package pe.edu.pucp.softinv.daoImp;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.daoImp.util.ComentariosParametros;
import pe.edu.pucp.softinv.daoImp.util.ComentariosParametrosBuilder;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;
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
        //if(comentario.getServicio()!=null)
            this.statement.setObject(1,comentario.getServicio().getIdServicio(),java.sql.Types.INTEGER);
        //else
        //    this.statement.setObject(1,null,java.sql.Types.INTEGER);
        //if(comentario.getProducto()!=null)
            this.statement.setObject(2,comentario.getProducto().getIdProducto(),java.sql.Types.INTEGER);
        //else
        //    this.statement.setObject(2,null ,java.sql.Types.INTEGER);
        this.statement.setInt(3,comentario.getCliente().getIdUsuario());
        this.statement.setString(4,comentario.getComentario());
        this.statement.setInt(5,comentario.getValoracion());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        // SET ( =?)
        //if(comentario.getServicio()!=null)
            this.statement.setObject(1,comentario.getServicio().getIdServicio(),java.sql.Types.INTEGER);
        //else
        //    this.statement.setObject(1,null,java.sql.Types.INTEGER);
        //if(comentario.getProducto()!=null)
            this.statement.setObject(2,comentario.getProducto().getIdProducto(),java.sql.Types.INTEGER);
        //else
        //    this.statement.setObject(2,null ,java.sql.Types.INTEGER);
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
    protected void instanciarObjetoDelResultSet() throws SQLException{
        this.comentario = new ComentarioDTO();
        this.comentario.setIdComentario(this.resultSet.getInt("COMENTARIO_ID"));
        Integer Producto_ID = this.resultSet.getInt("PRODUCTO_ID");
        if(Producto_ID.equals(0)) Producto_ID = null;
        Integer Servicio_ID = this.resultSet.getInt("SERVICIO_ID");
        if(Servicio_ID.equals(0)) Servicio_ID = null;
        ProductoDTO producto = new ProductoDTO();
        ServicioDTO servicio = new ServicioDTO();
        producto.setIdProducto(Producto_ID);
        servicio.setIdServicio(Servicio_ID);
        this.comentario.setServicio(servicio);
        this.comentario.setProducto(producto);
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(this.resultSet.getInt("CLIENTE_ID"));
        this.comentario.setCliente(cliente);
        this.comentario.setComentario(this.resultSet.getString("DESCRIPCION"));
        this.comentario.setValoracion(this.resultSet.getInt("VALORACION"));
    }

    protected void instanciarObjetoDelResultSetSP() throws SQLException {
        this.comentario = new ComentarioDTO();
        this.comentario.setIdComentario(this.resultSet.getInt("COMENTARIO_ID"));
        Integer Producto_ID = this.resultSet.getInt("PRODUCTO_ID");
        Integer Servicio_ID = this.resultSet.getInt("SERVICIO_ID");
        ProductoDTO producto = new ProductoDTO();
        ServicioDTO servicio = new ServicioDTO();
        producto.setIdProducto(Producto_ID);
        servicio.setIdServicio(Servicio_ID);
        if (Producto_ID.compareTo(0)>0  && Servicio_ID.equals(0)) {
            comentario.getProducto().setIdProducto(Producto_ID);
            comentario.getServicio().setIdServicio(null);
        }else if(Producto_ID.equals(0) && Servicio_ID.compareTo(0)>0){
            comentario.getServicio().setIdServicio(Servicio_ID);
            comentario.getProducto().setIdProducto(null);
        }
        ClienteDTO cliente = this.instanciarCliente();
        this.comentario.setCliente(cliente);
        this.comentario.setComentario(this.resultSet.getString("DESCRIPCION"));
        this.comentario.setValoracion(this.resultSet.getInt("VALORACION"));
    }



    private ClienteDTO instanciarCliente() throws SQLException {
        ClienteDTO cliente = new ClienteDTO();
        cliente.setIdUsuario(this.resultSet.getInt("CLIENTE_ID"));
        cliente.setNombre(this.resultSet.getString("NOMBRE"));
        cliente.setPrimerapellido(this.resultSet.getString("PRIMER_APELLIDO"));
        cliente.setSegundoapellido(this.resultSet.getString("SEGUNDO_APELLIDO"));
        cliente.setCorreoElectronico(this.resultSet.getString("CORREO_ELECTRONICO"));
        cliente.setUrlFotoPerfil(this.resultSet.getString("URL_IMAGEN"));
        return cliente;
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.comentario = null;
    }

    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSetSP();
        lista.add(comentario);
    }

    @Override
    public Integer insertar(ComentarioDTO comentario)  {
        this.comentario = comentario;
        return super.insertar();
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

    @Override
    public ComentarioDTO obtenerPorId(Integer idComentario) {
        this.comentario = new ComentarioDTO();
        comentario.setIdComentario(idComentario);
        super.obtenerPorId();
        return comentario;
    }

    @Override
    public ArrayList<ComentarioDTO> obtenerComentariosPorProducto(Integer idProducto){
        Object parametros = new ComentariosParametrosBuilder()
                .conProducto_Id(idProducto)
                .BuildComentariosParametros();
        String sql = "SELECT \n" +
            "        c.COMENTARIO_ID,\n" +
            "        c.SERVICIO_ID,\n" +
            "        c.PRODUCTO_ID,\n" +
            "        c.DESCRIPCION,\n" +
            "        c.VALORACION,\n" +
            "        u.USUARIO_ID AS CLIENTE_ID,\n" +
            "        u.NOMBRE,\n" +
            "        u.PRIMER_APELLIDO,\n" +
            "        u.SEGUNDO_APELLIDO,\n" +
            "        u.CORREO_ELECTRONICO,\n" +
            "        u.URL_IMAGEN\n" +
            "    FROM COMENTARIOS c\n" +
            "    INNER JOIN USUARIOS u ON u.USUARIO_ID = c.CLIENTE_ID\n" +
            "    WHERE c.PRODUCTO_ID = ?\n" +
            "    ORDER BY c.VALORACION DESC;";
        return (ArrayList<ComentarioDTO>) super.listarTodos(sql,this::incluirParametrosParaListarPorProducto,parametros);
    }

    private void incluirParametrosParaListarPorProducto(Object Parametros){
        ComentariosParametros parametros = (ComentariosParametros) Parametros;
        try {
            this.statement.setInt(1,parametros.getProducto_Id());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public ArrayList<ComentarioDTO> obtenerComentariosPorServicio(Integer idServicio){
        Object parametros = new ComentariosParametrosBuilder()
                .conServicio_Id(idServicio)
                .BuildComentariosParametros();
        String sql = "SELECT \n" +
        "        c.COMENTARIO_ID,\n" +
        "	 c.SERVICIO_ID,\n" +
        "	 c.PRODUCTO_ID,\n" +
        "        c.DESCRIPCION,\n" +
        "        c.VALORACION,\n" +
        "        u.USUARIO_ID AS CLIENTE_ID,\n" +
        "        u.NOMBRE,\n" +
        "        u.PRIMER_APELLIDO,\n" +
        "        u.SEGUNDO_APELLIDO,\n" +
        "        u.CORREO_ELECTRONICO,\n" +
        "        u.URL_IMAGEN\n" +
        "    FROM COMENTARIOS c\n" +
        "    INNER JOIN USUARIOS u ON u.USUARIO_ID = c.CLIENTE_ID\n" +
        "    WHERE c.SERVICIO_ID = ?\n" +
        "    ORDER BY c.VALORACION DESC;";
        return (ArrayList<ComentarioDTO>) super.listarTodos(sql,this::incluirParametrosParaListarPorServicio,parametros);
    }

    private void incluirParametrosParaListarPorServicio(Object Parametros){
        ComentariosParametros parametros = (ComentariosParametros) Parametros;
        try {
            this.statement.setInt(1,parametros.getServicio_Id());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

}
