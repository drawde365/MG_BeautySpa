package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

public class ServicioDAOImpl extends DAOImplBase implements ServicioDAO {

    private ServicioDTO servicio;

    public ServicioDAOImpl() {
        super("SERVICIOS");
        this.servicio = null;
        this.retornarLlavePrimaria=true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        this.listaColumnas.add(new Columna("SERVICIO_ID", true, true));
        this.listaColumnas.add(new Columna("NOMBRE", false, false));
        this.listaColumnas.add(new Columna("TIPO", false, false));
        this.listaColumnas.add(new Columna("PRECIO", false, false));
        this.listaColumnas.add(new Columna("DESCRIPCION", false, false));
        this.listaColumnas.add(new Columna("PROM_VALORACIONES", false, false));
        this.listaColumnas.add(new Columna("URL_IMAGEN", false, false));
        this.listaColumnas.add(new Columna("DURACION_HORAS", false, false));
        this.listaColumnas.add(new Columna("ACTIVO", false, false));
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setString(1, servicio.getNombre());
        this.statement.setString(2, servicio.getTipo());
        this.statement.setDouble(3, servicio.getPrecio());
        this.statement.setString(4, servicio.getDescripcion());
        this.statement.setDouble(5, servicio.getPromedioValoracion());
        this.statement.setString(6, servicio.getUrlImagen());
        this.statement.setInt(7, servicio.getDuracionHora());
        this.statement.setInt(8, servicio.getActivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setString(1, servicio.getNombre());
        this.statement.setString(2, servicio.getTipo());
        this.statement.setDouble(3, servicio.getPrecio());
        this.statement.setString(4, servicio.getDescripcion());
        this.statement.setDouble(5, servicio.getPromedioValoracion());
        this.statement.setString(6, servicio.getUrlImagen());
        this.statement.setInt(7, servicio.getDuracionHora());
        this.statement.setInt(8, servicio.getActivo());
        this.statement.setInt(9, servicio.getIdServicio());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        this.statement.setInt(1, servicio.getIdServicio());
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        this.statement.setInt(1, this.servicio.getIdServicio());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.servicio = new ServicioDTO();
        this.servicio.setIdServicio(this.resultSet.getInt("SERVICIO_ID"));
        this.servicio.setNombre(this.resultSet.getString("NOMBRE"));
        this.servicio.setPrecio(this.resultSet.getDouble("PRECIO"));
        this.servicio.setDescripcion(this.resultSet.getString("DESCRIPCION"));
        this.servicio.setPromedioValoracion(this.resultSet.getDouble("PROM_VALORACIONES"));
        this.servicio.setUrlImagen(this.resultSet.getString("URL_IMAGEN"));
        this.servicio.setDuracionHora(this.resultSet.getInt("DURACION_HORAS"));
        this.servicio.setActivo(this.resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet() {
        this.servicio = null;
    }

    @Override
    public Integer insertar(ServicioDTO servicio){
        this.servicio = servicio;
        return super.insertar();
    }

    @Override
    public ServicioDTO obtenerPorId(Integer idServicio){
        this.servicio = new ServicioDTO();
        this.servicio.setIdServicio(idServicio);
        super.obtenerPorId();
        return this.servicio;
    }

    @Override
    public Integer modificar(ServicioDTO servicio){
        this.servicio=servicio;
        return super.modificar();
    }

    @Override
    public Integer eliminar(ServicioDTO servicio){
        this.servicio=servicio;
        return super.eliminar();
    }
    
    @Override
    public ArrayList<ServicioDTO> obtenerPorPagina(Integer pag){
        String sql = "SELECT * FROM PRODUCTOS LIMIT ?, ?";
        return (ArrayList<ServicioDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPagina,pag);
    }
    
    @Override
    public ArrayList<ServicioDTO> obtenerPorNombre(String nombre){
        String sql = "SELECT * FROM SERVICIOS WHERE NOMBRE LIKE ?";
        return (ArrayList<ServicioDTO>)super.listarTodos(sql,this::incluirValoresDeParametrosParaListarPorNombre,nombre);
    }
    
    @Override
    public ArrayList<ServicioDTO> listarTodos(){
        return (ArrayList<ServicioDTO>)super.listarTodos();
    }
    
    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException {
        this.instanciarObjetoDelResultSet();
        lista.add(this.servicio);
    }
    
    private void incluirValoresDeParametrosParaListarPorNombre(Object objetoParametros){
        String nombre = (String) objetoParametros;
        try {
            this.statement.setString(1,'%'+nombre+'%');
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    private void incluirValoresDeParametrosParaListarPagina(Object objetoParametros){
        Integer pag = (Integer) objetoParametros;
        try {
            this.statement.setInt(1, (pag-1)*10+1);
            this.statement.setInt(2, 10);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
    
    @Override
    public Integer obtenerCantPaginas() {
        int cant;
        String sql = "SELECT COUNT(*) AS COUNT FROM SERVICIOS";
        try {
            this.iniciarTransaccion();
            this.colocarSQLEnStatement(sql);
            this.ejecutarSelectEnDB();
            cant=this.resultSet.getInt("COUNT");
            if(cant%10==0) return cant/10;
            return cant/10+1;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}
