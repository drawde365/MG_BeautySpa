package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.db.DBManager;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class EmpleadoDAOImpl implements EmpleadoDAO {

    private Connection getConnection() throws SQLException {
        return DBManager.getInstance().getConnection();
    }

    @Override
    public int insertar(EmpleadoDTO empleado) {
        String sql = "INSERT INTO USUARIOS (PRIMER_APELLIDO, SEGUNDO_APELLIDO, NOMBRE, " +
                "CORREO_ELECTRONICO, CONTRASENA, CELULAR, ROL, URL_IMAGEN) " +
                "VALUES (?, ?, ?, ?, ?, ?, 'EMPLEADO', ?)";
        try (Connection con = getConnection();
             PreparedStatement ps = con.prepareStatement(sql)) {
            ps.setString(1, empleado.getPrimerapellido());
            ps.setString(2, empleado.getSegundoapellido());
            ps.setString(3, empleado.getNombre());
            ps.setString(4, empleado.getCorreoElectronico());
            ps.setString(5, empleado.getContrasenha());
            ps.setString(6, empleado.getCelular());
            ps.setString(7, empleado.getUrlFotoPerfil());
            return ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
            return 0;
        }
    }

    @Override
    public int modificar(EmpleadoDTO empleado) {
        String sql = "UPDATE USUARIOS SET PRIMER_APELLIDO=?, SEGUNDO_APELLIDO=?, NOMBRE=?, " +
                "CORREO_ELECTRONICO=?, CONTRASENA=?, CELULAR=?, URL_IMAGEN=? " +
                "WHERE USUARIO_ID=? AND ROL='EMPLEADO'";
        try (Connection con = getConnection();
             PreparedStatement ps = con.prepareStatement(sql)) {
            ps.setString(1, empleado.getPrimerapellido());
            ps.setString(2, empleado.getSegundoapellido());
            ps.setString(3, empleado.getNombre());
            ps.setString(4, empleado.getCorreoElectronico());
            ps.setString(5, empleado.getContrasenha());
            ps.setString(6, empleado.getCelular());
            ps.setString(7, empleado.getUrlFotoPerfil());
            ps.setInt(8, empleado.getIdUsuario());
            return ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
            return 0;
        }
    }

    @Override
    public int eliminar(int empleadoId) {
        String sql = "DELETE FROM USUARIOS WHERE USUARIO_ID=? AND ROL='EMPLEADO'";
        try (Connection con = getConnection();
             PreparedStatement ps = con.prepareStatement(sql)) {
            ps.setInt(1, empleadoId);
            return ps.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
            return 0;
        }
    }

    @Override
    public EmpleadoDTO obtenerPorId(int empleadoId) {
        EmpleadoDTO empleado = null;
        String sql = "SELECT u.USUARIO_ID, u.PRIMER_APELLIDO, u.SEGUNDO_APELLIDO, u.NOMBRE, " +
                "u.CORREO_ELECTRONICO, u.CONTRASENA, u.CELULAR, u.ROL, u.URL_IMAGEN, " +
                "s.SERVICIO_ID, s.NOMBRE AS SERVICIO_NOMBRE, s.TIPO, s.PRECIO, " +
                "s.DESCRIPCION, s.PROM_VALORACIONES, s.URL_IMAGEN AS SERVICIO_IMAGEN, s.DURACION_HORAS " +
                "FROM USUARIOS u " +
                "LEFT JOIN EMPLEADOS_SERVICIOS es ON u.USUARIO_ID = es.EMPLEADO_ID " +
                "LEFT JOIN SERVICIOS s ON es.SERVICIO_ID = s.SERVICIO_ID " +
                "WHERE u.USUARIO_ID = ? AND u.ROL = 'EMPLEADO'";

        try (Connection con = getConnection();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, empleadoId);
            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    if (empleado == null) { // 📌 solo una vez se llena el empleado
                        empleado = new EmpleadoDTO();
                        empleado.setIdUsuario(rs.getInt("USUARIO_ID"));
                        empleado.setPrimerapellido(rs.getString("PRIMER_APELLIDO"));
                        empleado.setSegundoapellido(rs.getString("SEGUNDO_APELLIDO"));
                        empleado.setNombre(rs.getString("NOMBRE"));
                        empleado.setCorreoElectronico(rs.getString("CORREO_ELECTRONICO"));
                        empleado.setContrasenha(rs.getString("CONTRASENA"));
                        empleado.setCelular(rs.getString("CELULAR"));
                        empleado.setRol(rs.getString("ROL"));
                        empleado.setUrlFotoPerfil(rs.getString("URL_IMAGEN"));
                        empleado.setServicios(new ArrayList<>());
                    }

                    int servicioId = rs.getInt("SERVICIO_ID");
                    if (servicioId != 0) { // si hay servicios relacionados
                        ServicioDTO servicio = new ServicioDTO();
                        servicio.setIdServicio(servicioId);
                        servicio.setNombre(rs.getString("SERVICIO_NOMBRE"));
                        servicio.setTipo(rs.getString("TIPO"));
                        servicio.setPrecio(rs.getDouble("PRECIO"));
                        servicio.setDescripcion(rs.getString("DESCRIPCION"));
                        servicio.setPromedioValoracion(rs.getDouble("PROM_VALORACIONES"));
                        servicio.setUrlImagen(rs.getString("SERVICIO_IMAGEN"));
                        servicio.setDuracionHora(rs.getInt("DURACION_HORAS"));
                        empleado.getServicios().add(servicio);
                    }
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return empleado;
    }

    @Override
    public List<EmpleadoDTO> listarTodos() {
        List<EmpleadoDTO> lista = new ArrayList<>();
        String sql = "SELECT * FROM USUARIOS WHERE ROL='EMPLEADO'";
        try (Connection con = getConnection();
             Statement st = con.createStatement();
             ResultSet rs = st.executeQuery(sql)) {
            while (rs.next()) {
                lista.add(mapRow(rs, con));
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return lista;
    }

    // 🔹 Ahora mapRow también carga servicios asociados
    private EmpleadoDTO mapRow(ResultSet rs, Connection con) throws SQLException {
        EmpleadoDTO e = new EmpleadoDTO();
        e.setIdUsuario(rs.getInt("USUARIO_ID"));
        e.setPrimerapellido(rs.getString("PRIMER_APELLIDO"));
        e.setSegundoapellido(rs.getString("SEGUNDO_APELLIDO"));
        e.setNombre(rs.getString("NOMBRE"));
        e.setCorreoElectronico(rs.getString("CORREO_ELECTRONICO"));
        e.setContrasenha(rs.getString("CONTRASENA"));
        e.setCelular(rs.getString("CELULAR"));
        e.setRol(rs.getString("ROL"));
        e.setUrlFotoPerfil(rs.getString("URL_IMAGEN"));

        // 👇 cargar servicios asociados con JOIN ANSI-92
        e.setServicios(obtenerServiciosPorEmpleado(e.getIdUsuario(), con));

        return e;
    }
}