using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace softinv.model
{
    public abstract class UsuarioDTO
    {
        private string nombre;
        private string Primerapellido;
        private string Segundoapellido;
        private string correoElectronico;
        private string contrasenha;
        private string celular;
        private string urlFotoPerfil;
        protected string rol;
        private BindingList<CitaDTO> citas;
        private int idUsuario;

        public abstract void setRol();

        public string getRol()
        {
            return rol;
        }

        public UsuarioDTO()
        {
            nombre = null;
            Primerapellido = null;
            Segundoapellido = null;
            rol = null;
            contrasenha = null;
            correoElectronico = null;
            celular = null;
            urlFotoPerfil = null;
            citas = null;
        }

        public UsuarioDTO(string nombre, string PrimerApellido, string SegundoApellido, string correoElectronico, string contrasenha, string celular, string urlFotoPerfil, int idUsuario)
        {
            this.nombre = nombre;
            this.Primerapellido = PrimerApellido;
            this.Segundoapellido = SegundoApellido;
            this.correoElectronico = correoElectronico;
            this.contrasenha = contrasenha;
            this.celular = celular;
            this.urlFotoPerfil = urlFotoPerfil;
            this.idUsuario = idUsuario;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Primerapellido1 { get => Primerapellido; set => Primerapellido = value; }
        public string Segundoapellido1 { get => Segundoapellido; set => Segundoapellido = value; }
        public string CorreoElectronico { get => correoElectronico; set => correoElectronico = value; }
        public string Contrasenha { get => contrasenha; set => contrasenha = value; }
        public string Celular { get => celular; set => celular = value; }
        public string UrlFotoPerfil { get => urlFotoPerfil; set => urlFotoPerfil = value; }
        internal BindingList<CitaDTO> Citas { get => citas; set => citas = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }

    }
}