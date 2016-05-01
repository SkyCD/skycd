Imports SkyCD.App.Forms
Imports SkyCD.Libraries.AdvancedFunctions.File

Public Class PlugInsSupport
    'Inherits System.ComponentModel.Component

    Private Settings As New SCD_XSettings("SkyCD PlugIns")

    Friend LoadedPlugIn As String = ""

    Public Property Path() As String
        Get
            Return modGlobal.Settings.ReadSetting("PlugInsPath", , My.Application.Info.DirectoryPath + "\Plug-Ins\")
        End Get
        Set(ByVal value As String)
            'MsgBox(Path)
            modGlobal.Settings.WriteSetting("PlugInsPath", value)
        End Set
    End Property

    Public Function GetPlugIns() As PlugInInfo()
        Dim Kx As String(,) = Me.Settings.ReadSettings("PlugIns")
        If IsNothing(Kx) Then Return Nothing
        Dim I As Integer = UBound(Kx)
        Dim Mas(I) As PlugInInfo
        For I = LBound(Kx) To UBound(Kx)
            Mas(I) = New PlugInInfo(Kx(I, 0))
        Next
        Return Mas
    End Function

    Public Function Exists(ByVal Name As String) As Boolean
        Return Settings.ReadSetting("PlugIn." + Name, "FileName", "") <> ""
    End Function

    Public Sub LoadStartupPlugIns(ByVal FormX As Main)
        Try
            Dim Kx As String(,) = Me.Settings.ReadSettings("StartUp")
            Dim I As Integer = UBound(Kx)
            For I = LBound(Kx) To UBound(Kx)
                Me.Load(Of iInterfacePlugIn)(Kx(I, 0)).Create(FormX)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Public Sub Add(ByVal FileName As String)
        Dim Dll As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(FileName)
        Dim Instance As Object = Dll.CreateInstance("Main", True)
        If IsNothing(Instance.GetType.GetInterface("iFileFormat", True)) = False Then
            Settings.WriteSetting("PlugIns", Dll.GetName.Name, Dll.GetName.FullName)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "FileName", FileName)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "HasConfig", Instance.HasConfig)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "ImageRuntimeVersion", Dll.ImageRuntimeVersion)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "ProcessorArchitecture", Dll.GetName.ProcessorArchitecture)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "Type", 1)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "CanOpenSize", Instance.CanOpenSize)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "CanSaveSize", Instance.CanSaveSize)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "CanReadExtendedInfo", Instance.CanReadExtendedInfo)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "CanSaveExtentedInfo", Instance.CanSaveExtentedInfo)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "IsExactFormat", Instance.IsExactFormat)
            Dim FK() As String
            For Each Item As String In Instance.GetSupportedFileFormats
                FK = Item.Split("|")
                Settings.WriteSetting("FileFormats", FK(0), FK(1))
                Settings.WriteSetting("FileFormat." + FK(0), "Handler", Dll.GetName.Name)
                Settings.WriteSetting("FileFormat." + FK(0), "CanDo", Instance.CanDo.ToString)
            Next
            'Tkx1.SupportedFormats = "All files|*.*"
            'Me.Settings.WriteSetting(Dll.GetName.Name, SkyAdvancedFunctionsLibrary.Strings.SerializeToText(Tkx1), True)            
        ElseIf IsNothing(Instance.GetType.GetInterface("iInterfacePlugIn", True)) = False Then
            Settings.WriteSetting("PlugIns", Dll.GetName.Name, Dll.GetName.FullName)
            Settings.WriteSetting("StartUp", Dll.GetName.Name, Dll.GetName.FullName)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "FileName", FileName)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "HasConfig", Instance.HasConfig)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "ImageRuntimeVersion", Dll.ImageRuntimeVersion)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "ProcessorArchitecture", Dll.GetName.ProcessorArchitecture)
            Settings.WriteSetting("PlugIn." + Dll.GetName.Name, "Type", 2)
        End If
        Instance = Nothing
    End Sub

    Public Function GetSupportedFileFormatsForRead() As String
        Dim data(,) = Settings.ReadSettings("FileFormats")
        Dim I As Integer, Supported As String = "", Supported2 As String = ""
        For I = LBound(data) To UBound(data)
            If InStr(Settings.ReadSetting("CanDo", "FileFormat." & data(I, 0)).ToString.ToLower, "read") > -1 Then
                Supported = Supported & data(I, 0) & " (" & data(I, 1) & ")" & "|" & data(I, 1) & "|"
                If InStr(Supported2, data(I, 1) & ";") < 1 Then
                    Supported2 = Supported2 & data(I, 1) & ";"
                End If
            End If
        Next
        Supported2 = Left(Supported2, Supported2.Length - 1)
        Return Supported & "All Supported File Formats (" & Supported2 & ")|" & Supported2
    End Function

    Public Function GetSupportedFileFormatsForWrite() As String
        Dim data(,) = Settings.ReadSettings("FileFormats")
        Dim I As Integer, Supported As String = ""
        For I = LBound(data) To UBound(data)
            If InStr(Settings.ReadSetting("CanDo", "FileFormat." & data(I, 0)).ToString.ToLower, "write") > -1 Then
                Supported = Supported & data(I, 0) & " (" & data(I, 1) & ")" & "|" & data(I, 1) & "|"
            End If
        Next
        Return Left(Supported, Supported.Length - 1)
    End Function

    Public Sub UpdatePlugInsList(Optional ByVal CheckCount As Boolean = True)
        Dim Katalogas As New System.IO.DirectoryInfo(Me.Path)
        If Katalogas.Exists = False Then Exit Sub
        Dim Files() As System.IO.FileInfo = Katalogas.GetFiles("*.dll")
        If CheckCount Then _
            If UBound(Files) = Me.Settings.ReadSetting("DLLsCount", "Default", 0) Then Exit Sub
        Dim I As Integer
        Dim Kx As String(,) = Me.Settings.ReadSettings("PlugIns")
        Me.Settings.DeleteSetting("StartUp")
        If IsNothing(Kx) = False Then
            For I = LBound(Kx) To UBound(Kx)
                Me.Settings.DeleteSetting("PlugIns", Kx(I, 0))
                Me.Settings.DeleteSetting("PlugIn." + Kx(I, 0))
            Next
            Kx = Me.Settings.ReadSettings("FileFormats")
            For I = LBound(Kx) To UBound(Kx)
                Me.Settings.DeleteSetting("FileFormat." + Kx(I, 0))
            Next
            Me.Settings.DeleteSetting("FileFormats")
        End If
        For Each File As System.IO.FileInfo In Files
            Try
                Me.Add(Me.Path + "\" + File.Name)
            Catch ex As Exception

            End Try
        Next
        Me.Settings.WriteSetting("DLLsCount", UBound(Files))
    End Sub

    Public Function GetHandlerForFile(ByVal FileName As String) As iFileFormat
        Dim data(,) = Settings.ReadSettings("FileFormats")
        Dim C As New Collection
        Dim I As Integer
        For I = LBound(data) To UBound(data)
            If GetFileExtension(data(I, 1)) = GetFileExtension(FileName) Then
                'MsgBox(SkyAdvancedFunctionsLibrary.File.GetFileExtension(data(I, 1)))
                If InStr(Settings.ReadSetting("CanDo", "FileFormat." & data(I, 0)).ToString.ToLower, "read") > -1 Then
                    C.Add(data(I, 0))
                End If
            End If
        Next
        Dim dll As iFileFormat, Hl As String = ""
        For I = 1 To C.Count
            Hl = Settings.ReadSetting("Handler", "FileFormat." + C.Item(I).ToString)
            If Hl.Length > 0 Then
                dll = Me.Load(Of iFileFormat)(Hl)
                If dll.IsSupported(FileName) Then Return dll
            End If
        Next
        Return Nothing
    End Function

    Public Function GetHandlerForFilterIndex(ByVal Index As Integer) As iFileFormat
        Dim data(,) = Settings.ReadSettings("FileFormats")
        Dim I As Integer
        Dim C As New Collection
        For I = LBound(data) To UBound(data)
            If InStr(Settings.ReadSetting("CanDo", "FileFormat." & data(I, 0)).ToString.ToLower, "write") > -1 Then C.Add(data(I, 0))
        Next
        If C.Count < 1 Then Return Nothing
        Dim Hl As String = Settings.ReadSetting("Handler", "FileFormat." + C.Item(Index).ToString)
        '        MsgBox(Hl)
        If Hl.Length > 0 Then
            If Hl.Length > 0 Then
                Return Me.Load(Of iFileFormat)(Hl)
            End If
        End If
        Return Nothing
    End Function

    Public Function Load(Of Type)(ByVal Name As String) As Type
        If Name.Trim = "" Then Return Nothing
        Dim FileName As String = Settings.ReadSetting("FileName", "PlugIn." + Name)
        Dim Dll As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(FileName)
        Dim Instance As Object = Dll.CreateInstance("Main", True)
        Me.LoadedPlugIn = Name
        Return Instance
    End Function

    Public Function Load(ByVal Name As String) As Object
        Dim FileName As String = Settings.ReadSetting("FileName", "PlugIn." + Name)
        Dim Dll As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(FileName)
        Dim Instance As Object = Dll.CreateInstance("Main", True)
        Me.LoadedPlugIn = Name
        Return Instance
    End Function

    Public Class PlugInInfo

        Private Settings As New SCD_XSettings("SkyCD PlugIns")
        Protected Friend Name As String

        Sub New(ByVal Name As String)
            Me.Name = Name
        End Sub

        Public ReadOnly Property FileName() As String
            Get
                Return Settings.ReadSetting("FileName", "PlugIn." + Me.Name)
            End Get
        End Property

        Public ReadOnly Property FullInfo() As String
            Get
                Return Settings.ReadSetting("FullInfo", "PlugIn." + Me.Name)
            End Get
        End Property

        Public ReadOnly Property Type() As String
            Get
                Select Case Settings.ReadSetting("Type", "PlugIn." + Me.Name)
                    Case 1
                        Return Translate(Me, "File Format Support")
                    Case 2
                        Return Translate(Me, "Interface PlugIn")
                End Select
                Return Translate(Me, "Unknown")
            End Get
        End Property

        Public ReadOnly Property CanOpenSize() As String
            Get
                Return Settings.ReadSetting("CanOpenSize", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property CanSaveSize() As String
            Get
                Return Settings.ReadSetting("CanSaveSize", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property CanReadExtendedInfo() As String
            Get
                Return Settings.ReadSetting("CanReadExtendedInfo", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property CanSaveExtentedInfo() As String
            Get
                Return Settings.ReadSetting("CanSaveExtentedInfo", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property IsExactFormat() As String
            Get
                Return Settings.ReadSetting("IsExactFormat", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property ProcessorArchitecture() As String
            Get
                Return Settings.ReadSetting("ProcessorArchitecture", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property ImageRuntimeVersion() As String
            Get
                Return Settings.ReadSetting("ImageRuntimeVersion", "PlugIn." + Me.Name).ToString
            End Get
        End Property

        Public ReadOnly Property HasConfig() As Boolean
            Get
                Return Settings.ReadSetting("HasConfig", "PlugIn." + Me.Name, False)
            End Get
        End Property

    End Class

End Class