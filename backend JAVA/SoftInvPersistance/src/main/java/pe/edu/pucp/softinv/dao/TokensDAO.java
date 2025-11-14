/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Token.ContrasenhaTokenDTO;

/**
 *
 * @author Rodrigo Cahuana
 */
public interface TokensDAO {
    public Integer insertarTokenRecuperacion(Integer usuarioId,String token);
    public ContrasenhaTokenDTO obtenerToken(String token);
    public Integer marcarTokenUsado(ContrasenhaTokenDTO token);
}
