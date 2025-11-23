/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/UnitTests/JUnit5TestClass.java to edit this template
 */
package pe.edu.pucp.softinv.business.BO.Impl;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static pe.edu.pucp.softinv.business.BO.Util.Cifrado.cifrarMD5;
import static pe.edu.pucp.softinv.business.BO.Util.Cifrado.descifrarMD5;
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
        System.out.println(descifrarMD5("FMmYXBhup8o="));
        System.out.println(cifrarMD5("cliente"));
        System.out.println(descifrarMD5("Aoe2+iWpHMTiLvyljJ1KUg=="));
        System.out.println(cifrarMD5("empleado"));
        System.out.println(descifrarMD5("U5FJGCHV8tM="));
        System.out.println(cifrarMD5("admin"));
        
        UsuarioDTO usuario=usu.inicioSesion("flavios0226@gmail.com", "cliente");        
    }
    
}
