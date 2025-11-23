/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp.util;
import com.lowagie.text.Document;
import com.lowagie.text.pdf.PdfContentByte;
import com.lowagie.text.pdf.PdfPageEventHelper;
import com.lowagie.text.pdf.PdfWriter;
import java.awt.Color;

public class FondoOlasEvent extends PdfPageEventHelper {

    @Override
    public void onEndPage(PdfWriter writer, Document document) {
        PdfContentByte canvas = writer.getDirectContentUnder(); // Dibujar DETRÁS del texto
        
        float width = document.getPageSize().getWidth();
        float height = document.getPageSize().getHeight();

        // --- 1. OLA SUPERIOR (Verde Turquesa) ---
        canvas.saveState();
        canvas.setColorFill(new Color(26, 188, 156)); // Color del diseño original
        
        canvas.moveTo(0, height); // Empezar esquina superior izquierda
        canvas.lineTo(0, height - 80); // Bajar un poco por la izquierda
        
        // DIBUJAR LA CURVA (La magia está aquí)
        // curveTo(controlX1, controlY1, controlX2, controlY2, destinoX, destinoY)
        canvas.curveTo(width * 0.25f, height - 120, // Punto de control 1 (tira hacia abajo)
                       width * 0.75f, height - 40,  // Punto de control 2 (tira hacia arriba)
                       width, height - 80);         // Punto final (derecha)
                       
        canvas.lineTo(width, height); // Subir a esquina superior derecha
        canvas.lineTo(0, height); // Cerrar la forma volviendo al inicio
        canvas.fill(); // Rellenar de color
        canvas.restoreState();

        // --- 2. OLA INFERIOR (Opcional: Color diferente o el mismo) ---
        canvas.saveState();
        canvas.setColorFill(new Color(26, 188, 156)); // Amarillo/Dorado como el gráfico
        // O usa el mismo verde si prefieres
        
        canvas.moveTo(0, 0); // Esquina inferior izquierda
        canvas.lineTo(0, 50); // Subir un poco
        
        // Curva invertida para abajo
        canvas.curveTo(width * 0.25f, 20, 
                       width * 0.75f, 80, 
                       width, 50);
                       
        canvas.lineTo(width, 0); // Esquina inferior derecha
        canvas.lineTo(0, 0); // Cerrar
        canvas.fill();
        canvas.restoreState();
    }
}