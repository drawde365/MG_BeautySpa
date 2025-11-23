/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softpub;

import java.util.List;
import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.Reportable;
import pe.edu.pucp.softinv.dao.ReporteProductosDAO;
import pe.edu.pucp.softinv.dao.ReporteServiciosDAO;
import pe.edu.pucp.softinv.daoImp.GeneradorReporte;
import pe.edu.pucp.softinv.daoImp.ReporteProductosDAOImpl;
import pe.edu.pucp.softinv.daoImp.ReporteServiciosDAOImpl;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteProductos;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroBuilder;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;

/**
 *
 * @author Usuario
 */
public class ReporteDAOImplTest {
    GeneradorReporte reporteDAO;
    ReporteProductosDAO repProd;
    ReporteServiciosDAO repServ;
    
    public ReporteDAOImplTest(){
        reporteDAO = new GeneradorReporte();
        repProd = new ReporteProductosDAOImpl();
        repServ = new ReporteServiciosDAOImpl();
    }
//    
//    @Test
//    void testRepProds(){
//        FiltroReporte filtro = new FiltroBuilder().buildFiltro();
//        List<DatoReporteProductos> datos = repProd.obtenerReporte(filtro);
//        Reportable<DatoReporteProductos> adaptador = new ReporteProductosDAOImpl();
//        reporteDAO.generarReporte(datos, filtro, adaptador);
//    }
}
