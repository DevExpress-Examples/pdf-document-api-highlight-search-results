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


                //Comment the following "using" statement if you use annotations
                using (var brush = new SolidBrush(Color.FromArgb(130, 55, 155, 255)))
                    foreach (string word in words)
                    {
                        //Get the search results from the FindText method call with search text and search parameters
                        PdfTextSearchResults result = documentProcessor.FindText(word, searchParameters);

                        //Highlight the result
                        while (result.Status == PdfTextSearchStatus.Found)
                        {
                            using (PdfGraphics graphics = documentProcessor.CreateGraphics())
                            {
                                HighlightResult(graphics, result, brush);
                            }
                            //Use this method call to add annotations:
                            //HighlightResult(documentProcessor, result);
                            result = documentProcessor.FindText(word, searchParameters);
                        }
                    }
                //Save the document
                documentProcessor.SaveDocument(@"..\..\Result.pdf");
            }
        }

        //This method uses PdfGraphics to highlight text
        public static void HighlightResult(PdfGraphics graphics, PdfTextSearchResults result, SolidBrush brush)
        {
            for (int i = 0; i < result.Rectangles.Count; i++)
            {
                RectangleF rect = new RectangleF(new PointF((float)result.Rectangles[i].Left, (float)result.Page.CropBox.Height - (float)result.Rectangles[i].Top),
                    new SizeF((float)result.Rectangles[i].Width, (float)result.Rectangles[i].Height));

                graphics.FillRectangle(brush, rect);
            }
            graphics.AddToPageForeground(result.Page, 72, 72);
        }

        //This method uses annotations to highlight text
        public static void HighlightResult(PdfDocumentProcessor processor, PdfTextSearchResults result)
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


