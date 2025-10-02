using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    interface ServicioDAO
    {
        int insertar(ServicioDTO servicio);
        ServicioDTO obtenerPorId(int id);
        int modificar(ServicioDTO servicio);
        int eliminar(ServicioDTO servicio);
        IList<EmpleadoDTO> listarEmpleadosDeServicio(int servicioID);
    }
}
