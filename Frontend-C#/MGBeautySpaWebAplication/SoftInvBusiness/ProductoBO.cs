using SoftInvBusiness.SoftInvWSProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class ProductoBO
    {
        private ProductosClient productoSOAP;

        public ProductoBO()
        {
            productoSOAP = new ProductosClient();
        }

        public IList<productoDTO> buscarPorNombre(string texto)
        {
            return productoSOAP.buscarProductos(texto);
        }

        public IList<productoDTO> pagina(int n)
        {
            return productoSOAP.obtenerProdPag(n);
        }

        public int numeroPaginas()
        {
            return productoSOAP.cantidadPagProd();
        }

        public productoDTO buscarPorId(int id)
        {
            return productoSOAP.ObtenerProducto(id);
        }

        public IList<productoDTO> filtroCorporal() {
            return productoSOAP.obtenerCorporales();
        }
        public IList<productoDTO> filtroFacial() {
            return productoSOAP.obtenerFaciales();
        }

        public IList<productoDTO> ListarTodos()
        {
            return productoSOAP.listarTodos();
        }

        public int eliminar(productoDTO producto)
        {
            return productoSOAP.EliminarProducto(producto);
        }

        public int modificarProducto(productoDTO producto)
        {
            return productoSOAP.ModificarProducto(producto);
        }

        public int insertarProducto(productoDTO producto)
        {
            return productoSOAP.insertarProducto(producto);
        }
    }
}
