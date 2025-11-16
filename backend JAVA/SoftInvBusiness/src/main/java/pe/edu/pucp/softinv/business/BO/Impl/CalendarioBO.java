package pe.edu.pucp.softinv.business.BO.Impl;

import jakarta.xml.bind.annotation.XmlSchemaType;
import jakarta.xml.bind.annotation.adapters.XmlJavaTypeAdapter;
import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.CalendarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.CitaDAOImpl;
import pe.edu.pucp.softinv.daoImp.HorarioTrabajoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Calendar;
import java.time.LocalTime;
import pe.edu.pucp.softinv.model.util.LocalTimeAdapter;

public class CalendarioBO {
    private CalendarioDAO calendarioDAO;
    private HorarioTrabajoDAO horarioTrabajoDAO;
    private CitaDAO citaDAO;
    
    public CalendarioBO(){
        calendarioDAO = new CalendarioDAOImpl();
        horarioTrabajoDAO = new HorarioTrabajoDAOImpl();
        citaDAO = new CitaDAOImpl();
    }
    @XmlJavaTypeAdapter(LocalTimeAdapter.class)
    public List<LocalTime> calcularBloquesDisponibles(Integer empleadoId, Date fecha, int duracionServicioMinutos) {
        int diaSemana = obtenerDiaSemana(fecha);

        ArrayList<HorarioTrabajoDTO> turnosBase = horarioTrabajoDAO.obtenerPorEmpleadoYFecha(empleadoId, diaSemana);
        ArrayList<CitaDTO> citasReservadas = citaDAO.listarCitasPorEmpleadoYFecha(empleadoId, fecha);
        List<LocalTime> bloquesDisponibles = new ArrayList<>();
        
        CalendarioDTO calendario = calendarioDAO.obtenerPorId(empleadoId, fecha);
        if (calendario == null || calendario.getCantLibre() <= 0) {
            return bloquesDisponibles;
        }

        for (HorarioTrabajoDTO turno : turnosBase) {
            
            LocalTime inicioTurno = turno.getHoraInicio().toLocalTime();
            LocalTime finTurno = turno.getHoraFin().toLocalTime();
            LocalTime horaActual = inicioTurno;
            
            while (horaActual.plusMinutes(duracionServicioMinutos).isBefore(finTurno) || 
                   horaActual.plusMinutes(duracionServicioMinutos).equals(finTurno))
            {
                LocalTime bloqueFin = horaActual.plusMinutes(duracionServicioMinutos);
                boolean estaOcupado = false;
                
                for (CitaDTO cita : citasReservadas) {
                    LocalTime citaInicio = cita.getHoraIni().toLocalTime(); 
                    LocalTime citaFin = cita.getHoraFin().toLocalTime();

                    if (bloqueFin.isAfter(citaInicio) && horaActual.isBefore(citaFin)) {
                        estaOcupado = true;
                        break;
                    }
                }
                
                if (!estaOcupado) {
                    bloquesDisponibles.add(horaActual);
                }
                
                horaActual = horaActual.plusMinutes(duracionServicioMinutos);
            }
        }
        
        return bloquesDisponibles;
    }
    
    public Integer reservarBloqueYCita(CitaDTO cita) {
        
        CalendarioDTO calendarioActual = calendarioDAO.obtenerPorId(cita.getEmpleado().getIdUsuario(), cita.getFecha());
        if (calendarioActual == null || calendarioActual.getCantLibre() <= 0) {
            return -1;
        }
        
        calendarioActual.setCantLibre(calendarioActual.getCantLibre() - 1);
        calendarioDAO.modificar(calendarioActual);
        
        Integer citaId = citaDAO.insertar(cita);
        
        if (citaId <= 0) {
            throw new RuntimeException("Fallo al insertar cita.");
        }
        
        return citaId;
    }
    
    private int obtenerDiaSemana(Date fecha) {
        Calendar cal = Calendar.getInstance();
        cal.setTime(fecha);
        int day = cal.get(Calendar.DAY_OF_WEEK);
        if (day == 1) return 7; 
        return day - 1;         
    }

    public Integer insertar(CalendarioDTO calendario) {
        return calendarioDAO.insertar(calendario);
    }
    public Integer modificar(CalendarioDTO calendario) {
        return calendarioDAO.modificar(calendario);
    }
    public Integer eliminar(CalendarioDTO calendario){
        calendario.setCantLibre(-2);
        return calendarioDAO.modificar(calendario);
    }

    public ArrayList<CalendarioDTO> listarCalendarioDeEmpleado(Integer empleadoId) {
        return calendarioDAO.listarCalendarioDeEmpleado(empleadoId);
    }
    /*
    public Integer insertar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(idEmpleado);
        CalendarioDTO calendario = new CalendarioDTO(empleado, fecha, cantLibre, motivo);
        return calendarioDAO.insertar(calendario);
    }

    public Integer modificar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(idEmpleado);
        CalendarioDTO calendarioDTO = new CalendarioDTO(empleado, fecha, cantLibre, motivo);
        return calendarioDAO.modificar(calendarioDTO);
    }
    */
}