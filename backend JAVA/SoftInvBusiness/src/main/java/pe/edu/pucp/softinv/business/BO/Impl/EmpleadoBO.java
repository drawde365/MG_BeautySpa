package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.CitaDAO;
import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.dao.ServicioEmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.CitaDAOImpl;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.daoImp.ServicioEmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Cita.CitaDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Personas.UsuarioDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public class EmpleadoBO
{
    private EmpleadoDAO empleadoDAO;
    private ServicioEmpleadoDAO SXEDAO;
    private CitaDAO citaDAO;
    public  EmpleadoBO()
    {
        empleadoDAO = new EmpleadoDAOImpl();
        SXEDAO = new ServicioEmpleadoDAOImpl();
        citaDAO= new CitaDAOImpl();
    }

    public Integer insertar(String nombre, String Primerapellido, String Segundoapellido,String correoElectronico,
                     String contrasenha,String celular, String urlFotoPerfil, Boolean admin,Integer rol){
        EmpleadoDTO empleadoDTO = new EmpleadoDTO(nombre,  Primerapellido,  Segundoapellido,  correoElectronico,
                 contrasenha,celular,null, admin, urlFotoPerfil,null,rol);
        return empleadoDAO.insertar(empleadoDTO);
    }

    public Integer insertar(EmpleadoDTO empleado) {
        return empleadoDAO.insertar(empleado);
    }
/*
    Integer modificar(Integer idUsuario, String nombre, String Primerapellido, String Segundoapellido,String correoElectronico,
                      String contrasenha, String celular, String urlFotoPerfil, ArrayList<ServicioDTO> servicios,
                      Boolean admin){
        EmpleadoDTO empleadoDTO = new EmpleadoDTO(nombre,  Primerapellido,  Segundoapellido,  correoElectronico,
                contrasenha,celular,idUsuario, admin, urlFotoPerfil,servicios);
        return empleadoDAO.modificar(empleadoDTO);
    }
*/
    public Integer modificar(EmpleadoDTO empleado){
        return empleadoDAO.modificar(empleado);
    }

    public Integer eliminar(EmpleadoDTO empleado){
        return empleadoDAO.modificar(empleado);
    }

    public EmpleadoDTO ObtenerPorId(Integer idUsuario){
        return empleadoDAO.obtenerPorId(idUsuario);
    }

    public ArrayList<EmpleadoDTO> ListarTodos(){
        return empleadoDAO.listarTodos();
    }

    public void agregarServicio(Integer empleadoId, Integer servicioId) {
        SXEDAO.insertar(empleadoId,servicioId);
    }

    public void eliminarServicio(Integer empleadoId, Integer servicioId){
        SXEDAO.eliminarLogico(empleadoId, servicioId);
    }
    
    public ArrayList<ServicioDTO> listarServicios(Integer empleadoId) {
        return SXEDAO.listarServiciosDeEmpleado(empleadoId);
    }

    public ArrayList<CitaDTO> listarCitas(Integer empleadoId){

        UsuarioDTO empleado=new EmpleadoDTO();
        empleado.setIdUsuario(empleadoId);

        return citaDAO.listarCitasPorUsuario(empleado);
    }
    
    public ArrayList<ServicioDTO> listarServiciosNoBrindados(Integer empleadoId){
        return SXEDAO.listarServiciosNoBrindadosEmpleado(empleadoId);
    }
}
