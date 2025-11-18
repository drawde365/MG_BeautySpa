/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.business.BO.Impl;

import java.util.List;
import pe.edu.pucp.softinv.dao.ReporteProductosDAO;
import pe.edu.pucp.softinv.dao.ReporteServiciosDAO;
import pe.edu.pucp.softinv.daoImp.GeneradorReporte;
import pe.edu.pucp.softinv.daoImp.ReporteProductosDAOImpl;
import pe.edu.pucp.softinv.daoImp.ReporteServiciosDAOImpl;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteProductos;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteServicios;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;


/**
 *
 * @author Usuario
 */
public class ReportesBO {
    
    private ReporteProductosDAO reporteProductosDAO;
    private ReporteServiciosDAO reporteServiciosDAO;
    private GeneradorReporte generadorReporte;
    
    public ReportesBO(){
        reporteProductosDAO = new ReporteProductosDAOImpl();
        reporteServiciosDAO = new ReporteServiciosDAOImpl();
        generadorReporte = new GeneradorReporte();
    }
    
    //Generar Reporte de Productos
    public void exportarVentas(FiltroReporte filtro) {
        System.out.println("--- Iniciando Exportación de Ventas ---");
        
        List<DatoReporteProductos> lista = reporteProductosDAO.obtenerReporte(filtro);
        
        if (lista.isEmpty()) {
            System.out.println("⚠ No se encontraron ventas con esos filtros.");
            return;
        }
        
        //Generación del pdf
        String nombreArchivo = generadorReporte.obtenerRutaDescargas("Reporte_Ventas");
        generadorReporte.generarReporte(lista, filtro, nombreArchivo);

        // 4. (Opcional) Abrir el archivo automáticamente
        abrirArchivo(nombreArchivo);
    }

    //Generar Reporte de Servicios
    public void exportarCitas(FiltroReporte filtro) {
        System.out.println("--- Iniciando Exportación de Servicios ---");

        // 1. Persistencia: Obtener lista de servicios
        List<DatoReporteServicios> lista = reporteServiciosDAO.obtenerReporte(filtro);

        // 2. Validación
        if (lista.isEmpty()) {
            System.out.println("⚠ No se encontraron citas con esos filtros.");
            return;
        }

        // 3. Infraestructura: Generar PDF
        String nombreArchivo = generadorReporte.obtenerRutaDescargas("Reporte_Citas");
        generadorReporte.generarReporte(lista, filtro, nombreArchivo);

        abrirArchivo(nombreArchivo);
    }

    // Método utilitario para abrir el PDF al terminar
    private void abrirArchivo(String ruta) {
        try {
            if (java.awt.Desktop.isDesktopSupported()) {
                java.awt.Desktop.getDesktop().open(new java.io.File(ruta));
            }
        } catch (Exception e) {
            System.out.println("Archivo generado en: " + ruta);
        }
    }
    
}
