package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

import java.util.ArrayList;

public interface ComentarioDAO {

    public Integer insertar(ComentarioDTO comentario);

    public ComentarioDTO obtenerPorId(Integer idComentario); //ESTO ES INÚTIL fuera del dao

    ArrayList<ComentarioDTO> obtenerComentariosPorProducto(Integer contadorPagina, Integer idProducto);

    ArrayList<ComentarioDTO> obtenerComentariosPorServicio(Integer contadorPagina, Integer idServicio);

    public Integer modificar(ComentarioDTO comentario);

    public Integer eliminar(ComentarioDTO comentario);

}
