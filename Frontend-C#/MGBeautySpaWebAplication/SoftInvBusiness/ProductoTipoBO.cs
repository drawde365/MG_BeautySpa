using SoftInvBusiness.SoftInvWSProductoTipo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class ProductoTipoBO
    {
        private ProductosTipoClient productoTipoSOAP;
        public ProductoTipoBO()
        {
            productoTipoSOAP = new ProductosTipoClient();
        }

        public IList<productoTipoDTO> ObtenerProductosTiposId(int id)
        {
            return productoTipoSOAP.ObtenerProductosTiposId(id);
        }
    }
}
