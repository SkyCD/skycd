Imports System

Namespace Database

    Public Interface iConnection

        Function [Select](Of T)(query As String, ParamArray args() As Object) As T
        Function [Select](query As String, ParamArray args() As Object) As List(Of Item)
        Function SelectOnlyOne(query As String, ParamArray args() As Object) As Item

        Sub Execute(query As String, ParamArray args() As Object)

        Function Insert(table As String, item As Item) As Integer

    End Interface

End Namespace
