package pe.edu.pucp.softpub.db;

import pe.edu.pucp.softinv.db.DBManager;

import java.sql.Connection;

import static org.junit.jupiter.api.Assertions.*;
import pe.edu.pucp.softinv.db.util.Cifrado;

public class DBManagerTest {

    @org.junit.jupiter.api.Test
    public void getInstance() {
        System.out.println(Cifrado.cifrarMD5(""));
    
        System.out.println("getInstance");
        DBManager dBManager = DBManager.getInstance();
        assertNotNull(dBManager);
    }

    @org.junit.jupiter.api.Test
    public void getConnection() {
        System.out.println("getConnection");
        DBManager dBManager = DBManager.getInstance();
        Connection conexion = dBManager.getConnection();
        assertNotNull(conexion);
    }
}