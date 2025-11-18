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
    public void generarReporteCitas(@WebParam(name = "Filtros") FiltroReporte filtro) {
        reporteBO.exportarCitas(filtro);
    }
    
    @WebMethod(operationName = "generarReporteVentas")
    public void generarReporteVentas(@WebParam(name = "Filtros") FiltroReporte filtro) {
        reporteBO.exportarVentas(filtro);
    }
    
}
