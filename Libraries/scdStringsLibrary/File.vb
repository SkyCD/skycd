Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Convert

Public Class File

    Public Shared Function GetFileExtension(ByVal FileName As String) As String
        If InStrRev(FileName, ".") > -1 Then
            Return Right(FileName, Len(FileName) - InStrRev(FileName, ".") + 1)
        End If
        Return ""
    End Function

    Public Shared Function GetFileIcon(ByVal FileName As String) As Icon
        Dim FL As New FileInfo(My.Application.Info.DirectoryPath & "\" & FileName)
        Dim Img As Icon = Nothing
        Try
            If Not FL.Exists Then FL.Create().Close()
            Img = Icon.ExtractAssociatedIcon(My.Application.Info.DirectoryPath & "\" & FileName)
            'Dim Img As Image = Image.FromHbitmap(Ico.ToBitmap.GetHbitmap)
            FL.Delete()
        Catch ex As Exception

        End Try
        Return Img
    End Function

    Public Shared Function GetFileIcon2(ByVal FileName As String) As String
        Dim File As New MemoryStream()
        Icon.ExtractAssociatedIcon(FileName).ToBitmap.Save(File, ImageFormat.Png)
        File.Seek(0, SeekOrigin.Begin)
        Return ToBase64String(File.GetBuffer)
    End Function

    Public Shared Function GetFileIcon3(ByVal IconText As String) As Image
        Dim File As New MemoryStream(FromBase64String(IconText))
        Return New Bitmap(File)
    End Function

End Class
