Public Class Format

    Public Shared Function Double2FixedLengthString(ByRef Number As Double, ByRef Length As Integer) As String
        Dim RD As Double = Math.Round(Number, 10)
        Dim Text As String = RD.ToString
        If Text.Length < Length Then
            Return Text.Replace(",", ".") & Space(Length - Text.Length)
        End If
        Return Left(Text, Length)
    End Function


End Class
