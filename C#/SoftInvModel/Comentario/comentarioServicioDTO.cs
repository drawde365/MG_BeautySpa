using System;

namespace softinv.model
{
    public class ComentarioServicioDTO: ComentarioDTO
    {
        private ServicioDTO servicio;

        public ServicioDTO Servicio { get => servicio; set => servicio = value; }

        public ComentarioServicioDTO():base()
        {
            Servicio = null;
        }

        public ComentarioServicioDTO(int idComentario, ClienteDTO cliente, String comentario, int valoracion, ServicioDTO servicio): base(idComentario, cliente, comentario, valoracion)
        {
            this.Servicio = servicio;
        }

        public int? IdServicio { get => servicio.IdServicio; set => servicio.IdServicio = value; }

    }

}
