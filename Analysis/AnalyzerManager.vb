Imports System.Threading

Public Class AnalyzerManager
    WithEvents DataSource As AnalizeSourceViewModel
    Public ReadOnly Property Rules As RulesAnalyzer
    Public ReadOnly Property Art As ArtAnalyzer
    Public ReadOnly Property AI As AIAnalyzer
    Public ReadOnly Property Ra2 As INIAnalyzer
    Public ReadOnly Property Updating As Boolean
    Dim _UpdateDelay As New TimeSpan(0, 0, 1)
    Public Property UpdateDelay As TimeSpan
        Get
            Return _UpdateDelay
        End Get
        Set(value As TimeSpan)
            _UpdateDelay = value
            DelayTimer.Change(1, DelayTime)
            DelayTimer.Change(-1, DelayTime)
        End Set
    End Property
    Private ReadOnly Property DelayTime As Integer
        Get
            Return CInt(_UpdateDelay.TotalMilliseconds)
        End Get
    End Property
    Dim Update As TimerCallback = Async Sub(state) Await UpdateNowAsync()
    Dim DelayTimer As New Timer(Update, Nothing, Timeout.Infinite, DelayTime)
    Public Sub DelayedUpdateAnalizeDataRequest()
        DelayTimer.Change(DelayTime, DelayTime)
    End Sub
    ''' <summary>
    ''' 强制检查分析结果的更新。如果正在更新则会不进行更新并返回False。
    ''' </summary>
    ''' <returns>是否成功更新(没遇到正在更新的情况)</returns>
    Public Async Function UpdateNowAsync() As Task(Of Boolean)
        If _Updating Then
            Return False
        Else
            _Updating = True
            Await Task.WhenAll(EnsureRulesUpdatedAsync(), EnsureArtUpdatedAsync(), EnsureAIUpdatedAsync(), EnsureRa2UpdatedAsync())
            UpdateDelay = UpdateDelay
            _Updating = False
            Return True
        End If
    End Function
    Sub New(DataSource As AnalizeSourceViewModel)
        With DataSource
            Rules = New RulesAnalyzer(.RulesText)
            Art = New ArtAnalyzer(.ArtText, Rules)
            AI = New AIAnalyzer(.AIText, Rules)
            Ra2 = New INIAnalyzer(.Ra2Text)
        End With
        Me.DataSource = DataSource
    End Sub
    Public Async Function EnsureRulesUpdatedAsync() As Task
        If DataSource.IsRulesInvalid Then
            Await Rules.ReloadAsync(DataSource.RulesText)
            DataSource.IsRulesInvalid = False
        End If
    End Function
    Public Async Function EnsureArtUpdatedAsync() As Task
        If DataSource.IsArtInvalid Then
            Await Art.ReloadAsync(DataSource.ArtText)
            DataSource.IsArtInvalid = False
        End If
    End Function
    Public Async Function EnsureAIUpdatedAsync() As Task
        If DataSource.IsAIInvalid Then
            Await AI.ReloadAsync(DataSource.AIText)
            DataSource.IsAIInvalid = False
        End If
    End Function
    Public Async Function EnsureRa2UpdatedAsync() As Task
        If DataSource.IsRa2Invalid Then
            Await Ra2.ReloadAsync(DataSource.Ra2Text)
            DataSource.IsRa2Invalid = False
        End If
    End Function

    Private Sub DataSource_DataChanged() Handles DataSource.DataChanged
        DelayedUpdateAnalizeDataRequest()
    End Sub
End Class