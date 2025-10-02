using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using softinv.model;
using SoftInv.DAO;
using SoftInv.DAOImpl.Util;
using static System.Collections.Specialized.BitVector32;

namespace SoftInv.DAOImpl
{
    public class CitaDAOImpl: DAOImplBase, CitaDAO {

        private CitaDTO cita;

        public CitaDAOImpl(): base("CITAS"){
            cita = null;
            retornarLlavePrimaria=true;
        }

        protected override void ConfigurarListaDeColumnas() {
            listaColumnas.Add(new Columna("CITA_ID", true, true));
            listaColumnas.Add(new Columna("EMPLEADO_ID", false, false));
            listaColumnas.Add(new Columna("CLIENTE_ID", false, false));
            listaColumnas.Add(new Columna("SERVICIO_ID", false, false));
            listaColumnas.Add(new Columna("FECHA", false, false));
            listaColumnas.Add(new Columna("HORA_INICIO", false, false));
            listaColumnas.Add(new Columna("HORA_FIN", false, false));
        }
        public int Insertar(CitaDTO cita) {
            this.cita = cita;
            return base.Insertar();
        }

        public int modificar(CitaDTO cita) {
            this.cita = cita;
            return base.Modificar();
        }

        public int eliminar(CitaDTO cita) {
            this.cita = cita;
            return base.Eliminar();
        }

        IList<CitaDTO> listarCitasPorPeriodo(DateTime inicio, DateTime fin) {
            IList<CitaDTO> citas = new IList<>();
            String sql = "SELECT * FROM CITAS";
            this.AbrirConexion();
            try {
                this.ColocarSQLenComando(sql);
                this.EjecutarConsultaEnBD();
                while (this.resultSet.next()) {

                    if (resultSet.getDate("FECHA")>=inicio and resultSet.getDate("FECHA")<=fin){

                        CitaDTO cita = new Cita();
                        cita.setId(resultset.GEtIn("CITA_ID");
                        cita.setEmpleado(resultset.GEtIn("EMPLEADO_ID");
                        cita.setClienteId(resultset.GEtIn("CLIENTE_ID");
                        cita.setServicioId(resultset.GEtIn("SERVICIO_ID");
                        cita.setFecha(resultset.GEtIn("FECHA");
                        cita.setHoraIni(resultset.GEtIn("HORA_INICIO");
                        cita.setHoraFin(resultset.GEtIn("HORA_FIN");

                        citas.Add(cita);
                    }
                }
            } catch (SQLException ex) {
                throw new RuntimeException(ex);
            }
                return citas;
            }
        }
}