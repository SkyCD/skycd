Attribute VB_Name = "modNewStyleVBBoxes"

Function nsInputBox(Text As String, Optional Caption As String = "") As String
  Load frmInputBox
  frmInputBox.Caption = Caption
  frmInputBox.lblCaption.Caption = Text
  frmInputBox.Show 1
  nsInputBox = frmInputBox.txtAnswer.Text
  Unload frmInputBox
End Function
