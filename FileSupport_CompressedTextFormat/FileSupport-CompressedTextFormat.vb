Imports Microsoft.VisualBasic.FileIO.FileSystem
Imports Microsoft.VisualBasic
Imports SkyCD_Simple
Imports SkyCD

Public Class Main
    Inherits SkyCD_Simple.skycd_simple
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
            Return False
        End Get
    End Property

    Public ReadOnly Property CanReadExtendedInfo() As Boolean Implements iFileFormat.CanReadExtendedInfo
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsExactFormat() As Boolean Implements iFileFormat.IsExactFormat
        Get
            Return False
        End Get
    End Property

    Public Function IsSupported(ByVal Filename As String) As Boolean Implements iFileFormat.IsSupported
        Dim L As Integer = FreeFile()
        Dim Rez As Boolean = True
        Dim Text As String
        Dim K As Integer = 500
        'FileSystem.FileOpen(L, Filename, OpenMode.Input)
        Dim SP As New System.IO.FileStream(Filename, IO.FileMode.Open)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Decompress)
        Dim aF As New System.IO.StreamReader(CPR)
        Do
            Text = aF.ReadLine()
            Rez = Rez And (((Left(Trim(Text), 1) = "[") Or (Left(Trim(Text), 1) = "#")) Or (Trim(Text) = ""))
            K = K - 1
            If K < 1 Then Exit Do
        Loop Until aF.EndOfStream
        aF.Close()
        'FileSystem.FileClose(L)
        Return Rez
    End Function

    Sub Load(ByVal Filename As String) Implements iFileFormat.Load
        Dim L As Integer = FreeFile()
        Dim I As Integer, O As Integer
        Dim Text As String, Index As Integer
        Dim tmpText As String = ""
        Dim Mx As Long, Mp As Long
        Dim Veiksmas As iFileFormat.scdStatus
        Veiksmas.scdEvent = iFileFormat.scdStatus.scdProcedure.scdLoading
        Veiksmas.scdValue = 0
        RaiseEvent UpdateStatus(Veiksmas)
        Dim SP As New System.IO.FileStream(Filename, IO.FileMode.Open)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Decompress)
        Dim aF As New System.IO.StreamReader(CPR)
        'Mx = FileSystem.FileLen(Filename)
        Mx = aF.ReadToEnd.Length
        aF.Close()
        SP = New System.IO.FileStream(Filename, IO.FileMode.Open)
        CPR = New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Decompress)
        aF = New System.IO.StreamReader(CPR)
        Do
            '            Form1.StatusStrip1.Items(1).Text = Mp.ToString + "/" + Mx.ToString
            Text = aF.ReadLine()
            'Text = FileSystem.LineInput(L)
            Mp = Mp + Text.Length
            If Trim(Text) = "" Then Continue Do
            If Left(Trim(Text), 1) = "#" Then Continue Do
            I = InStr(Text, "[")
            O = InStr(Text, "]")
            If (I > -1) And (O > -1) Then
                Text = Text.Substring(I, O - 2) & Text.Substring(O)
            End If
            I = SkyAdvancedFunctionsLibrary.Strings.FindLast(Text, "\")
            If (I > -1) Then
                O = SkyAdvancedFunctionsLibrary.Strings.FindLast(Text.Substring(I), "] ")
                If ((Text.Substring(I, 1) = "[") And ((O = 18) Or (O = 19) Or (O = 17))) Then
                    tmpText = Text
                    Text = Text.Substring(0, I) + Text.Substring(I + O + 1)
                    Index = Me.AddPath(Text, , scdItemType.scdCDDisk, scdItemType.scdFile)
                    If Index > -1 Then
                        Me.Items(Index).AdvancedInfo.Item("Size") = SkyAdvancedFunctionsLibrary.Convert2.Size2Long(tmpText.Substring(I + 1, O - 2).Trim)
                    End If
                Else
                    Me.AddPath(Text, , scdItemType.scdCDDisk)
                End If
            Else
                Me.Add(Text, scdItemType.scdCDDisk)
            End If
            If Rnd() * 10 > 5 Then RaiseEvent NeedDoEvents()
            Veiksmas.scdValue = Convert.ToByte(100 / Mx * Mp)
            RaiseEvent UpdateStatus(Veiksmas)
        Loop Until aF.EndOfStream
        aF.Close()
        'FileSystem.FileClose(L)
        Me.Export()
        Veiksmas.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
        RaiseEvent UpdateStatus(Veiksmas)
    End Sub

    Private Function GetItem(ByVal ID As Integer) As String
        Dim Arx As New Collection
        Do
            Arx.Add(ID)
            ID = Me.Items(ID).ParentID
        Loop Until ID < 0
        Dim Clx(Arx.Count - 1) As String
        Dim I As Integer, O As Integer = 0
        For I = Arx.Count To 1 Step -1
            If Me.Items(Arx.Item(I)).ItemType <> scdItemType.scdFile Then
                If Me.Items(Arx.Item(I)).ParentID < 0 Then
                    Clx(O) = "[" + Me.Items(Arx.Item(I)).Name + "]"
                Else
                    Clx(O) = Me.Items(Arx.Item(I)).Name
                End If
            Else
                Clx(O) = "[" & Convert2.Long2Size(Val(Me.Items(Arx.Item(I)).AdvancedInfo.Item("Size"))) & "] " & Me.Items(Arx.Item(I)).Name
            End If
            O = O + 1
        Next
        Return Strings.Join(Clx, "\")
    End Function

    Public Sub Save(ByVal FileName As String) Implements iFileFormat.Save
        On Error Resume Next
        Dim status As iFileFormat.scdStatus
        'Dim L As Integer = FreeFile()
        Dim I As Integer = 0, K As Integer = UBound(Me.Items), O As Integer
        'Dim Sm As New System.IO.StreamWriter(FileName)
        Me.Import()
        Dim SP As New System.IO.FileStream(FileName, IO.FileMode.Create)
        Dim CPR As New System.IO.Compression.DeflateStream(SP, IO.Compression.CompressionMode.Compress)
        Dim GF As New System.IO.StreamWriter(CPR)
        'FileSystem.FileOpen(L, FileName, OpenMode.Output)
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdSaving
        status.scdValue = 0
        Me.clx.Clear()
        RaiseEvent UpdateStatus(status)
        For I = LBound(Me.Items) To UBound(Me.Items)
            If IsNothing(Me.Items(I).Name) = False Then
                If Me.Items(I).ItemType = scdItemType.scdFile Then
                    'st.
                    'Sm.WriteLine(Text)
                    GF.WriteLine(GetItem(I))
                    'ST.WriteLine()
                    '                    FileSystem.PrintLine(L, GetItem(I))
                    Me.Finalize()
                End If
            End If
            O = K * I
            If O > 0 Then
                status.scdValue = 100 / O
            Else
                status.scdValue = 0
            End If
            RaiseEvent UpdateStatus(status)
            '    RaiseEvent DebugWrite(Text)
            If status.scdValue Mod 2 = 0 Then RaiseEvent NeedDoEvents()
        Next
        'Sm.Close()
        GF.Close()
        'FileSystem.FileClose(L)
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
        RaiseEvent UpdateStatus(status)
    End Sub

    Private clx As New Collection

    Private Function GetItem2(ByVal ID As Integer) As String
        Static Dim Text As String = ""
        Static gid As Integer = ID
        Text = ""
        'SkyAdvancedFunctionsLibrary.
start:
        'RaiseEvent DebugWrite(ID.ToString & "=" & Me.Items(ID).ParentID.ToString)
        If clx.Contains(Me.Items(ID).ParentID.ToString) Then
            Return clx.Item(Me.Items(ID).ParentID.ToString).ToString & "\" & Me.Items(ID).Name
        End If
        If Me.Items(ID).ParentID < 0 Then
            Text = "[" & Me.Items(ID).Name & "]" & "\" & Text
            clx.Add(Text, gid.ToString)
            Return Text
        End If
        If Me.Items(ID).ItemType = scdItemType.scdFile Then
            Text = "[" & Convert2.Long2Size(Val(Me.Items(ID).AdvancedInfo.Item("Size"))) & "] " & Me.Items(ID).Name
            ID = Me.Items(ID).ParentID
            GoTo start
        End If
        Text = Me.Items(ID).Name & "\" & Text
        ID = Me.Items(ID).ParentID
        GoTo start
    End Function

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

    Public Sub Import()
        Dim I As Integer
        Me.Clear()
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdImporting
        status.scdValue = 0
        RaiseEvent UpdateStatus(status)
        Dim Count As Integer = GetSelectRezCount("SELECT * FROM list WHERE AID = '" + Me.application_guid + "'")
        ReDim Me.Items(Count)
        Me.db.SelectCommand.CommandText = "SELECT * FROM list WHERE AID = '" + Me.application_guid + "'"
        Dim rez As System.Data.OleDb.OleDbDataReader = Me.db.SelectCommand.ExecuteReader()
        Do While rez.Read()
            I = Val(rez.Item("ID"))
            If rez.Item("Type").ToString.ToLower = "scdfile" Then
                Me.Items(I).ItemType = scdItemType.scdFile
            Else
                Me.Items(I).ItemType = scdItemType.scdUnknown
            End If
            Me.Items(I).AdvancedInfo = New scdProperties(rez.Item("Properties").ToString)
            Me.Items(I).Name = rez.Item("Name").ToString
            'Me.Items(I).Size = Val(rez.Item("Size"))
            Me.Items(I).ParentID = Val(rez.Item("ParentID"))
            If I Mod 3 = 15 Then RaiseEvent NeedDoEvents()
        Loop
        rez.Close()
        rez.Dispose()
        status.scdValue = 100
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdDone
        RaiseEvent UpdateStatus(status)
        rez.Close()
    End Sub

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
        Dim I As Integer = LBound(Me.Items)
        Dim status As iFileFormat.scdStatus
        status.scdEvent = iFileFormat.scdStatus.scdProcedure.scdExporting
        status.scdValue = 0
        RaiseEvent UpdateStatus(status)
        Me.db.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.application_guid + "'"
        Me.db.DeleteCommand.ExecuteNonQuery()
        For Each Item As scdItem In Me.Items
            If Item.Name = "" Then Continue For
            Me.db.InsertCommand.CommandText = "INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`, `AID`) VALUES ('" + I.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.Name) + "', '" + Item.ParentID.ToString + "', '" + Item.ItemType.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.AdvancedInfo.All.ToString) + "','" + Me.application_guid + "')"
            I = I + 1
            If I Mod 3 = 15 Then RaiseEvent NeedDoEvents()
            Me.db.InsertCommand.ExecuteScalar()
            status.scdValue = Convert.ToByte(100 / Me.Items.Length * I)
            RaiseEvent UpdateStatus(status)
        Next
    End Sub

    Public Function GetSupportedFileFormats() As Microsoft.VisualBasic.Collection Implements SkyCD.iFileFormat.GetSupportedFileFormats
        Dim Col As New Collection
        Col.Add("SkyCD Compressed Text Format|*.cscd")
        Return Col
    End Function

    Public ReadOnly Property HasConfig() As Boolean Implements SkyCD.ISkyCDPlugIn.HasConfig
        Get
            Return False
        End Get
    End Property

    Public Sub ShowConfig() Implements SkyCD.ISkyCDPlugIn.ShowConfig        
    End Sub

End Class
