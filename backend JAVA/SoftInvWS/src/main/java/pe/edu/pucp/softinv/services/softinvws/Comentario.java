package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.ArrayList;
import pe.edu.pucp.softinv.business.BO.Impl.ComentarioBO;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

/**
 *
 * @author softinv
 */
@WebService(serviceName = "Comentario")
public class Comentario {
    
    private ComentarioBO comentarioBO;
    
    public Comentario() {
        comentarioBO = new ComentarioBO();
    }
    
    @WebMethod(operationName = "InsertarComentarioDeProducto")
    public Integer insertarComentarioDeProducto(
            @WebParam(name = "clienteId") Integer clienteId,
            @WebParam(name = "comentario") String comentario,
            @WebParam(name = "valoracion") Integer valoracion,
            @WebParam(name = "productoId") Integer productoId) {
        
        return comentarioBO.insertarComentarioDeProducto(clienteId, comentario, valoracion, productoId);
    }
    
    @WebMethod(operationName = "InsertarComentarioDeServicio")
    public Integer insertarComentarioDeServicio(
            @WebParam(name = "clienteId") Integer clienteId,
            @WebParam(name = "comentario") String comentario,
            @WebParam(name = "valoracion") Integer valoracion,
            @WebParam(name = "servicioId") Integer servicioId) {
        
        return comentarioBO.insertarComentarioDeServicio(clienteId, comentario, valoracion, servicioId);
    }
    
    @WebMethod(operationName = "ModificarComentario")
    public Integer modificarComentario(
            @WebParam(name = "comentario") ComentarioDTO comentario) {
        return comentarioBO.modificar(comentario);
    }
    
    @WebMethod(operationName = "EliminarComentario")
    public Integer eliminarComentario(
            @WebParam(name = "comentario") ComentarioDTO comentario) {
        return comentarioBO.eliminar(comentario);
    }
    
    @WebMethod(operationName = "ObtenerComentariosPorProducto")
    public ArrayList<ComentarioDTO> obtenerComentariosPorProducto(
            @WebParam(name = "idProducto") Integer idProducto) {
        return comentarioBO.obtenerComentariosPorProducto(idProducto);
    }
    
    @WebMethod(operationName = "ObtenerComentariosPorServicio")
    public ArrayList<ComentarioDTO> obtenerComentariosPorServicio(
            @WebParam(name = "idServicio") Integer idServicio) {
        return comentarioBO.obtenerComentariosPorServicio(idServicio);
    }
}