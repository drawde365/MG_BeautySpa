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
import pe.edu.pucp.sotfinv.model.Reportes.FiltroBuilder;
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
    public byte[] exportarVentas(FiltroReporte filtro) {
        System.out.println("--- Iniciando Exportación de Ventas ---");
        
        //Creación segura del filtro para Ventas
        FiltroReporte filtroReporte = new FiltroBuilder()
                .filtrarFechaInicio(filtro.getFechaInicio())
                .filtrarFechaFin(filtro.getFechaFin())
                .filtrarProductoNombreProducto(filtro.getNombreProducto())
                .filtrarProductoEstadoPedido(filtro.getEstadoPedido())
                .filtrarProductoTipoProducto(filtro.getTipoProducto())
                .buildFiltro();
        
        List<DatoReporteProductos> lista = reporteProductosDAO.obtenerReporte(filtroReporte);
        
        if (lista.isEmpty()) {
            System.out.println("⚠ No se encontraron ventas con esos filtros.");
            return new byte[0];
        }
        
        return generadorReporte.generarReporte(lista, filtroReporte);
    }

    //Generar Reporte de Servicios
    public byte[] exportarCitas(FiltroReporte filtro) {
        System.out.println("--- Iniciando Exportación de Servicios ---");

        //Creación segura del filtro para Citas
        FiltroReporte filtroReporte = new FiltroBuilder()
                .filtrarFechaInicio(filtro.getFechaInicio())
                .filtrarFechaFin(filtro.getFechaFin())
                .filtrarServicioNombreEmpleado(filtro.getNombreEmpleado())
                .filtrarServicioNombreServicio(filtro.getNombreServicio())
                .filtrarServicioTipoServicio(filtro.getTipoServicio())
                .buildFiltro();
        
        List<DatoReporteServicios> lista = reporteServiciosDAO.obtenerReporte(filtroReporte);
        
        if (lista.isEmpty()) {
            System.out.println("⚠ No se encontraron citas con esos filtros.");
            return new byte[0];
        }

        return generadorReporte.generarReporte(lista, filtroReporte);
    }
    
}
