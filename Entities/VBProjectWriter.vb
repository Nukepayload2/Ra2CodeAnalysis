Imports System.Reflection
Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper
Imports Nukepayload2.Ra2CodeAnalysis.Linq

Public Class VBProjectWriter

    ''' <summary>
    ''' 项目名称（不含拓展名） 
    ''' </summary>
    Public Property ProjectName As String
    ''' <summary>
    ''' 生成与 .NET Standard 1.2 等价目标的 VB 14 类库项目文件
    ''' </summary>
    ''' <param name="FileNames">生成的文件名（不含拓展名，包含相对项目的路径）</param>
    Public Function GeneratePcl44Proj(FileNames As IEnumerable(Of String)) As GeneratedCodeFile
        Dim xdoc =
$"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
    <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')""/>
    <PropertyGroup>
        <MinimumVisualStudioVersion>10.0</MinimumVisualStudioVersion>
        <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
        <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
        <ProjectGuid>{{1FCBC551-B7E3-4FC8-8CCF-111A7A1F1469}}</ProjectGuid>
        <OutputType>Library</OutputType>
        <RootNamespace>{ProjectName}</RootNamespace>
        <AssemblyName>{ProjectName}</AssemblyName>
        <DefaultLanguage>zh-CN</DefaultLanguage>
        <ProjectTypeGuids>{{14182A97-F7F0-4C62-8B27-98AA8AE2109A}};{{F184B08F-C81C-45F6-A57F-5ABD9991F28F}}</ProjectTypeGuids>
        <TargetFrameworkProfile>Profile44</TargetFrameworkProfile>
        <TargetFrameworkVersion>v4.6</TargetFrameworkVersion>
    </PropertyGroup>
    <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
        <DebugSymbols>true</DebugSymbols>
        <DebugType>full</DebugType>
        <DefineDebug>true</DefineDebug>
        <DefineTrace>true</DefineTrace>
        <DefineConstants>
        </DefineConstants>
        <OutputPath>bin\Debug</OutputPath>
        <DocumentationFile>{ProjectName}.xml</DocumentationFile>
        <NoWarn>40057,42016,41999,42020,42021,42022</NoWarn>
        <WarningsAsErrors>42017,42018,42019,42032,42036</WarningsAsErrors>
    </PropertyGroup>
    <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
        <DebugType>pdbonly</DebugType>
        <DefineDebug>false</DefineDebug>
        <DefineTrace>true</DefineTrace>
        <DefineConstants>
        </DefineConstants>
        <Optimize>true</Optimize>
        <OutputPath>bin\Release</OutputPath>
        <DocumentationFile>{ProjectName}.xml</DocumentationFile>
        <NoWarn>40057,42016,41999,42020,42021,42022</NoWarn>
        <WarningsAsErrors>42017,42018,42019,42032,42036</WarningsAsErrors>
    </PropertyGroup>
    <PropertyGroup>
        <OptionExplicit>On</OptionExplicit>
    </PropertyGroup>
    <PropertyGroup>
        <OptionCompare>Binary</OptionCompare>
    </PropertyGroup>
    <PropertyGroup>
        <OptionStrict>On</OptionStrict>
    </PropertyGroup>
    <PropertyGroup>
        <OptionInfer>On</OptionInfer>
    </PropertyGroup>
    <ItemGroup>
        <Import Include=""Microsoft.VisualBasic""/>
        <Import Include=""System""/>
        <Import Include=""System.Collections""/>
        <Import Include=""System.Collections.Generic""/>
        <Import Include=""System.Collections.ObjectModel""/>
        <Import Include=""System.ComponentModel""/>
        <Import Include=""System.Diagnostics""/>
        <Import Include=""System.IO""/>
        <Import Include=""System.Linq""/>
        <Import Include=""System.Runtime.CompilerServices""/>
        <Import Include=""System.Threading.Tasks""/>
        <Import Include=""System.Xml.Linq""/>
        <Import Include=""System.ComponentModel.DataAnnotations""/>
    </ItemGroup>
    <ItemGroup>
{Aggregate f In FileNames Select $"<Compile Include=""{f}.vb""/>" Into Join(8)}
        <Compile Include=""My Project\AssemblyInfo.vb""/>
    </ItemGroup>
    <ItemGroup>
        <None Include=""packages.config""/>
    </ItemGroup>
    <Import Project=""$(MSBuildExtensionsPath32)\Microsoft\Portable\$(TargetFrameworkVersion)\Microsoft.Portable.VisualBasic.targets""/>
</Project>"
        Return New GeneratedCodeFile(ProjectName + ".vbproj", xdoc)
    End Function

    Public Function GenerateAssemblyInfo() As GeneratedCodeFile
        Dim fileName = "My Project\AssemblyInfo.vb"
        Dim content = $"Imports System
Imports System.Resources
Imports System.Reflection

' 有关程序集的一般信息由以下
' 控制。更改这些特性值可修改
' 与程序集关联的信息。

'查看程序集特性的值

<Assembly: AssemblyTitle(""{ProjectName}"")> 
<Assembly: AssemblyDescription("""")> 
<Assembly: AssemblyCompany("""")> 
<Assembly: AssemblyProduct(""{ProjectName}"")> 
<Assembly: AssemblyCopyright(""版权所有(C)  2016"")> 
<Assembly: AssemblyTrademark("""")> 
<Assembly: NeutralResourcesLanguage(""zh-Hans"")>

' 程序集的版本信息由下列四个值组成: 
'
'      主版本
'      次版本
'      生成号
'      修订号
'
'可以指定所有这些值，也可以使用 ""生成号"" 和 ""修订号"" 的默认值，
' 方法是按如下所示使用""*"": :
' <Assembly: AssemblyVersion(""1.0.*"")> 

<Assembly: AssemblyVersion(""1.0.0.0"")> 
<Assembly: AssemblyFileVersion(""1.0.0.0"")> 
"
        Return New GeneratedCodeFile(fileName, content)
    End Function

    ''' <summary>
    ''' 获取嵌入的类型以便解析某些ini文件
    ''' </summary>
    Public Async Function GetEmbeddedCodeFileAsync() As Task(Of GeneratedCodeFile)
        Dim content As String
        Using strm = Me.GetType.GetTypeInfo.Assembly.GetManifestResourceStream("Nukepayload2.Ra2CodeAnalysis.Percentage.vb"), sr = New StreamReader(strm, Encoding.UTF8)
            content = Await sr.ReadToEndAsync()
        End Using
        Return New GeneratedCodeFile("Percentage", content)
    End Function

    Public Function WriteRa2IniGroup(data As IEnumerable(Of NamedIniAnalyzer)) As IEnumerable(Of GeneratedCodeFile)

    End Function
End Class