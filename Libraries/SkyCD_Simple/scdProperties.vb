Imports SkyCD.Libraries.AdvancedFunctions

Public Class scdProperties

    Structure scdPropertyItem
        Dim Key As String
        Dim Value As Object
    End Structure

    Private Items(0) As scdPropertyItem

    Public Property Item(ByVal Name As String) As Object
        Get
            If Me.Items.Length = 0 Then
                Return ""
            End If
            For Each This As scdProperties.scdPropertyItem In Me.Items
                If This.Key = Name Then
                    Return This.Value
                End If
            Next
            Return ""
        End Get
        Set(ByVal value)
            If Me.Items.Length > 0 Then
                Dim I As Integer = 0
                For I = LBound(Me.Items) To UBound(Me.Items)
                    If Me.Items(I).Key = Name Then
                        Me.Items(I).Value = value
                        Exit Property
                    End If
                Next
            End If
            Dim k As Integer = Me.Items.Length + 1
            ReDim Preserve Me.Items(k)
            k = UBound(Me.Items)
            Me.Items(k).Key = Name
            Me.Items(k).Value = value
        End Set
    End Property

    Public Property All() As String
        Get
            Return Strings.SerializeToText(Items)
        End Get
        Set(ByVal value As String)
            Me.Items = Strings.UnSerializeFromText(value, Items.GetType)
        End Set
    End Property

    Public Sub New(ByVal AllData As String)
        Me.All = AllData
    End Sub

    Public Sub New()
    End Sub

End Class