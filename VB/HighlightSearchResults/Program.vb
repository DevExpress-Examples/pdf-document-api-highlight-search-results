Imports DevExpress.Drawing
Imports DevExpress.Pdf
Imports System.Diagnostics
Imports System.Drawing

Namespace HighlightSearchResults
    Class Program
        Shared Sub Main(ByVal args As String())
            'Create a PDF document processor.
            Using documentProcessor As New PdfDocumentProcessor()
                'Define search words
                Dim words As String() = {"Get", "DX-RX809", "HD", "DX-B5000"}

                'Load a PDF document
                documentProcessor.LoadDocument("..\..\..\Document.pdf")

                'Specify the search parameters
                Dim searchParameters As New PdfTextSearchParameters()
                searchParameters.CaseSensitive = True
                searchParameters.WholeWords = True

                For Each word As String In words
                    Dim result As PdfTextSearchResults
                    'Get the search results from the FindText method call with search text and search parameters
                    Do While InlineAssignHelper(result, documentProcessor.FindText(word, searchParameters)).Status = PdfTextSearchStatus.Found
                        'HighlightResultWithGraphics(documentProcessor, result)
                        HighlightResultWithAnnotations(documentProcessor, result)
                    Loop
                Next

                'Save the document
                documentProcessor.SaveDocument("..\..\..\Result.pdf")
                Process.Start(New ProcessStartInfo("..\..\..\Result.pdf") With {.UseShellExecute = True})
            End Using
        End Sub

        'This method uses PdfGraphics to highlight text
        Private Shared Sub HighlightResultWithGraphics(ByVal processor As PdfDocumentProcessor, ByVal result As PdfTextSearchResults)
            Using graphics As PdfGraphics = processor.CreateGraphicsPageSystem()
                For i As Integer = 0 To result.Rectangles.Count - 1
                    Dim rect As New RectangleF(New PointF(CSng(result.Rectangles(i).Left), CSng(result.Rectangles(i).Top - result.Rectangles(i).Height)),
                        New SizeF(CSng(result.Rectangles(i).Width), CSng(result.Rectangles(i).Height)))
                    Using brush = New DXSolidBrush(Color.FromArgb(130, 55, 155, 255))
                        graphics.FillRectangle(brush, rect)
                    End Using
                Next
                graphics.AddToPageForeground(result.Page)
            End Using
        End Sub

        'This method uses annotations to highlight text
        Private Shared Sub HighlightResultWithAnnotations(ByVal processor As PdfDocumentProcessor, ByVal result As PdfTextSearchResults)
            Dim facade As PdfDocumentFacade = processor.DocumentFacade
            Dim page As PdfPageFacade = facade.Pages(result.Page.GetPageIndex())

            For i As Integer = 0 To result.Rectangles.Count - 1
                Dim annotation As PdfTextMarkupAnnotationFacade = page.AddTextMarkupAnnotation(result.Rectangles(i), PdfTextMarkupAnnotationType.Highlight)
                If annotation IsNot Nothing Then
                    annotation.Color = New PdfRGBColor(0.2, 0.6, 0)
                End If
            Next
        End Sub

        'Helper function to assign and return value in Do While loop
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace
