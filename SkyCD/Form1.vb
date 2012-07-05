Public Class Form1

    Private SortBy As String = "type"

    Public WithEvents FileName As String = ""

    Private WithEvents FileStream As iFileFormat = Nothing

    'Friend Settings As New SCD_XSettings(My.Application.Info.AssemblyName)
    Public PlugInsSuport As New PlugInsSupport()

    Private Sub FileStream_DebugWrite(ByVal Text As Object) Handles FileStream.DebugWrite
        Debug.Print(Text.ToString)
    End Sub

    Private Sub FileStream_NeedDoEvents() Handles FileStream.NeedDoEvents
        Application.DoEvents()
    End Sub

    Private Sub FileStream_UpdateStatus(ByVal e As iFileFormat.scdStatus) Handles FileStream.UpdateStatus
        'Debug.Print(e.scdEvent.ToString)        
        Select Case e.scdEvent
            Case iFileFormat.scdStatus.scdProcedure.scdLoading
                If Not Me.tsProgressBar.Visible Then
                    Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Loading...")
                    Me.tsProgressBar.Minimum = 0
                    Me.tsProgressBar.Maximum = 100
                    Me.tsProgressBar.Value = 0
                    Me.tsProgressBar.Visible = True
                    Me.tsProgressBar.Width = Me.StatusStrip1.Width - Me.StatusStrip1.Items(0).Width - 10
                Else
                    Me.tsProgressBar.Value = e.scdValue
                End If
                Exit Select
            Case iFileFormat.scdStatus.scdProcedure.scdSaving
                If Not Me.tsProgressBar.Visible Then
                    Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Saving...")
                    Me.tsProgressBar.Minimum = 0
                    Me.tsProgressBar.Maximum = 100
                    Me.tsProgressBar.Value = 0
                    Me.tsProgressBar.Visible = True
                    Me.tsProgressBar.Style = ProgressBarStyle.Blocks
                    Me.tsProgressBar.Width = Me.StatusStrip1.Width - Me.StatusStrip1.Items(0).Width - 10
                Else
                    Me.tsProgressBar.Value = e.scdValue
                End If
                Exit Select
            Case iFileFormat.scdStatus.scdProcedure.scdDone
                Me.tsProgressBar.Visible = False
                Me.tsProgressBar.Style = ProgressBarStyle.Blocks
                Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Done.")
                Exit Select
            Case iFileFormat.scdStatus.scdProcedure.scdExporting
                If Me.StatusStrip1.Items(0).Text <> modGlobal.Translate(Me, "Updating...") Then
                    Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Updating...")
                    Me.tsProgressBar.Minimum = 0
                    Me.tsProgressBar.Maximum = 100
                    Me.tsProgressBar.Value = 0
                    Me.tsProgressBar.Visible = True
                    Me.tsProgressBar.Width = Me.StatusStrip1.Width - Me.StatusStrip1.Items(0).Width - 10
                    Application.DoEvents()
                Else
                    Me.tsProgressBar.Value = e.scdValue
                End If
                Exit Select
            Case iFileFormat.scdStatus.scdProcedure.scdImporting
                If Me.StatusStrip1.Items(0).Text <> modGlobal.Translate(Me, "Parsing...") Then
                    Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Parsing...")
                    Me.tsProgressBar.Minimum = 0
                    Me.tsProgressBar.Maximum = 100
                    Me.tsProgressBar.Value = 0
                    Me.tsProgressBar.Visible = True
                    Me.tsProgressBar.Width = Me.StatusStrip1.Width - Me.StatusStrip1.Items(0).Width - 10
                    Me.tsProgressBar.Style = ProgressBarStyle.Blocks
                    Application.DoEvents()
                Else
                    Me.tsProgressBar.Value = e.scdValue
                End If
                Exit Select
        End Select
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        Me.lvBrowse.Width = Me.SplitContainer1.Panel2.Width
        Me.tvTree.Width = Me.SplitContainer1.Panel1.Width
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me.saveToolStripButton.Enabled Then
            Dim Rez As MsgBoxResult = MsgBox(modGlobal.Translate(Me, "This collection was edited. Do you want to save it?"), MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation)
            If Rez = MsgBoxResult.Cancel Then Exit Sub
            If Rez = MsgBoxResult.Yes Then Me.SaveToolStripMenuItem_Click(sender, e)
        End If
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
        Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Left = modGlobal.Settings.ReadSetting("Left", "Window", Me.Left)
        Me.Top = modGlobal.Settings.ReadSetting("Top", "Window", Me.Top)
        Me.Width = modGlobal.Settings.ReadSetting("Width", "Window", Me.Width)
        Me.Height = modGlobal.Settings.ReadSetting("Height", "Window", Me.Height)

        Me.StatusStrip1.Visible = modGlobal.Settings.ReadSetting("StatusBar", "Window", Me.StatusStrip1.Visible)
        Me.StatusBarToolStripMenuItem.Checked = Me.StatusStrip1.Visible

        Select Case modGlobal.Settings.ReadSetting("ListView/View", "Window", "largeicon").ToString.ToLower
            Case "details"
                Me.DetailsToolStripMenuItem.Checked = True
                Me.TilesToolStripMenuItem.Checked = False
                Me.IconsToolStripMenuItem.Checked = False
                Me.ListToolStripMenuItem.Checked = False
                Me.LargeIconsToolStripMenuItem.Checked = False
                Me.lvBrowse.View = View.Details
            Case "list"
                Me.DetailsToolStripMenuItem.Checked = False
                Me.TilesToolStripMenuItem.Checked = False
                Me.IconsToolStripMenuItem.Checked = False
                Me.ListToolStripMenuItem.Checked = True
                Me.LargeIconsToolStripMenuItem.Checked = False
                Me.lvBrowse.View = View.List
            Case "tile"
                Me.DetailsToolStripMenuItem.Checked = False
                Me.TilesToolStripMenuItem.Checked = True
                Me.IconsToolStripMenuItem.Checked = False
                Me.ListToolStripMenuItem.Checked = False
                Me.LargeIconsToolStripMenuItem.Checked = False
                Me.lvBrowse.View = View.Tile
            Case "smallicon"
                Me.DetailsToolStripMenuItem.Checked = False
                Me.TilesToolStripMenuItem.Checked = False
                Me.IconsToolStripMenuItem.Checked = True
                Me.ListToolStripMenuItem.Checked = False
                Me.LargeIconsToolStripMenuItem.Checked = False
                Me.lvBrowse.View = View.SmallIcon
            Case "largeicon"
                Me.DetailsToolStripMenuItem.Checked = False
                Me.TilesToolStripMenuItem.Checked = False
                Me.IconsToolStripMenuItem.Checked = False
                Me.ListToolStripMenuItem.Checked = False
                Me.LargeIconsToolStripMenuItem.Checked = True
                Me.lvBrowse.View = View.LargeIcon
        End Select

        Select Case modGlobal.Settings.ReadSetting("ListView/SortBy", "Window", "name")
            Case "name"
                Me.FileNameToolStripMenuItem.Checked = True
                Me.TypeToolStripMenuItem.Checked = False
                Me.SortBy = "name"
            Case "type"
                Me.FileNameToolStripMenuItem.Checked = False
                Me.TypeToolStripMenuItem.Checked = True
                Me.SortBy = "type"
            Case "size"
                Me.FileNameToolStripMenuItem.Checked = False
                Me.TypeToolStripMenuItem.Checked = False
                Me.SortBy = "size"
        End Select

        Me.SaveToolStripMenuItem.Enabled = False
        Me.SaveAsToolStripMenuItem.Enabled = False

    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'Me.lvBrowse.Width = Me.SplitContainer1.Panel2.Width
        'Me.tvTree.Width = Me.SplitContainer1.Panel1.Width
        'Me.tvTree.Height = Me.SplitContainer1.Panel1.Height
        'Me.lvBrowse.Height = Me.SplitContainer1.Panel2.Height
        Me.tvTree.Top = 0
        Me.lvBrowse.Top = 0
        Me.tvTree.Left = 0
        Me.lvBrowse.Left = 0
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim dr As New System.Windows.Forms.DialogResult
        Dim ofdOpen As New System.Windows.Forms.OpenFileDialog()
        Me.PlugInsSuport.UpdatePlugInsList()
        ofdOpen.Filter = Me.PlugInsSuport.GetSupportedFileFormatsForRead()
        ofdOpen.Title = Translate(ofdOpen, "Open")
        ofdOpen.FilterIndex = modGlobal.Settings.ReadSetting("FilterIndex", "OpenDialog", 1)
        dr = ofdOpen.ShowDialog()
        If dr = Windows.Forms.DialogResult.Cancel Then Exit Sub
        'Me.FileStream = me.PlugInsSuport.GetHandlerForFile   System.Reflection.Assembly.LoadFrom(My.Application.Info.DirectoryPath & "\..\Plug-Ins\FileSupport_TextFormat.dll").CreateInstance("Main", True)
        Me.FileStream = Me.PlugInsSuport.GetHandlerForFile(ofdOpen.FileName)
        If IsNothing(Me.FileStream) Then
            MsgBox(modGlobal.Translate(Me, "Bad file format!"), MsgBoxStyle.Exclamation, modGlobal.Translate(Me, "Error"))
            Exit Sub
        End If
        modGlobal.Settings.WriteSetting("OpenDialog", "FilterIndex", ofdOpen.FilterIndex)
        Me.LoadFile(ofdOpen.FileName)
        ''MsgBox(TF.IsSupported(Me.ofdOpen.FileName), MsgBoxStyle.Information)
    End Sub

    Public Sub LoadFile(ByVal FileName As String)
        'Me.PlugInsSuport.UpdatePlugInsList()
        Me.FileStream = Me.PlugInsSuport.GetHandlerForFile(FileName)
        If Me.FileStream Is Nothing Then Exit Sub
        Me.MainMenuStrip.Enabled = False
        Me.tvTree.Enabled = False
        Me.tsToolBarFile.Enabled = False
        Me.tsToolBarFile.Enabled = False
        Me.lvBrowse.Enabled = False
        Me.ToolStripStatusLabel1.Text = Translate(Me, "Loading...")
        Me.FileStream.Database = Me.odbcDatabase
        Me.FileStream.ApplicationGUID = Me.Handle.ToInt64.ToString
        Me.tvTree.Nodes.Clear()
        Me.lvBrowse.Items.Clear()
        If Me.OleDbConnection1.State = ConnectionState.Closed Then Me.OleDbConnection1.Open()
        Me.FileStream.Load(FileName)
        Me.UpdateTree()
        Me.FileName = FileName
        Dim FI As New System.IO.FileInfo(Me.FileName)
        Me.Text = "SkyCD - " + FI.Name
        FI = Nothing
        Me.tvTree.SelectedNode = Me.tvTree.Nodes.Item(0)
        Me.tvTree.SelectedNode.Expand()
        Me.OleDbConnection1.Close()
        FileStream = Nothing
        Me.SaveToolStripMenuItem.Enabled = False
        Me.SaveAsToolStripMenuItem.Enabled = True
        Me.ToolStripStatusLabel1.Text = Translate(Me, "Done.")
        Me.MainMenuStrip.Enabled = True
        Me.tvTree.Enabled = True
        Me.tsToolBarFile.Enabled = True
        Me.tsToolBarFile.Enabled = True
        Me.lvBrowse.Enabled = True
    End Sub

    Public Function UpdateList(Optional ByVal ParentID As Integer = -1) As Boolean
        'On Error Resume Next
        Dim rez As System.Data.OleDb.OleDbDataReader
        Dim Ext As String
        Me.StatusStrip1.Text = modGlobal.Translate(Me, "Reading directory content...")
        Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Reading directory content...")
        'Me.tsProgressBar.Width = Me.StatusStrip1.Width - Me.StatusStrip1.Items(0).Width - 10
        'Me.tsProgressBar.Visible = True
        'Me.tsProgressBar.Value = 0        
        'Me.odbcDatabase.SelectCommand.Cancel()
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        If Me.SortBy.ToLower = "type" Then
            Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE ParentID = " + ParentID.ToString + " AND AID = '" + Me.Handle.ToInt64.ToString + "' ORDER BY Type DESC"
        Else
            Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE ParentID = " + ParentID.ToString + " AND AID = '" + Me.Handle.ToInt64.ToString + "' ORDER BY " + Me.SortBy
        End If
        'Me.odbcDatabase.SelectCommand.Cancel()
        Me.odbcDatabase.SelectCommand.Prepare()
        rez = Me.odbcDatabase.SelectCommand.ExecuteReader()
        Me.lvBrowse.Items.Clear()
        'Me.lvBrowse.LargeImageList = Nothing
        'Me.imlBrowseIcons.Images.Clear()           
        'If Me.imlBrowseIcons.Images.IndexOfKey("FOLDER") Then
        ' Me.imlBrowseIcons.Images.Add("FOLDER", My.Resources.NeededIcons.scdFolder)
        'End If
        Me.tsProgressBar.Maximum = 1
        Dim Nfo As SkyCD_Simple.scdProperties
        Dim tvar As String
        Do While rez.Read()
            Select Case rez.Item("Type").ToString
                Case "scdFile"
                    Ext = SkyAdvancedFunctionsLibrary.File.GetFileExtension(rez.Item("Name").ToString)
                    Nfo = New SkyCD_Simple.scdProperties(rez.Item("Properties").ToString)
                    tvar = Me.imlBrowseIcons.Images.IndexOfKey(rez.Item("Type").ToString & "/../" & rez.Item("Name").ToString)
                    If Nfo.Item("Icon").ToString <> "" Then
                        If tvar < 0 Then Me.imlBrowseIcons.Images.Add(tvar, SkyAdvancedFunctionsLibrary.File.GetFileIcon3(Nfo.Item("Icon").ToString))
                        Me.lvBrowse.Items.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString & "/../" & rez.Item("Name").ToString).SubItems.Add(SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Nfo.Item("Size")))
                    Else
                        If Me.imlBrowseIcons.Images.IndexOfKey(rez.Item("Type").ToString & "/" & Ext) < 0 Then
                            Me.imlBrowseIcons.Images.Add(rez.Item("Type").ToString & "/" & Ext, SkyAdvancedFunctionsLibrary.File.GetFileIcon(rez.Item("Name").ToString))
                        End If
                        Me.lvBrowse.Items.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString + "/" + Ext).SubItems.Add(SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Val(Nfo.Item("Size"))))
                    End If
                Case Else
                    If Not Me.imlBrowseIcons.Images.ContainsKey(rez.Item("Type").ToString) Then
                        Me.imlBrowseIcons.Images.Add(rez.Item("Type").ToString, My.Resources.NeededIcons.ResourceManager.GetObject(rez.Item("Type").ToString))
                    End If
                    Me.lvBrowse.Items.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString)
            End Select
            'Me.tsProgressBar.Value = Me.tsProgressBar.Value + 1
            'Me.tsProgressBar.Maximum = Me.tsProgressBar.Maximum + 1
        Loop
        If Me.lvBrowse.Items.Count > 0 Then
            Me.lvBrowse.SelectedItems.Clear()
            Me.lvBrowse.FocusedItem = Me.lvBrowse.Items(0)
            Me.lvBrowse.Select()
        End If
        'MsgBox(Me.lvBrowse.Items(0).ImageKey)
        'Me.lvBrowse.LargeImageList = Me.imlBrowseIcons
        'Me.lvBrowse.SmallImageList = Me.imlBrowseIcons
        rez.Close()
        Me.tsProgressBar.Visible = False
        Me.StatusStrip1.Text = modGlobal.Translate(Me, "Done.")
        Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Done.")
        Me.tvTree.SelectedNode.Expand()
        Return True
    End Function

    Public Sub UpdateTree(Optional ByVal ParentID As Integer = -1)
        Dim rez As System.Data.OleDb.OleDbDataReader
        Dim tnode As Windows.Forms.TreeNode = Nothing
        Dim tnodes() As Windows.Forms.TreeNode
        Dim chk As New Collection
        Me.StatusStrip1.Text = modGlobal.Translate(Me, "Reading directory tree...")
        Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Reading directory tree...")
        'Me.tsProgressBar.Visible = True
        'Me.tsProgressBar.Value = 0        
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE ParentID = " + ParentID.ToString + " AND (NOT (Type = 'scdFile')) AND AID = '" + Me.Handle.ToInt64.ToString + "'"
        Me.odbcDatabase.SelectCommand.Prepare()
        rez = Me.odbcDatabase.SelectCommand.ExecuteReader()
        If Not (ParentID = -1) Then
            tnodes = Me.tvTree.Nodes.Find(ParentID.ToString, True)
            tnode = tnodes(0)
        End If
        Me.tsProgressBar.Maximum = rez.RecordsAffected
        Do While rez.Read()
            If Not Me.imlTreeIcons.Images.ContainsKey(rez.Item("Type").ToString) Then
                Me.imlTreeIcons.Images.Add(rez.Item("Type").ToString, My.Resources.NeededIcons.ResourceManager.GetObject(rez.Item("Type").ToString))
            End If
            If ParentID = -1 Then
                Me.tvTree.Nodes.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString, rez.Item("Type").ToString).EnsureVisible()
            Else
                tnode.Nodes.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString, rez.Item("Type").ToString).EnsureVisible()
            End If
            Me.tsProgressBar.Value = Me.tsProgressBar.Value + 0.5
            chk.Add(rez.Item("ID").ToString)
        Loop
        rez.Close()
        For Each This As String In chk
            Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE ParentID = " + This.ToString + " AND (NOT (Type = 'scdFile')) AND AID = '" + Me.Handle.ToInt64.ToString + "'"
            Me.odbcDatabase.SelectCommand.Prepare()
            rez = Me.odbcDatabase.SelectCommand.ExecuteReader()
            tnodes = Me.tvTree.Nodes.Find(This, True)
            If IsNothing(tnodes) Then
                rez.Close()
                Continue For
            End If
            tnode = tnodes(LBound(tnodes))
            If rez.HasRows Then
                Do While rez.Read()
                    If Not Me.imlTreeIcons.Images.ContainsKey(rez.Item("Type").ToString) Then
                        Me.imlTreeIcons.Images.Add(rez.Item("Type").ToString, My.Resources.NeededIcons.ResourceManager.GetObject(rez.Item("Type").ToString))
                    End If
                    If Not Me.imlTreeIcons.Images.ContainsKey(rez.Item("Type").ToString & "_selected") Then
                        tnode.Nodes.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString, rez.Item("Type").ToString)
                    Else
                        tnode.Nodes.Add(rez.Item("ID").ToString, rez.Item("Name").ToString, rez.Item("Type").ToString, rez.Item("Type").ToString & "_selected")
                    End If
                Loop
            End If
            rez.Close()
            Me.tsProgressBar.Value = Me.tsProgressBar.Value + 0.5
        Next
        'Me.tsProgressBar.Visible = False
        Me.StatusStrip1.Text = modGlobal.Translate(Me, "Done.")
        Me.StatusStrip1.Items(0).Text = modGlobal.Translate(Me, "Done.")
        Me.SplitContainer1.Panel1Collapsed = Me.tvTree.GetNodeCount(True) = 1
    End Sub

    Private Sub tvTree_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvTree.AfterExpand
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        e.Node.Nodes.Clear()
        Me.UpdateTree(e.Node.Name)
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub tvTree_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvTree.AfterSelect
        If IsNothing(e.Node) Then Exit Sub
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.UpdateList(e.Node.Name)
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub lvBrowse_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvBrowse.ItemActivate
        Dim tnodes() As Windows.Forms.TreeNode = Me.tvTree.SelectedNode.Nodes.Find(Me.lvBrowse.FocusedItem.Name, False)
        If tnodes.Length > 0 Then Me.tvTree.SelectedNode = tnodes(0)
        Exit Sub
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip1.Visible = Me.StatusBarToolStripMenuItem.Checked
        modGlobal.Settings.WriteSetting("Window", "StatusBar", Me.StatusStrip1.Visible)
        Me.Form1_Resize(sender, e)
    End Sub

    Private Sub TilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TilesToolStripMenuItem.Click
        Me.lvBrowse.View = View.Tile
        Me.DetailsToolStripMenuItem.Checked = False
        Me.TilesToolStripMenuItem.Checked = True
        Me.IconsToolStripMenuItem.Checked = False
        Me.ListToolStripMenuItem.Checked = False
        Me.LargeIconsToolStripMenuItem.Checked = False
        modGlobal.Settings.WriteSetting("Window", "ListView/View", "Tile")
    End Sub

    Private Sub IconsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IconsToolStripMenuItem.Click
        Me.lvBrowse.View = View.SmallIcon
        Me.DetailsToolStripMenuItem.Checked = False
        Me.TilesToolStripMenuItem.Checked = False
        Me.IconsToolStripMenuItem.Checked = True
        Me.ListToolStripMenuItem.Checked = False
        Me.LargeIconsToolStripMenuItem.Checked = False
        modGlobal.Settings.WriteSetting("Window", "ListView/View", "SmallIcon")
    End Sub

    Private Sub LargeIconsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LargeIconsToolStripMenuItem.Click
        Me.lvBrowse.View = View.LargeIcon
        Me.DetailsToolStripMenuItem.Checked = False
        Me.TilesToolStripMenuItem.Checked = False
        Me.IconsToolStripMenuItem.Checked = False
        Me.ListToolStripMenuItem.Checked = False
        Me.LargeIconsToolStripMenuItem.Checked = True
        modGlobal.Settings.WriteSetting("Window", "ListView/View", "LargeIcon")
    End Sub

    Private Sub ListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListToolStripMenuItem.Click
        Me.lvBrowse.View = View.List
        Me.DetailsToolStripMenuItem.Checked = False
        Me.TilesToolStripMenuItem.Checked = False
        Me.IconsToolStripMenuItem.Checked = False
        Me.ListToolStripMenuItem.Checked = True
        Me.LargeIconsToolStripMenuItem.Checked = False
        modGlobal.Settings.WriteSetting("Window", "ListView/View", "List")
    End Sub

    Private Sub DetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailsToolStripMenuItem.Click
        Me.lvBrowse.View = View.Details
        Me.DetailsToolStripMenuItem.Checked = True
        Me.TilesToolStripMenuItem.Checked = False
        Me.IconsToolStripMenuItem.Checked = False
        Me.ListToolStripMenuItem.Checked = False
        Me.LargeIconsToolStripMenuItem.Checked = False
        modGlobal.Settings.WriteSetting("Window", "ListView/View", "Details")
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Me.RefreshData()
    End Sub

    Public Sub RefreshData()
        If IsNothing(Me.tvTree.SelectedNode) Then Exit Sub
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.tvTree.SelectedNode.Nodes.Clear()
        Me.UpdateTree(Me.tvTree.SelectedNode.Name)
        Me.UpdateList(Me.tvTree.SelectedNode.Name)
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub FileNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileNameToolStripMenuItem.Click
        Me.FileNameToolStripMenuItem.Checked = True
        Me.TypeToolStripMenuItem.Checked = False
        Me.SortBy = "name"
        modGlobal.Settings.WriteSetting("Window", "ListView/SortBy", Me.SortBy)
        Me.RefreshData()
    End Sub

    Private Sub TypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TypeToolStripMenuItem.Click
        Me.FileNameToolStripMenuItem.Checked = False
        Me.TypeToolStripMenuItem.Checked = True
        Me.SortBy = "type"
        modGlobal.Settings.WriteSetting("Window", "ListView/SortBy", Me.SortBy)
        Me.RefreshData()
    End Sub

    Private Sub Form1_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        If Me.WindowState = FormWindowState.Normal Then
            modGlobal.Settings.WriteSetting("Window", "Left", Me.Left)
            modGlobal.Settings.WriteSetting("Window", "Top", Me.Top)
            modGlobal.Settings.WriteSetting("Window", "Width", Me.Width)
            modGlobal.Settings.WriteSetting("Window", "Height", Me.Height)
        End If
        modGlobal.Settings.WriteSetting("Window", "State", Me.WindowState)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim Frx As New Form1()
        Frx.Left = Me.Left + 50
        Frx.Top = Me.Left + 50
        Frx.Show()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If Me.FileName = "" Then Me.SaveAsToolStripMenuItem_Click(sender, e)
        Me.SaveFile(Me.FileName)
    End Sub

    Public Sub SaveFile(ByVal FileName As String)
        Me.FileStream = Me.PlugInsSuport.Load(Of iFileFormat)(Me.PlugInsSuport.LoadedPlugIn)
        If Me.FileStream.IsExactFormat = False Then
            Select Case MsgBox(Translate(Me, "You selected to save this file in format, witch can't correctly store all features supported by this " + Application.ProductName + " version. If you are using some of this features (for example: file comments), some data can be lost. Do you want to continue?"), MsgBoxStyle.Information Or MsgBoxStyle.YesNoCancel, Translate(Me, "File Save"))
                Case MsgBoxResult.No
                    Me.FileStream = Nothing
                    Me.SaveAsToolStripMenuItem_Click(Me, New System.EventArgs())
                    Exit Sub
                Case MsgBoxResult.Cancel
                    Me.FileStream = Nothing
                    Exit Sub
            End Select
        End If
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.cmsContext.Enabled = False
        Me.tsToolBarFile.Enabled = False
        Me.MainMenuStrip.Enabled = False
        Me.FileStream.ApplicationGUID = Me.Handle.ToInt64.ToString
        Me.FileStream.Database = Me.odbcDatabase
        Me.FileStream.Save(FileName)
        If Me.FileName <> FileName Then Me.FileName = FileName
        Me.cmsContext.Enabled = True
        Me.tsToolBarFile.Enabled = True
        Me.MainMenuStrip.Enabled = True
        Me.FileStream = Nothing
        Me.saveToolStripButton.Enabled = False
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim fsDialog As New System.Windows.Forms.SaveFileDialog()
        With fsDialog
            Me.PlugInsSuport.UpdatePlugInsList()
            .CheckFileExists = False
            .CheckPathExists = True
            .FileName = Me.FileName
            .Title = modGlobal.Translate(fsDialog, "Save As")
            .ValidateNames = True
            .Filter = Me.PlugInsSuport.GetSupportedFileFormatsForWrite
            .FilterIndex = modGlobal.Settings.ReadSetting("FilterIndex", "SaveAsDialog", 1)
            If .ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Me.FileName = .FileName
            If Me.OleDbConnection1.State = ConnectionState.Closed Then
                Me.OleDbConnection1.Open()
            End If
            modGlobal.Settings.WriteSetting("SaveAsDialog", "FilterIndex", .FilterIndex)
            Me.tvTree.Enabled = False
            Me.lvBrowse.Enabled = False
            Me.MainMenuStrip.Enabled = False
            Me.FileStream = Me.PlugInsSuport.GetHandlerForFilterIndex(.FilterIndex)
            If Me.FileStream.IsExactFormat = False Then
                Select Case MsgBox(Translate(Me, "You selected to save this file in format, witch can't correctly store all features supported by this " + Application.ProductName + " version. If you are using some of this features (for example: file comments), some data can be lost. Do you want to continue?"), MsgBoxStyle.Information Or MsgBoxStyle.YesNoCancel, Translate(Me, "File Save"))
                    Case MsgBoxResult.No
                        Me.FileStream = Nothing
                        Me.lvBrowse.Enabled = True
                        Me.tvTree.Enabled = True
                        Me.MainMenuStrip.Enabled = True
                        Me.saveToolStripButton.Enabled = False
                        Me.SaveAsToolStripMenuItem_Click(Me, New System.EventArgs())
                        Exit Sub
                    Case MsgBoxResult.Cancel
                        Me.FileStream = Nothing
                        Me.lvBrowse.Enabled = True
                        Me.tvTree.Enabled = True
                        Me.MainMenuStrip.Enabled = True
                        Me.saveToolStripButton.Enabled = False
                        Exit Sub
                End Select
            End If
            Me.FileStream.ApplicationGUID = Me.Handle.ToInt64.ToString
            Me.FileStream.Database = Me.odbcDatabase
            Me.FileStream.Save(.FileName)
            Dim FI As New System.IO.FileInfo(.FileName)
            Me.Text = "SkyCD - " + FI.Name
            FI = Nothing
            If Me.OleDbConnection1.State = ConnectionState.Open Then
                Me.OleDbConnection1.Close()
            End If
            Me.FileStream = Nothing
            Me.lvBrowse.Enabled = True
            Me.tvTree.Enabled = True
            Me.MainMenuStrip.Enabled = True
            Me.saveToolStripButton.Enabled = False
        End With
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        If Me.saveToolStripButton.Enabled Then
            Dim Rez As MsgBoxResult = MsgBox(modGlobal.Translate(Me, "This collection was edited. Do you want to save it?"), MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation)
            If Rez = MsgBoxResult.Cancel Then Exit Sub
            If Rez = MsgBoxResult.Yes Then Me.SaveToolStripMenuItem_Click(sender, e)
        End If
        Application.Exit()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        'MsgBox(My.Application.Info.DirectoryPath & "\Languages\" & SkyCD.modGlobal.Settings.ReadSetting("Language", , "English") & ".xml", , "language")
        Me.PlugInsSuport.LoadStartupPlugIns(Me)

        Me.TranslateMenu(Me.MenuStrip1)
        Me.TranslateMenu(Me.cmsContext.Items)
        Me.newToolStripButton.Text = Translate(Me, Me.newToolStripButton.Text)
        Me.openToolStripButton.Text = Translate(Me, Me.openToolStripButton.Text)
        Me.saveToolStripButton.Text = Translate(Me, Me.saveToolStripButton.Text)
        Me.helpToolStripButton.Text = Translate(Me, Me.helpToolStripButton.Text)
        Me.ToolStripStatusLabel1.Text = Translate(Me, Me.ToolStripStatusLabel1.Text)

        Exit Sub
        Me.Show()

        If modGlobal.CanCreateForms = False Then Exit Sub
        If My.Application.CommandLineArgs.Count > 0 Then
            modGlobal.CanCreateForms = False
            Dim FI As System.IO.FileInfo = New System.IO.FileInfo(My.Application.CommandLineArgs.Item(0))
            If FI.Exists Then Me.LoadFile(FI.FullName)
            Dim I As Integer
            For I = 1 To My.Application.CommandLineArgs.Count - 1
                My.Application.OpenNew(My.Application.CommandLineArgs.Item(I))
            Next
        End If
    End Sub

    Public Sub TranslateMenu(ByVal Control As System.Windows.Forms.MenuStrip)
        Dim I As Integer, Obj As ToolStripMenuItem
        For I = 0 To Control.Items.Count - 1
            Try
                Obj = Control.Items.Item(I)
                Obj.Text = modGlobal.Translate(Me, Control.Items.Item(I).Text)
                Me.TranslateMenu(Obj.DropDownItems)
            Catch ex As Exception

            End Try
            'Me.TranslateMenu(Control.Items.Item(I).ToolStripItemAccessibleObjec)
        Next
    End Sub

    Public Sub TranslateMenu(ByVal Control As System.Windows.Forms.ToolStripItemCollection)
        Dim I As Integer, Obj As ToolStripMenuItem
        If Control.Count = 0 Then Exit Sub
        For I = 0 To Control.Count - 1
            Control.Item(I).Text = modGlobal.Translate(Me, Control.Item(I).Text)
            Try
                Obj = Control.Item(I)
                Obj.Text = modGlobal.Translate(Me, Control.Item(I).Text)
                Me.TranslateMenu(Obj.DropDownItems)
            Catch ex As Exception

            End Try
        Next
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        About.ShowDialog()
    End Sub

    Private Sub newToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripButton.Click
        Me.NewToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub openToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles openToolStripButton.Click
        Me.OpenToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub saveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveToolStripButton.Click
        Me.SaveToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub AddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToolStripMenuItem.Click
        Dim AddToList As New frmAddToList()
        Dim I As Integer = 0
        If AddToList.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        Dim Adding As New frmAdding()
        Me.Enabled = False
        Try
            If AddToList.tsbFromFolder.Checked Or AddToList.tsbFromCDROM.Checked Then
                Dim Folders As New Collection
                Dim IDs As New Collection
                Dim DInfo As System.IO.DirectoryInfo
                Dim Buffer As New SkyCD_Simple.skycd_simple()
                Dim ID As Integer
                Adding.Show(Me)
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.Open()
                End If
                Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Dim rez As System.Data.OleDb.OleDbDataReader = Me.odbcDatabase.SelectCommand.ExecuteReader()
                Adding.DoIt(modGlobal.Translate(Me, "Preparing database for modifications..."), Nothing)
                Do While rez.Read()
                    Select Case rez.Item("Type").ToString.ToLower
                        Case "scdfile"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdFile, Val(rez.Item("ParentID")))
                        Case "scdcddisk"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdCDDisk, Val(rez.Item("ParentID")))
                        Case "scdfolder"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, Val(rez.Item("ParentID")))
                        Case "scdnetworkrecource"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdNetworkRecource, Val(rez.Item("ParentID")))
                        Case Else
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdUnknown, Val(rez.Item("ParentID")))
                    End Select
                    Buffer.Items(ID).Size = Val(rez.Item("Size"))
                    Buffer.Items(ID).AdvancedInfo.All = rez.Item("Properties").ToString
                Loop
                rez.Close()
                If AddToList.rbAllContentAddToSelectedMediaFolder.Checked Then
                    If IsNothing(Me.tvTree.SelectedNode) Then
                        ID = -1
                    Else
                        ID = Me.tvTree.SelectedNode.Name
                    End If
                Else
                    ID = -1
                End If
                If Buffer.Exists(AddToList.txtMediaName.Text, ID) > -1 Then
                    Adding.Close()
                    MsgBox(modGlobal.Translate(Me, "There already exists media named exatcly in selected place"), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, modGlobal.Translate(Me, "Error"))
                    Me.Enabled = True
                    Exit Sub
                End If
                Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
                If AddToList.tsbFromFolder.Checked Then
                    DInfo = New System.IO.DirectoryInfo(AddToList.txtSelexc.Text)
                    ID = Buffer.Add(AddToList.txtMediaName.Text, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, ID)
                    IDs.Add(ID)
                    If AddToList.chkIncludeMediaInfo.Checked Then
                        Buffer.Items(ID).AdvancedInfo.Item("RealDirectoryFullName") = DInfo.FullName
                        Buffer.Items(ID).AdvancedInfo.Item("RealDirectoryName") = DInfo.Name
                        Buffer.Items(ID).AdvancedInfo.Item("CreationTime") = DInfo.CreationTimeUtc
                        Buffer.Items(ID).AdvancedInfo.Item("LastAccessTime") = DInfo.LastAccessTimeUtc
                        Buffer.Items(ID).AdvancedInfo.Item("LastWriteTime") = DInfo.LastWriteTimeUtc
                        Buffer.Items(ID).AdvancedInfo.Item("Attributes") = DInfo.Attributes
                    End If
                    Folders.Add(AddToList.txtSelexc.Text)
                Else
                    Dim DSK As New System.IO.DriveInfo(AddToList.cmbDrives.Text.Substring(0, 2))
                    ID = Buffer.Add(AddToList.txtMediaName.Text, SkyCD_Simple.skycd_simple.scdItemType.scdCDDisk, ID)
                    IDs.Add(ID)
                    If AddToList.chkIncludeExtendedInfo.Checked Then
                        Buffer.Items(ID).AdvancedInfo.Item("Title") = DSK.Name
                        Buffer.Items(ID).AdvancedInfo.Item("AvailableFreeSpace") = DSK.AvailableFreeSpace * 8
                        Buffer.Items(ID).AdvancedInfo.Item("RootDirectory") = DSK.RootDirectory.ToString
                        Buffer.Items(ID).AdvancedInfo.Item("TotalFreeSpace") = DSK.TotalFreeSpace * 8
                        Buffer.Items(ID).AdvancedInfo.Item("TotalSize") = DSK.TotalSize * 8
                        Buffer.Items(ID).AdvancedInfo.Item("VolumeLabel") = DSK.VolumeLabel
                    End If
                    Folders.Add(DSK.RootDirectory)
                End If
                Adding.pbProgress.Minimum = 0
                Do
                    DInfo = New System.IO.DirectoryInfo(Folders(I + 1).ToString)
                    Adding.DoIt(Strings.Format(DInfo.Name, Translate(Adding, "Reading '{0}'...")), Nothing)
                    If Not (AddToList.tsbFromFolder.Checked And AddToList.chkIncludeSubFolders.Checked = False) Then
                        For Each That As System.IO.DirectoryInfo In DInfo.GetDirectories()
                            Folders.Add(That.FullName)
                            ID = Buffer.Add(That.Name, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, IDs(I + 1))
                            IDs.Add(ID)
                            If AddToList.chkIncludeExtendedInfo.Checked Then
                                Buffer.Items(ID).AdvancedInfo.Item("RealDirectoryFullName") = That.FullName
                                Buffer.Items(ID).AdvancedInfo.Item("RealDirectoryName") = That.Name
                                Buffer.Items(ID).AdvancedInfo.Item("CreationTime") = That.CreationTimeUtc
                                Buffer.Items(ID).AdvancedInfo.Item("LastAccessTime") = That.LastAccessTimeUtc
                                Buffer.Items(ID).AdvancedInfo.Item("LastWriteTime") = That.LastWriteTimeUtc
                                Buffer.Items(ID).AdvancedInfo.Item("Attributes") = That.Attributes
                            End If
                        Next
                    End If
                    For Each That As System.IO.FileInfo In DInfo.GetFiles()
                        ID = Buffer.Add(That.Name, SkyCD_Simple.skycd_simple.scdItemType.scdFile, IDs(I + 1))
                        If AddToList.chkIncludeExtendedInfo.Checked Then
                            Buffer.Items(ID).AdvancedInfo.Item("Name") = That.Name.ToString
                            Buffer.Items(ID).AdvancedInfo.Item("Size") = That.Length * 8
                            Buffer.Items(ID).AdvancedInfo.Item("LastWriteTime") = That.LastWriteTimeUtc
                            Buffer.Items(ID).AdvancedInfo.Item("LastAccessTime") = That.LastAccessTimeUtc
                            Buffer.Items(ID).AdvancedInfo.Item("IsReadOnly") = That.IsReadOnly
                            Buffer.Items(ID).AdvancedInfo.Item("Attributes") = That.Attributes
                            Buffer.Items(ID).AdvancedInfo.Item("CreationTime") = That.CreationTimeUtc
                            If That.Extension.ToLower = ".exe" Or That.Extension.ToLower = ".cur" Or That.Extension.ToLower = ".ico" Then Buffer.Items(ID).AdvancedInfo.Item("Icon") = SkyAdvancedFunctionsLibrary.File.GetFileIcon2(That.FullName)
                        End If
                    Next
                    I = I + 1
                    Application.DoEvents()
                Loop Until I >= Folders.Count
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.Open()
                End If
                I = LBound(Buffer.Items)
                Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
                For Each Item As SkyCD_Simple.skycd_simple.scdItem In Buffer.Items
                    If Item.Name = "" Then Continue For
                    Me.odbcDatabase.InsertCommand.CommandText = "INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + I.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.Name) + "', '" + Item.ParentID.ToString + "', '" + Item.ItemType.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.AdvancedInfo.All.ToString) + "','" + Item.Size.ToString + "','" + Me.Handle.ToInt64.ToString + "')"
                    I = I + 1
                    If I Mod 3 = 15 Then Application.DoEvents()
                    Try
                        Me.odbcDatabase.InsertCommand.ExecuteScalar()
                    Catch ex As OleDb.OleDbException
                        MsgBox(Translate("Errors", ex.Message), MsgBoxStyle.Critical, Translate(Me, "Error"))
                        Me.Enabled = True
                        Exit Sub
                    End Try
                Next
                If Me.OleDbConnection1.State = ConnectionState.Open Then
                    Me.OleDbConnection1.Close()
                End If
                Me.tvTree.Nodes.Clear()
                Me.UpdateTree()
                Me.tvTree.SelectedNode = Me.tvTree.Nodes.Item(0)
                Adding.Close()
                'MsgBox(Buffer.Items.Length)
            ElseIf AddToList.tsbFromInternet.Checked Then
                Dim Url As New System.Uri(AddToList.txtEnterInternetAdress.Text)
                If Url.Scheme.ToLower <> "ftp" Then
                    MsgBox(Translate(Me, "Only File Transfer Protocol currently is supported"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
                    Me.Enabled = True
                    Exit Sub
                End If
                Dim Login As New frmLogin()
                If Login.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                    MsgBox(Translate(Me, "Because you are pressed cancel, other operations are not completed"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
                    Me.Enabled = True
                    Exit Sub
                End If
                If Login.UsernameTextBox.Text.Length = 0 Then
                    MsgBox(Translate(Me, "Bad username"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
                    Me.Enabled = True
                    Exit Sub
                End If
                If Login.PasswordTextBox.Text.Length = 0 Then
                    Me.Enabled = True
                    MsgBox(Translate(Me, "Bad password"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
                    Exit Sub
                End If
                Adding.Show(Me)
                Dim FTP As New SkyAdvancedFunctionsLibrary.Network.FTP.FTPclient(AddToList.txtEnterInternetAdress.Text, Login.UsernameTextBox.Text, Login.PasswordTextBox.Text)
                Dim Folders As New Collection
                Dim IDs As New Collection
                Dim Buffer As New SkyCD_Simple.skycd_simple()
                Dim ID As Integer
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.Open()
                End If
                Me.odbcDatabase.SelectCommand.CommandText = "SELECT * FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Dim rez As System.Data.OleDb.OleDbDataReader = Me.odbcDatabase.SelectCommand.ExecuteReader()
                Adding.DoIt(modGlobal.Translate(Me, "Preparing database for modifications..."), Nothing)
                Do While rez.Read()
                    Select Case rez.Item("Type").ToString.ToLower
                        Case "scdfile"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdFile, Val(rez.Item("ParentID")))
                        Case "scdcddisk"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdCDDisk, Val(rez.Item("ParentID")))
                        Case "scdfolder"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, Val(rez.Item("ParentID")))
                        Case "scdnetworkrecource"
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdNetworkRecource, Val(rez.Item("ParentID")))
                        Case Else
                            ID = Buffer.Add(rez.Item("Name").ToString, SkyCD_Simple.skycd_simple.scdItemType.scdUnknown, Val(rez.Item("ParentID")))
                    End Select
                    Buffer.Items(ID).Size = Val(rez.Item("Size"))
                    Buffer.Items(ID).AdvancedInfo.All = rez.Item("Properties").ToString
                Loop
                rez.Close()
                If AddToList.rbAllContentAddToSelectedMediaFolder.Checked Then
                    If IsNothing(Me.tvTree.SelectedNode) Then
                        ID = -1
                    Else
                        ID = Me.tvTree.SelectedNode.Name
                    End If
                Else
                    ID = -1
                End If
                If Buffer.Exists(AddToList.txtMediaName.Text, ID) > -1 Then
                    Adding.Close()
                    MsgBox(modGlobal.Translate(Me, "There already exists media named exatcly in selected place"), MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, modGlobal.Translate(Me, "Error"))
                    Me.Enabled = True
                    Exit Sub
                End If
                Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
                ID = Buffer.Add(AddToList.txtMediaName.Text, SkyCD_Simple.skycd_simple.scdItemType.scdNetworkRecource, ID)
                If AddToList.chkIncludeExtendedInfo.Checked Then
                    Buffer.Items(ID).AdvancedInfo.Item("URL") = Url.ToString
                End If
                IDs.Add(ID)
                Folders.Add(FTP.CurrentDirectory)
                Adding.pbProgress.Minimum = 0
                Dim entry As SkyAdvancedFunctionsLibrary.Network.FTP.FTPdirectory
                Dim O As Integer
                I = 1
                Adding.DoIt(Translate(Adding, "Reading directory from remote server..."), Nothing)
                Do
                    entry = FTP.ListDirectoryDetail(Folders.Item(I))
                    For O = 0 To entry.Count - 1
                        Application.DoEvents()
                        Select Case entry.Item(O).FileType
                            Case SkyAdvancedFunctionsLibrary.Network.FTP.FTPfileInfo.DirectoryEntryTypes.File
                                Try
                                    ID = Buffer.Add(entry.Item(O).Filename, SkyCD_Simple.skycd_simple.scdItemType.scdFile, IDs.Item(I))
                                Catch ex As Exception

                                End Try
                                If AddToList.chkIncludeExtendedInfo.Checked Then
                                    Buffer.Items(ID).AdvancedInfo.Item("RealPath") = entry.Item(O).Path
                                    Buffer.Items(ID).AdvancedInfo.Item("FullName") = entry.Item(O).FullName
                                    Buffer.Items(ID).AdvancedInfo.Item("Permission") = entry.Item(O).Permission
                                    Buffer.Items(ID).AdvancedInfo.Item("Size") = entry.Item(O).Size * 8
                                    Buffer.Items(ID).AdvancedInfo.Item("FileDateTime") = entry.Item(O).FileDateTime
                                End If
                            Case SkyAdvancedFunctionsLibrary.Network.FTP.FTPfileInfo.DirectoryEntryTypes.Directory
                                ID = Buffer.Add(entry.Item(O).Filename, SkyCD_Simple.skycd_simple.scdItemType.scdFolder, IDs.Item(I))
                                IDs.Add(ID)
                                Folders.Add(entry.Item(O).FullName)
                                If AddToList.chkIncludeExtendedInfo.Checked Then
                                    Buffer.Items(ID).AdvancedInfo.Item("RealPath") = entry.Item(O).Path
                                    Buffer.Items(ID).AdvancedInfo.Item("FullName") = entry.Item(O).FullName
                                    Buffer.Items(ID).AdvancedInfo.Item("Permission") = entry.Item(O).Permission
                                    Buffer.Items(ID).AdvancedInfo.Item("FileDateTime") = entry.Item(O).FileDateTime
                                End If
                        End Select
                    Next
                    Adding.DoIt(Strings.Format(Folders.Item(I), Translate(Adding, "Reading '{0}'...")), Nothing)
                    I = I + 1
                    Application.DoEvents()
                Loop Until I >= Folders.Count
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.Open()
                End If
                I = LBound(Buffer.Items)
                Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "'"
                Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
                For Each Item As SkyCD_Simple.skycd_simple.scdItem In Buffer.Items
                    If Item.Name = "" Then Continue For
                    Me.odbcDatabase.InsertCommand.CommandText = "INSERT INTO list (`ID`, `Name`, `ParentID`, `Type`, `Properties`,`Size`, `AID`) VALUES ('" + I.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.Name) + "', '" + Item.ParentID.ToString + "', '" + Item.ItemType.ToString + "', '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Item.AdvancedInfo.All.ToString) + "','" + Item.Size.ToString + "','" + Me.Handle.ToInt64.ToString + "')"
                    I = I + 1
                    If I Mod 3 = 15 Then Application.DoEvents()
                    Try
                        Me.odbcDatabase.InsertCommand.ExecuteScalar()
                    Catch ex As OleDb.OleDbException
                        MsgBox(Translate("Errors", ex.Message), MsgBoxStyle.Critical, Translate(Me, "Error"))
                        Me.Enabled = True
                        Exit Sub
                    End Try
                Next
                If Me.OleDbConnection1.State = ConnectionState.Open Then
                    Me.OleDbConnection1.Close()
                End If
                Me.tvTree.Nodes.Clear()
                Me.UpdateTree()
                Me.tvTree.SelectedNode = Me.tvTree.Nodes.Item(0)
                Adding.Close()
                'MsgBox(Buffer.Items.Length)
            End If
        Catch ex As System.IO.DirectoryNotFoundException
            If (Adding Is Nothing) = False Then Adding.Close()
            MsgBox(Translate(Me, "Directory not found."), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
        Catch ex As System.IO.PathTooLongException
            If (Adding Is Nothing) = False Then Adding.Close()
            MsgBox(Translate(Me, "Your entered path is too long."), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
        Catch ex As System.IO.DriveNotFoundException
            If (Adding Is Nothing) = False Then Adding.Close()
            MsgBox(Translate(Me, "Drive not found."), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
        Catch ex As System.IO.FileNotFoundException
            If (Adding Is Nothing) = False Then Adding.Close()
            MsgBox(Translate(Me, "File not found."), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
        End Try
        Me.saveToolStripButton.Enabled = True
        Me.Enabled = True
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim I As Integer
        If Me.lvBrowse.Focused Then
            For I = 0 To Me.lvBrowse.SelectedItems.Count - 1
                Me.Delete(Me.lvBrowse.SelectedItems(I).Name)
            Next
            Dim Text As String = Me.tvTree.SelectedNode.Name
            Me.lvBrowse.Items.Clear()
            Me.UpdateList(Text)
            Try
                Me.tvTree.Nodes.Clear()
                Me.UpdateTree()
                Me.tvTree.SelectedNode.Expand()
            Catch ex As Exception

            End Try
        Else
            Me.Delete(Me.tvTree.SelectedNode.Name)
            Me.tvTree.Nodes.Clear()
            Me.UpdateTree()
        End If
        Me.saveToolStripButton.Enabled = True
    End Sub

    Public Sub Delete(ByVal ID As Integer)
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "' AND ID = " + ID.ToString + ""
        Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
        Me.odbcDatabase.DeleteCommand.CommandText = "DELETE FROM list WHERE AID = '" + Me.Handle.ToInt64.ToString + "' AND ParentID = " + ID.ToString
        Me.odbcDatabase.DeleteCommand.ExecuteNonQuery()
        If Me.OleDbConnection1.State = ConnectionState.Open Then
            Me.OleDbConnection1.Close()
        End If
    End Sub

    Private Sub HomePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomePageToolStripMenuItem.Click
        System.Diagnostics.Process.Start("http://skycd.sf.net")
    End Sub

    Private Sub DonateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DonateToolStripMenuItem.Click
        System.Diagnostics.Process.Start("http://sourceforge.net/donate/index.php?group_id=100263")
    End Sub

    Private Sub helpToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles helpToolStripButton.Click
        Me.ContentsToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub lvBrowse_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvBrowse.KeyUp
        If e.Modifiers = Keys.None And (e.KeyData = Keys.Back Or e.KeyData = Keys.BrowserBack) Then
            If IsNothing(Me.tvTree.SelectedNode.Parent) = False Then
                Me.tvTree.SelectedNode = Me.tvTree.SelectedNode.Parent
            End If
        End If
    End Sub

    Private Sub cmsContext_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmsContext.Opening

        If Me.tvTree.Focused Then
            Me.DeleteStripMenuItem.Enabled = Not IsNothing(Me.tvTree.SelectedNode)
            If IsNothing(Me.tvTree.SelectedNode) = False Then
                If Me.tvTree.SelectedNode.IsExpanded Then
                    Me.ExpandMenuItem.Visible = False
                    Me.CollapseMenuItem.Visible = True
                Else
                    Me.ExpandMenuItem.Visible = True
                    Me.CollapseMenuItem.Visible = False
                End If
                'Me.CollapseMenuItem.Visible = Not Me.tvTree.SelectedNode.IsExpanded
                Me.ExpandOrCollapseSeparator11.Visible = True
                Me.PropertiesToolStripMenuItem.Visible = True
                Me.ToolStripSeparator12.Visible = True
                Me.DeleteStripMenuItem.Enabled = True
            Else
                Me.ExpandMenuItem.Visible = False
                Me.CollapseMenuItem.Visible = False
                Me.ExpandOrCollapseSeparator11.Visible = False
                Me.PropertiesToolStripMenuItem.Visible = False
                Me.ToolStripSeparator12.Visible = False
                Me.DeleteStripMenuItem.Enabled = False
            End If
            Me.ViewToolStripMenuItem1.Visible = False
            Me.ToolStripSeparator8.Visible = False
            Me.ArrangeIconsByMenuItem.Visible = False
        ElseIf Me.lvBrowse.Focused Then
            Me.DeleteStripMenuItem.Enabled = Me.lvBrowse.SelectedItems.Count > 0
            Me.ExpandMenuItem.Visible = False
            Me.CollapseMenuItem.Visible = False
            Me.ExpandOrCollapseSeparator11.Visible = False
            Me.ViewToolStripMenuItem1.Visible = True
            Me.ToolStripSeparator8.Visible = True
            Me.ArrangeIconsByMenuItem.Visible = True
            Me.PropertiesToolStripMenuItem.Visible = True
            Me.ToolStripSeparator12.Visible = True
        End If
        Me.TilesStripMenuItem.Checked = (Me.lvBrowse.View = View.Tile)
        Me.DetailsStripMenuItem.Checked = (Me.lvBrowse.View = View.Details)
        Me.SmallIconsStripMenuItem.Checked = (Me.lvBrowse.View = View.SmallIcon)
        Me.LargeIconsStripMenuItem.Checked = (Me.lvBrowse.View = View.LargeIcon)
        Me.ListStripMenuItem.Checked = (Me.lvBrowse.View = View.List)
        Me.ArrangeIconsByNameStripMenuItem.Checked = Me.FileNameToolStripMenuItem.Checked
        Me.ArrangeIconsByTypeMenuToolStripItem.Checked = Me.TypeToolStripMenuItem.Checked
        Me.PropertiesToolStripMenuItem.Enabled = Not ((Me.tvTree.SelectedNode Is Nothing) And (Me.lvBrowse.SelectedItems.Count < 1))
    End Sub

    Private Sub ExpandMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandMenuItem.Click
        Me.tvTree.SelectedNode.Expand()
    End Sub

    Private Sub CollapseMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseMenuItem.Click
        Me.tvTree.SelectedNode.Collapse()
    End Sub

    Private Sub DeleteStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteStripMenuItem.Click
        Me.DeleteToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub AddStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStripMenuItem.Click
        Me.AddToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        Me.PropertiesToolStripMenuItem1_Click(sender, e)
    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem1.Click
        Dim Fm As New frmProperties()
        Dim ObjInfo As New SkyCD_Simple.scdProperties()
        If Me.OleDbConnection1.State = ConnectionState.Closed Then
            Me.OleDbConnection1.Open()
        End If
        If Me.tvTree.Focused Then
            Fm.Tag = Me.tvTree.SelectedNode.Name
            Fm.pbIcon.Image = Me.imlTreeIcons.Images(Me.tvTree.SelectedNode.ImageKey)
        ElseIf Me.lvBrowse.Focused Then
            If IsNothing(Me.lvBrowse.FocusedItem) Then
                Fm.Tag = Me.tvTree.SelectedNode.Name
                Fm.pbIcon.Image = Me.imlTreeIcons.Images(Me.tvTree.SelectedNode.ImageKey)
            Else
                Fm.Tag = Me.lvBrowse.FocusedItem.Name
                Fm.pbIcon.Image = Me.imlBrowseIcons.Images(Me.lvBrowse.FocusedItem.ImageKey)
            End If
        Else
            MsgBox(Translate(Fm, "Unknown selected object"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
            Exit Sub
        End If
        Me.odbcDatabase.SelectCommand.CommandText = "SELECT Name, Properties, Type FROM list WHERE ID = " + Fm.Tag + " AND AID = '" + Me.Handle.ToInt64.ToString + "'"
        Me.odbcDatabase.SelectCommand.Prepare()
        Dim rez As System.Data.OleDb.OleDbDataReader = Me.odbcDatabase.SelectCommand.ExecuteReader()
        If rez.Read() = False Then
            MsgBox(Translate(Fm, "Can't read item info"), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
            rez.Close()
            Exit Sub
        End If
        Fm.txtName.Text = rez.Item("Name").ToString
        Dim Nfo As New SkyCD_Simple.scdProperties(rez.Item("Properties").ToString)
        Fm.rtbComments.Rtf = Nfo.Item("Comments").ToString
        Select Case rez.Item("Type").ToString.ToLower
            Case "scdfile"
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty("Last Access Time", Nfo.Item("LastAccessTime"))
                Fm.AddProperty("Last Write Time", Nfo.Item("LastWriteTime"))
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty("Size", SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Val(Nfo.Item("Size"))))
                Fm.AddProperty(Of System.IO.FileAttributes)("Attributes", Nfo.Item("Attributes"))
                Fm.AddProperty("Is Read Only", Nfo.Item("IsReadOnly"))
            Case "scdfolder"
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty("Last Access Time", Nfo.Item("LastAccessTime"))
                Fm.AddProperty("Last Write Time", Nfo.Item("LastWriteTime"))
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty(Of System.IO.FileAttributes)("Attributes", Nfo.Item("Attributes"))
            Case "scdCDDisk".ToLower
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty("Last Access Time", Nfo.Item("LastAccessTime"))
                Fm.AddProperty("Last Write Time", Nfo.Item("LastWriteTime"))
                Fm.AddProperty("Creation Time", Nfo.Item("CreationTime"))
                Fm.AddProperty("Attributes", Nfo.Item("Attributes"))
                Fm.AddProperty("Title", Nfo.Item("Title"))
                Fm.AddProperty("Available FreeSpace", SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Val(Nfo.Item("AvailableFreeSpace"))))
                Fm.AddProperty("Root Directory", Nfo.Item("RootDirectory"))
                Fm.AddProperty("Volume Label", Nfo.Item("VolumeLabel"))
                Fm.AddProperty("Total Size", SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Val(Nfo.Item("TotalSize"))))
                Fm.AddProperty("Total Free Space", SkyAdvancedFunctionsLibrary.Convert2.Long2Size(Val(Nfo.Item("TotalFreeSpace"))))
            Case "scdNetworkRecource".ToLower
                Fm.AddProperty("Address", Nfo.Item("URL"))
            Case Else
                'Fm.tpObjectInfo.v()
                Fm.tbControl.TabPages.RemoveByKey("tpObjectInfo")
        End Select
        rez.Close()
        Select Case Fm.ShowDialog()
            Case Windows.Forms.DialogResult.Cancel
                Fm.Dispose()
                Exit Sub
            Case Windows.Forms.DialogResult.OK
                Nfo.Item("Comments") = Fm.Text
                If Me.OleDbConnection1.State = ConnectionState.Closed Then
                    Me.OleDbConnection1.Open()
                End If
                Me.odbcDatabase.UpdateCommand.CommandText = "UPDATE list SET Properties = '" + SkyAdvancedFunctionsLibrary.Strings.AddSlashes(Nfo.All) + "' WHERE ID = " + Fm.Tag + " AND AID = '" + Me.Handle.ToInt64.ToString + "'"
                'Me.odbcDatabase.UpdateCommand.Prepare()
                Me.odbcDatabase.UpdateCommand.ExecuteNonQuery()
                Fm.Dispose()
                Me.saveToolStripButton.Enabled = True
        End Select
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        Me.RefreshToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub ArrangeIconsByNameStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArrangeIconsByNameStripMenuItem.Click
        Me.FileNameToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub ArrangeIconsByTypeMenuToolStripItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArrangeIconsByTypeMenuToolStripItem.Click
        Me.TypeToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub SmallIconsStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmallIconsStripMenuItem.Click
        Me.IconsToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub LargeIconsStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LargeIconsStripMenuItem.Click
        Me.LargeIconsToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub ListStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListStripMenuItem.Click
        Me.ListToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub DetailsStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DetailsStripMenuItem.Click
        Me.DetailsToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub TilesStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TilesStripMenuItem.Click
        Me.TilesToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        frmOptions.ShowDialog()
    End Sub

    Private Sub ViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.Click
        Me.TilesToolStripMenuItem.Checked = (Me.lvBrowse.View = View.Tile)
        Me.LargeIconsToolStripMenuItem.Checked = (Me.lvBrowse.View = View.LargeIcon)
        Me.IconsToolStripMenuItem.Checked = (Me.lvBrowse.View = View.SmallIcon)
        Me.DetailsToolStripMenuItem.Checked = (Me.lvBrowse.View = View.Details)
        Me.ListToolStripMenuItem.Checked = (Me.lvBrowse.View = View.List)
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked
        Me.DeleteToolStripMenuItem.Enabled = Not ((Me.tvTree.SelectedNode Is Nothing) And (Me.lvBrowse.SelectedItems.Count < 1))
        Me.SaveAsToolStripMenuItem.Enabled = Me.tvTree.Nodes.Count > 0
        Me.SaveToolStripMenuItem.Enabled = Me.saveToolStripButton.Enabled
        Me.PropertiesToolStripMenuItem1.Enabled = Not ((Me.tvTree.SelectedNode Is Nothing) And (Me.lvBrowse.SelectedItems.Count < 1))
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContentsToolStripMenuItem.Click
        Dim Fi As New System.IO.FileInfo(Global.SkyCD.modGlobal.Settings.ReadSetting("HelpFile", , Application.StartupPath & "\Help\index.html"))
        If Fi.Exists Then
            Help.ShowHelp(Me, Fi.FullName)
        Else
            MsgBox(Translate(Me, "Can't find help file." + vbCrLf + vbCrLf + "Please reinstall application."), MsgBoxStyle.Exclamation, Translate(Me, "Error"))
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
