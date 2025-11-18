using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;


namespace SoftInvBusiness
{
    public class UsuarioBO
    {
        private UsuarioClient usuarioSOAP;

        public UsuarioBO()
        {
            usuarioSOAP = new UsuarioClient();
        }

        public usuarioDTO IniciarSesion(string correoElectronico, string contrasenha)
        {
            return usuarioSOAP.IniciarSesion(correoElectronico, contrasenha);
        }

        public usuarioDTO ObtenerUsuarioPorCorreo(string correoElectronico)
        {
            return usuarioSOAP.ObtenerUsuario(correoElectronico);
        }

        public int GuardarTokenRecuperacion(int idUsuario, string token)
        {
            return usuarioSOAP.RegistrarToken(idUsuario, token);
        }

        public contrasenhaTokenDTO recuperarToken(string token)
        {
            return usuarioSOAP.ObtenerTokenDelUsuario(token);
        }

        public int actualizarContraseña(int idUsuario, string nuevaContrasenha)
        {
            return usuarioSOAP.ModificarContrasenha(idUsuario, nuevaContrasenha);
        }

        public int tokenUsado(contrasenhaTokenDTO token)
        {
            return usuarioSOAP.MarcarTokenComoUsado(token);
        }

        public List<usuarioDTO> ObtenerTodosUsuarios()
        {
            return usuarioSOAP.ListarUsuarios().ToList();
        }

        public int actividadUsuario(int idUsuario, int estado)
        {
            return usuarioSOAP.ActivoUsuario(idUsuario, estado);
        }
    }
}

