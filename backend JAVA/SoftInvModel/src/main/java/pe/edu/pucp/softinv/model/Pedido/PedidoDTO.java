package pe.edu.pucp.softinv.model.Pedido;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import java.util.ArrayList;
import java.util.Date;

public class PedidoDTO {
    private Integer idPedido;
    private Date fechaPago;
    private Date fechaRecojo;
    private Date fechaListaParaRecojo;
    private Double total;
    private Double IGV;
    private EstadoPedido estadoPedido;
    private ArrayList<DetallePedidoDTO> detallesPedido;
    private ClienteDTO cliente;
    private String codigoTransaccion;

    public String getCodigoTransaccion() {
        return codigoTransaccion;
    }

    public void setCodigoTransaccion(String codigoTransaccion) {
        this.codigoTransaccion = codigoTransaccion;
    }

    public Integer getIDCliente(){
        return this.cliente.getIdUsuario();
    }

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

    public Double getTotal() {
        return total;
    }

    public void setTotal(Double total) {
        this.total = total;
    }

    public EstadoPedido getEstadoPedido() {
        return estadoPedido;
    }

    public String getEstadoPedidoS() {
        return estadoPedido.name();
    }

    public void setEstadoPedido(EstadoPedido estadoPedido) {
        this.estadoPedido = estadoPedido;
    }

    public ArrayList<DetallePedidoDTO> getDetallesPedido() {
        return detallesPedido;
    }

    public void agregarDetallesPedido(DetallePedidoDTO detallePedido) {
        this.detallesPedido.add(detallePedido);
    }

    public PedidoDTO(){
        idPedido = null;
        cliente = null;
        fechaPago = null;
        fechaRecojo=null;
        fechaListaParaRecojo=null;
        total = null;
        IGV = null;
        estadoPedido = null;
        detallesPedido = null;
        codigoTransaccion = null;
    }

    public PedidoDTO(Integer idPedido,Date fechaPago, Date fechaRecojo, Date fechaListo, Double total,Double IGV, EstadoPedido estadoPedido,
                     ArrayList<DetallePedidoDTO> detallesPedido, ClienteDTO cliente, String codigoTransaccion) {
        this.idPedido=idPedido;
        this.detallesPedido=detallesPedido;
        this.total=total;
        this.IGV=IGV;
        this.estadoPedido=estadoPedido;
        this.fechaPago=fechaPago;
        this.fechaRecojo=fechaRecojo;
        this.fechaListaParaRecojo=fechaListo;
        this.cliente=cliente;
        this.codigoTransaccion=codigoTransaccion;
    }

    public Date getFechaPago(){
        return this.fechaPago;
    }

    public void setFechaPago(Date fechaPago){
        this.fechaPago = fechaPago;
    }

    public Date getFechaRecojo(){
        return this.fechaRecojo;
    }

    public void setFechaRecojo(Date fechaRecojo){
        this.fechaRecojo=fechaRecojo;
    }

    public Date getFechaListaParaRecojo(){
        return this.fechaListaParaRecojo;
    }

    public void setFechaListaParaRecojo(Date fechaListaParaRecojo){
        this.fechaListaParaRecojo=fechaListaParaRecojo;
    }

}
