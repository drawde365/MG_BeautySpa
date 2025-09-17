package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

import java.sql.SQLException;

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
        listaColumnas.add(new Columna("ROL",false,false));
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
        statement.setString(5,cliente.getContrasenha());
        statement.setString(6,cliente.getCelular());
        statement.setString(7,cliente.getRol());
    }

    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        statement.setString(7,cliente.getPrimerapellido());
        statement.setString(1,cliente.getSegundoapellido());
        statement.setString(2,cliente.getNombre());
        statement.setString(3,cliente.getCorreoElectronico());
        statement.setString(4,cliente.getContrasenha());
        statement.setString(5,cliente.getCelular());
        statement.setString(6,cliente.getRol());
    }

    @Override
    protected void incluirValorDeParametrosParaEliminacion() throws SQLException {
        statement.setInt(1,cliente.getIdUsuario());
    }

    @Override
    public ClienteDTO obtenerPorId(Integer id){
        ClienteDTO usuario = null;
        try {
            conexion = DBManager.getInstance().getConnection();
            String sql = "SELECT USUARIO_ID, PRIMER_APELLIDO, SEGUNDO_APELLIDO, NOMBRE, CORREO_ELECTRONICO,CONTRASENHA,CELULAR,ROL" +
                    " WHERE USUARIO_ID=?";
            statement = conexion.prepareCall(sql);
            statement.setInt(1,id);
            resultSet = statement.executeQuery();
            if(resultSet.next()){
                usuario = new ClienteDTO();
                usuario.setPrimerapellido(resultSet.getString("PRIMER_APELLIDO"));
                usuario.setSegundoapellido(resultSet.getString("SEGUNDO_APELLIDO"));
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        } finally{
            try{
                if(this.conexion!=null)
                    conexion.close();
            } catch (SQLException e) {
                throw new RuntimeException(e);
            }
        }
        return usuario;
    }
}
