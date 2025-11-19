using SoftInvBusiness.SoftInvWSEmpleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class EmpleadoBO
    {
        private EmpleadoClient empleadoSOAP;

        public EmpleadoBO()
        {
            empleadoSOAP = new EmpleadoClient();
        }

        // ----- Empleados -----

        public int InsertarEmpleadoPorPartes(
            string nombre,
            string primerApellido,
            string segundoApellido,
            string correoElectronico,
            string contrasenha,
            string celular,
            string urlFotoPerfil,
            bool admin)
        {
            return empleadoSOAP.InsertarEmpleadoPorPartes(
                nombre, primerApellido, segundoApellido,
                correoElectronico, contrasenha, celular, urlFotoPerfil, admin);
        }

        public int InsertarEmpleado(empleadoDTO empleado)
        {
            return empleadoSOAP.InsertarEmpleado(empleado);
        }

        public int ModificarEmpleado(empleadoDTO empleado)
        {
            return empleadoSOAP.ModificarEmpleado(empleado);
        }

        public int EliminarEmpleado(empleadoDTO empleado)
        {
            return empleadoSOAP.EliminarEmpleado(empleado);
        }

        public empleadoDTO ObtenerEmpleadoPorId(int idUsuario)
        {
            return empleadoSOAP.ObtenerEmpleadoPorId(idUsuario);
        }

        public IList<empleadoDTO> ListarTodosEmpleados()
        {
            return empleadoSOAP.ListarTodosEmpleados();
        }

        // ----- Servicios del Empleado -----

        public void AgregarServicioAEmpleado(int empleadoId, int servicioId)
        {
            empleadoSOAP.AgregarServicioAEmpleado(empleadoId, servicioId);
        }

        public void EliminarServicioDeEmpleado(int empleadoId, int servicioId)
        {
            empleadoSOAP.QuitarServicioAEmpleado(empleadoId, servicioId);
        }

        public IList<servicioDTO> ListarServiciosDeEmpleado(int empleadoId)
        {
            return empleadoSOAP.ListarServiciosDeEmpleado(empleadoId);
        }

        // ----- Citas del Empleado -----

        public IList<citaDTO> ListarCitasDeEmpleado(int empleadoId)
        {
            return empleadoSOAP.ListarCitasDeEmpleado(empleadoId);
        }

        public IList<servicioDTO> ObtenerServiciosNoBrindadosDeEmpleado(int empleadoId)
        {
            return empleadoSOAP.ObtenerServiciosNoBrindados(empleadoId);
        }
    }
}

