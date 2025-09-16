package pe.edu.pucp.softinv.model;

import java.util.ArrayList;
import java.util.Date;

public class PedidoDTO {
    private ArrayList<Date> fechas;
    private Double total;
    private EstadoPedido estadoPedido;
    private ArrayList<DetallePedidoDTO> detallesPedido;

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
        fechas = null;
        total = null;
        estadoPedido = null;
        detallesPedido = null;
    }

    public PedidoDTO(ArrayList<Date> fechas, Double total, EstadoPedido estadoPedido,ArrayList<DetallePedidoDTO> detallesPedido) {
        this.detallesPedido=detallesPedido;
        this.total=total;
        this.estadoPedido=estadoPedido;
        this.fechas=fechas;
    }
}
