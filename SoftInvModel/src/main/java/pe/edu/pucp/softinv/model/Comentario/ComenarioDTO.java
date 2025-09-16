package pe.edu.pucp.softinv.model.Comentario;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

public class ComenarioDTO {
    private Integer idComentario;
    private ClienteDTO cliente;
    private String comentario;
    private Integer valoracion;

    public Integer getIdComentario() {
        return idComentario;
    }

    public void setIdComentario(Integer idComentario) {
        this.idComentario = idComentario;
    }

    public ClienteDTO getCliente() {
        return cliente;
    }

    public void setCliente(ClienteDTO cliente) {
        this.cliente = cliente;
    }

    public String getComentario() {
        return comentario;
    }

    public void setComentario(String comentario) {
        this.comentario = comentario;
    }

    public Integer getValoracion() {
        return valoracion;
    }

    public void setValoracion(Integer valoracion) {
        this.valoracion = valoracion;
    }

    public ComenarioDTO() {
        idComentario = null;
        cliente = null;
        comentario = null;
        valoracion = null;
    }

    public ComenarioDTO(Integer idComentario,ClienteDTO cliente, String comentario, Integer valoracion) {
        this.idComentario = idComentario;
        this.cliente = cliente;
        this.comentario = comentario;
        this.valoracion = valoracion;
    }
}
