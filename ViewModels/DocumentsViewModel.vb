Namespace Document
    Public MustInherit Class DocumentsViewModel(Of TObservable As {IList(Of IniBlock), New})
        Public ReadOnly Property RulesDocument As IniDocument(Of TObservable)
        Public ReadOnly Property ArtDocument As IniDocument(Of TObservable)
        Public ReadOnly Property AIDocument As IniDocument(Of TObservable)
        Public ReadOnly Property Ra2Document As IniDocument(Of TObservable)
        Public ReadOnly Property Help As New HelpProviderManager
        Public ReadOnly Property HelpUtil As New HelpDataProvider
        WithEvents ViewSource As AnalizeSourceViewModel
        Sub New(ViewSource As AnalizeSourceViewModel)
            Me.ViewSource = ViewSource
            RulesDocument = New IniDocument(Of TObservable)(ViewSource.RulesText)
            ArtDocument = New IniDocument(Of TObservable)(ViewSource.ArtText)
            AIDocument = New IniDocument(Of TObservable)(ViewSource.AIText)
            Ra2Document = New IniDocument(Of TObservable)(ViewSource.Ra2Text)
        End Sub

        Private Sub ViewSource_DataChanged(DocumentCategory As String, NewData As String) Handles ViewSource.DataChanged
            Select Case DocumentCategory
                Case DocumentCategories.Rules
                    RulesDocument.Text = NewData
                Case DocumentCategories.AI
                    AIDocument.Text = NewData
                Case DocumentCategories.Art
                    ArtDocument.Text = NewData
                Case DocumentCategories.Ra2
                    Ra2Document.Text = NewData
            End Select
        End Sub
    End Class
End Namespace
