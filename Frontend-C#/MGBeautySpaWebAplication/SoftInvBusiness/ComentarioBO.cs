using SoftInvBusiness.SoftInvWSComentario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SoftInvBusiness
{
    public class ComentarioBO
    {
        private ComentarioClient comentarioSOAP;

        public ComentarioBO()
        {
            comentarioSOAP = new ComentarioClient();
        }

        // ----- Inserciones -----

        public int InsertarComentarioDeProducto(int clienteId, string comentario, int valoracion, int productoId)
        {
            return comentarioSOAP.InsertarComentarioDeProducto(clienteId, comentario, valoracion, productoId);
        }

        public int InsertarComentarioDeServicio(int clienteId, string comentario, int valoracion, int servicioId)
        {
            return comentarioSOAP.InsertarComentarioDeServicio(clienteId, comentario, valoracion, servicioId);
        }

        // ----- Modificar / Eliminar -----

        public int ModificarComentario(comentarioDTO comentario)
        {
            return comentarioSOAP.ModificarComentario(comentario);
        }

        public int EliminarComentario(comentarioDTO comentario)
        {
            return comentarioSOAP.EliminarComentario(comentario);
        }

        // ----- Consultas -----

        public IList<comentarioDTO> ObtenerComentariosPorProducto(int idProducto)
        {
            return comentarioSOAP.ObtenerComentariosPorProducto(idProducto);
        }

        public IList<comentarioDTO> ObtenerComentariosPorServicio(int idServicio)
        {
            return comentarioSOAP.ObtenerComentariosPorServicio(idServicio);
        }

        public comentarioDTO ObtenerComentarioPorId(int idComentario)
        {
            return comentarioSOAP.obtenerComentarioPorId(idComentario);
        }
    }
}
