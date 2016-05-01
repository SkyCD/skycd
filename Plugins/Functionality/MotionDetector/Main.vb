Imports System
Imports System.Data
Imports System.IO
Imports System.IO.Path
Imports System.Reflection.Assembly
Imports System.Drawing
Imports System.Data.OleDb
Imports System.Windows.Forms.Application

Public Class Main
    Implements Global.SkyCD.iInterfacePlugIn

    Private Debug As Boolean = False
    Private Parent As SkyCD.Form1

    Public Path As String = GetDirectoryName(GetExecutingAssembly().GetName().CodeBase().Remove(0, "file:\\\".Length).Replace("/", "\")) + "\motion\"
    Public SkyCDSettings As New SkyCD.SCD_XSettings("SkyCD")
    Public db As String = SkyCDSettings.ReadSetting("Database.Connection.String", "", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\temp.mdb")
    Public OleDbConnection1 As New OleDbConnection(db)
    Public odbcDatabase As New OleDbDataAdapter()

    Public Sub Create(ByVal Parent As SkyCD.Form1) Implements SkyCD.iInterfacePlugIn.Create
        Me.Parent = Parent
        With Parent
            .ToolsToolStripMenuItem.DropDownItems.Add("-")
            .ToolsToolStripMenuItem.DropDownItems.Add("Motion Detector", New Bitmap(16, 16), New EventHandler(AddressOf Me.OnClick)).MergeIndex = 0
            '.ToolsToolStripMenuItem.DropDownItems.Add("View File", New System.Drawing.Bitmap(16, 16), New System.EventHandler(AddressOf Me.OnClick2)).MergeIndex = 0
            .ToolsToolStripMenuItem.DropDownItems.Add("-")
        End With
    End Sub

    Public Buffer As New SkyCD_Simple.skycd_simple()
    Public aviID As Integer = Buffer.Add("Movies", SkyCD_Simple.skycd_simple.scdItemType.scdFolder)
    Public imgID As Integer = Buffer.Add("Pictures", SkyCD_Simple.skycd_simple.scdItemType.scdFolder)
    Public lastDate As Date = Date.Now.Date
    Public caID As Integer = Buffer.Add(lastDate, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, aviID)
    Public ciID As Integer = Buffer.Add(lastDate, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, imgID)


    Public Sub OnClick2(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Public Sub OnClick(ByVal sender As Object, ByVal e As EventArgs)
        'On Error Resume Next
        Try
            Dim FileName As String = Me.Path & "motion.exe"
            If ((New FileInfo(FileName)).Exists) Then
                ChDir(Me.Path)
                Dim pr As Process = Process.Start(FileName)
                Dim I As Integer
                'FindChanges()
                '                Me.odbcDatabase.InsertCommand.Connection = OleDbConnection1
                Me.odbcDatabase.InsertCommand = New OleDbCommand("", Me.OleDbConnection1)
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.ConnectionString = db
                    Me.OleDbConnection1.Open()
                End If
                Me.Parent.Enabled = False
                While (True)
                    If IsNothing(pr) Then Exit While
                    If pr.HasExited Then Exit While
                    FindChanges()
                    DoEvents()
                End While

                'Me.odbcDatabase.DeleteCommand = New System.Data.OleDb.OleDbCommand("DELETE FROM list WHERE AID =" + Me.Parent.Handle.ToInt64.ToString , Me.OleDbConnection1)
                'Try
                'Me.odbcDatabase.DeleteCommand.ExecuteScalar()
                'Catch ex As OleDb.OleDbException
                'MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
                'End Try
                'Me.Parent.Text = Me.Buffer.Items(0).Name                
                For Each Item As SkyCD_Simple.skycd_simple.scdItem In Me.Buffer.Items
                    If Item.Name = "" Then Continue For
                    Me.odbcDatabase.InsertCommand = New OleDbCommand("INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + I.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.Name) + "', '" + Item.ParentID.ToString + "', '" + Item.ItemType.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.AdvancedInfo.All.ToString) + "','" + Item.Size.ToString + "','" + Me.Parent.Handle.ToInt64.ToString + "')", Me.OleDbConnection1)
                    I = I + 1
                    If I Mod 3 = 15 Then DoEvents()
                    Try
                        Me.odbcDatabase.InsertCommand.ExecuteScalar()
                    Catch ex As OleDb.OleDbException
                        MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
                        Exit For
                    End Try
                    DoEvents()
                Next
                If Me.OleDbConnection1.State = ConnectionState.Open Then
                    Me.OleDbConnection1.Close()
                End If
                Me.Parent.UpdateTree()
                Me.Parent.Enabled = True
                'Me.Parent.lvBrowse.Items.Clear()
                'Me.Parent.UpdateList()
                'System.Windows.Forms.MessageBox.Show(Buffer.ToString())
                'If Me.Buffer.Items.Length > 0 Then
                'System.Windows.Forms.MessageBox.Show(Buffer.Items.Length)
                'For Each Item As SkyCD_Simple.skycd_simple.scdItem In Me.Buffer.Items
                'If Item.Name = "" Then Continue For
                '    System.Windows.Forms.MessageBox.Show(Item.Name)
                ' Me.odbcDatabase.InsertCommand = New System.Data.OleDb.OleDbCommand("INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + I.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.Name) + "', '" + Item.ParentID.ToString + "', '" + Item.ItemType.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.AdvancedInfo.All.ToString) + "','" + Item.Size.ToString + "','" + Me.Parent.Handle.ToInt64.ToString + "')", Me.OleDbConnection1)                
                'Me.Parent.lvBrowse.Items.Clear()
                'Me.Parent.UpdateList()
                'End If

                'If Me.OleDbConnection1.State = ConnectionState.Open Then
                'Me.OleDbConnection1.Close()
                'End If

                'Dim C As Collection = FindChanges()
                'Microsoft.VisualBasic.Interaction.Shell(FileName, AppWinStyle.MinimizedNoFocus)
                '//AddHandler Me.Changed.Created, AddressOf Me.fswImages_Created
                '//AddHandler Me.Changed.Changed, AddressOf Me.fswImages_Changed
            Else
                System.Windows.Forms.MessageBox.Show("Can't find needed files to run this plug-ins." + vbCrLf + " Please Check if File exists:" + FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Function FindChanges() As SkyCD_Simple.skycd_simple
        Static C() As FileInfo = (New DirectoryInfo(Me.Path)).GetFiles("*.*")
        Dim files() As FileInfo = (New DirectoryInfo(Me.Path)).GetFiles("*.*")
        If files.Length > C.Length Then            
            Dim I As Integer, id As Integer
            For I = 0 To files.Length - 1
                If Not IsInArray(C, files(I)) Then
                    If Not files(I).Exists Then Continue For
                    If Not lastDate.Day = Date.Now.Date.Day Then
                        lastDate = Date.Now.Date
                        caID = Buffer.Add(lastDate, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, aviID)
                        ciID = Buffer.Add(lastDate, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, imgID)
                    End If
                    Select Case LCase(SkyAdvancedFunctionsLibrary.File.GetFileExtension(files(I).FullName))
                        Case ".avi", ".mpg", ".wmv"
                            id = Buffer.Add(files(I).Name, SkyCD_Simple.skycd_simple.scdItemType.scdFile, caID)
                        Case ".jpg", ".bmp"
                            id = Buffer.Add(files(I).Name, SkyCD_Simple.skycd_simple.scdItemType.scdFile, ciID)
                    End Select
                    Buffer.Items(id).AdvancedInfo.Item("Name") = files(I).Name.ToString
                    Buffer.Items(id).AdvancedInfo.Item("Size") = files(I).Length * 8
                    Buffer.Items(id).AdvancedInfo.Item("LastWriteTime") = files(I).LastWriteTimeUtc
                    Buffer.Items(id).AdvancedInfo.Item("LastAccessTime") = files(I).LastAccessTimeUtc
                    Buffer.Items(id).AdvancedInfo.Item("IsReadOnly") = files(I).IsReadOnly
                    Buffer.Items(id).AdvancedInfo.Item("Attributes") = files(I).Attributes
                    Buffer.Items(id).AdvancedInfo.Item("CreationTime") = files(I).CreationTimeUtc
                    Buffer.Items(id).AdvancedInfo.Item("FullName") = files(I).FullName
                End If
                DoEvents()
            Next
            C = (New DirectoryInfo(Me.Path)).GetFiles("*.*")
            Return Nothing
        End If
        Return Nothing
    End Function

    Public Function IsInArray(ByVal C() As FileInfo, ByVal el As FileInfo) As Boolean
        Dim i As Integer
        For i = 0 To C.Length - 1
            If C(i).FullName = el.FullName Then Return True
        Next
        Return False
    End Function

    Public Property DebugMode() As Boolean Implements SkyCD.ISkyCDPlugIn.DebugMode
        Get
            Return Me.Debug
        End Get
        Set(ByVal value As Boolean)
            Me.Debug = value
        End Set
    End Property

    Public ReadOnly Property HasConfig() As Boolean Implements SkyCD.ISkyCDPlugIn.HasConfig
        Get
            Return False
        End Get
    End Property

    Public Sub ShowConfig() Implements SkyCD.ISkyCDPlugIn.ShowConfig
    End Sub
End Class
