Namespace Database
    Public Class Item

        Public Sub New()

        End Sub

        Public Sub New(Name As String, parentID As Integer, Type As String, Properties As String, AID As Integer)
            Me.ID = Nothing
            Me.Name = Name
            Me.ParentID = parentID
            Me.Type = Type
            Me.Properties = Properties
            Me.AID = AID
        End Sub

        Public Sub New(ID As Integer, Name As String, parentID As Integer, Type As String, Properties As String, AID As Integer)
            Me.ID = ID
            Me.Name = Name
            Me.ParentID = parentID
            Me.Type = Type
            Me.Properties = Properties
            Me.AID = AID
        End Sub

        Public Sub New(ID As Integer, Name As String, parentID As Integer, Type As String, Properties As String, Size As Long, AID As Integer)
            Me.ID = ID
            Me.Name = Name
            Me.ParentID = parentID
            Me.Type = Type
            Me.Properties = Properties
            Me.AID = AID
            Me.Size = Size
        End Sub

        Public Property ID As Integer

        Public Property Name As String

        Public Property Properties As String

        Public Property Type As String

        Public Property Size As Long

        Public Property AID As Integer

        Public Property ParentID As Integer

    End Class

End Namespace