using SoftInvBusiness.SoftInvWSCita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class CitaBO
    {
        private CitaClient citaSOAP;

        public CitaBO()
        {
            citaSOAP = new CitaClient();
        }

        // ----- Citas -----
        public int InsertarCitaPorPartes(
            int empleadoId,
            int clienteId,
            int servicioId,
            DateTime fecha,
            time horaIni,
            time horaFin,
            double igv,
            int activo,
            string codTransacc)
        {
            return citaSOAP.InsertarCitaPorPartes(
                empleadoId, clienteId, servicioId, fecha, horaIni, horaFin, igv, activo, codTransacc);
        }

        public int ModificarCita(citaDTO cita)
        {
            return citaSOAP.ModificarCita(cita);
        }

        public int EliminarCita(citaDTO cita)
        {
            return citaSOAP.EliminarCita(cita);
        }

        public citaDTO ObtenerCitaPorId(citaDTO citaBusqueda)
        {
            return citaSOAP.ObtenerCitaPorId(citaBusqueda);
        }
        public IList<citaDTO> ListarPorUsuario(usuarioDTO usuario)
        {
            return citaSOAP.ListarCitasPorUsuario(usuario);
        }

        public int AceptarCita(citaDTO cita)
        {
            return citaSOAP.AceptarCita(cita);
        }
    }
}
