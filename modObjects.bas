Attribute VB_Name = "modObjects"

Sub EnableX(Obj As Object, value As Boolean)
  If Obj.Enabled <> value Then _
     Obj.Enabled = value
End Sub
