using SoftInvBusiness.SoftInvWSReportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftInvBusiness
{
    public class ReporteBO
    {
        private ReportesClient reporteSOAP;
        public ReporteBO()
        {
            reporteSOAP = new ReportesClient();
        }

        public byte[] generarReporteVentas(filtroReporte filtro)
        {
            return reporteSOAP.generarReporteVentas(filtro);
        }

        public byte[] generarReporteCitas(filtroReporte filtro)
        {
            return reporteSOAP.generarReporteCitas(filtro);
        }

    }
}
