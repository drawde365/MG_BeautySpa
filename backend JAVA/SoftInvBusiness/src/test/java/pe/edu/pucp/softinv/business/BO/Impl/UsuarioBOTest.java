/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/UnitTests/JUnit5TestClass.java to edit this template
 */
package pe.edu.pucp.softinv.business.BO.Impl;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;

/**
 *
 * @author danie
 */
public class UsuarioBOTest {
    
    private UsuarioBO usu;
    
    public UsuarioBOTest() {
        usu = new UsuarioBO();
    }

    /**
     * Test of inicioSesion method, of class UsuarioBO.
     */
    @Test
    public void testInicioSesion() {
        System.out.println("inicioSesion");
        String correoElectronico = "CLIENTE";
        String contrasenha = "123";
        UsuarioDTO usuario = usu.inicioSesion(correoElectronico, contrasenha);
        assertNotNull(usuario);
    }
    
}
