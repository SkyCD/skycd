Public Class Main
    Implements iInterfacePlugIn

    Private Debug As Boolean = False
    Private Parent As iMainForm

    Public Sub Create(ByRef Parent As iMainForm) Implements iInterfacePlugIn.Create
        Me.Parent = Parent
        Dim menu As System.Windows.Forms.Menu = Me.Parent.Menu
        With Parent
            '.ToolsToolStripMenuItem.DropDownItems.Add("Browser", New System.Drawing.Bitmap(16, 16), New System.EventHandler(AddressOf Me.OnClick)).MergeIndex = 0
        End With
    End Sub

    Public Sub OnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Windows.Forms.Help.ShowHelp(Me.Parent, "about:blank")
    End Sub

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
