Public Class FileSystemWatcher

    Private WithEvents dc As New System.IO.FileSystemWatcher()

    Public Structure ItemChanges
        Public Enum CanBeChanges As Byte
            Changed = 0
            Created = 1
            Delete = 2
            Renamed = 3
        End Enum
        Dim Changes As ItemChanges.CanBeChanges
        Dim Name As String
        Dim FullPath As String
        Dim ChangeType As System.IO.WatcherChangeTypes
        Dim Param As Object
    End Structure

    Protected Changes() As FileSystemWatcher.ItemChanges

    Private Sub Add(ByVal Changes As FileSystemWatcher.ItemChanges.CanBeChanges, ByVal Name As String, ByVal FullName As String, ByVal ChangeType As System.IO.WatcherChangeTypes, Optional ByVal Param As Object = Nothing)
        Dim Ilgis As Integer
        If IsNothing(Me.Changes) Then
            Ilgis = 0
        Else
            Ilgis = Me.Changes.Length
        End If
        ReDim Preserve Me.Changes(Ilgis + 1)
        With Me.Changes(Ilgis)
            .Changes = Changes
            .FullPath = FullName
            .Name = Name
            .ChangeType = ChangeType
            .Param = Param
        End With
    End Sub

    Private Sub dc_Changed(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles dc.Changed
        Me.Add(ItemChanges.CanBeChanges.Changed, e.Name, e.FullPath, e.ChangeType)
    End Sub

    Private Sub dc_Created(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles dc.Created
        Me.Add(ItemChanges.CanBeChanges.Created, e.Name, e.FullPath, e.ChangeType)        
    End Sub

    Private Sub dc_Deleted(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles dc.Deleted
        Me.Add(ItemChanges.CanBeChanges.Delete, e.Name, e.FullPath, e.ChangeType)
    End Sub

    Private Sub dc_Renamed(ByVal sender As Object, ByVal e As System.IO.RenamedEventArgs) Handles dc.Renamed
        Dim Mas() As Object = {e.OldName, e.OldFullPath}
        Me.Add(ItemChanges.CanBeChanges.Renamed, e.Name, e.FullPath, e.ChangeType)
    End Sub

    Public Sub Clear()
        ReDim Me.Changes(0)
    End Sub

    Public ReadOnly Property Length()
        Get
            If IsNothing(Me.Changes) Then Return 0
            Return Me.Changes.Length            
        End Get
    End Property

    Sub New(ByVal Path As String)
        Try
            Me.dc = New System.IO.FileSystemWatcher(Path)
        Catch ex As Exception

        End Try        
    End Sub

End Class
