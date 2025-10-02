using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    public interface ClienteDAO
    {
        int insertar(ClienteDTO usuario);
        ClienteDTO obtenerPorId(int id);
        int modificar(ClienteDTO usuario);
        int eliminar(ClienteDTO cliente);
    }
}
