/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.softinv.dao;

import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;

/**
 *
 * @author Usuario
 */
public interface Reportable <T>{
    String[] getTitulosColumnas();
    String[] getDatosFila();
    float[] getAnchosColumnas();
    double getMontoTotal();
    void assign(T item);
    String getTitulo();
    String[] getSubtitulos(FiltroReporte filtro);
}
