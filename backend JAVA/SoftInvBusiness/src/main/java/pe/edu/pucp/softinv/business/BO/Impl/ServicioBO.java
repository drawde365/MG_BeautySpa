package pe.edu.pucp.softinv.business.BO.Impl;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.dao.ServicioEmpleadoDAO;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.daoImp.ServicioEmpleadoDAOImpl;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

import java.util.ArrayList;

public class ServicioBO {

    private ServicioDAO servicioDAO;
    private ServicioEmpleadoDAO SXEDAO;
    public ServicioBO() {
        servicioDAO = new ServicioDAOImpl();
        SXEDAO = new ServicioEmpleadoDAOImpl();
    }

    public Integer insertar(String nombre, String descripcion, TipoServicio tipo, Double precio, String urlImagen,
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
     public Integer insertar(ServicioDTO servicio){
        return servicioDAO.insertar(servicio);
    }

/*
    public Integer eliminar(Integer idServicio){
        ServicioDTO servicioDTO = new ServicioDTO();
        servicioDTO = ObtenerPorId()
        servicioDTO.setIdServicio(idServicio);
        servicioDTO.setActivo(0);
        return servicioDAO.modificar(servicioDTO);
        //return  servicioDAO.eliminar(servicioDTO);
    }

    public Integer modificar(Integer idServicio, String nombre, String descripcion, TipoServicio tipo, Double precio, String urlImagen,
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
*/

    //desde el front se llama a obtener por id, se modifica el campo que se quiere y se llama a este metodo
    public Integer modificar(ServicioDTO servicio) {
        return servicioDAO.modificar(servicio);
    }

    //cambia el estado a inactivo, primero desde el front llamo a obtener por id
    public Integer eliminar(ServicioDTO servicio) {
        servicio.setActivo(0);
        return servicioDAO.modificar(servicio);
    }

    public ServicioDTO ObtenerPorId(Integer idServicio){
        return servicioDAO.obtenerPorId(idServicio);
    }

    public ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(Integer servicioId){
        return SXEDAO.listarEmpleadosDeServicio(servicioId);
    }
    
    public ArrayList<ServicioDTO> listarTodos (){
        return servicioDAO.listarTodos();
    }
    
    public ArrayList<ServicioDTO> listarFiltro (String filtro) {
        return servicioDAO.listarFiltro(filtro);
    }

}
