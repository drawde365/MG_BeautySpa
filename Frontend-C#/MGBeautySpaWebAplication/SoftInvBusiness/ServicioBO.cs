using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class ServicioBO
    {
        private ServicioClient servicioSOAP;

        public ServicioBO()
        {
            servicioSOAP = new ServicioClient();
        }

        public IList<servicioDTO> ListarFiltro(string filtro) {
            return servicioSOAP.ListarFiltro(filtro);
        }
    }
}
