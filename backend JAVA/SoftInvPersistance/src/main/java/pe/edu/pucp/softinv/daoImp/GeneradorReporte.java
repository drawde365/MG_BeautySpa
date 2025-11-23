/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package pe.edu.pucp.softinv.daoImp;
import com.lowagie.text.*;
import com.lowagie.text.pdf.*;
import com.lowagie.text.pdf.BaseFont;
import java.awt.Color;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.NumberFormat;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Locale;
import com.lowagie.text.Document;
import com.lowagie.text.pdf.draw.LineSeparator;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.time.LocalDate;
import pe.edu.pucp.softinv.dao.Reportable;
import pe.edu.pucp.softinv.daoImp.util.FondoOlasEvent;
import pe.edu.pucp.sotfinv.model.Reportes.FiltroReporte;
/**
 *
 * @author Usuario
 */
public class GeneradorReporte {
    
    private Font fontTitulo;
    private Font fontSubtitulo;
    private Font fontCampoTabla;
    private Font fontCuerpo;
    private Font fontTotal;
    
    public GeneradorReporte() {
        try {
            // 1. Obtener la ruta absoluta real del archivo en el sistema compilado
            String rutaFuenteTitulo = getRutaRecurso("Fonts/ZCOOLXiaoWei-Regular.ttf");
            String rutaFuenteGeneral = getRutaRecurso("Fonts/PlusJakartaSans.ttf");

            // 2. Definir FontFactory (usando la ruta resuelta)
            // Si ruta es null, usamos Helvetica por defecto para no romper el programa
            
            if (rutaFuenteTitulo != null) {
                fontTitulo = FontFactory.getFont(rutaFuenteTitulo, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 30, Font.NORMAL, Color.WHITE);
            } else {
                System.err.println("ADVERTENCIA: No se encontró fuente ZCOOL. Usando defecto.");
                fontTitulo = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 18, Color.WHITE);
            }

            if (rutaFuenteGeneral != null) {
                fontSubtitulo = FontFactory.getFont(rutaFuenteGeneral, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL, Color.BLACK);
                fontCuerpo = FontFactory.getFont(rutaFuenteGeneral, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 9, Font.NORMAL, Color.BLACK);
                fontTotal = FontFactory.getFont(rutaFuenteGeneral, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 11, Font.BOLD, Color.BLACK);
                fontCampoTabla = FontFactory.getFont(rutaFuenteGeneral, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 9, Font.NORMAL, Color.WHITE);
            } else {
                System.err.println("ADVERTENCIA: No se encontró fuente Jakarta. Usando defecto.");
                fontSubtitulo = FontFactory.getFont(FontFactory.HELVETICA, 12, Color.GRAY);
                fontCuerpo = FontFactory.getFont(FontFactory.HELVETICA, 12, Color.BLACK);
                fontTotal = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 12, Color.BLACK);
            }
            
        } catch (Exception e) {
            e.printStackTrace();
            fontTitulo = FontFactory.getFont(FontFactory.HELVETICA, 18);
            fontCuerpo = FontFactory.getFont(FontFactory.HELVETICA, 10);
            fontSubtitulo = FontFactory.getFont(FontFactory.HELVETICA, 10);
            fontTotal = FontFactory.getFont(FontFactory.HELVETICA, 10);
        }
    }

    
    private String getRutaRecurso(String rutaRelativa) {
        try {
            java.net.URL url = getClass().getClassLoader().getResource(rutaRelativa);
            if (url != null) {
                // Convertir URL a ruta de archivo
                return java.net.URLDecoder.decode(url.getPath(), "UTF-8");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    public <T> byte[] generarReporte(List<T> datos, FiltroReporte filtro, Reportable<T> adaptador) {
        
        if (datos == null || datos.isEmpty()) {
            System.out.println("La lista está vacía, no se genera PDF.");
            return new byte[0];
        }
        
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        Document document = new Document(PageSize.A4); 
        try {
            PdfWriter writer = PdfWriter.getInstance(document,baos);
            
            FondoOlasEvent eventoFondo = new FondoOlasEvent();
            writer.setPageEvent(eventoFondo);
            
            document.open();

            // 1. Títulos y Encabezado (Igual que antes)
            agregarEncabezadoGeneral(document, filtro, adaptador);

            // 2. CONFIGURACIÓN AUTOMÁTICA DE LA TABLA
            
            float[] anchos = adaptador.getAnchosColumnas();
            String[] titulos = adaptador.getTitulosColumnas();

            PdfPTable table = new PdfPTable(anchos);
            table.setWidthPercentage(100);

            // 3. Pintar Cabeceras
            for (String titulo : titulos) {
                PdfPCell cell = new PdfPCell(new Phrase(titulo, fontCampoTabla));
                cell.setBackgroundColor(new Color(26, 188, 156));
                cell.setHorizontalAlignment(Element.ALIGN_CENTER);
                cell.setPadding(6);
                table.addCell(cell);
            }

            // 4. Pintar Datos y Calcular Total
            double sumaTotal = 0;

            for (T item : datos) {
                adaptador.assign(item);
                String[] valores = adaptador.getDatosFila();
                
                for (String valor : valores) {
                    PdfPCell cell = new PdfPCell(new Phrase(valor, fontCuerpo));
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
                sumaTotal += adaptador.getMontoTotal();
            }

            document.add(table);

            // 5. Mostrar Total
            Paragraph pTotal = new Paragraph("Total: S/." + String.format("%.2f", sumaTotal), fontTotal);
            pTotal.setAlignment(Element.ALIGN_RIGHT);
            pTotal.setSpacingBefore(10);
            document.add(pTotal);
            
            document.close();
            return baos.toByteArray();

        } catch (Exception e) {
            System.err.println("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            System.err.println("ERROR CRÍTICO GENERANDO PDF:");
            System.err.println("Mensaje: " + e.getMessage());
            System.err.println("Causa: " + e.getCause());
            e.printStackTrace();
            System.err.println("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            return new byte[0];
        }
    }
    
    private <T> void agregarEncabezadoGeneral(Document document, FiltroReporte filtro, Reportable<T> adaptador) throws DocumentException, IOException {
        // 1. Crear tabla de 2 columnas (Logo | Texto)
        PdfPTable headerTable = new PdfPTable(new float[]{1, 4}); // El texto ocupa 4 veces más que el logo
        headerTable.setWidthPercentage(100);
    
        try {
            java.net.URL urlLogo = getClass().getClassLoader().getResource("logo.png");
            
            if (urlLogo != null) {
                Image logo = Image.getInstance(urlLogo); // Pasamos la URL, no el String
                logo.scaleToFit(50, 50);
                
                PdfPCell cellLogo = new PdfPCell(logo);
                cellLogo.setBorder(Rectangle.NO_BORDER);
                cellLogo.setVerticalAlignment(Element.ALIGN_MIDDLE);
                headerTable.addCell(cellLogo);
            } else {
                // Si no hay logo, celda vacía para que no falle
                PdfPCell cellVacia = new PdfPCell(new Phrase("Sin Logo"));
                cellVacia.setBorder(Rectangle.NO_BORDER);
                headerTable.addCell(cellVacia);
                System.err.println("ADVERTENCIA: No se encontró logo.png en resources");
            }
        } catch (Exception e) {
            // Capturar error de imagen específicamente para no detener el reporte
            headerTable.addCell(new PdfPCell(new Phrase("Error img")));
            e.printStackTrace();
        }

        // 4. Bloque de Texto (Título + Filtros)
        Paragraph p = new Paragraph();
        String titulo = adaptador.getTitulo();
        p.add(new Phrase(titulo, fontTitulo)); //
        String[] subtitulos = adaptador.getSubtitulos(filtro);
        for(int i = 0; i<3; i++){
            p.add(new Phrase(subtitulos[i],fontSubtitulo));
        }
        String periodo = "Periodo: ";
        if (filtro.getFechaInicio()!= null && filtro.getFechaFin()!=null)
            periodo += "Del" + filtro.getFechaInicio().toString() + " al " + filtro.getFechaFin().toString() + "\n";
        else if (filtro.getFechaInicio()!= null){
            periodo += "A partir del " + filtro.getFechaFin().toString() + "\n";
        }else if (filtro.getFechaFin()!=null)
            periodo += "Hasta el " + filtro.getFechaFin().toString() + "\n";
        else
            periodo += "Completo histórico\n";
        p.add(new Phrase(periodo,fontSubtitulo));
    
        PdfPCell cellTexto = new PdfPCell(p);
        cellTexto.setBorder(Rectangle.NO_BORDER);
        cellTexto.setVerticalAlignment(Element.ALIGN_MIDDLE);
        cellTexto.setHorizontalAlignment(Element.ALIGN_LEFT); // Alinear a la derecha como en tu ejemplo
        headerTable.addCell(cellTexto);

        document.add(headerTable);
    }
}
