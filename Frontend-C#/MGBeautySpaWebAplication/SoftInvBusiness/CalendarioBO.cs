using SoftInvBusiness.SoftInvWSCalendario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class CalendarioBO
    {
        private CalendarioClient calendarioSOAP;

        public CalendarioBO()
        {
            calendarioSOAP = new CalendarioClient();
        }

        // ----- Scheduling Logic (New Methods) -----

        public string[] CalcularBloquesDisponibles(
            int empleadoId,
            DateTime fecha, // <-- Aceptas un DateTime estándar
            int duracionServicioMinutos)
        {
            return calendarioSOAP.CalcularBloquesDisponibles(
                empleadoId,
                fecha, 
                duracionServicioMinutos);
        }

        public int ReservarBloqueYCita(citaDTO cita)
        {
            return calendarioSOAP.ReservarBloqueYCita(cita);
        }

        // ----- Calendario (CRUD Básico) -----

        public int InsertarCalendario(calendarioDTO calendario)
        {
            return calendarioSOAP.InsertarCalendario(calendario);
        }

        public int ModificarCalendario(calendarioDTO calendario)
        {
            return calendarioSOAP.ModificarCalendario(calendario);
        }

        public int EliminarCalendario(calendarioDTO calendario)
        {
            return calendarioSOAP.EliminarCalendario(calendario);
        }

        public IList<calendarioDTO> ListarCalendarioDeEmpleado(int empleadoId)
        {
            return calendarioSOAP.ListarCalendarioDeEmpleado(empleadoId);
        }
    }
}