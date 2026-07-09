using DevExpress.Drawing;
using DevExpress.Pdf;
using System.Diagnostics;
using System.Drawing;

namespace HighlightSearchResults
{
    class Program
    {

        static void Main(string[] args)
        {
            //Create a PDF document processor.
            using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
            {
                //Define search words
                string[] words = { "Get", "DX-RX809", "HD", "DX-B5000" };

                //Load a PDF document
                documentProcessor.LoadDocument(@"..\..\..\Document.pdf");

                //Specify the search parameters
                PdfTextSearchParameters searchParameters = new PdfTextSearchParameters();
                searchParameters.CaseSensitive = true;
                searchParameters.WholeWords = true;

                foreach (string word in words)
                {
                    PdfTextSearchResults result;
                    //Get the search results from the FindText method call with search text and search parameters
                    while ((result = documentProcessor.FindText(word, searchParameters)).Status == PdfTextSearchStatus.Found)
                    {
                        //HighlightResultWithGraphics(documentProcessor, result);
                        HighlightResultWithAnnotations(documentProcessor, result);
                    }
                }
                //Save the document
                documentProcessor.SaveDocument(@"..\..\..\Result.pdf");
                Process.Start(new ProcessStartInfo(@"..\..\..\Result.pdf") { UseShellExecute = true });
            }
        }

        //This method uses PdfGraphics to highlight text
        static void HighlightResultWithGraphics(PdfDocumentProcessor processor, PdfTextSearchResults result)
        {
            using var graphics = processor.CreateGraphicsPageSystem();
            using var brush = new DXSolidBrush(Color.FromArgb(130, 55, 155, 255));
            foreach (var rect in result.Rectangles) {
                var fillRectangle = new RectangleF((float)rect.Left,
                    (float)rect.Top - (float)rect.Height,
                    (float)rect.Width,
                    (float)rect.Height);
                graphics.FillRectangle(brush, fillRectangle);
            }
            graphics.AddToPageForeground(result.Page);
        }

        //This method uses annotations to highlight text
        static void HighlightResultWithAnnotations(PdfDocumentProcessor processor, PdfTextSearchResults result)
        {
            PdfDocumentFacade facade = processor.DocumentFacade;
            PdfPageFacade page = facade.Pages[result.Page.GetPageIndex()];

            for (int i = 0; i < result.Rectangles.Count; i++)
            {
                PdfTextMarkupAnnotationFacade annotation =
                          page.AddTextMarkupAnnotation(result.Rectangles[i], PdfTextMarkupAnnotationType.Highlight);
                if (annotation != null)
                {
                    annotation.Color = new PdfRGBColor(0.2, 0.6, 0);
                }
            }
        }
    }
}
