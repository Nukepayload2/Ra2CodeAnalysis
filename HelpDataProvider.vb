Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class HelpDataProvider
    Protected Function GetHelpTextFromDic(code As String, dic As Dictionary(Of String, String)) As String
        If String.IsNullOrEmpty(code) Then Return "(空)"
        If dic.ContainsKey(code) Then
            Return dic(code)
        Else
            'If code.IsNumeric Then
            '    Return "编号或其它数字"
            'Else
            Return ""
            'End If
        End If
    End Function

    Dim vbcs As New Dictionary(Of String, String) From {{"String", "string"}, {"Integer", "int"},
        {"Single", "float"}, {"Boolean", "bool"}, {"Structure", "struct"}}
    Protected Function FormatUsage(Code As String, tp As String) As String
        Dim cs As String
        If vbcs.ContainsKey(tp) Then
            cs = vbcs(tp)
        Else
            cs = tp.Replace("(Of ", "<").Replace(")", ">")
            For Each k In vbcs.Keys
                cs = cs.Replace(k, vbcs(k))
            Next
        End If

        If String.IsNullOrEmpty(Code) Then
            Return String.Empty
        End If
        If Code.IsInteger Then
            Return "整数常量" & vbCrLf & "用法1:Structure System.Int32" & vbCrLf & "用法2:struct System.Int32;"
        ElseIf Code.IsNumeric
            Return "小数常量" & vbCrLf & "用法1:Structure System.Single" & vbCrLf & "用法2:struct System.Single;"
        ElseIf Code.Replace("%", "").IsNumeric
            Return "百分数常量" & vbCrLf & "用法1:Structure Nukepayload2.CodeAnalysis.Percentage" & vbCrLf & "用法2:struct Nukepayload2.CodeAnalysis.Percentage;"
        ElseIf Code.Chars(0).IsNumeric
            Code = "_" & Code
        ElseIf Code.Contains(".")
            Code = Code.Replace("."c, "_"c)
        ElseIf {"yes", "no"}.Contains(Code.ToLowerInvariant)
            Return "布尔值常量" & vbCrLf & "用法1:Const " & Code & " As " & tp & vbCrLf & "用法2:const " & cs & " " & Code & ";"
        ElseIf {"true", "false"}.Contains(Code.ToLowerInvariant)
            Return "表示布尔值" & vbCrLf & "用法1:Structure System.Boolean" & vbCrLf & "用法2:struct System.Boolean;"
        End If
        Return vbCrLf & "用法1:Dim " & Code & " As " & tp & vbCrLf & "用法2:" & cs & " " & Code & ";"
    End Function

    Protected Function GetHelpTextWithUsage(code As String, text As Dictionary(Of String, String), usage As Dictionary(Of String, String)) As String
        If String.IsNullOrEmpty(code) Then Return "(空)"
        Dim tx = GetHelpTextFromDic(code, text)
        If usage.ContainsKey(code) Then
            If String.IsNullOrEmpty(tx) Then
                tx = "确定的用法:"
            End If
            tx &= FormatUsage(code, usage(code))
        End If
        Return tx
    End Function

    Public Function TempAnalizeFormatUsage(Key As String, Value As String) As String
        Return FormatUsage(Key, TempAnalizeUsage(Key, Value))
    End Function
    Public Function DeepAnalizeFormatUsage(Key As String, Value As String, ini As INIAnalizer) As String
        Dim tp = TempAnalizeUsage(Key, Value)
        If tp = "String" Then
            For Each mkv In ini.Values
                For Each kv In mkv.Value
                    If Not kv.Key.IsNumeric Then
                        Exit For
                    End If
                    If kv.Value.Item1 = Value Then
                        Return FormatUsage(Value, mkv.Key)
                    End If
                Next
            Next
        End If
        Return FormatUsage(Value, tp)
    End Function
    Protected Function TempAnalizeUsage(Key As String, Value As String) As String
        Dim rig = Value
        If String.IsNullOrEmpty(rig) Then
            Return "String"
        End If
        If rig.Contains(",") Then
            Dim sp = rig.Split(","c).First.Trim
            If sp.IsInteger Then
                Return "IEnumerable(Of Integer)"
            ElseIf sp.IsFraction
                Return "IEnumerable(Of Single)"
            ElseIf sp.Replace("%", "").IsInteger
                Return "Percentage"
            Else
                Return "IEnumerable(Of String)"
            End If
        Else
            If rig.IsInteger Then
                Return "Integer"
            ElseIf rig.IsFraction
                Return "Single"
            ElseIf {"true", "false", "yes", "no"}.Contains(rig.ToLowerInvariant)
                Return "Boolean"
            ElseIf rig.Replace("%", "").IsInteger
                Return "Percentage"
            ElseIf rig.StartsWith("{") AndAlso rig.EndsWith("}") AndAlso rig.Contains("-")
                Return "ClassID"
            Else
                Return "String"
            End If
        End If
    End Function
End Class
