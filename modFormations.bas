Attribute VB_Name = "modFormations"

Function FormatFileSize(Size As Double) As String
 If Size < 8 Then
    FormatFileSize = FormatRoundFX(Size, 1, HM) & FormatToFixedLengthText(" " & LoadResString(154), MTL)
    Exit Function
 End If
 If Size < 1024 Then
    FormatFileSize = FormatRoundFX(Size, 8, HM) & FormatToFixedLengthText(" " & LoadResString(155), MTL)
    Exit Function
 End If
 If Size < 1024 ^ 2 Then
    FormatFileSize = FormatRoundFX(Size, 1024, HM) & FormatToFixedLengthText(" " & LoadResString(156), MTL)
    Exit Function
 End If
 If Size < 1024 ^ 3 Then
    FormatFileSize = FormatRoundFX(Size, 1024 ^ 2, HM) & FormatToFixedLengthText(" " & LoadResString(157), MTL)
    Exit Function
 End If
 If Size < 1024 ^ 4 Then
    FormatFileSize = FormatRoundFX(Size, 1024 ^ 3, HM) & FormatToFixedLengthText(" " & LoadResString(158), MTL)
    Exit Function
 End If
 FormatFileSize = FormatRoundFX(Size, 1024 ^ 4, HM) & FormatToFixedLengthText(" " & LoadResString(159), MTL)
End Function

Function FormatRoundFX(Value1 As Double, Value2 As Double, Value3 As Double) As String
 Dim U As Integer
 FormatRoundFX = Trim(Str(Round(Value1 / Value2, Value3)))
 U = Len(FormatRoundFX)
 If U > Value3 Then U = Value3
 FormatRoundFX = Left(FormatRoundFX, U)
 U = Len(FormatRoundFX)
 If U < Value3 Then FormatRoundFX = FormatRoundFX & String(Value3 - U, " ")
End Function

Function FormatToFixedLengthText(Text As String, Length As Double) As String
 If Len(Text) < Length Then
    Text = Text & String(Abs(Length - Len(Text)), " ")
 ElseIf Len(Text) > Length Then
    Text = Left(Text, Length - 3) & "..."
 End If
 FormatToFixedLengthText = Text
End Function

Function cFormat(Text As String) As String
  cFormat = Replace(Text, "\n", Chr(13))
  cFormat = Replace(cFormat, "\r", Chr(10))
End Function
