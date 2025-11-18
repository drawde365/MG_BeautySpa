/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package pe.edu.pucp.softinv.services.softinvws;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import pe.edu.pucp.softinv.business.BO.Impl.ReportesBO;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroBuilder;

/**
 *
 * @author Usuario
 */
@WebService(serviceName = "Reportes")
public class Reportes {

    ReportesBO reporteBO;
    
    public Reportes(){
        reporteBO = new ReportesBO();
    }
    
    @WebMethod(operationName = "generarReporteCitas")
    public byte[] generarReporteCitas(@WebParam(name = "Filtros") FiltroReporte filtro) {
        return reporteBO.exportarCitas(filtro);
    }
    
    @WebMethod(operationName = "generarReporteVentas")
    public byte[] generarReporteVentas(@WebParam(name = "Filtros") FiltroReporte filtro) {
        return reporteBO.exportarVentas(filtro);
    }
    
}
