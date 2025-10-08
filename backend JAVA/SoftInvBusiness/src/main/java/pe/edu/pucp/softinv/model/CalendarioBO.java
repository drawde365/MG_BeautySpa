package pe.edu.pucp.softinv.model;

import pe.edu.pucp.softinv.dao.CalendarioDAO;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.CalendarioDAOImpl;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.CalendarioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Date;

public class CalendarioBO {
    private CalendarioDAO calendarioDAO;
    private Integer ultimoEmpleadoEncontrado;

    public CalendarioBO(){
        calendarioDAO = new CalendarioDAOImpl();
        ultimoEmpleadoEncontrado = null;
    }

    Integer Insertar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        //MEJORAR

        EmpleadoDAO empleadoDAO = new EmpleadoDAOImpl();
        EmpleadoDTO empleadoDTO = null;
        if (ultimoEmpleadoEncontrado != null){
            if(ultimoEmpleadoEncontrado != idEmpleado) {
                empleadoDTO = empleadoDAO.obtenerPorId(idEmpleado);
                if (empleadoDTO !=null)
                ultimoEmpleadoEncontrado = idEmpleado;
                else return null;
            }
        }else{
            empleadoDTO = empleadoDAO.obtenerPorId(idEmpleado);
            if(empleadoDTO != null)
                ultimoEmpleadoEncontrado = idEmpleado;
            else return null;
        }
        CalendarioDTO calendarioDTO = new CalendarioDTO(empleadoDTO,fecha,cantLibre,motivo);

    }

    Integer Modificar(Integer idEmpleado, Date fecha, Integer cantLibre, String motivo){
        CalendarioDTO calendarioDTO = new CalendarioDTO();
        EmpleadoDTO empleadoDTO = new EmpleadoDTO();
        empleadoDTO.setIdUsuario(idEmpleado);
        calendarioDTO.setEmpleado(empleadoDTO);
        calendarioDTO.setFecha(fecha);
        calendarioDTO.setCantLibre(cantLibre);
        calendarioDTO.setMotivo(motivo);
        return calendarioDAO.modificar(calendarioDTO);
    }

    Integer Eliminar(Integer empleadoId, Date fecha){
        CalendarioDTO calendarioDTO = new CalendarioDTO();
        EmpleadoDTO empleadoDTO = new EmpleadoDTO();
        empleadoDTO.setIdUsuario(empleadoId);
        calendarioDTO.setEmpleado(empleadoDTO);
        calendarioDTO.setFecha(fecha);
        return calendarioDAO.eliminar(calendarioDTO);
    }

}
