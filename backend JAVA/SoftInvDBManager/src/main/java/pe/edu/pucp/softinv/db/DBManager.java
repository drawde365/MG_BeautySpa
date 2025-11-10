package pe.edu.pucp.softinv.db;

import com.zaxxer.hikari.HikariConfig;
import com.zaxxer.hikari.HikariDataSource;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;
import javax.sql.DataSource;
import pe.edu.pucp.softinv.db.util.Cifrado;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class DBManager {
    private static DBManager dbManager = null;
    private DataSource dataSource;
    private static final String archvo_conf = "jdbc.properties";
    private String driver;
    private String tipo_de_driver;
    private String base_de_datos;
    private String host;
    private String puerto;
    private String usuario;
    private String contraseña;

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
        DBManager.dbManager.createDataSource();
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
            return dataSource.getConnection();
        } catch (SQLException ex) {
            System.getLogger(DBManager.class.getName()).log(System.Logger.Level.ERROR, (String) null, ex);
        }
        return null;
    }

    private String getUrl() {
        return tipo_de_driver+"://"+host+":"+puerto+"/"+base_de_datos+"?useSSL=false";
    }

    private void createDataSource() {
        HikariConfig config = new HikariConfig();
        config.setJdbcUrl(this.getUrl());
        config.setUsername(usuario);
        config.setPassword(Cifrado.descifrarMD5(contraseña));
        config.setMaximumPoolSize(15);
        config.setMinimumIdle(3);
        config.setIdleTimeout(60000); 
        config.setConnectionTimeout(20000);
        config.setLeakDetectionThreshold(10000);
        dataSource = new HikariDataSource(config);
    }
}