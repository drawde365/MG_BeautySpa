/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp;
import com.lowagie.text.*;
import com.lowagie.text.pdf.*;
//Dependencia OpenPDF
import java.awt.Color;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.NumberFormat;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Locale;
import com.lowagie.text.Document;
import java.io.ByteArrayOutputStream;
import java.io.File;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;
import pe.edu.pucp.sotfinv.model.Reportes.Interfaces.Reportable;
/**
 *
 * @author Usuario
 */
public class GeneradorReporte {
    
    private static final Font F_HEADER = new Font(Font.HELVETICA, 10, Font.BOLD, Color.WHITE);
    private static final Font F_DATA = new Font(Font.HELVETICA, 9, Font.NORMAL, Color.BLACK);
    private static final Font F_TOTAL = new Font(Font.HELVETICA, 12, Font.BOLD, Color.BLACK);

    /**
     * Método único que sirve para AMBOS reportes.
     * Recibe una lista de cualquier cosa que implemente Reportable.
     */
    
    
    public byte[] generarReporte(List<? extends Reportable> datos, FiltroReporte filtro) {
        
        if (datos == null || datos.isEmpty()) {
            System.out.println("La lista está vacía, no se genera PDF.");
            return new byte[0];
        }
        
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        Document document = new Document(PageSize.A4.rotate()); // Hoja Horizontal
        try {
            PdfWriter.getInstance(document,baos);
            document.open();

            // 1. Títulos y Encabezado (Igual que antes)
            agregarEncabezadoGeneral(document, filtro);

            // 2. CONFIGURACIÓN AUTOMÁTICA DE LA TABLA
            // Tomamos el primer elemento como "prototipo" para saber cómo dibujar la tabla
            Reportable prototipo = datos.get(0);
            
            float[] anchos = prototipo.getAnchosColumnas();
            String[] titulos = prototipo.getTitulosColumnas();

            PdfPTable table = new PdfPTable(anchos);
            table.setWidthPercentage(100);

            // 3. Pintar Cabeceras
            for (String titulo : titulos) {
                PdfPCell cell = new PdfPCell(new Phrase(titulo, F_HEADER));
                cell.setBackgroundColor(new Color(41, 128, 185)); // Azul
                cell.setHorizontalAlignment(Element.ALIGN_CENTER);
                cell.setPadding(6);
                table.addCell(cell);
            }

            // 4. Pintar Datos y Calcular Total
            double sumaTotal = 0;

            for (Reportable item : datos) {
                String[] valores = item.getDatosFila();
                
                for (String valor : valores) {
                    PdfPCell cell = new PdfPCell(new Phrase(valor, F_DATA));
                    cell.setPadding(4);
                    
                    // Pequeño truco visual: Si tiene signo '$', alinear a la derecha
                    if (valor.contains("S/.") || valor.matches(".*\\d\\.\\d.*")) {
                        cell.setHorizontalAlignment(Element.ALIGN_RIGHT);
                    } else {
                        cell.setHorizontalAlignment(Element.ALIGN_LEFT);
                    }
                    
                    table.addCell(cell);
                }
                // Sumamos al acumulador
                sumaTotal += item.getMontoTotal();
            }

            document.add(table);

            // 5. Mostrar Total
            Paragraph pTotal = new Paragraph("Total Generado: S/." + String.format("%.2f", sumaTotal), F_TOTAL);
            pTotal.setAlignment(Element.ALIGN_RIGHT);
            pTotal.setSpacingBefore(10);
            document.add(pTotal);
            
            document.close();
            return baos.toByteArray();

        } catch (Exception e) {
            e.printStackTrace();
            return new byte[0];
        }
    }

    private void agregarEncabezadoGeneral(Document doc, FiltroReporte filtro) throws DocumentException {
        Paragraph titulo = new Paragraph("Reporte del Sistema", new Font(Font.HELVETICA, 18, Font.BOLD));
        titulo.setAlignment(Element.ALIGN_CENTER);
        doc.add(titulo);
        doc.add(new Paragraph("\n")); // Espacio vacio
    }
}
