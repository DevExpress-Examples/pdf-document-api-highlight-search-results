Imports DevExpress.Pdf
Imports System.Drawing

Namespace HighlightSearchResults

    Friend Class Program

        Shared Sub Main(ByVal args As String())
            'Create a PDF document processor.
            Using documentProcessor As PdfDocumentProcessor = New PdfDocumentProcessor()
                'Define search words
                Dim words As String() = {"Get", "DX-RX809", "HD", "DX-B5000"}
                'Load a PDF document
                documentProcessor.LoadDocument("..\..\Document.pdf")
                'Specify the search parameters
                Dim searchParameters As PdfTextSearchParameters = New PdfTextSearchParameters()
                searchParameters.CaseSensitive = True
                searchParameters.WholeWords = True
                For Each word As String In words
                    Dim result As PdfTextSearchResults
                    'Get the search results from the FindText method call with search text and search parameters
                    While(CSharpImpl.__Assign(result, documentProcessor.FindText(word, searchParameters))).Status = PdfTextSearchStatus.Found
                        'HighlightResultWithGraphics(documentProcessor, result);
                        HighlightResultUsingAnnotations(documentProcessor, result)
                    End While
                Next

                'Save the document
                documentProcessor.SaveDocument("..\..\Result.pdf")
            End Using
        End Sub

        'This method uses PdfGraphics to highlight text
        Private Shared Sub HighlightResultWithGraphics(ByVal processor As PdfDocumentProcessor, ByVal result As PdfTextSearchResults)
            Using graphics As PdfGraphics = processor.CreateGraphics()
                For i As Integer = 0 To result.Rectangles.Count - 1
                    Dim rect As RectangleF = New RectangleF(New PointF(CSng(result.Rectangles(i).Left), CSng(result.Page.CropBox.Height) - CSng(result.Rectangles(i).Top)), New SizeF(CSng(result.Rectangles(i).Width), CSng(result.Rectangles(i).Height)))
                    Using brush = New SolidBrush(Color.FromArgb(130, 55, 155, 255))
                        graphics.FillRectangle(brush, rect)
                    End Using
                Next

                graphics.AddToPageForeground(result.Page, 72, 72)
            End Using
        End Sub

        'This method uses annotations to highlight text
        Private Shared Sub HighlightResultUsingAnnotations(ByVal processor As PdfDocumentProcessor, ByVal result As PdfTextSearchResults)
            For i As Integer = 0 To result.Rectangles.Count - 1
                Dim annotation As PdfTextMarkupAnnotationData = processor.AddTextMarkupAnnotation(result.PageNumber, result.Rectangles(i), PdfTextMarkupAnnotationType.Highlight)
                If annotation IsNot Nothing Then
                    annotation.Color = New PdfRGBColor(0.2, 0.6, 0)
                End If
            Next
        End Sub

        Private Class CSharpImpl

            <System.Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
