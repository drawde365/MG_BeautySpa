using System;

namespace softinv.model
{
    public class DisponibilidadDTO
    {
        private int? disponibilidadId;
        private EmpleadoDTO empleado;
        private int? diaSemana;
        private DateTime? horaInicio;
        private DateTime? horaFin;
        public int? DisponibilidadId { get => disponibilidadId; set => disponibilidadId = value; }
        public EmpleadoDTO Empleado { get => empleado; set => empleado = value; }
        public int? DiaSemana { get => diaSemana; set => diaSemana = value; }
        public DateTime? HoraInicio { get => horaInicio; set => horaInicio = value; }
        public DateTime? HoraFin { get => horaFin; set => horaFin = value; }
    }
}
