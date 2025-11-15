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

        public int insertarHorarioDeEmpleado(horarioTrabajoDTO horario)
        {
            return horarioSOAP.InsertarHorarioTrabajo(horario);
        }

        public horarioTrabajoDTO obtenerHorarioPorId(int horarioId)
        {
            return horarioSOAP.ObtenerHorarioTrabajoPorId(horarioId);
        }

        public IList<horarioTrabajoDTO> obtenerHorariosDeEmpleadoPorDia(int idEmpleado, int idDiaSemana)
        {
            return horarioSOAP.ObtenerHorariosPorEmpleadoYFecha(idEmpleado, idDiaSemana);
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