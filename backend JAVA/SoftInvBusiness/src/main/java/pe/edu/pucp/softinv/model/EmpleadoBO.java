package pe.edu.pucp.softinv.model;

import pe.edu.pucp.softinv.dao.EmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.EmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public class EmpleadoBO
{
    private EmpleadoDAO empleadoDAO;

    public  EmpleadoBO()
    {
        empleadoDAO = new EmpleadoDAOImpl();
    }

    Integer Insertar(String nombre, String Primerapellido, String Segundoapellido,String correoElectronico, String contrasenha,
                     String celular, String urlFotoPerfil, Boolean admin){
        EmpleadoDTO empleadoDTO = new EmpleadoDTO(nombre,  Primerapellido,  Segundoapellido,  correoElectronico,
                 contrasenha,celular,null, admin, urlFotoPerfil,null);
        return empleadoDAO.insertar(empleadoDTO);
    }

    Integer Modificar(Integer idUsuario, String nombre, String Primerapellido, String Segundoapellido,String correoElectronico,
                      String contrasenha, String celular, String urlFotoPerfil, ArrayList<ServicioDTO> servicios, Boolean admin){
        EmpleadoDTO empleadoDTO = new EmpleadoDTO(nombre,  Primerapellido,  Segundoapellido,  correoElectronico,
                contrasenha,celular,idUsuario, admin, urlFotoPerfil,servicios);
        return empleadoDAO.modificar(empleadoDTO);
    }

    Integer Eliminar(Integer idUsuario){
        return empleadoDAO.eliminar(idUsuario);
    }

    EmpleadoDTO ObtenerPorId(Integer idUsuario){
        return empleadoDAO.obtenerPorId(idUsuario);
    }

    ArrayList<EmpleadoDTO> ListarTodos(){
        return empleadoDAO.listarTodos();
    }

}
