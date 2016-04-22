Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Nukepayload2.Ra2CodeAnalysis.Imaging

Namespace AnalysisHelper
    Public Module Extensions
        <Extension>
        Public Function GetText(Root As IEnumerable(Of Imaging.MainKeyTreeNode), Generator As Type) As String
            Dim sb As New StringBuilder(";<生成的代码(生成器:=" + If(Generator?.FullName, "null") + ",版本:=" + If(Generator.Assembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version, "null").ToString + ")>" & vbCrLf)
            For Each s In Root
                sb.AppendLine(s.ToString)
            Next
            Return sb.ToString
        End Function
        <Extension>
        Public Function IsComment(Text As String, Index As Integer) As Boolean
            Dim StartPos As Integer = Index
            If String.IsNullOrEmpty(Text) Then
                Return True
            End If
            If StartPos > Text.Length Then
                Throw New ArgumentOutOfRangeException("Position", "位置位于字符串外侧")
            End If
            If StartPos = 0 Then
                Return False
            Else
                Do Until StartPos = 0
                    StartPos -= 1
                    Dim ch = Text.Chars(StartPos)
                    If ch = ";"c Then
                        Return True
                    Else
                        If ch = vbCr OrElse ch = vbLf Then
                            Exit Do
                        End If
                    End If
                Loop
                Return False
            End If
        End Function
        <Extension>
        Public Function FindOne(Of T As IniTreeNode)(RT As IEnumerable(Of T), ObjName As String) As T
            For Each mk In RT
                If mk.Text = ObjName Then Return mk
            Next
            Return Nothing
        End Function
        <Extension>
        Public Function SelectLine(Text As String, index As Integer) As String
            If index = -1 Then
                Return String.Empty
            End If
            Dim StartPos As Integer = index
            If String.IsNullOrEmpty(Text) Then
                Return String.Empty
            End If
            If StartPos > Text.Length Then
                Throw New ArgumentOutOfRangeException("Position", "位置位于字符串外侧")
            End If
            Dim wrd As New List(Of Char)
            Do Until StartPos = 0
                StartPos -= 1
                Dim ch = Text.Chars(StartPos)
                If ch <> vbCr AndAlso ch <> vbLf Then
                    wrd.Add(ch)
                Else
                    Exit Do
                End If
            Loop
            wrd.Reverse()
            Dim LeftPart As New String(wrd.ToArray)
            Dim RightPart As New StringBuilder
            Do Until index >= Text.Length
                Dim ch = Text.Chars(index)
                If ch <> vbCr AndAlso ch <> vbLf Then
                    RightPart.Append(ch)
                Else
                    Exit Do
                End If
                index += 1
            Loop
            Return LeftPart + RightPart.ToString
        End Function
        <Extension>
        Public Function IsValidValueChar(Text As Char) As Boolean
            Return Not {ChrW(10), ChrW(13), "="c, ";"c}.Contains(Text)
        End Function
        <Extension>
        Public Function IsValidSingleWordValueChar(Text As Char) As Boolean
            Return Char.IsLetterOrDigit(Text) OrElse Text = "{"c OrElse Text = "}"c
        End Function
        <Extension>
        Public Function IsValidTemplatedWordValueChar(Text As Char) As Boolean
            Return Char.IsLetterOrDigit(Text) OrElse Text = "<"c OrElse Text = ">"c OrElse Text = "_"c OrElse Text = "-"c
        End Function
        <Extension>
        Public Function SelectWord(Text As String, index As Integer, Optional Filter As Func(Of Char, Boolean) = Nothing) As String
            If Filter Is Nothing Then
                Filter = AddressOf IsValidValueChar
            End If
            Dim StartPos As Integer = index
            If String.IsNullOrEmpty(Text) Then
                Return String.Empty
            End If
            If StartPos > Text.Length Then
                Throw New ArgumentOutOfRangeException("Position", "位置位于字符串外侧")
            End If
            If StartPos = 0 Then
                Return String.Empty
            Else
                Dim wrd As New List(Of Char)
                Dim Terminating = False
                Do Until StartPos = 0
                    StartPos -= 1
                    Dim ch = Text.Chars(StartPos)
                    If Filter(ch) AndAlso Not Terminating Then
                        If ch = "<" Then Terminating = True
                        wrd.Add(ch)
                    Else
                        Exit Do
                    End If
                Loop
                Terminating = False
                wrd.Reverse()
                Dim LeftPart As New String(wrd.ToArray)
                Dim RightPart As New StringBuilder
                Do Until index >= Text.Length
                    Dim ch = Text.Chars(index)
                    If Filter(ch) AndAlso Not Terminating Then
                        If ch = ">" Then Terminating = True
                        RightPart.Append(ch)
                    Else
                        Exit Do
                    End If
                    index += 1
                Loop
                Return LeftPart.Trim + RightPart.ToString.Trim
            End If
        End Function
        ''' <summary>
        ''' 判断是否是\w,_,%和.
        ''' </summary>
        ''' <param name="ch"></param>
        ''' <returns></returns>
        <Extension>
        Function IsRegisterableChar(ch As Char) As Boolean
            Return New Regex("(\w|_|\.|%|-)").IsMatch(ch)
        End Function
        <Extension>
        Function BitToInt64(n As ULong) As Long
            Return BitConverter.ToInt64(BitConverter.GetBytes(n), 0)
        End Function
        <Extension>
        Function BitToUInt64(n As Long) As ULong
            Return BitConverter.ToUInt64(BitConverter.GetBytes(n), 0)
        End Function
        <Extension>
        Function BitToInt32(n As UInteger) As Integer
            Return BitConverter.ToInt32(BitConverter.GetBytes(n), 0)
        End Function
        <Extension>
        Function BitToUInt32(n As Integer) As UInteger
            Return BitConverter.ToUInt32(BitConverter.GetBytes(n), 0)
        End Function
        <Extension>
        Function CountOf(Of T)(txt As IEnumerable(Of T), chr As T) As Integer
            Return Aggregate co In From ch In txt Where ch.Equals(chr) Into Count
        End Function
        <Extension>
        Function CountOf(txt As String, chr As Char) As Integer
            Return Aggregate co In From ch In txt.ToCharArray Where ch = chr Into Count
        End Function
        <Extension>
        Function IsNumeric(t As Char) As Boolean
            Return t >= "0"c AndAlso t <= "9"c
        End Function
        <Extension>
        Function IsInteger(txt As String) As Boolean
            If txt.StartsWith("-") OrElse txt.StartsWith("+") Then txt = txt.Substring(1)
            If txt.Length = 0 Then Return False
            Return txt.Length = Aggregate c In From t In txt.ToCharArray Take While t >= "0"c AndAlso t <= "9"c Into Count
        End Function
        <Extension>
        Function IsFraction(txt As String) As Boolean
            If txt.StartsWith("-") OrElse txt.StartsWith("+") Then txt = txt.Substring(1)
            If txt.CountOf("."c) <> 1 Then Return False
            Return txt.Length = Aggregate c In From t In txt.ToCharArray Take While t >= "0"c AndAlso t <= "9"c OrElse t = "." Into Count
        End Function
        <Extension>
        Function IsUInteger(txt As String) As Boolean
            If txt.Length = 0 Then Return False
            Return txt.Length = Aggregate c In From t In txt.ToCharArray Take While t >= "0"c AndAlso t <= "9"c Into Count
        End Function
        <Extension>
        Function IsUFraction(txt As String) As Boolean
            If txt.CountOf("."c) <> 1 Then Return False
            Return txt.Length = Aggregate c In From t In txt.ToCharArray Take While t >= "0"c AndAlso t <= "9"c OrElse t = "." Into Count
        End Function
        <Extension>
        Function IsNumeric(txt As String) As Boolean
            If txt.StartsWith("-") OrElse txt.StartsWith("+") Then txt = txt.Substring(1)
            If txt.Length = 0 Then Return False
            Return txt.Length = Aggregate c In From t In txt.ToCharArray Take While t >= "0"c AndAlso t <= "9"c OrElse t = "." Into Count
        End Function
        <Extension>
        Function ContainsEachTrim(Main As IEnumerable(Of String), Second As IEnumerable(Of String)) As Boolean
            For Each s In Second
                If Main.Contains(s.Trim) Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Module
End Namespace
