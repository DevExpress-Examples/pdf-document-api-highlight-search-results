using DevExpress.Pdf;
using System.Drawing;

namespace HighlightSearchResults {
    class Program {

        static void Main(string[] args) {
            // Create a PDF document processor.
            using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor()) {

                // Define search words.
                string[] words = { "Get", "DX-RX809", "HD", "DX-B5000" };

                // Load a PDF document. 
                documentProcessor.LoadDocument(@"..\..\Document.pdf");

                // Specify the search parameters.
                PdfTextSearchParameters searchParameters = new PdfTextSearchParameters();
                searchParameters.CaseSensitive = true;
                searchParameters.WholeWords = true;

                foreach (string word in words) {
                    // Get the search results from the FindText method called with search text and search parameters.
                    PdfTextSearchResults result = documentProcessor.FindText(word, searchParameters);

                    // If the desired text is found, create a rectangle that corresponds to the area containing the found text and fill the rectangle.
                    while (result.Status == PdfTextSearchStatus.Found) {
                        using (PdfGraphics graphics = documentProcessor.CreateGraphics()) {
                            HighlightFoundText(graphics, result, new SolidBrush(Color.FromArgb(130, 55, 155, 255)));
                        }
                        result = documentProcessor.FindText(word, searchParameters);
                    }
                }

                // Save the modified document.
                documentProcessor.SaveDocument(@"..\..\Result.pdf");
            }
        }

        public static void HighlightFoundText(PdfGraphics graphics, PdfTextSearchResults result, SolidBrush brush) {
            for (int i = 0; i < result.Rectangles.Count; i++) {
                RectangleF rect = new RectangleF(new PointF((float)result.Rectangles[i].Left, (float)result.Page.CropBox.Height - (float)result.Rectangles[i].Top),
                    new SizeF((float)result.Rectangles[i].Width, (float)result.Rectangles[i].Height));

                graphics.FillRectangle(brush, rect);
            }
            graphics.AddToPageForeground(result.Page, 72, 72);
        }
    }
}


