package pe.edu.pucp.softinv.model;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

public class ServicioBO {

    private ServicioDAO servicioDAO;

    public ServicioBO() {
        servicioDAO = new ServicioDAOImpl();
    }

    public int Insertar(String nombre, String descripcion, TipoServicio tipo, Double precio, String urlImagen,
                        Integer duracionHora){
        ServicioDTO servicioDTO = new ServicioDTO();
        servicioDTO.setNombre(nombre);
        servicioDTO.setDescripcion(descripcion);
    }

}
