'Imports SkyCD_Simple
Imports System.IO
Imports SkyCD.Libraries.AdvancedFunctions.Strings
Imports System.Collections.Generic

Public Class Main
    Implements iFileFormat

    Private db As Database.iConnection
    Private dm As Boolean = False
    Private application_guid As String = ""

    Public ReadOnly Property CanDo() As iFileFormat.scdCanDo Implements iFileFormat.CanDo
        Get
            Return iFileFormat.scdCanDo.scdReadWrite
        End Get
    End Property

    Public Property ApplicationGUID() As String Implements iFileFormat.ApplicationGUID
        Get
            Return Me.application_guid
        End Get
        Set(ByVal value As String)
            Me.application_guid = value
            If Me.dm Then RaiseEvent DebugWrite("Application GUID Changed: " + Me.application_guid)
        End Set
    End Property

    Public Property DebugMode() As Boolean Implements iFileFormat.DebugMode
        Get
            Return Me.dm
        End Get
        Set(ByVal value As Boolean)
            If Me.dm And value = False Then RaiseEvent DebugWrite("Debuging mode disabled")
            Me.dm = value
            If Me.dm And value = True Then RaiseEvent DebugWrite("Debuging mode enabled")
        End Set
    End Property

    Public ReadOnly Property CanSaveSize() As Boolean Implements iFileFormat.CanSaveSize
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanOpenSize() As Boolean Implements iFileFormat.CanOpenSize
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSaveExtentedInfo() As Boolean Implements iFileFormat.CanSaveExtentedInfo
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanReadExtendedInfo() As Boolean Implements iFileFormat.CanReadExtendedInfo
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property IsExactFormat() As Boolean Implements iFileFormat.IsExactFormat
        Get
            Return True
        End Get
    End Property

    Public Function IsSupported(ByVal Filename As String) As Boolean Implements iFileFormat.IsSupported
        Dim Rez As Boolean = True
        Dim K As Integer = 500
        'FileSystem.FileOpen(L, Filename, OpenMode.Input)
        Dim SP As New FileStream(Filename, FileMode.Open)
        Dim CPR As New Compression.DeflateStream(SP, Compression.CompressionMode.Decompress)
        Dim TK As New StreamReader(CPR)
        If TK.ReadLine().Substring(0, "# format: skycd-nf".Length).ToLower <> "# format: skycd-nf" Then Rez = False
        TK.Close()
        Return Rez
    End Function

    Public Sub Save(ByVal FileName As String) Implements iFileFormat.Save
        Dim SP As New FileStream(FileName, FileMode.OpenOrCreate)
        Dim CPR As New Compression.DeflateStream(SP, Compression.CompressionMode.Compress)
        Dim TK As New StreamWriter(CPR)
        Dim I As Integer = 0
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdSaving
        status.scdValue = 0
        RaiseEvent UpdateStatus(status)
        'Dim Count As Integer = GetSelectRezCount("SELECT * FROM list WHERE AID = '" + Me.application_guid + "'")
        Dim sql As String = "SELECT * FROM list WHERE AID = ?"
        TK.WriteLine("# format: skycd-nf 1.0")
        For Each item As Database.Item In Me.db.Select(sql, Me.application_guid)
            TK.WriteLine("INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + item.ID.ToString + "', '" + AddSlashes(item.Name).Replace(vbCrLf, " ") + "', " + item.ParentID.ToString + "', '" + item.Type.ToString + "', '" + AddSlashes(item.Properties.ToString).Replace(vbCrLf, " ") + "'," + item.Size + ",'<?Application_ID?>')")
            I = I + 1
            If I Mod 3 = 15 Then RaiseEvent NeedDoEvents()
        Next
        status.scdValue = 100
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
        RaiseEvent UpdateStatus(status)
    End Sub

    Public Event UpdateStatus(ByVal e As iFileFormat.scdStatus) Implements iFileFormat.UpdateStatus
    Public Event WasError(ByVal e As iFileFormat.scdError) Implements iFileFormat.WasError
    Public Event NeedDoEvents() Implements iFileFormat.NeedDoEvents
    Public Event DebugWrite(ByVal Msg As Object) Implements iFileFormat.DebugWrite

    Property Database() As Database.iConnection Implements iFileFormat.Database
        Get
            Return db
        End Get
        Set(ByVal value As Database.iConnection)
            Me.db = value
        End Set
    End Property

    Sub Export()
        Me.db.Execute("DELETE FROM list WHERE AID = ?", Me.application_guid)
    End Sub

    Public Function GetSupportedFileFormats() As List(Of String) Implements iFileFormat.GetSupportedFileFormats
        Dim Col As New List(Of String)
        Col.Add("SkyCD Advanced File Format|*.ascd")
        Return Col
    End Function

    Public Sub Load(ByVal FileName As String) Implements iFileFormat.Load
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdLoading
        status.scdValue = 0
        Dim SP As New FileStream(FileName, FileMode.Open)
        Dim CPR As New Compression.DeflateStream(SP, Compression.CompressionMode.Decompress)
        Dim TK As New StreamReader(CPR)
        RaiseEvent NeedDoEvents()
        Me.db.Execute("DELETE FROM list WHERE AID = ?", Me.application_guid)
        Dim Text As String
        Do
            Text = TK.ReadLine().Replace("'<?Application_ID?>'", Me.application_guid)
            If Left(Text.Trim.ToUpper, "INSERT INTO".Length) = "INSERT INTO" Then
                Me.db.Execute(Text)
                RaiseEvent NeedDoEvents()
            End If
        Loop Until TK.EndOfStream
        TK.Close()
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
    End Sub

    Public ReadOnly Property HasConfig() As Boolean Implements ISkyCDPlugIn.HasConfig
        Get
            Return False
        End Get
    End Property

    Public Sub ShowConfig() Implements ISkyCDPlugIn.ShowConfig
    End Sub

End Class
