package pe.edu.pucp.softinv.model;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.daoImp.CitaDAOImpl;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.Time;
import java.sql.Date;

public class CitaBO {

    private CitaDAO citaDAO;

    public CitaBO(){
        citaDAO = new CitaDAOImpl();
    }

    public Integer insertar(Integer empleadoId, Integer clienteId, Integer servicioId, Date fecha, Time horaIni,
                            Time horaFin,Double igv,Integer activo,String codTransacc) {
        /*
        private Integer id;
        private Time horaIni;
        private Time horaFin;
        private ClienteDTO cliente;
        private ServicioDTO servicio;
        private EmpleadoDTO empleado;
        private java.sql.Date fecha;
        private Integer activo;
        private Double IGV;
        private String codigoTransaccion;
        */
         ClienteDTO cliente=new ClienteDTO();
         cliente.setIdUsuario(empleadoId);

        EmpleadoDTO empleado=new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);

        ServicioDTO servicio=new ServicioDTO();
        servicio.setIdServicio(servicioId);

        CitaDTO cita=new CitaDTO(1,horaIni,horaFin,cliente,servicio,empleado,fecha,activo,igv,codTransacc);
        return citaDAO.insertar(cita);
    }

    public Integer modificar(CitaDTO cita) {
        return citaDAO.modificar(cita);
    }

    public Integer eliminar(CitaDTO cita) {
        cita.setActivo(0);
        return citaDAO.modificar(cita);
    }

    public CitaDTO obtenerPorId(CitaDTO idCita) {
        return citaDAO.obtenerPorId(idCita);
    }
}
