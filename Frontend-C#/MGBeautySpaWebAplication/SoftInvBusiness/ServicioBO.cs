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

        public int insertar(servicioDTO servicio)
        {
            return servicioSOAP.InsertarServicioDTO(servicio); 
        }

        public int eliminar(servicioDTO servicio)
        {
            return servicioSOAP.EliminarServicio(servicio);
        }

        public servicioDTO obtenerPorId(int id)
        {
            return servicioSOAP.ObtenerServicioPorId(id);
        }

        public IList<servicioDTO> ListarFiltro(string filtro) {
            return servicioSOAP.ListarFiltro(filtro);
        }
        public IList<servicioDTO> ListarTodo()
        {
            return servicioSOAP.ListarTodos();
        }
    }
}
