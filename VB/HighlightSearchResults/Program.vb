Imports DevExpress.Pdf
Imports System.Drawing

Namespace HighlightSearchResults

    Friend Class Program

        Shared Sub Main(ByVal args As String())
            ' Create a PDF document processor.
            Using documentProcessor As PdfDocumentProcessor = New PdfDocumentProcessor()
                ' Define search words.
                Dim words As String() = {"Get", "DX-RX809", "HD", "DX-B5000"}
                ' Load a PDF document. 
                documentProcessor.LoadDocument("..\..\Document.pdf")
                ' Specify the search parameters.
                Dim searchParameters As PdfTextSearchParameters = New PdfTextSearchParameters()
                searchParameters.CaseSensitive = True
                searchParameters.WholeWords = True
                For Each word As String In words
                    ' Get the search results from the FindText method called with search text and search parameters.
                    Dim result As PdfTextSearchResults = documentProcessor.FindText(word, searchParameters)
                    ' If the desired text is found, create a rectangle that corresponds to the area containing the found text and fill the rectangle.
                    While result.Status = PdfTextSearchStatus.Found
                        Using graphics As PdfGraphics = documentProcessor.CreateGraphics()
                            Call HighlightFoundText(graphics, result, New SolidBrush(Color.FromArgb(130, 55, 155, 255)))
                        End Using

                        result = documentProcessor.FindText(word, searchParameters)
                    End While
                Next

                ' Save the modified document.
                documentProcessor.SaveDocument("..\..\Result.pdf")
            End Using
        End Sub

        Public Shared Sub HighlightFoundText(ByVal graphics As PdfGraphics, ByVal result As PdfTextSearchResults, ByVal brush As SolidBrush)
            For i As Integer = 0 To result.Rectangles.Count - 1
                Dim rect As RectangleF = New RectangleF(New PointF(CSng(result.Rectangles(i).Left), CSng(result.Page.CropBox.Height) - CSng(result.Rectangles(i).Top)), New SizeF(CSng(result.Rectangles(i).Width), CSng(result.Rectangles(i).Height)))
                graphics.FillRectangle(brush, rect)
            Next

            graphics.AddToPageForeground(result.Page, 72, 72)
        End Sub
    End Class
End Namespace
