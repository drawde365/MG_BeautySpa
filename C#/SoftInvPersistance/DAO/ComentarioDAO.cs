using softinv.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInv.DAO
{
    public interface ComentarioDAO
    {

        int insertar(ComentarioDTO comentario);

        ComentarioDTO obtenerPorId(int idComentario);

        int modificar(ComentarioDTO comentario);

        int eliminar(ComentarioDTO comentario);

    }
}
