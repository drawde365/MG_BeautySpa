package pe.edu.pucp.softinv.model.Disponibilidad;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;

public class HorarioTrabajoDTO {
    private Integer id;
    private EmpleadoDTO empleado;
    private Integer diaSemana;
    private String horaInicio;
    private String horaFin;

    public String getHoraFin() {
        return horaFin;
    }

    public void setHoraFin(String horaFin) {
        this.horaFin = horaFin;
    }

    public String getHoraInicio() {
        return horaInicio;
    }

    public void setHoraInicio(String horaInicio) {
        this.horaInicio = horaInicio;
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

    public HorarioTrabajoDTO(Integer id, EmpleadoDTO empleado, Integer diaSemana, String horaInicio, String horaFin) {
        this.id=id;
        this.empleado=empleado;
        this.diaSemana=diaSemana;
        this.horaInicio=horaInicio;
        this.horaFin=horaFin;
    }
}
