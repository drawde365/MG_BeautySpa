package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.CalendarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Date;
import java.util.ArrayList;

public class CalendarioBO {
    private CalendarioDAO calendarioDAO;
    private EmpleadoDTO ultimoEmpleadoEncontrado;

    public CalendarioBO(){
        calendarioDAO = new CalendarioDAOImpl();
        ultimoEmpleadoEncontrado = null;
    }

    public Integer insertar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(idEmpleado);
        CalendarioDTO calendario = new CalendarioDTO(empleado, fecha, cantLibre, motivo);
        return calendarioDAO.insertar(calendario);
    }

    public Integer insertar(CalendarioDTO calendario) {
        return calendarioDAO.insertar(calendario);
    }

    public Integer modificar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        EmpleadoDTO empleado = new EmpleadoDTO();
        empleado.setIdUsuario(idEmpleado);
        CalendarioDTO calendarioDTO = new CalendarioDTO(empleado, fecha, cantLibre, motivo);
        return calendarioDAO.modificar(calendarioDTO);
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
}
