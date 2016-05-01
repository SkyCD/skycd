Imports System.Math

Public Module Convert2

    Public Function Size2Long(ByRef Text As String) As Long
        Dim I As Long = InStrRev(Text.Trim, " ")
        Dim SizeUnit As String = Right(Text.Trim, Text.Trim.Length - I).Trim
        Dim Size As Double = Val(Left(Text.Trim, Text.Trim.Length - SizeUnit.Length).Trim)
        'MsgBox("=>" + Size.ToString)
        Select Case SizeUnit.ToLower
            Case "kb"
                Return 8 * Size * (1024 ^ 1)
            Case "mb"
                Return 8 * Size * (1024 ^ 2)
            Case "gb"
                Return 8 * Size * (1024 ^ 3)
            Case "tb"
                Return 8 * Size * (1024 ^ 4)
            Case "bits"
                Return Size
            Case "bytes"
                Return Size * 8
        End Select
        Return Size
    End Function

    Public Function Long2Size(ByVal Size As Long, Optional RoundSpaces As Integer = 10) As String
        Static Dim RD As String = ""
        Static Dim Format As String = ""
        Static Dim Tarpas As String = Space(10)
        Select Case Size
            Case Is < 8
                RD = Size.ToString
                Format = " Bits "
            Case 8 To 1024 * 8
                RD = Round(Size / 8, RoundSpaces).ToString
                Format = " Bytes"
            Case 8 * 1024 To (1024 ^ 2) * 8
                RD = Round(Size / (8 * 1024), RoundSpaces).ToString
                Format = " KB   "
            Case 8 * (1024 ^ 2) To (1024 ^ 3) * 8
                RD = Round(Size / (8 * 1024 ^ 2), RoundSpaces).ToString
                Format = " MB   "
            Case 8 * (1024 ^ 3) To (1024 ^ 4) * 8
                RD = Round(Size / (8 * 1024 ^ 3), RoundSpaces).ToString
                Format = " GB   "
            Case Else
                RD = Round(Size / (8 * 1024 ^ 4), RoundSpaces).ToString
                Format = " TB   "
        End Select
        If RD.Length < RoundSpaces Then Return RD & Left(Tarpas, RoundSpaces - RD.Length) & Format
        Return Left(RD, RoundSpaces) & Format
    End Function

End Module
