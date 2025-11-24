package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.sql.SQLException;
import static pe.edu.pucp.softinv.daoImp.util.Cifrado.cifrarMD5;

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
}
