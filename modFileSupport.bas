Attribute VB_Name = "modFileSupport"
Function ReadSettingsListFile(Filename As String) As Collection
  Dim I As Integer
  Dim Text As String
  Let I = FreeFile(255)
  Open Filename For Input As #I
  Do
    Line Input #I, Text
    ReadSettingsListFile.Add Text
  Loop Until EOF(I)
End Function

Function ReadTextFileToLine(Filename As String) As String
  On Error Resume Next
  Dim I As Integer
  Dim Text As String
  Let I = FreeFile(255)
  ReadTextFileToLine = ""
  Open Filename For Input As #I
  Do
    Line Input #I, Text
    ReadTextFileToLine = ReadTextFileToLine + vbCrLf + Text
  Loop Until EOF(I)
End Function
