/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.softinv.dao;

import java.util.ArrayList;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author Rodrigo Cahuana
 */
public interface UsuarioDAO {
    
    public UsuarioDTO busquedaPorCorreo(String correo);
    public Integer actualizarContrasenha(Integer usuarioId,String nuevaContrasenha);
    public ArrayList<UsuarioDTO> obtenerUsuarios(); 
    public Integer modificarActivoCliente(Integer usuarioId,Integer activo);
}
