Imports System.Text

Namespace AnalysisHelper
    ''' <summary>
    ''' 用于计算ini中的数值
    ''' </summary>
    Public Class ExpressionCalculator
        Protected Numbers As New Stack(Of Decimal)
        Protected Operators As New Stack(Of String)
        Protected ReadOnly Ops As New Dictionary(Of String, Func(Of Decimal, Decimal, Decimal)) From {
        {"+", Function(a, b) a + b},
        {"-", Function(a, b) a - b},
        {"*", Function(a, b) a * b},
        {"x", Function(a, b) a * b},
        {"/", Function(a, b) a / b},
        {"\", Function(a, b) CLng(a) \ CLng(b)},
        {"÷", Function(a, b) a / b},
        {"^", Function(a, b) CDec(a ^ b)},
        {"mod", Function(a, b) a Mod b},
        {"%", Function(a, b) a Mod b},
        {"not", Function(a, b) Not CLng(b)},
        {"!", Function(a, b) Not CLng(b)},
        {"and", Function(a, b) CLng(a) And CLng(b)},
        {"or", Function(a, b) CLng(a) Or CLng(b)},
        {"xor", Function(a, b) CLng(a) Xor CLng(b)},
        {"<<", Function(a, b) CLng(a) << CInt(b)},
        {">>", Function(a, b) CLng(a) >> CInt(b)},
        {"<<<", Function(a, b) CLng(a) << CInt(b)},
        {">>>", Function(a, b) CLng(a).BitToUInt64 >> CInt(b)},
        {"sal", Function(a, b) CLng(a) << CInt(b)},
        {"shl", Function(a, b) CLng(a) << CInt(b)},
        {"sar", Function(a, b) CLng(a) >> CInt(b)},
        {"shr", Function(a, b) CLng(a).BitToUInt64 >> CInt(b)},
        {"ror", Function(a, b)
                    Dim num As ULong = CLng(a).BitToUInt64, Count As Integer = CInt(b)
                    Count = Count Mod 64
                    Dim lo = num >> Count
                    Dim hi = num << 64 - Count
                    Return BitToInt64(hi Or lo)
                End Function},
        {"rol", Function(a, b)
                    Dim num As ULong = CLng(a).BitToUInt64, Count As Integer = CInt(b)
                    Count = Count Mod 64
                    Dim lo = num >> 64 - Count
                    Dim hi = num << Count
                    Return BitToInt64(hi Or lo)
                End Function}
        }
        Public ReadOnly Property SupportedOperators As ICollection(Of String)
            Get
                Return Ops.Keys
            End Get
        End Property
        Protected Overridable Function CalculateBlock(Num2 As Decimal, Num1 As Decimal, OpCode As String) As Decimal
            If OpCode.Length > 1 Then OpCode = OpCode.ToLowerInvariant
            If Not Ops.ContainsKey(OpCode) Then
                Throw New ArgumentException(String.Format("运算符{0}不支持", OpCode))
            End If
            Return Ops(OpCode).Invoke(Num1, Num2)
        End Function
        Protected Function Seperate(chrs As IEnumerable(Of Char)) As IEnumerable(Of String)
            Dim Expression = New String(chrs.ToArray)
            Dim Result As New List(Of String)
            If chrs.CountOf("("c) <> chrs.CountOf(")"c) Then
                Throw New ArgumentException("表达式中的(的数量不等于)的数量")
            End If
            For pos As Integer = 0 To chrs.Count - 1
                If IsNumeric(chrs(pos)) OrElse (chrs(pos) = "-"c AndAlso (pos = 0 OrElse Not chrs(pos - 1).IsNumeric)) Then
                    Dim LastStr As New StringBuilder
                    Do
                        LastStr.Append(chrs(pos))
                        pos += 1
                    Loop While IsNumeric(chrs(pos)) OrElse chrs(pos) = "."
                    pos -= 1
                    Result.Add(LastStr.ToString)
                Else
                    If chrs(pos) = "(" OrElse chrs(pos) = ")" Then
                        Result.Add(chrs(pos))
                    Else
                        Dim p As Integer = 0
                        Dim pm As Integer
                        Dim LongStr As String = ""
                        Dim ssb As New StringBuilder
                        Do
                            ssb.Append(chrs(pos + p))
                            If Ops.Keys.Contains(ssb.ToString) Then
                                LongStr = ssb.ToString
                                pm = p
                            End If
                            p += 1
                            If pos + p >= chrs.Count - 1 Then
                                Exit Do
                            End If
                        Loop Until IsNumeric(chrs(pos + p))
                        Result.Add(LongStr)
                        pos += pm
                    End If
                End If
            Next
            Return Result
        End Function

        Private Function GetPriority(s As String) As Integer
            Select Case s
                Case "("
                    Return 0
                Case "<<", ">>", "shr", "shl", "rol", "ror", "<<<", ">>>", "sar", "sal"
                    Return 1
                Case "xor", "or", "and"
                    Return 2
                Case "+", "-"
                    Return 3
                Case "mod", "%"
                    Return 4
                Case "\"
                    Return 5
                Case "*", "x", "/", "÷"
                    Return 6
                Case "^"
                    Return 7
                Case ")"
                    Return 8
                Case Else
                    Throw New ArgumentException("不支持的运算符" & s)
            End Select
        End Function
        Public Function Eval(Expression As String) As Decimal
            Numbers.Clear()
            Operators.Clear()
            Dim Seped = Seperate(Expression.Replace("（", "(").Replace("）", ")").Replace(" ", "").ToCharArray)
            For i As Integer = 0 To Seped.Count - 1
                Dim current = Seped(i)
                If current.IsNumeric Then
                    Numbers.Push(CDec(current))
                Else
                    If current <> ")" Then
                        If current <> "(" AndAlso Operators.Count > 0 Then
                            Do While GetPriority(Operators.Peek) >= GetPriority(current)
                                Numbers.Push(CalculateBlock(Numbers.Pop, Numbers.Pop, Operators.Pop))
                                If Operators.Count = 0 Then Exit Do
                                If Operators.Peek = "(" Then Exit Do
                            Loop
                        End If
                        Operators.Push(current)
                    ElseIf GetPriority(Operators.Peek) < GetPriority(current) Then
                        If Operators.Peek <> "(" Then
                            Do Until Operators.Peek = "("
                                Numbers.Push(CalculateBlock(Numbers.Pop, Numbers.Pop, Operators.Pop))
                            Loop
                        End If
                        Operators.Pop()
                    Else
                        Numbers.Push(CalculateBlock(Numbers.Pop, Numbers.Pop, Operators.Pop))
                    End If
                End If
            Next
            Do While Operators.Count > 0
                Numbers.Push(CalculateBlock(Numbers.Pop, Numbers.Pop, Operators.Pop))
            Loop
            If Numbers.Count = 1 Then
                Return Numbers.First
            Else
                Throw New ArgumentException("不能省略*或x运算符")
                Dim tmp As Decimal = 1
                For Each n In Numbers
                    tmp *= n
                Next
                Return tmp
            End If
        End Function
        Public Function EvalAsync(Expression As String) As Task(Of Decimal)
            Return New Task(Of Decimal)(Function() Eval(Expression))
        End Function
    End Class
End Namespace