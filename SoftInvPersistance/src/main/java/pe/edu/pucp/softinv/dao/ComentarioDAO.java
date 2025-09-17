package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.softinv.model.Comentario.ComentarioDTO;

public interface ComentarioDAO {

    public Integer insertar(ComentarioDTO comentario);

    public ComentarioDTO obtenerPorId(Integer idComentario);

    public Integer modificar(ComentarioDTO comentario);

    public Integer eliminar(ComentarioDTO comentario);

}
