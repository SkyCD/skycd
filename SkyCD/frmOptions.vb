Public Class frmOptions

    Public Event CancelPressed()
    Public Event OKPressed()

    Dim PPath As String = Form1.PlugInsSuport.Path

    Private Sub frmOptions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me.txtPlugIsPath.Text = "" Then
            Form1.PlugInsSuport.Path = Me.PPath
        End If
    End Sub

    Private Sub frmOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtPlugIsPath.Text = PPath
        Me.lvPlugIns.View = View.Details
        UpdatePlugInsList()
        Me.lstLanguages.Items.Clear()
        'Exit Sub
        Try
            Dim Dir As New System.IO.DirectoryInfo(My.Application.Info.DirectoryPath & "\Languages")
            For Each That As System.IO.FileInfo In Dir.GetFiles("*.xml")
                Me.lstLanguages.Items.Add(That.Name.Substring(0, That.Name.Length - That.Extension.Length))
            Next
        Catch ex As Exception

        End Try
        Dim Fk As Integer = Me.lstLanguages.Items.IndexOf(modGlobal.Settings.ReadSetting("Language", , "English"))
        If Fk > -1 Then
            Me.lstLanguages.SelectedIndex = Fk
        Else
            Me.lstLanguages.SelectedIndex = 0
        End If
        Me.txtDatabaseConnectionString.Text = SkyCD.modGlobal.Settings.ReadSetting("Database.Connection.String", "", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\temp.mdb")
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.txtPlugIsPath.Text = ""
        RaiseEvent CancelPressed()
        Me.Close()
    End Sub

    Private Sub cmdBrowsePlugInPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowsePlugInPath.Click
        Dim FS As New FolderBrowserDialog()
        With FS
            .SelectedPath = Me.txtPlugIsPath.Text
            .ShowNewFolderButton = False
            .Description = Translate(FS, "Select Plug-Ins Folder")
            If .ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Me.txtPlugIsPath.Text = .SelectedPath
        End With
    End Sub

    Private Sub cmdRefreshPlugIns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefreshPlugIns.Click
        Form1.PlugInsSuport.UpdatePlugInsList(False)
        UpdatePlugInsList()
    End Sub

    Public Sub UpdatePlugInsList()
        Me.lvPlugIns.Items.Clear()
        'Dim LV As ListViewItem
        Dim List() As PlugInsSupport.PlugInInfo = Form1.PlugInsSuport.GetPlugIns()
        If IsNothing(List) Then Exit Sub
        For Each That As PlugInsSupport.PlugInInfo In List
            Try
                Me.lvPlugIns.Items.Add(That.Name).SubItems.Add(That.Type)
            Catch ex As NullReferenceException

            End Try
        Next
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.lvPlugIns.Columns.Item(0).Width = Me.lvPlugIns.Width / 2
        Me.lvPlugIns.Columns.Item(1).Width = Me.lvPlugIns.Width / 3
    End Sub

    Private Sub lvPlugIns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvPlugIns.Click
        Me.cmdConfigurePlugIn.Enabled = (New PlugInsSupport.PlugInInfo(Me.lvPlugIns.SelectedItems.Item(0).Text)).HasConfig
    End Sub

    Private Sub cmdConfigurePlugIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfigurePlugIn.Click
        Dim Dll As New PlugInsSupport()
        Dll.Load(Me.lvPlugIns.SelectedItems.Item(0).Text).ShowDialog()
    End Sub

    Private Sub txtPlugIsPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPlugIsPath.TextChanged
        Form1.PlugInsSuport.Path = Me.txtPlugIsPath.Text
        Form1.PlugInsSuport.UpdatePlugInsList(False)
        UpdatePlugInsList()
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        RaiseEvent OKPressed()
        Dim t As Boolean = (Me.lstLanguages.SelectedItem.ToString <> modGlobal.Settings.ReadSetting("Language", , "English"))
        t = t Or (SkyCD.modGlobal.Settings.ReadSetting("Database.Connection.String", "", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\temp.mdb") <> Me.txtDatabaseConnectionString.Text)
        t = t Or (Form1.PlugInsSuport.Path <> PPath)
        SkyCD.modGlobal.Settings.WriteSetting("Language", Me.lstLanguages.SelectedItem.ToString)
        SkyCD.modGlobal.Settings.WriteSetting("Database.Connection.String", Me.txtDatabaseConnectionString.Text)
        Form1.PlugInsSuport.Path = Me.txtPlugIsPath.Text
        Me.Close()
        If t Then
            If MsgBox(Translate(Me, "Because you have changed some setting, this application must restart. Do you want restart now?"), MsgBoxStyle.YesNo Or MsgBoxStyle.Question, Translate(Me, "Restart")) = MsgBoxResult.Yes Then
                Application.Restart()
            Else
                MsgBox(Translate(Me, "New settings will be applied after application restart"), MsgBoxStyle.Information Or MsgBoxStyle.OKOnly, Translate(Me, "Restart"))
            End If
        End If
    End Sub

    Private Sub cmdTestConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTestConnection.Click
        Dim Value As Boolean
        Form1.OleDbConnection1.ConnectionString = Me.txtDatabaseConnectionString.Text
        Form1.OleDbConnection1.ResetState()
        Try
            Form1.OleDbConnection1.Open()
            Value = True
        Catch ex As Exception
            Value = False
        End Try
        If Value Then
            MsgBox(SkyCD.modGlobal.Translate(Me, "Connection test passed"), MsgBoxStyle.Information, SkyCD.modGlobal.Translate(Me, "Connection Test"))
            Form1.OleDbConnection1.Close()
        Else
            MsgBox(SkyCD.modGlobal.Translate(Me, "Connection test was not passed. Please verify connection string if it is correct."), MsgBoxStyle.Exclamation, SkyCD.modGlobal.Translate(Me, "Connection Test"))
        End If
        Form1.OleDbConnection1.ConnectionString = SkyCD.modGlobal.Settings.ReadSetting("Database.Connection.String", "", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\temp.mdb")
    End Sub
End Class