using System;
using System.ComponentModel;

namespace softinv.model
{
    public class EmpleadoDTO : UsuarioDTO
    {
        private bool? admin;
        private BindingList<ServicioDTO> servicios;

        public bool? Admin { get => admin; set => admin = value; }
        internal BindingList<ServicioDTO> Servicios { get => servicios; set => servicios = value; }

        public override void setRol()
        {
            if ((bool)Admin) rol = "Admin";
            else rol = "Empleado";
        }

        public void setRol(string rol)
        {
            this.rol = rol;
        }


        public EmpleadoDTO() : base()
        {
            Admin = null;
            Servicios = null;
        }

        public EmpleadoDTO(string nombre, string PrimerApellido, string SegundoApellido, string correoElectronico,
                           string contrasenha, string celular, int idUsuario, bool admin, string urlFotoPerfil, BindingList<ServicioDTO> servicios) : base(nombre, PrimerApellido, SegundoApellido, correoElectronico, contrasenha, celular, urlFotoPerfil, idUsuario)
        {
            this.Admin = admin;
            this.Servicios = servicios;
            setRol();
        }

        public EmpleadoDTO(bool admin, BindingList<ServicioDTO> servicios) : base()
        {
            this.Admin = admin;
            this.Servicios = servicios;
        }
    }
}