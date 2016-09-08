Public Class ProjectBasicInformation
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Property GenerateDataClass As Boolean

    Dim _ProjectName As String

    Public Property ProjectName As String
        Get
            Return _ProjectName
        End Get
        Set
            _ProjectName = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ProjectName)))
        End Set
    End Property


    Dim _ProjectDirectory As String
    Public Property ProjectDirectory As String
        Get
            Return _ProjectDirectory
        End Get
        Set
            _ProjectDirectory = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ProjectDirectory)))
        End Set
    End Property

End Class