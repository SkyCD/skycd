Imports System.Collections.Generic
Imports SkyCD.Database
Imports System.Data.SQLite
Imports System.Data.Common
Imports System.Reflection
Imports System.Data.SQLite.Linq

Public Class DatabaseProxy
    Implements iConnection

    Protected Database As SQLiteConnection

    Public Sub New(Filename As String)
        Me.Database = New SQLiteConnection("Data Source=" + Filename + ";Version=3;New=True;Compress=True;")
    End Sub

    Public Sub Execute(query As String, ParamArray args() As Object) Implements iConnection.Execute
        With Me.Database.CreateCommand
            For Each arg As Object In args
                .Parameters.Add(New SQLiteParameter(DbType.String, arg))
            Next
            .CommandText = query
            .ExecuteNonQuery()
        End With
    End Sub

    Public Function [Select](query As String, ParamArray args() As Object) As List(Of Item) Implements iConnection.Select
        Dim ret As New List(Of Item)
        Dim cmd As New SQLiteCommand(query, Me.Database)
        For Each arg As Object In args
            cmd.Parameters.Add(New SQLiteParameter(DbType.String, arg))
        Next
        Me.Database.Open()
        Dim reader As DbDataReader = cmd.ExecuteReader()
        Dim properties As PropertyInfo() = GetType(Item).GetProperties
        Dim item As Item
        Do While reader.NextResult
            item = New Item()
            For Each p As PropertyInfo In properties
                p.SetValue(item, reader.Item(p.Name))
            Next
            ret.Add(item)
        Loop
        Return ret
    End Function

    Public Function [Select](Of T)(query As String, ParamArray args() As Object) As T Implements iConnection.Select
        Dim ret As T
        With Me.Database.CreateCommand
            For Each arg As Object In args
                .Parameters.Add(New SQLiteParameter(DbType.String, arg))
            Next
            .CommandText = query
            ret = .ExecuteScalar()
        End With
        Return ret
    End Function

    Public Function SelectOnlyOne(query As String, ParamArray args() As Object) As Item Implements iConnection.SelectOnlyOne
        Dim ret As List(Of Item) = [Select](query, args)
        If ret.Count > 0 Then
            Return ret(0)
        End If
        Return Nothing
    End Function

End Class
