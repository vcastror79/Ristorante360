using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Ristorante360Admin.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using iTextSharp.tool.xml;
using System.IO.Pipelines;
using Rectangle = iTextSharp.text.Rectangle;


namespace Ristorante_360_Admin.Controllers
{
    public class PDFController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly RistoranteContext _dbContext; // Agrega tu contexto de base de datos aquí

        public PDFController(IWebHostEnvironment hostingEnvironment, RistoranteContext dbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbContext = dbContext;
        }

        public IActionResult GeneratePDF()

        {

            // Obtener datos de inventario desde la base de datos
             List<Inventory> tuListaDeInventario = ObtenerDatosDeInventario();

           

              // Crear un documento PDF
            Document doc = new Document();

            // Establecer la ubicación para guardar el archivo PDF
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Genera una marca de tiempo
            string pdfFileName = $"inventario_{timestamp}.pdf"; // Agrega la marca de tiempo al nombre del archivo

            string reportsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Reports");

            // Verificar si la carpeta 'Reports' existe, si no, crearla
            if (!Directory.Exists(reportsFolderPath))
            {
                Directory.CreateDirectory(reportsFolderPath);
            }

            string pdfFilePath = Path.Combine(reportsFolderPath, pdfFileName);

            // Crear un escritor PDF
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                        // Agregar eventos de encabezado y pie de página
                        writer.PageEvent = new PdfPageEvents();

                        // Abrir el documento para escribir contenido
                        doc.Open();

            // Agregar título al PDF con imagen utilizando la etiqueta <img>
            Paragraph title = new Paragraph();
            title.Alignment = Element.ALIGN_CENTER;

            // Ruta de la imagen
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "logoChanteCircular.png");

            // Crear una instancia de la clase Image y cargar la imagen desde la ruta
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = Element.ALIGN_CENTER;
            image.ScaleToFit(150f, 150f); // Ajustar tamaño de la imagen

            // Agregar la imagen al párrafo
            title.Add(image);

            // Agregar un salto de línea
            title.Add(Environment.NewLine);

            // Agregar el texto del título
            Chunk chunk = new Chunk("Restaurante Dulce Vida");
            chunk.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f); // Establecer el tamaño y la fuente del texto
            title.Add(chunk);

            doc.Add(title);

            // Agregar espacio después del título
            Paragraph space = new Paragraph(" "); // Párrafo vacío
            space.Font.Size = 10f; // Tamaño de fuente pequeño para el espacio
            doc.Add(space);

            // Continuar con el código para agregar la tabla y el resto del contenido


            // Agregar tabla al PDF
            PdfPTable pdfTable = new PdfPTable(8); // El número 8 representa la cantidad de columnas
            pdfTable.WidthPercentage = 100;

            // Agregar encabezados de columna a la tabla
            foreach (var columnHeader in new string[]
            {
                "Nombre del insumo", "Cantidad", "Unidad", "Cantidad mínima",
                "Lote", "Fecha de ingreso", "Fecha de expiración", "Estado"
            })
            {
                PdfPCell cell = new PdfPCell(new Phrase(columnHeader));
                cell.BackgroundColor = new BaseColor(0, 119, 204); // Color de fondo del encabezado
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);
            }

            // Agregar filas y celdas a la tabla
            foreach (var inventory in tuListaDeInventario)
            {
                pdfTable.AddCell(inventory.oSupplies.Description);
                pdfTable.AddCell(inventory.Amount.ToString());
                pdfTable.AddCell(inventory.oUnitType.Unit);
                pdfTable.AddCell(inventory.MinimumAmount.ToString());
                pdfTable.AddCell(inventory.Lote.ToString()); // Cambiado de int a string
                pdfTable.AddCell(inventory.AdmissionDate.ToString("dd/MM/yyyy"));
                pdfTable.AddCell(inventory.ExpirationDate.ToString("dd/MM/yyyy"));
                pdfTable.AddCell(inventory.Status ? "Disponible" : "Agotado");
                //pdfTable.AddCell(""); // Celda de acción, puedes personalizar esto según tus necesidades
            }

            doc.Add(pdfTable);

            // Cerrar el documento
            doc.Close();

            // Devolver el archivo PDF como descarga
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            return File(pdfBytes, "application/pdf", pdfFileName);
        }

        // Función para obtener los datos de inventario desde la base de datos
        private List<Inventory> ObtenerDatosDeInventario()
        {
            return _dbContext.Inventories
                .Include(i => i.oSupplies)
                .Include(u => u.oUnitType)
                .ToList();
        }
    }
    //Personalizar el encabezado
    public class PDFHeaderFooter : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Crear el contenido del encabezado HTML
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<div style='text-align:center; font-size:12px;'>");
            htmlContent.Append("<strong>Encabezado HTML personalizado</strong><br>");
            htmlContent.Append("Fecha de impresión: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            htmlContent.Append("</div>");

            // Convertir el HTML a elementos PDF
            using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent.ToString())))
            {
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlStream, Encoding.UTF8);
            }
        }
    }

    // Clase para definir los eventos de encabezado y pie de página
    public class PdfPageEvents : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Agregar encabezado al PDF
            string headerText = "Reporte de inventario de insumos descargado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            PdfPTable headerTable = new PdfPTable(1);
            PdfPCell headerCell = new PdfPCell(new Phrase(headerText));
            headerCell.Border = Rectangle.NO_BORDER;
            headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerTable.AddCell(headerCell);

            // Agregar espacio entre el encabezado y la tabla
            headerTable.AddCell(new PdfPCell(new Phrase(""))); // Esta celda vacía crea el espacio
          

            headerTable.TotalWidth = document.Right - document.Left;
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.GetTop(document.TopMargin) + 10, writer.DirectContent);


            // Agregar número de página al pie de página
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            footerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.AddCell(new Phrase("Página " + writer.PageNumber));
            footerTable.TotalWidth = document.Right - document.Left;
            footerTable.WriteSelectedRows(0, -1, document.Left, document.Bottom, writer.DirectContent);
        }
    }

}
