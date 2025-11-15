package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.HorarioTrabajoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.HorarioTrabajoDAOImpl;
import pe.edu.pucp.softinv.model.Disponibilidad.HorarioTrabajoDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;
import java.time.Duration;
import java.util.ArrayList;

public class HorarioTrabajoBO
{
    private HorarioTrabajoDAO horarioTrabajoDAO;
    private EmpleadoDAO empleadoDAO;
    private EmpleadoDTO ultimoEmpleadoEncontrado;

    public HorarioTrabajoBO(){

        horarioTrabajoDAO = new HorarioTrabajoDAOImpl();
        empleadoDAO=new  EmpleadoDAOImpl();
    }

    public Integer insertar(HorarioTrabajoDTO horarioTrabajo) {
        return horarioTrabajoDAO.insertar(horarioTrabajo);
    }

    public HorarioTrabajoDTO obtenerPorId(Integer horarioId) {
        return horarioTrabajoDAO.obtenerPorId(horarioId);
    }

    public ArrayList<HorarioTrabajoDTO> obtenerPorEmpleadoYFecha(Integer empleadoId, Integer diaSemana) {
        return horarioTrabajoDAO.obtenerPorEmpleadoYFecha(empleadoId, diaSemana);
    }

    public Integer modificar(HorarioTrabajoDTO horarioTrabajo){
        return horarioTrabajoDAO.modificar(horarioTrabajo);
    }

    public Integer eliminar(HorarioTrabajoDTO horarioTrabajo){
        return horarioTrabajoDAO.eliminar(horarioTrabajo);
    }
    
    public ArrayList<HorarioTrabajoDTO> listarPorEmpleado(Integer empleadoId){
        return horarioTrabajoDAO.listarPorEmpleado(empleadoId);
    }
}