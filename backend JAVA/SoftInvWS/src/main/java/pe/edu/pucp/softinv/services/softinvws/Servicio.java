package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.ServicioBO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

/**
 *
 * @author softinv
 */
@WebService(serviceName = "Servicio")
public class Servicio {
    
    private ServicioBO servicioBO;
    
    public Servicio() {
        servicioBO = new ServicioBO();
    }
    
    @WebMethod(operationName = "InsertarServicioPorPartes")
    public Integer insertarServicioPorPartes(
            @WebParam(name = "nombre") String nombre,
            @WebParam(name = "descripcion") String descripcion,
            @WebParam(name = "tipo") TipoServicio tipo,
            @WebParam(name = "precio") Double precio,
            @WebParam(name = "urlImagen") String urlImagen,
            @WebParam(name = "duracionHora") Integer duracionHora) {
        
        return servicioBO.insertar(nombre, descripcion, tipo, precio, urlImagen, duracionHora);
    }
    
    @WebMethod(operationName = "ModificarServicio")
    public Integer modificarServicio(
            @WebParam(name = "servicio") ServicioDTO servicio) {
        return servicioBO.modificar(servicio);
    }
    
    @WebMethod(operationName = "EliminarServicio")
    public Integer eliminarServicio(
            @WebParam(name = "servicio") ServicioDTO servicio) {
        return servicioBO.eliminar(servicio);
    }
    
    @WebMethod(operationName = "ObtenerServicioPorId")
    public ServicioDTO ObtenerServicioPorId(
            @WebParam(name = "idServicio") Integer idServicio) {
        return servicioBO.ObtenerPorId(idServicio);
    }
    
    @WebMethod(operationName = "ListarEmpleadosDeServicio")
    public ArrayList<EmpleadoDTO> listarEmpleadosDeServicio(
            @WebParam(name = "servicioId") Integer servicioId) {
        return servicioBO.listarEmpleadosDeServicio(servicioId);
    }
}