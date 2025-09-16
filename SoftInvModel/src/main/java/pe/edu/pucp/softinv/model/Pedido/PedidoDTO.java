package pe.edu.pucp.softinv.model.Pedido;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;

import java.util.ArrayList;
import java.util.Date;

public class PedidoDTO {
    private Integer idPedido;
    private ArrayList<Date> fechas;
    private Double total;
    private Double IGV;
    private EstadoPedido estadoPedido;
    private ArrayList<DetallePedidoDTO> detallesPedido;
    private ClienteDTO cliente;

    public Double getIGV() {
        return IGV;
    }

    public void setIGV(Double IGV) {
        this.IGV = IGV;
    }

    public Integer getIdPedido() {
        return idPedido;
    }

    public void setIdPedido(Integer idPedido) {
        this.idPedido = idPedido;
    }

    public ClienteDTO getCliente() {
        return cliente;
    }

    public void setCliente(ClienteDTO cliente) {
        this.cliente = cliente;
    }

    public ArrayList<Date> getFechas() {
        return fechas;
    }

    public void setFechas(ArrayList<Date> fechas) {
        this.fechas = fechas;
    }

    public Double getTotal() {
        return total;
    }

    public void setTotal(Double total) {
        this.total = total;
    }

    public EstadoPedido getEstadoPedido() {
        return estadoPedido;
    }

    public void setEstadoPedido(EstadoPedido estadoPedido) {
        this.estadoPedido = estadoPedido;
    }

    public ArrayList<DetallePedidoDTO> getDetallesPedido() {
        return detallesPedido;
    }

    public void setDetallesPedido(ArrayList<DetallePedidoDTO> detallesPedido) {
        this.detallesPedido = detallesPedido;
    }

    public PedidoDTO(){
        idPedido = null;
        cliente = null;
        fechas = null;
        total = null;
        IGV = null;
        estadoPedido = null;
        detallesPedido = null;
    }

    public PedidoDTO(Integer idPedido,ArrayList<Date> fechas, Double total,Double IGV, EstadoPedido estadoPedido,
                     ArrayList<DetallePedidoDTO> detallesPedido, ClienteDTO cliente) {
        this.idPedido=idPedido;
        this.detallesPedido=detallesPedido;
        this.total=total;
        this.IGV=IGV;
        this.estadoPedido=estadoPedido;
        this.fechas=fechas;
        this.cliente=cliente;
    }
}
