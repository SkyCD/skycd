Namespace My

    'The following events are available for MyApplication
    '
    'Startup: Raised when the application starts, before the startup form is created.
    'Shutdown: Raised after all application forms are closed.  This event is not raised if the application is terminating abnormally.
    'UnhandledException: Raised if the application encounters an unhandled exception.
    'StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    'NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Class MyApplication

        Public Function OpenNew(Optional ByVal FileName As String = "") As Form
            'On Error Resume Next
            Dim Fm As New Form1()
            Fm.InitializeLifetimeService()
            Application.DoEvents()
            Fm.Show()
            Application.DoEvents()
            Fm.Refresh()
            Application.DoEvents()
            Fm.RefreshData()
            Application.DoEvents()
            If FileName.Length > 0 Then
                Dim FI As New System.IO.FileInfo(FileName)
                If FI.Exists Then Fm.LoadFile(FileName)
            End If
            Return Fm
        End Function

        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Global.SkyCD.Startup.Open()
            Exit Sub
            If e.CommandLine.Count > 0 Then
                Me.OpenNew(e.CommandLine.Item(0))
            Else
                Me.OpenNew()
            End If
            'Dim I As Integer, Fl As System.IO.FileInfo
            '   For I = 0 To e.CommandLine.Count - 1
            '      Fl = New System.IO.FileInfo(e.CommandLine.Item(I))
            '     If Fl.Exists Then Me.OpenNew(Fl.FullName)
            'Next
        End Sub


        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MsgBox(modGlobal.Translate(Me, e.Exception.ToString), MsgBoxStyle.Critical, modGlobal.Translate(Me, "Error"))
        End Sub
    End Class

End Namespace
