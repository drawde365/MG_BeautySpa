using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    public interface EmpleadoDAO
    {
        int insertar(EmpleadoDTO empleado);
        int modificar(EmpleadoDTO empleado);
        int eliminar(int empleadoId);
        EmpleadoDTO obtenerPorId(int empleadoId);
        IList<EmpleadoDTO> listarTodos();
    }
}
