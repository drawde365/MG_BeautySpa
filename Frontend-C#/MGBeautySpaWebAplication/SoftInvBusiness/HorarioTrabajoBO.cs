using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class HorarioTrabajoBO
    {
        private HorarioTrabajoClient horarioSOAP;

        public HorarioTrabajoBO()
        {
            horarioSOAP = new HorarioTrabajoClient();
        }

        public int insertarHorarioDeEmpleadoPorPartes(int idEmpleado, int idDiaSemana, string horaInicio, string horaFin)
        {
            return horarioSOAP.InsertarHorarioTrabajoPorPartes(idEmpleado, idDiaSemana, horaInicio, horaFin);
        }

        public int insertarHorarioDeEmpleado(horarioTrabajoDTO horario)
        {
            return horarioSOAP.InsertarHorarioTrabajo(horario);
        }

        public horarioTrabajoDTO obtenerHorarioDeEmpleadoPorId(int idEmpleado, int idDiaSemana)
        {
            return horarioSOAP.ObtenerHorarioTrabajoPorId(idEmpleado, idDiaSemana);
        }

        public int modificarHorarioTrabajo(horarioTrabajoDTO horario)
        {
            return horarioSOAP.ModificarHorarioTrabajo(horario);
        }

        public int eliminarHorarioDeEmpleado(horarioTrabajoDTO horario)
        {
            return horarioSOAP.EliminarHorarioTrabajo(horario);
        }

        public IList<horarioTrabajoDTO> ListarHorarioDeEmpleado(int empleadoId)
        {
            return horarioSOAP.ListarPorEmpleado(empleadoId);
        }
    }
}
