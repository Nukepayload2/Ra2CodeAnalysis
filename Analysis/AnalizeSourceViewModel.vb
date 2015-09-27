Public Class AnalizeSourceViewModel
    Public Event DataChanged(DocumentCategory$, NewData$)
    Dim _RulesText$ = "", _ArtText$ = "", _AIText$ = "", _Ra2Text$ = ""
    Public Property RulesText$
        Get
            Return _RulesText
        End Get
        Set
            _RulesText = Value.Replace(vbCr, "").Replace(vbLf, vbCrLf)
            IsRulesInvalid = True
            RaiseEvent DataChanged("Rules", Value)
        End Set
    End Property
    Public Property Ra2Text$
        Get
            Return _Ra2Text
        End Get
        Set
            _Ra2Text = Value.Replace(vbCr, "").Replace(vbLf, vbCrLf)
            IsRa2Invalid = True
            RaiseEvent DataChanged("Ra2", Value)
        End Set
    End Property
    Public Property ArtText$
        Get
            Return _ArtText
        End Get
        Set
            _ArtText = Value.Replace(vbCr, "").Replace(vbLf, vbCrLf)
            IsArtInvalid = True
            RaiseEvent DataChanged("Art", Value)
        End Set
    End Property
    Public Property AIText$
        Get
            Return _AIText
        End Get
        Set
            _AIText = Value.Replace(vbCr, "").Replace(vbLf, vbCrLf)
            IsAIInvalid = True
            RaiseEvent DataChanged("AI", Value)
        End Set
    End Property
    Public Property IsRulesInvalid As Boolean = True
    Public Property IsArtInvalid As Boolean = True
    Public Property IsAIInvalid As Boolean = True
    Public Property IsRa2Invalid As Boolean = True
    Sub New()

    End Sub
    Sub New(RulesText$, ArtText$, AIText$, Ra2Text$)
        Me.RulesText = RulesText
        Me.Ra2Text = Ra2Text
        Me.ArtText = ArtText
        Me.AIText = AIText
    End Sub
End Class
