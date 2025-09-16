package pe.edu.pucp.softinv.db;

import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;
import pe.edu.pucp.softinv.db.util.Cifrado;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class DBManager {
    private static DBManager dbManager = null;
    private static final String archvo_conf = "jdbc.properties";
    private String driver;
    private String tipo_de_driver;
    private String base_de_datos;
    private String host;
    private String puerto;
    private String usuario;
    private String contraseña;
    private Connection conexion;

    private DBManager() {

    }

    public static DBManager getInstance() {
        if (dbManager == null) {
            DBManager.createInstance();
        }
        return dbManager;
    }

    private static void createInstance() {
        dbManager = new DBManager();
        DBManager.dbManager.leer_archivo_configuracion();
    }

    private void leer_archivo_configuracion() {
        Properties prop = new Properties();
        String nomArch = "/"+archvo_conf;
        try {
            prop.load(this.getClass().getResourceAsStream(nomArch));
            driver = prop.getProperty("driver");
            tipo_de_driver = prop.getProperty("tipo_de_driver");
            base_de_datos = prop.getProperty("base_de_datos");
            host = prop.getProperty("host");
            puerto = prop.getProperty("puerto");
            usuario = prop.getProperty("usuario");
            contraseña = prop.getProperty("contrasenha");
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }

    public Connection getConnection() {
        try {
            Class.forName(driver);
            conexion = DriverManager.getConnection(getUrl(),usuario,contraseña);
        } catch (ClassNotFoundException | SQLException e) {
            throw new RuntimeException(e);
        }
        return conexion;
    }

    private String getUrl() {
        return tipo_de_driver+"://"+host+":"+puerto+"/"+base_de_datos;
    }
}