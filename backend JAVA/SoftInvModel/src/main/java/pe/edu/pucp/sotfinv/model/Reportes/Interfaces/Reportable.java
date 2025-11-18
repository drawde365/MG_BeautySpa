/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Interface.java to edit this template
 */
package pe.edu.pucp.sotfinv.model.Reportes.Interfaces;

/**
 *
 * @author Usuario
 */
public interface Reportable {
    
    String[] getTitulosColumnas();
    String[] getDatosFila();
    float[] getAnchosColumnas();
    double getMontoTotal();
    
}
