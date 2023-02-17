using DevExpress.Pdf;
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
                documentProcessor.LoadDocument(@"..\..\Document.pdf");

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
                        HighlightResultUsingAnnotations(documentProcessor, result);
                    }
                }
                //Save the document
                documentProcessor.SaveDocument(@"..\..\Result.pdf");
            }
        }

        //This method uses PdfGraphics to highlight text
        static void HighlightResultWithGraphics(PdfDocumentProcessor processor, PdfTextSearchResults result)
        {
            using (PdfGraphics graphics = processor.CreateGraphics()) 
            {
                for (int i = 0; i < result.Rectangles.Count; i++)
                {
                    RectangleF rect = new RectangleF(new PointF((float)result.Rectangles[i].Left, (float)result.Page.CropBox.Height - (float)result.Rectangles[i].Top),
                        new SizeF((float)result.Rectangles[i].Width, (float)result.Rectangles[i].Height));
                    using (var brush = new SolidBrush(Color.FromArgb(130, 55, 155, 255)))
                        graphics.FillRectangle(brush, rect);
                }
                graphics.AddToPageForeground(result.Page, 72, 72);
            }
        }

        //This method uses annotations to highlight text
        static void HighlightResultUsingAnnotations(PdfDocumentProcessor processor, PdfTextSearchResults result)
        {
            for (int i = 0; i < result.Rectangles.Count; i++)
            {
                PdfTextMarkupAnnotationData annotation =
                processor.AddTextMarkupAnnotation(result.PageNumber, result.Rectangles[i], PdfTextMarkupAnnotationType.Highlight);
                if (annotation != null)
                {
                    annotation.Color = new PdfRGBColor(0.2, 0.6, 0);
                }
            }
        }
    }
}


