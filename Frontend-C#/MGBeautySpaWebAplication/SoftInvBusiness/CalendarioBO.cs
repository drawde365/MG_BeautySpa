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

        // ----- Calendario -----

        public int InsertarCalendarioPorPartes(
            int idEmpleado,
            date fecha,
            int cantLibre,
            string motivo)
        {
            return calendarioSOAP.InsertarCalendarioPorPartes(idEmpleado, fecha, cantLibre, motivo);
        }

        public int InsertarCalendario(calendarioDTO calendario)
        {
            return calendarioSOAP.InsertarCalendario(calendario);
        }

        public int ModificarCalendarioPorPartes(
            int idEmpleado,
            date fecha,
            int cantLibre,
            string motivo)
        {
            return calendarioSOAP.ModificarCalendarioPorPartes(idEmpleado, fecha, cantLibre, motivo);
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

