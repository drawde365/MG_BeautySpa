using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

