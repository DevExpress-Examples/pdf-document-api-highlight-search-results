Imports DevExpress.Pdf
Imports System.Drawing

Namespace HighlightSearchResults

    Friend Class Program

        Shared Sub Main(ByVal args As String())
            ' Create a PDF document processor.
            Using documentProcessor As PdfDocumentProcessor = New PdfDocumentProcessor()
                ' Define search words
                Dim words As String() = {"Get", "DX-RX809", "HD", "DX-B5000"}
                ' Load a PDF document
                documentProcessor.LoadDocument("..\..\Document.pdf")
                ' Specify search parameters
                Dim searchParameters As PdfTextSearchParameters = New PdfTextSearchParameters()
                searchParameters.CaseSensitive = True
                searchParameters.WholeWords = True
                ' Comment the following "using" statement if you use annotations
                Using brush = New SolidBrush(Color.FromArgb(130, 55, 155, 255))
                    For Each word As String In words
                        ' Get the search results from the FindText method call
                        ' with search text and search parameters
                        Dim result As PdfTextSearchResults = documentProcessor.FindText(word, searchParameters)
                        ' Highlight the result
                        While result.Status = PdfTextSearchStatus.Found
                            Using graphics As PdfGraphics = documentProcessor.CreateGraphics()
                                HighlightResult(graphics, result, brush)
                            End Using

                            ' Use this method call to add annotations:
                            ' HighlightResult(documentProcessor, result);
                            result = documentProcessor.FindText(word, searchParameters)
                        End While
                    Next
                End Using

                ' Save the document
                documentProcessor.SaveDocument("..\..\Result.pdf")
            End Using
        End Sub

        ' This method uses PdfGraphics to highlight text
        Public Shared Sub HighlightResult(ByVal graphics As PdfGraphics, ByVal result As PdfTextSearchResults, ByVal brush As SolidBrush)
            For i As Integer = 0 To result.Rectangles.Count - 1
                Dim rect As RectangleF = New RectangleF(New PointF(CSng(result.Rectangles(i).Left), CSng(result.Page.CropBox.Height) - CSng(result.Rectangles(i).Top)), New SizeF(CSng(result.Rectangles(i).Width), CSng(result.Rectangles(i).Height)))
                graphics.FillRectangle(brush, rect)
            Next

            graphics.AddToPageForeground(result.Page, 72, 72)
        End Sub

        ' This method uses annotations to highlight text
        Public Shared Sub HighlightResult(ByVal processor As PdfDocumentProcessor, ByVal result As PdfTextSearchResults)
            For i As Integer = 0 To result.Rectangles.Count - 1
                Dim annotation As PdfTextMarkupAnnotationData = processor.AddTextMarkupAnnotation(result.PageNumber, result.Rectangles(i), PdfTextMarkupAnnotationType.Highlight)
                If annotation IsNot Nothing Then
                    annotation.Color = New PdfRGBColor(0.2, 0.6, 0)
                End If
            Next
        End Sub
    End Class
End Namespace
