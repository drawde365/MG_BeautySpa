/*
package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.dao.ClienteDAO;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;

public class CitaDAOImpl extends DAOImplBase implements CitaDAO {

    private CitaDTO cita;

    public CitaDAOImpl() {
        super("CITAS");
        cita = null;
        retornarLlavePrimaria=true;
    }

    @Override
    protected void configurarListaDeColumnas() {
        listaColumnas.add(new Columna("CITA_ID", true, true));
        listaColumnas.add(new Columna("EMPLEADO_ID", false, false));
        listaColumnas.add(new Columna("CLIENTE_ID", false, false));
        listaColumnas.add(new Columna("SERVICIO_ID", false, false));
        listaColumnas.add(new Columna("FECHA", false, false));
        listaColumnas.add(new Columna("HORA_INICIO", false, false));
        listaColumnas.add(new Columna("HORA_FIN", false, false));
    }

    @Override
    public Integer insertar(CitaDTO cita) {
        this.cita = cita;
        return super.insertar();
    }

    @Override
    public Integer modificar(CitaDTO cita) {
        this.cita = cita;
        return super.modificar();
    }

    @Override
    public Integer eliminar(CitaDTO cita) {
        this.cita = cita;
        return super.eliminar();
    }

    @Override
    ArrayList<CitaDTO> listarCitasPorPeriodo(Date inicio, Date fin) {
        ArrayList<CitaDTO> citas = new ArrayList<>();
        String sql = "SELECT * FROM CITAS";
        this.abrirConexion();
        try {
            this.colocarSQLEnStatement(sql);
            this.ejecutarSelectEnDB();
            while (this.resultSet.next()) {
                Date fecha=statement.getDate("FECHA");
                if (fecha.compareTo(inicio) >= 0 && fecha.compareTo(fin) <= 0){
                    int empleadoId, clienteId, servicioId;
                    CitaDTO cita = new CitaDTO();
                    EmpleadoDAO empleado = new EmpleadoDAOImpl();
                    ClienteDAO cliente = new ClienteDAOimpl();
                    ServicioDAO servicio = new ServicioDAOImpl();

                    cita.setId(resultSet.getInt("CITA_ID"));
                    empleadoId= resultSet.getInt("EMPLEADO_ID");
                    cita.setEmpleado(empleado.obtenerPorId(empleadoId));
                    clienteId= resultSet.getInt("CLIENTE_ID");
                    cita.setCliente(cliente.obtenerPorId(clienteId));
                    servicioId= resultSet.getInt("SERVICIO_ID");
                    cita.setServicio(servicio.obtenerPorId(servicioId));
                    cita.setFecha(resultSet.getDate("FECHA"));
                    cita.setHoraIni(resultSet.getTime("HORA_INICIO"));
                    cita.setHoraFin(resultSet.getTime("HORA_FIN"));

                    citas.add(cita);
                }
            }
        } catch (SQLException ex) {
            throw new RuntimeException(ex);
        }
            return citas;
        }
    }
}
*/
