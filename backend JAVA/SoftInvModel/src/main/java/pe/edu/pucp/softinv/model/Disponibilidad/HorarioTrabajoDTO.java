package pe.edu.pucp.softinv.model.Disponibilidad;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;

public class HorarioTrabajoDTO {
    private EmpleadoDTO empleado;
    private Integer diaSemana;
    private Integer intervalos;
    private Time horaInicio;
    private Time horaFin;

    public Time getHoraFin() {
        return horaFin;
    }

    public void setHoraFin(Time horaFin) {
        this.horaFin = horaFin;
    }

    public Time getHoraInicio() {
        return horaInicio;
    }

    public void setHoraInicio(Time horaInicio) {
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

    public Integer getIntervalos() {
        return intervalos;
    }

    public void setIntervalos(Integer intervalos) {
        this.intervalos = intervalos;
    }


    public HorarioTrabajoDTO () {
        this.intervalos=null;
        this.empleado=null;
        this.diaSemana=null;
        this.horaInicio=null;
        this.horaFin=null;
    }

    public HorarioTrabajoDTO(Integer intervalos, EmpleadoDTO empleado, Integer diaSemana, Time horaInicio, Time horaFin) {
        this.intervalos=intervalos;
        this.empleado=empleado;
        this.diaSemana=diaSemana;
        this.horaInicio=horaInicio;
        this.horaFin=horaFin;
    }
}
