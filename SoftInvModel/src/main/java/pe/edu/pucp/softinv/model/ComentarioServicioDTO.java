package pe.edu.pucp.softinv.model;

public class ComentarioServicioDTO extends ComenarioDTO {
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

    public ComentarioServicioDTO(ClienteDTO cliente,String comentario,Integer valoracion,ServicioDTO servicio) {
        super(cliente,comentario,valoracion);
        this.servicio = servicio;
    }
}
