Imports SkyAdvancedFunctionsLibrary.Strings

Public Class XMLSettings

    Private Structure Setting
        Dim Name As String
        Dim Value As Object
    End Structure

    Private Structure Settings
        Dim Settings() As XMLSettings.Setting
        Dim Category As String
    End Structure

    Private Data() As XMLSettings.Settings
    Private FileName As String

    Sub New(ByVal FileName As String)
        Me.FileName = FileName
        Try
            Dim File As New System.IO.StreamReader(FileName)
            Me.Data = UnSerializeFromText(File.ReadToEnd, Me.Data.GetType)
            File.Close()
        Catch ex As Exception
            ReDim Preserve Me.Data(0)            
        End Try
    End Sub

    Public Sub WriteSetting(ByVal Category As String, ByVal Name As String, ByVal Value As String)
        Dim I As Integer, O As Integer, B As Boolean = False
        For I = 0 To Me.Data.GetLength(0) - 1
            If Me.Data(I).Category = Category Then
                For O = 0 To Me.Data(I).Settings.GetLength(0)
                    If Me.Data(I).Settings(O).Name = Name Then
                        Me.Data(I).Settings(O).Value = Value
                        B = True
                        Exit For
                    End If
                Next
                If B Then Exit For
                ReDim Preserve Me.Data(I).Settings(Me.Data(I).Settings.GetLength(0) + 1)
                Me.Data(I).Settings(Me.Data(I).Settings.GetLength(0)).Name = Name
                Me.Data(I).Settings(Me.Data(I).Settings.GetLength(0)).Value = Value
                B = True
                Exit For
            End If
        Next
        If B = False Then
            ReDim Preserve Me.Data(Me.Data.GetLength(0) + 1)
            ReDim Preserve Me.Data(Me.Data.GetLength(0)).Settings(1)
            Me.Data(Me.Data.GetLength(0)).Settings(0).Name = Name
            Me.Data(Me.Data.GetLength(0)).Settings(0).Value = Value
        End If
        Me.Save()
    End Sub

    Public Sub Save()
        Dim File As New System.IO.StreamWriter(Me.FileName)
        File.Write(SerializeToText(Me.Data))
        File.Close()
    End Sub

End Class
