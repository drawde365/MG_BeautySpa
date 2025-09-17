package pe.edu.pucp.softinv.model.Comentario;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

public class ComentarioServicioDTO extends ComentarioDTO {
    private ServicioDTO servicio;

    public ServicioDTO getServicio() {
        return servicio;
    }

    public void setServicio(ServicioDTO servicio) {
        this.servicio = servicio;
    }

    public ComentarioServicioDTO() {
        super();
        servicio = null;
    }

    public ComentarioServicioDTO(Integer idComentario, ClienteDTO cliente, String comentario, Integer valoracion, ServicioDTO servicio) {
        super(idComentario,cliente,comentario,valoracion);
        this.servicio = servicio;
    }

    @Override
    public Integer getIdProducto(){
        return null;
    }

    @Override
    public Integer getIdServicio(){
        return servicio.getIdServicio();
    }

}
