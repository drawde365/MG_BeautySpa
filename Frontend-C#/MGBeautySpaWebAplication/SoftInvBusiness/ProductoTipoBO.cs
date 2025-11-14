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

        public IList<productoTipoDTO> ObtenerPorIdProducto(int idProducto)
        {
            return productoTipoSOAP.ObtenerProductosTiposId(idProducto);
        }

        public IList<productoTipoDTO> ObtenerPorIdProductoActivo(int idProducto)
        {
            return productoTipoSOAP.ObtenerProductosTiposIdActivo(idProducto);
        }

        public int Insertar(productoTipoDTO productoTipo)
        {
            return productoTipoSOAP.Insertar(productoTipo);
        }

        public int Modificar(productoTipoDTO productoTipo)
        {
            return productoTipoSOAP.Modificar(productoTipo);
        }

        public int Eliminar(productoTipoDTO productoTipo)
        {
            return productoTipoSOAP.Eliminar(productoTipo);
        }
    }
}

