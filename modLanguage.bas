Attribute VB_Name = "modLanguage"
Public Const skyLanguageIDCount As Integer = 100

Function GetLanguageString(ID As Long) As String
  GetLanguageString = GetSetting("SkyCD", "Language", Str(ID), LoadResString(ID))
End Function

Sub SetLanguageString(ID As Integer, NewValue As String)
  SaveSetting "SkyCD", "Language", Str(ID), NewValue
End Sub

Function GetLanguageAllStrings() As String
  Dim I As Long
  Dim Text As String
  Dim AllText As String
  Let I = 101
  Do
    Text = GetLanguageString(I)
    If Text = "" Then Exit Do
    AllText = AllText & "\n" & I & "=" & Text
  Loop
  GetLanguageAllStrings = AllText
End Function
