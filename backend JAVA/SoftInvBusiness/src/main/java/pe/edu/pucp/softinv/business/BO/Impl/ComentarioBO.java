package pe.edu.pucp.softinv.business.BO.Impl;

import pe.edu.pucp.softinv.dao.ComentarioDAO;
import pe.edu.pucp.softinv.daoImp.ComentarioDAOImpl;
import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;
import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.util.ArrayList;

public class ComentarioBO {

    ComentarioDAO comentarioDAO;

    public ComentarioBO(){
        comentarioDAO=new ComentarioDAOImpl();
    }

    public Integer insertarComentarioDeProducto(Integer clienteId,String comentario,Integer valoracion,
                                                Integer productoId){

        ComentarioDTO comentarioDTO=new ComentarioDTO();

        ClienteDTO cliente=new ClienteDTO();
        cliente.setIdUsuario(clienteId);

        ProductoDTO producto=new ProductoDTO();
        producto.setIdProducto(productoId);

        comentarioDTO.setCliente(cliente);
        comentarioDTO.setComentario(comentario);
        comentarioDTO.setValoracion(valoracion);
        comentarioDTO.setProducto(producto);

        return comentarioDAO.insertar(comentarioDTO);
    }

    public Integer insertarComentarioDeServicio(Integer clienteId,String comentario,Integer valoracion,
                                                Integer servicioId){

        ComentarioDTO comentarioDTO=new ComentarioDTO();

        ClienteDTO cliente=new ClienteDTO();
        cliente.setIdUsuario(clienteId);

        ServicioDTO servicio=new ServicioDTO();
        servicio.setIdServicio(servicioId);

        comentarioDTO.setCliente(cliente);
        comentarioDTO.setComentario(comentario);
        comentarioDTO.setValoracion(valoracion);
        comentarioDTO.setServicio(servicio);

        return comentarioDAO.insertar(comentarioDTO);
    }

    public Integer modificar(ComentarioDTO comentario) {
        return comentarioDAO.modificar(comentario);
    }

    public Integer eliminar(ComentarioDTO comentario) {
        return comentarioDAO.eliminar(comentario);
    }

    public ArrayList<ComentarioDTO> obtenerComentariosPorProducto(Integer idProducto){
        return comentarioDAO.obtenerComentariosPorProducto(idProducto);
    }

    public ArrayList<ComentarioDTO> obtenerComentariosPorServicio(Integer idServicio){
        return comentarioDAO.obtenerComentariosPorServicio(idServicio);
    }
}
