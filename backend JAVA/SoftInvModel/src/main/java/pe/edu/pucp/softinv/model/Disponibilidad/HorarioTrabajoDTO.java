package pe.edu.pucp.softinv.model.Disponibilidad;

import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;
import pe.edu.pucp.softinv.model.util.TimeAdapter; // Asumo que creaste esto
import java.sql.Time;

// --- AÑADE ESTAS IMPORTACIONES ---
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

// --- AÑADE ESTA ANOTACIÓN A LA CLASE ---
@XmlAccessorType(XmlAccessType.FIELD)
public class HorarioTrabajoDTO {
    
    @XmlJavaTypeAdapter(TimeAdapter.class) // Esta anotación ahora es segura
    private Time horaInicio;
    
    @XmlJavaTypeAdapter(TimeAdapter.class) // Esta anotación ahora es segura
    private Time horaFin;
    private Integer id;
    private EmpleadoDTO empleado;
    private Integer diaSemana;
    private Integer numIntervalo;
  
    public Integer getNumIntervalo() {
        return numIntervalo;
    }

    public void setNumIntervalo(Integer numIntervalo) {
        this.numIntervalo = numIntervalo;
    }
    
    public Integer getDiaSemana() {
        return diaSemana;
    }

    public void setDiaSemana(Integer diaSemana) {
        this.diaSemana = diaSemana;
    }

    public EmpleadoDTO getEmpleado() {
        return empleado;
    }

    public void setEmpleado(EmpleadoDTO empleado) {
        this.empleado = empleado;
    }

    public Integer getId() {return id; }

    public void setId(Integer id) {
        this.id = id;
    }

    public HorarioTrabajoDTO () {
        this.id=null;
        this.empleado=null;
        this.diaSemana=null;
        this.horaInicio=null;
        this.horaFin=null;
    }

    public HorarioTrabajoDTO(Integer id, EmpleadoDTO empleado, Integer diaSemana, Time horaInicio, Time horaFin) {
        this.id=id;
        this.empleado=empleado;
        this.diaSemana=diaSemana;
        this.horaInicio=horaInicio;
        this.horaFin=horaFin;
    }

    public Time getHoraInicio() {
        return horaInicio;
    }

    public void setHoraInicio(Time horaInicio) {
        this.horaInicio = horaInicio;
    }

    public Time getHoraFin() {
        return horaFin;
    }

    public void setHoraFin(Time horaFin) {
        this.horaFin = horaFin;
    }
}