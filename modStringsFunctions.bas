Attribute VB_Name = "modStringsFunctions"

Function FindLast(Text1 As String, Text2 As String) As Long
 Dim I As Long, O As Long
 O = 0
 Do
    I = InStr(O + 1, Text1, Text2)
    If I <> 0 Then O = I
 Loop Until I = 0
 FindLast = O
End Function

Function RemoveUnused(Text As String, Char As String) As String
  RemoveUnused = Text
  If Right(RemoveUnused, 1) = Char Then RemoveUnused = Left(RemoveUnused, Len(RemoveUnused) - 1)
  If Left(RemoveUnused, 1) = Char Then RemoveUnused = Right(RemoveUnused, Len(RemoveUnused) - 1)
  'Debug.Print RemoveUnused
End Function

Function IsSkInFunction(Number1 As Integer, Number2 As Integer) As Boolean
 Dim Sk1 As String, Sk2 As String
 Sk1 = Trim(Str(Number1))
 Sk2 = Trim(Str(Number2))
 IsSkInFunction = (InStr(1, Sk1, Sk2) > 0)
End Function

Function AddSk(Number1 As Integer, Number2 As Integer) As Integer
 Dim Sk1 As String, Sk2 As String
 AddSk = Number1
 If IsSkInFunction(AddSk, Number2) Then Exit Function
 Sk1 = Trim(Str(Number1))
 Sk2 = Trim(Str(Number2))
 AddSk = CInt(Val(Sk2 & Sk1))
End Function

Function xReplace(Text As String, ReplaceWidth As String) As String
 xReplace = Replace(Text, "%1", ReplaceWidth)
End Function

Function ToXCode(Text As String) As String
 ToXCode = Replace(Text, "#", "#0")
 ToXCode = Replace(ToXCode, ":", "#1")
 ToXCode = Replace(ToXCode, "\", "#2")
 ToXCode = Replace(ToXCode, "/", "#3")
 ToXCode = Replace(ToXCode, "*", "#4")
End Function

Function EqLeftString(Text As String, Text2 As String) As Boolean
 EqLeftString = Left(Text, Len(Text2)) = Text2
End Function

Function EqLeftStringNC(Text As String, Text2 As String) As Boolean
 EqLeftStringNC = EqLeftString(LCase(Text), LCase(Text2))
End Function
