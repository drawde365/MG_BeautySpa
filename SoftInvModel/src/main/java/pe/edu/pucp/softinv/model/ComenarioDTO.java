package pe.edu.pucp.softinv.model;

public class ComenarioDTO {
    private ClienteDTO cliente;
    private String comentario;
    private Integer valoracion;

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
        cliente = null;
        comentario = null;
        valoracion = null;
    }

    public ComenarioDTO(ClienteDTO cliente, String comentario, Integer valoracion) {
        this.cliente = cliente;
        this.comentario = comentario;
        this.valoracion = valoracion;
    }
}
