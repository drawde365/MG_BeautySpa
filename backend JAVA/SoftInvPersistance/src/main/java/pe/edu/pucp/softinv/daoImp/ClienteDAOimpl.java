package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import static pe.edu.pucp.softinv.daoImp.util.Cifrado.cifrarMD5;
import pe.edu.pucp.softinv.daoImp.util.ParametrosCliente;

public class ClienteDAOimpl extends DAOImplBase implements ClienteDAO {
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
        listaColumnas.add(new Columna("ROL_ID",false,false));
        listaColumnas.add(new Columna("URL_IMAGEN",false,false));
        listaColumnas.add(new Columna("ACTIVO",false,false));
    }

    @Override
    public Integer insertar(ClienteDTO cliente) {
        this.cliente=cliente;
        return super.insertar();
    }

    @Override
    public Integer modificar(ClienteDTO cliente) {
        this.cliente = cliente;
        return super.modificar();
    }

    @Override
    public Integer eliminar(ClienteDTO cliente){
        this.cliente = cliente;
        return super.eliminar();
    }

    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        statement.setString(1,cliente.getPrimerapellido());
        statement.setString(2,cliente.getSegundoapellido());
        statement.setString(3,cliente.getNombre());
        statement.setString(4,cliente.getCorreoElectronico());
        statement.setString(5,cifrarMD5(cliente.getContrasenha()));
        statement.setString(6,cliente.getCelular());
        statement.setInt(7,cliente.getRol());
        statement.setString(8,cliente.getUrlFotoPerfil());
        statement.setInt(9,cliente.getActivo());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        statement.setString(1,cliente.getPrimerapellido());
        statement.setString(2,cliente.getSegundoapellido());
        statement.setString(3,cliente.getNombre());
        statement.setString(4,cliente.getCorreoElectronico());
        statement.setString(5,cifrarMD5(cliente.getContrasenha()));
        statement.setString(6,cliente.getCelular());
        statement.setInt(7,cliente.getRol());
        statement.setString(8,cliente.getUrlFotoPerfil());
        statement.setInt(9, cliente.getActivo());
        statement.setInt(10,cliente.getIdUsuario());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        statement.setInt(1,cliente.getIdUsuario());
    }

    @Override
    public ClienteDTO obtenerPorId(Integer id){
        cliente = new ClienteDTO();
        cliente.setIdUsuario(id);
        super.obtenerPorId();
        return cliente;
    }

    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException {
        statement.setInt(1,cliente.getIdUsuario());
    }

    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.cliente = new ClienteDTO();
        cliente.setIdUsuario(resultSet.getInt("USUARIO_ID"));
        cliente.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
        cliente.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
        cliente.setNombre(resultSet.getString("NOMBRE"));
        cliente.setCorreoElectronico(resultSet.getString("CORREO_ELECTRONICO"));
        cliente.setContrasenha(resultSet.getString("CONTRASENHA"));
        cliente.setCelular(resultSet.getString("CELULAR"));
        cliente.setRol(resultSet.getInt("ROL_ID"));
        cliente.setUrlFotoPerfil(resultSet.getString("URL_IMAGEN"));
        cliente.setActivo(resultSet.getInt("ACTIVO"));
    }

    @Override
    protected void limpiarObjetoDelResultSet(){
        cliente=null;
    }
    
    @Override
    protected void agregarObjetoALaLista(List lista) throws SQLException{
        this.instanciarObjetoDelResultSet();
        lista.add(this.cliente);
    }
    
    @Override
    public ArrayList<ClienteDTO> buscarClienteAdmin(String nombre,String pApe,String sApe,String correo,String celular){
        String sql="SELECT \n" +
                    "    U.USUARIO_ID,\n" +
                    "    U.NOMBRE,\n" +
                    "    U.PRIMER_APELLIDO,\n" +
                    "    U.SEGUNDO_APELLIDO,\n" +
                    "    U.CORREO_ELECTRONICO,\n" +
                    "    U.CELULAR,\n" +
                    "    U.URL_IMAGEN,\n" +
                    "    U.ROL_ID,\n"+
                    "    U.ACTIVO,\n" +
                    "    U.CONTRASENHA\n"+
                    "FROM USUARIOS U\n" +
                    "WHERE \n" +
                    "    U.ROL_ID = 1 AND U.NOMBRE LIKE ? AND U.PRIMER_APELLIDO LIKE ? "+
                    "AND U.SEGUNDO_APELLIDO LIKE ? AND U.CORREO_ELECTRONICO LIKE ? AND U.CELULAR LIKE ?";
        Object parametros = new ParametrosCliente(nombre,pApe,sApe,correo,celular);
        return (ArrayList<ClienteDTO>) super.listarTodos(sql, this::incluirParametrosBusqueda, parametros);
    }
    
    private void incluirParametrosBusqueda(Object parametros){
        try {
            ParametrosCliente personaBuscar = (ParametrosCliente)parametros;
            statement.setString(1, "%"+personaBuscar.getNombre()+"%");
            statement.setString(2, "%"+personaBuscar.getpApe()+"%");
            statement.setString(3, "%"+personaBuscar.getsApe()+"%");
            statement.setString(4, "%"+personaBuscar.getCorreo()+"%");
            statement.setString(5, "%"+personaBuscar.getCelular()+"%");
        } catch (SQLException ex) {
            System.getLogger(ClienteDAOimpl.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
        }
    }
}
