package pe.edu.pucp.softinv.model.Disponibilidad;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.Date;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;

@XmlAccessorType(XmlAccessType.FIELD)
public class CalendarioDTO {
    private EmpleadoDTO empleado;
    private Date fecha;
    private Integer cantLibre;
    private String motivo;

    public CalendarioDTO() {
        this.empleado = null;
        this.fecha = null;
        this.cantLibre = null;
        this.motivo = null;
    }

    public CalendarioDTO(EmpleadoDTO empleado, Date fecha, Integer cantLibre, String motivo) {
        this.empleado = empleado;
        this.fecha = fecha;
        this.cantLibre = cantLibre;
        this.motivo = motivo;
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

    public Integer getCantLibre() {
        return cantLibre;
    }

    public void setCantLibre(Integer cantLibre) {
        this.cantLibre = cantLibre;
    }

    public String getMotivo() {
        return motivo;
    }

    public void setMotivo(String motivo) {
        this.motivo = motivo;
    }
}