using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    interface PedidoDAO {
        int insertar(PedidoDTO pedido);
        PedidoDTO obtenerPorId(int id);
        int modificar(PedidoDTO pedido);
        int eliminar(PedidoDTO pedido);
        IList<PedidoDTO> listarPedidos(int id);
    }
}
