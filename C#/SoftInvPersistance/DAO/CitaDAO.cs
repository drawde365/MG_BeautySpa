using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    public interface CitaDAO
    {
        int insertar(CitaDTO cita);
        CitaDTO obtenerPorId(CitaDTO idCita);
        int modificar(CitaDTO cita);
        int eliminar(CitaDTO cita);
        IList<CitaDTO> listarCitasPorPeriodo(DateTime inicio, DateTime fin);
    }
}
