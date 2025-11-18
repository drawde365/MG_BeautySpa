/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.softinv.dao;

import java.util.ArrayList;
import java.util.List;
import pe.edu.pucp.sotfinv.model.Reportes.DatoReporteProductos;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;

/**
 *
 * @author Usuario
 */
public interface ReporteProductosDAO {
    
    ArrayList<DatoReporteProductos>obtenerReporte(FiltroReporte filtro);
    
}
