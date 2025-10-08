package pe.edu.pucp.softinv.model.Comentario;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Producto.ProductoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

public class ComentarioDTO {
    private Integer idComentario;
    private ClienteDTO cliente;
    private String comentario;
    private Integer valoracion;
    private ServicioDTO servicio;
    private ProductoDTO producto;

    public ServicioDTO getServicio() {
        return servicio;
    }

    public void setServicio(ServicioDTO servicio) {
        this.servicio = servicio;
    }

    public ProductoDTO getProducto() {
        return producto;
    }

    public void setProducto(ProductoDTO producto) {
        this.producto = producto;
    }

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

    public ComentarioDTO() {
        idComentario = null;
        cliente = null;
        comentario = null;
        valoracion = null;
        producto = new ProductoDTO();
        servicio = new ServicioDTO();
    }

    public ComentarioDTO(Integer idComentario,ClienteDTO cliente, String comentario, Integer valoracion, ProductoDTO producto, ServicioDTO servicio) {
        this.idComentario = idComentario;
        this.cliente = cliente;
        this.comentario = comentario;
        this.valoracion = valoracion;
        this.producto = producto;
        this.servicio = servicio;
    }
}
