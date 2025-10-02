using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    public interface DisponibilidadDAO
    {
        int insertar(DisponibilidadDTO disponibilidad);

        int modificar(DisponibilidadDTO disponibilidad);

        int eliminar(DisponibilidadDTO disponibilidad);

        DisponibilidadDTO obtenerPorId(int disponibilidadId);

    }
}
