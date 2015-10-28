Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class HelpDataProvider
    Protected Function GetHelpTextFromDic(code As String, dic As Dictionary(Of String, String)) As String
        If String.IsNullOrEmpty(code) Then Return "(空)"
        If dic.ContainsKey(code) Then
            Return dic(code)
        Else
            Return ""
        End If
    End Function

    Dim vbcs As New Dictionary(Of String, String) From {{"String", "string"}, {"Integer", "int"},
        {"Single", "float"}, {"Boolean", "bool"}, {"Structure", "struct"}, {"Object", "object"}}
    ''' <summary>
    ''' 格式化用法为vb代码和c#代码
    ''' </summary>
    ''' <param name="Code"></param>
    ''' <param name="tp"></param>
    ''' <returns></returns>
    Public Function FormatUsage(Code As String, tp As String, Optional DisableTypeJudgeFormat As Boolean = True) As String
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
        If Not DisableTypeJudgeFormat Then
            If tp.Contains("IEnumerable(Of") Then
                Return "表示多个可枚举的" & tp.Substring(15, tp.Length - 16) & vbCrLf & "用法1:Dim <VariableName> As " & tp & vbCrLf & "用法2:" & cs & " <VariableName>;"
            End If
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
            Return "布尔值常量" & vbCrLf & "用法1:Const " & Code & " As " & tp & " = " & If(Code.ToLowerInvariant = "yes", "True", "False") & vbCrLf & "用法2:const " & cs & " " & Code & " = " & If(Code.ToLowerInvariant = "yes", "true", "false") & ";"
        ElseIf {"true", "false"}.Contains(Code.ToLowerInvariant)
            Return "表示布尔值" & vbCrLf & "用法1:Structure System.Boolean" & vbCrLf & "用法2:struct System.Boolean;"
        ElseIf Code.ToLowerInvariant = "none"
            Return "表示空值或空引用" & vbCrLf & "用法1:Const " & Code & " As Object = Nothing" & vbCrLf & "用法2:const object " & Code & " = null;"
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
        Return FormatUsage(Key, TempAnalizeUsage(Value))
    End Function
    Public Function GetRulesUsageForIme(Word As String, Helper As RulesHelpProvider, ini As RulesAnalizer) As String
        Dim hlp = Helper.GetHelpText(Word)
        If String.IsNullOrEmpty(hlp) Then
            hlp = DeepAnalizeFormatUsage(Word, Word, ini)
        End If
        Return hlp
    End Function
    Public Function GetUsageForIme(Word As String, Helper As IHelpProvider, ini As INIAnalizer) As String
        Dim hlp = Helper.GetHelpText(Word)
        If String.IsNullOrEmpty(hlp) Then
            hlp = DeepAnalizeFormatUsage(Word, Word, ini)
        End If
        Return hlp
    End Function
    Public Function DeepAnalizeType(Value As String, ini As INIAnalizer) As String
        Dim tp = TempAnalizeUsage(Value)
        If tp = "String" Then
            For Each mkv In ini.Values
                If mkv.Key = "AITriggerTypes" Then
                    If ini.Values(mkv.Key).ContainsKey(Value) Then
                        Return mkv.Key
                    End If
                Else
                    For Each kv In mkv.Value
                        For Each Name In {"Warhead", "Sequence"}
                            If kv.Key = Name AndAlso Value = kv.Value.Item1 Then
                                Return Name
                            End If
                        Next
                    Next
                    For Each kv In mkv.Value
                        If Not kv.Key.IsNumeric Then
                            Exit For
                        End If
                        If kv.Value.Item1 = Value Then
                            Return mkv.Key
                        End If
                    Next
                End If
            Next
        End If
        Return tp
    End Function
    Public Function DeepAnalizeFormatUsage(Key As String, Value As String, ini As INIAnalizer) As String
        Return FormatUsage(Value, DeepAnalizeType(Value, ini))
    End Function
    <Obsolete("这个重载版本已经过时了,它将不会工作")>
    Public Function DeepAnalizeType(Value As String, ini As INIAnalizer, textcomp As Boolean, Optional CsOverloadTemp As Object = Nothing) As String
        Return String.Empty
    End Function
    Public Function DeepAnalizeType(Value As String, ini As RulesAnalizer, Optional CsOverloadTemp As Object = Nothing) As String
        Dim tp = TempAnalizeUsage(Value)
        If tp = "String" Then
            For Each mkv In ini.Values
                For Each kv In mkv.Value
                    For Each v In kv.Value.Item1.Split(","c)
                        If v.Trim = Value Then
                            If kv.Key.IsNumeric Then
                                Return mkv.Key
                            ElseIf RulesAnalizer.IsWeaponKey(kv.Key)
                                Return "Weapon"
                            Else
                                For Each Name In {"Warhead", "Projectile", "MetallicDebris", "DeadBodies"}
                                    If kv.Key = Name AndAlso Value = kv.Value.Item1 Then
                                        Return Name
                                    End If
                                Next
                            End If
                        End If
                    Next
                Next
            Next
        End If
        Return tp
    End Function
    <Obsolete("这个重载版本已经过时了,它将不会工作")>
    Public Function DeepAnalizeFormatUsage(Key As String, Value As String, ini As RulesAnalizer, textcomp As Boolean, Optional CsOverloadTemp As Object = Nothing) As String
        Return String.Empty
    End Function
    Public Function DeepAnalizeFormatUsage(Key As String, Value As String, ini As RulesAnalizer, Optional CsOverloadTemp As Object = Nothing) As String
        Return FormatUsage(Value, DeepAnalizeType(Value, ini))
    End Function
    Public Function TempAnalizeUsage(Value As String) As String
        Dim rig = Value
        If String.IsNullOrEmpty(rig) OrElse rig.ToLower = "none" Then
            Return "Object"
        End If
        If rig.Contains(",") Then
            Dim spa = rig.Split(","c)
            Dim sp = spa.First.Trim
            If sp.IsInteger Then
                For Each tp In spa
                    If Not tp.Trim.IsInteger Then Return "IEnumerable(Of String)"
                Next
                Return "IEnumerable(Of Integer)"
            ElseIf sp.IsFraction
                For Each tp In spa
                    If Not tp.Trim.IsFraction Then Return "IEnumerable(Of String)"
                Next
                Return "IEnumerable(Of Single)"
            ElseIf sp.Replace("%", "").IsInteger
                Return "IEnumerable(Of Percentage)"
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
