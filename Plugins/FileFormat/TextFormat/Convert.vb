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


    Public Function Long2Size(ByVal Size As Long) As String
        Static Dim RD As String = ""
        Static Dim Format As String = ""
        Static Dim Tarpas As String = Space(10)
        Select Case Size
            Case Is < 8
                RD = Size.ToString
                Format = " Bits "
            Case 8 To 1024 * 8
                RD = Math.Round(Size / 8, 10).ToString
                Format = " Bytes"
            Case 8 * 1024 To (1024 ^ 2) * 8
                RD = Math.Round(Size / (8 * 1024), 10).ToString
                Format = " KB   "
            Case 8 * (1024 ^ 2) To (1024 ^ 3) * 8
                RD = Math.Round(Size / (8 * 1024 ^ 2), 10).ToString
                Format = " MB   "
            Case 8 * (1024 ^ 3) To (1024 ^ 4) * 8
                RD = Math.Round(Size / (8 * 1024 ^ 3), 10).ToString
                Format = " GB   "
            Case Else
                RD = Math.Round(Size / (8 * 1024 ^ 4), 10).ToString
                Format = " TB   "
        End Select
        If RD.Length < 10 Then Return RD & Left(Tarpas, 10 - RD.Length) & Format
        Return Left(RD, 10) & Format
    End Function

End Module
