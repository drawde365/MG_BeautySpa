package pe.edu.pucp.softinv.model.Disponibilidad;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.sql.Time;

public class DisponibilidadDTO {
    private Integer disponibilidadId;
    private EmpleadoDTO empleado;
    private Integer diaSemana;
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

    public Integer getDisponibilidadId() {
        return disponibilidadId;
    }

    public void setDisponibilidadId(Integer disponibilidadId) {
        this.disponibilidadId = disponibilidadId;
    }


}
