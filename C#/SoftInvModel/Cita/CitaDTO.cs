using System;

namespace softinv.model
{
    public class CitaDTO
    {
        private int? id;
        private DateTime? horaIni;
        private DateTime? horaFin;
        private ClienteDTO cliente;
        private ServicioDTO servicio;
        private EmpleadoDTO empleado;
        private DateTime? fecha;

        public CitaDTO()
        {
            this.Id = null;
            this.HoraIni = null;
            this.HoraFin = null;
            this.Cliente = null;
            this.Servicio = null;
            this.Empleado = null;
            this.Fecha = null;
        }

        public CitaDTO(int id, DateTime horaIni, DateTime horaFin, ClienteDTO cliente, ServicioDTO servicio, EmpleadoDTO empleado,
                       DateTime fecha)
        {
            this.Id = id;
            this.HoraIni = horaIni;
            this.HoraFin = horaFin;
            this.Cliente = cliente;
            this.Servicio = servicio;
            this.Empleado = empleado;
            this.Fecha = fecha;
        }

        public int? Id { get => id; set => id = value; }
        public DateTime? HoraIni { get => horaIni; set => horaIni = value; }
        public DateTime? HoraFin { get => horaFin; set => horaFin = value; }
        public ClienteDTO Cliente { get => cliente; set => cliente = value; }
        public ServicioDTO Servicio { get => servicio; set => servicio = value; }
        public EmpleadoDTO Empleado { get => empleado; set => empleado = value; }
        public DateTime? Fecha { get => fecha; set => fecha = value; }
    }
}


