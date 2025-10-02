using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    interface ProductoDAO
    {
        int insertar(ProductoDTO producto);
        ProductoDTO obtenerPorId(int id);
        int modificar(ProductoDTO producto);
        int eliminar(ProductoDTO idProducto);
    }
}
