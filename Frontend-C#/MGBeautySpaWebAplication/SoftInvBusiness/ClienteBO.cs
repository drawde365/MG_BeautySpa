using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SoftInvBusiness.SoftInvWSCliente; // Ajusta si el namespace del proxy SOAP es diferente

namespace SoftInvBusiness
{
    public class ClienteBO
    {
        private ClienteClient clienteSOAP;

        public ClienteBO()
        {
            clienteSOAP = new ClienteClient();
        }

        public int CrearCliente(clienteDTO cliente)
        {
            return clienteSOAP.crearCliente(cliente);
        }

        public int ModificarDatosCliente(clienteDTO cliente)
        {
            return clienteSOAP.modificarDatosCliente(cliente);
        }

        public int EliminarCuenta(clienteDTO cliente)
        {
            return clienteSOAP.eliminarCuenta(cliente);
        }

        public IList<clienteDTO> ObtenerClientes(string nombre, string primerApellido, string segundoApellido, string correo, string celular)
        {
            return clienteSOAP.buscarCliente(nombre,primerApellido,segundoApellido,correo,celular);
        }
    }
}
