package pe.edu.pucp.softinv.db;

import com.zaxxer.hikari.HikariConfig;
import com.zaxxer.hikari.HikariDataSource;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;
import javax.sql.DataSource;
import pe.edu.pucp.softinv.db.util.Cifrado;
import pe.edu.pucp.softinv.db.util.MotorDeBaseDeDatos;

public abstract class DBManager {
    private static DBManager dbManager = null;
    private DataSource dataSource;
    private static final String archvo_conf = "jdbc.properties";
    protected String driver;
    protected String tipo_de_driver;
    protected String base_de_datos;
    protected String nombre_de_host;
    protected String host;
    protected String puerto;
    protected String usuario;
    protected String contraseña;

    protected DBManager() {
        
    }

    public static DBManager getInstance() {
        if (DBManager.dbManager == null) {
            DBManager.createInstance();
        }
        return DBManager.dbManager;
    }

    private static void createInstance() {
        if (DBManager.dbManager == null) {
            if (DBManager.obtenerMotorDeBaseDeDato() == MotorDeBaseDeDatos.MYSQL) {
                DBManager.dbManager = new DBManagerMySQL();
            } else{
                DBManager.dbManager = new DBManagerMSSQL();
            }
            DBManager.dbManager.leer_archivo_configuracion();
            dbManager.createDataSource();
        }
    }

    private void leer_archivo_configuracion() {
        Properties prop = new Properties();
        String nomArch = "/"+archvo_conf;
        try {
            prop.load(this.getClass().getResourceAsStream(nomArch));
            tipo_de_driver = prop.getProperty("tipo_de_driver");
            if (tipo_de_driver.equals("jdbc:mysql")) {
                driver = prop.getProperty("driverMySQL");
                base_de_datos = prop.getProperty("base_de_datosMySQL");
                host = prop.getProperty("hostMySQL");
                puerto = prop.getProperty("puertoMySQL");
                usuario = prop.getProperty("usuarioMySQL");
                contraseña = prop.getProperty("contrasenhaMySQL");
            }
            else {
                driver = prop.getProperty("driverMSSQL");
                base_de_datos = prop.getProperty("base_de_datosMSSQL");
                host = prop.getProperty("hostMSSQL");
                puerto = prop.getProperty("puertoMSSQL");
                usuario = prop.getProperty("usuarioMSSQL");
                contraseña = prop.getProperty("contrasenhaMSSQL");
            }
            
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

    protected abstract String getURL();

    private void createDataSource() {
        HikariConfig config = new HikariConfig();
        config.setJdbcUrl(this.getURL());
        config.setUsername(usuario);
        config.setPassword(Cifrado.descifrarMD5(contraseña));
        config.setDriverClassName(driver);
        
        config.setMaximumPoolSize(15);
        config.setMinimumIdle(5);
        config.setIdleTimeout(600000); 
        config.setConnectionTimeout(20000);
        config.setMaxLifetime(1800000);
        
         // Optimizaciones
        config.setAutoCommit(false);
        config.addDataSourceProperty("cachePrepStmts", "true");
        config.addDataSourceProperty("prepStmtCacheSize", "250");
        config.addDataSourceProperty("prepStmtCacheSqlLimit", "2048");
        config.addDataSourceProperty("useServerPrepStmts", "true");

        // Pool name para debugging
        config.setPoolName("SoftInvHikariPool");
        
        dataSource = new HikariDataSource(config);
    }
    
    private static MotorDeBaseDeDatos obtenerMotorDeBaseDeDato() {
        Properties properties = new Properties();
        try {
            String nmArchivoConf = "/" + archvo_conf;

            //al ser un método estático, no se puede invocar al getResoucer así
            //properties.load(this.getClass().getResourceAsStream(nmArchivoConf));            
            properties.load(DBManager.class.getResourceAsStream(nmArchivoConf));            
            String tipo_de_driver = properties.getProperty("tipo_de_driver");
            if (tipo_de_driver.equals("jdbc:mysql"))
                return MotorDeBaseDeDatos.MYSQL;
            else
                return MotorDeBaseDeDatos.MSSQL;
        } catch (FileNotFoundException ex) {
            System.err.println("Error al leer el archivo de propiedades - " + ex);
        } catch (IOException ex) {
            System.err.println("Error al leer el archivo de propiedades - " + ex);
        }
        return null;
    }
     public abstract String retornarSQLParaUltimoAutoGenerado();
}