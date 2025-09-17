package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.daoImp.util.Columna;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

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

                if (resultSet.getDate("FECHA")>=inicio and resultSet.getDate("FECHA")<=fin){

                    CitaDTO cita = new Cita();
                    cita.setId(resultset.GEtIn("CITA_ID");
                    cita.setEmpleado(resultset.GEtIn("EMPLEADO_ID");
                    cita.setClienteId(resultset.GEtIn("CLIENTE_ID");
                    cita.setServicioId(resultset.GEtIn("SERVICIO_ID");
                    cita.setFecha(resultset.GEtIn("FECHA");
                    cita.setHoraIni(resultset.GEtIn("HORA_INICIO");
                    cita.setHoraFin(resultset.GEtIn("HORA_FIN");

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