/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp;

import java.sql.SQLException;
import java.sql.Timestamp;
import pe.edu.pucp.softinv.dao.TokensDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Token.ContrasenhaTokenDTO;

/**
 *
 * @author Rodrigo Cahuana
 */
public class TokensDAOImpl extends DAOImplBase implements TokensDAO{
    private ContrasenhaTokenDTO tokenDTO;
    
    public TokensDAOImpl(){
        super("RESTABLECER_CONTRASENHA_TOKENS");
        tokenDTO = null;
        retornarLlavePrimaria = true;
    }
    
    @Override
    protected void configurarListaDeColumnas() {
        listaColumnas.add(new Columna("TOKEN_ID", true, true));
        listaColumnas.add(new Columna("USUARIO_ID", false, false));
        listaColumnas.add(new Columna("TOKEN", false, false));
        listaColumnas.add(new Columna("FECHA_EXPIRACION", false, false));
        listaColumnas.add(new Columna("USADO", false, false));
    }
    
    @Override
    protected void incluirValorDeParametrosParaInsercion() throws SQLException {
        this.statement.setInt(1,tokenDTO.getUsuarioId());
        this.statement.setString(2, tokenDTO.getToken());
        this.statement.setTimestamp(3, new Timestamp(System.currentTimeMillis()+(30 * 60 * 1000)));
        this.statement.setInt(4,tokenDTO.getUsado());
    }
    
    @Override
    public Integer insertarTokenRecuperacion(Integer usuarioId,String token){
        tokenDTO = new ContrasenhaTokenDTO();
        tokenDTO.setToken(token);
        tokenDTO.setUsuarioId(usuarioId);
        tokenDTO.setUsado(0);
        return super.insertar();
    }
    
    @Override
    protected void instanciarObjetoDelResultSet() throws SQLException {
        this.tokenDTO = new ContrasenhaTokenDTO();
        tokenDTO.setTokenId(resultSet.getInt("TOKEN_ID"));
        tokenDTO.setUsuarioId(resultSet.getInt("USUARIO_ID"));
        tokenDTO.setToken(resultSet.getString("TOKEN"));
        tokenDTO.setFecha_expiracion(resultSet.getTimestamp("FECHA_EXPIRACION"));
        tokenDTO.setUsado(resultSet.getInt("USADO"));
    }
    
    @Override
    protected void limpiarObjetoDelResultSet() {
        this.tokenDTO = null;
    }
    
    @Override
    protected void incluirValorDeParametrosParaObtenerPorId() throws SQLException{
        statement.setString(1, tokenDTO.getToken());
    }
            
    @Override
    public ContrasenhaTokenDTO obtenerToken(String token){
        tokenDTO = new ContrasenhaTokenDTO();
        tokenDTO.setToken(token);
        String sql = "SELECT TOKEN_ID, USUARIO_ID, TOKEN, FECHA_EXPIRACION, USADO\n" +
        "FROM RESTABLECER_CONTRASENHA_TOKENS\n" +
        "WHERE TOKEN = ?";
        super.obtenerPorId(sql);
        return tokenDTO;
    }
    
    @Override
    protected void incluirValorDeParametrosParaModificacion() throws SQLException {
        this.statement.setInt(1, tokenDTO.getUsuarioId());
        this.statement.setString(2, tokenDTO.getToken());
        this.statement.setTimestamp(3, new Timestamp(tokenDTO.getFecha_expiracion().getTime()));
        this.statement.setInt(4, tokenDTO.getUsado());
        this.statement.setInt(5, tokenDTO.getTokenId());
    }
    
    @Override
    public Integer marcarTokenUsado(ContrasenhaTokenDTO token){
        tokenDTO = token;
        return super.modificar();
    }
}
