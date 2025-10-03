package pe.edu.pucp.softinv.model.Cita;

import pe.edu.pucp.softinv.model.Personas.ClienteDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;

import java.sql.Date;
import java.sql.Time;

public class CitaDTO {
    private Integer id;
    private Time horaIni;
    private Time horaFin;
    private ClienteDTO cliente;
    private ServicioDTO servicio;
    private EmpleadoDTO empleado;
    private Date fecha;
    private Integer activo;
    private Double IGV;
    private String codigoTransaccion;

    public Integer getActivo() {
        return activo;
    }

    public void setActivo(Integer activo) {
        this.activo = activo;
    }

    public Double getIgv() {
        return IGV;
    }

    public void setIgv(Double igv) {
        this.IGV = igv;
    }

    public String getCodigoTransaccion() {
        return codigoTransaccion;
    }

    public void setCodigoTransaccion(String codigoTransaccion) {
        this.codigoTransaccion = codigoTransaccion;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Time getHoraIni() {
        return horaIni;
    }

    public void setHoraIni(Time horaIni) {
        this.horaIni = horaIni;
    }

    public Time getHoraFin() {
        return horaFin;
    }

    public void setHoraFin(Time horaFin) {
        this.horaFin = horaFin;
    }

    public ClienteDTO getCliente() {
        return cliente;
    }

    public void setCliente(ClienteDTO cliente) {
        this.cliente = cliente;
    }

    public ServicioDTO getServicio() {
        return servicio;
    }

    public void setServicio(ServicioDTO servicio) {
        this.servicio = servicio;
    }

    public EmpleadoDTO getEmpleado() {
        return empleado;
    }

    public void setEmpleado(EmpleadoDTO empleado) {
        this.empleado = empleado;
    }

    public Date getFecha() {
        return fecha;
    }

    public void setFecha(Date fecha) {
        this.fecha = fecha;
    }

    public CitaDTO() {
        id=null;
        horaIni=null;
        horaFin=null;
        cliente=null;
        servicio=null;
        empleado=null;
        fecha=null;
        activo=null;
        IGV=null;
        codigoTransaccion=null;
    }

    public CitaDTO(Integer id,Time horaIni,Time horaFin,ClienteDTO cliente,ServicioDTO servicio,EmpleadoDTO empleado,
                   Date fecha,Integer activo,Double igv,String codigoTransaccion) {
        this.id = id;
        this.horaIni = horaIni;
        this.horaFin = horaFin;
        this.cliente = cliente;
        this.servicio = servicio;
        this.empleado = empleado;
        this.fecha = fecha;
        this.activo=activo;
        this.IGV=igv;
        this.codigoTransaccion=codigoTransaccion;
    }
}
