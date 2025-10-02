using System;

namespace softinv.model
{
    public class ComentarioDTO
    {
        private int? idComentario;
        private ClienteDTO cliente;
        private String comentario;
        private int? valoracion;

       
        public ComentarioDTO()
        {
            IdComentario = null;
            Cliente = null;
            Comentario = null;
            Valoracion = null;
        }

        public ComentarioDTO(int idComentario, ClienteDTO cliente, String comentario, int valoracion)
        {
            this.IdComentario = idComentario;
            this.Cliente = cliente;
            this.Comentario = comentario;
            this.Valoracion = valoracion;
        }

        public int? IdComentario { get => idComentario; set => idComentario = value; }
        public ClienteDTO Cliente { get => cliente; set => cliente = value; }
        public string Comentario { get => comentario; set => comentario = value; }
        public int? Valoracion { get => valoracion; set => valoracion = value; }
    }
}
