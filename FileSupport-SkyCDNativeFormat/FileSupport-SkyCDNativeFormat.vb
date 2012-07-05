Imports Microsoft.VisualBasic.FileIO.FileSystem
Imports Microsoft.VisualBasic
'Imports SkyCD_Simple
Imports SkyCD
Imports SkyAdvancedFunctionsLibrary

Public Class Main
    Implements iFileFormat

    Private db As System.Data.OleDb.OleDbDataAdapter
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
        Dim SP As New System.IO.FileStream(Filename, IO.FileMode.Open)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Decompress)
        Dim TK As New System.IO.StreamReader(CPR)
        If TK.ReadLine().Substring(0, "# format: skycd-nf".Length).ToLower <> "# format: skycd-nf" Then Rez = False
        TK.Close()
        Return Rez
    End Function

    Public Sub Save(ByVal FileName As String) Implements iFileFormat.Save
        Dim SP As New System.IO.FileStream(FileName, IO.FileMode.OpenOrCreate)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Compress)
        Dim TK As New System.IO.StreamWriter(CPR)
        Dim I As Integer = 0
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdSaving
        status.scdValue = 0
        RaiseEvent UpdateStatus(status)
        'Dim Count As Integer = GetSelectRezCount("SELECT * FROM list WHERE AID = '" + Me.application_guid + "'")
        Me.db.SelectCommand.CommandText = "SELECT * FROM list WHERE AID = '" + Me.application_guid + "'"
        Dim rez As System.Data.OleDb.OleDbDataReader = Me.db.SelectCommand.ExecuteReader()
        TK.WriteLine("# format: skycd-nf 1.0")
        Do While rez.Read()
            TK.WriteLine("INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + rez.Item("ID").ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(rez.Item("Name").ToString).Replace(vbCrLf, " ") + "', '" + rez.Item("ParentID").ToString + "', '" + rez.Item("Type").ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(rez.Item("Properties").ToString).Replace(vbCrLf, " ") + "','" + rez.Item("Size").ToString + "','<?Application_ID?>')")
            I = I + 1
            If I Mod 3 = 15 Then RaiseEvent NeedDoEvents()
        Loop
        status.scdValue = 100
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
        RaiseEvent UpdateStatus(status)
        rez.Close()
        TK.Close()
    End Sub

    Public Event UpdateStatus(ByVal e As iFileFormat.scdStatus) Implements iFileFormat.UpdateStatus
    Public Event WasError(ByVal e As iFileFormat.scdError) Implements iFileFormat.WasError
    Public Event NeedDoEvents() Implements iFileFormat.NeedDoEvents
    Public Event DebugWrite(ByVal Msg As Object) Implements iFileFormat.DebugWrite

    Property Database() As System.Data.OleDb.OleDbDataAdapter Implements iFileFormat.Database
        Get
            Return Me.db
        End Get
        Set(ByVal value As System.Data.OleDb.OleDbDataAdapter)
            Me.db = value
        End Set
    End Property


    Private Function GetSelectRezCount(ByVal sql As String) As Integer
        Dim rez As System.Data.OleDb.OleDbDataReader
        Dim dbx As New System.Data.OleDb.OleDbDataAdapter
        dbx = Me.db
        dbx.SelectCommand.CommandText = sql
        rez = dbx.SelectCommand.ExecuteReader()
        Dim Count As Integer = 0
        Do While rez.Read()
            Count = Count + 1
            If Count Mod 30 = 0 Then RaiseEvent NeedDoEvents()
        Loop
        rez.Close()
        rez.Dispose()
        Return Count
    End Function

    Sub Export()
        Me.db.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.application_guid + "'"
        Me.db.DeleteCommand.ExecuteNonQuery()
    End Sub

    Public Function GetSupportedFileFormats() As Microsoft.VisualBasic.Collection Implements SkyCD.iFileFormat.GetSupportedFileFormats
        Dim Col As New Collection
        Col.Add("SkyCD Advanced File Format|*.ascd")
        Return Col
    End Function

    Public Sub Load(ByVal FileName As String) Implements SkyCD.iFileFormat.Load
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdLoading
        status.scdValue = 0
        Dim SP As New System.IO.FileStream(FileName, IO.FileMode.Open)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Decompress)
        Dim TK As New System.IO.StreamReader(CPR)
        RaiseEvent NeedDoEvents()
        Me.db.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.application_guid + "'"
        Me.db.DeleteCommand.ExecuteNonQuery()
        Dim Text As String
        Do
            Text = TK.ReadLine().Replace("'<?Application_ID?>'", Me.application_guid)
            If Left(Text.Trim.ToUpper, "INSERT INTO".Length) = "INSERT INTO" Then
                Me.db.InsertCommand.CommandText = Text
                Me.db.InsertCommand.ExecuteNonQuery()
                RaiseEvent NeedDoEvents()
            End If            
        Loop Until TK.EndOfStream
        TK.Close()
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
    End Sub

    Public ReadOnly Property HasConfig() As Boolean Implements SkyCD.ISkyCDPlugIn.HasConfig
        Get
            Return False
        End Get
    End Property

    Public Sub ShowConfig() Implements SkyCD.ISkyCDPlugIn.ShowConfig        
    End Sub

End Class
