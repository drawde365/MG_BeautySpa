package pe.edu.pucp.softinv.model;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

import java.util.ArrayList;

public class ServicioBO {

    private ServicioDAO servicioDAO;

    public ServicioBO() {
        servicioDAO = new ServicioDAOImpl();
    }

    public Integer Insertar(String nombre, String descripcion, TipoServicio tipo, Double precio, String urlImagen,
                        Integer duracionHora){
        ServicioDTO servicioDTO = new ServicioDTO();
        servicioDTO.setNombre(nombre);
        servicioDTO.setDescripcion(descripcion);
        servicioDTO.setTipo(tipo);
        servicioDTO.setPrecio(precio);
        servicioDTO.setUrlImagen(urlImagen);
        servicioDTO.setDuracionHora(duracionHora);
        servicioDTO.setActivo(1);
        return servicioDAO.insertar(servicioDTO);
    }

    public Integer Eliminar(Integer idServicio){
        ServicioDTO servicioDTO = new ServicioDTO();
        servicioDTO.setIdServicio(idServicio);
        return  servicioDAO.eliminar(servicioDTO);
    }

    public Integer Modificar(Integer idServicio, String nombre, String descripcion, TipoServicio tipo, Double precio, String urlImagen,
                             Integer duracionHora, Integer activo){
        ServicioDTO servicioDTO = new ServicioDTO();
        servicioDTO.setIdServicio(idServicio);
        servicioDTO.setNombre(nombre);
        servicioDTO.setDescripcion(descripcion);
        servicioDTO.setTipo(tipo);
        servicioDTO.setPrecio(precio);
        servicioDTO.setUrlImagen(urlImagen);
        servicioDTO.setDuracionHora(duracionHora);
        servicioDTO.setActivo(activo);
        return servicioDAO.modificar(servicioDTO);
    }

    public ServicioDTO ObtenerPorId(Integer idServicio){
        return servicioDAO.obtenerPorId(idServicio);
    }

    public ArrayList<EmpleadoDTO> ListarEmpleadosPorServicio(Integer idServicio){
        return servicioDAO.listarEmpleadosDeServicio(idServicio);
    }

}
