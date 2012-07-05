Public Class SCD_XSettings

    Private Name As String

    Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub

    Sub WriteSetting(ByVal Section As String, ByVal Key As String, ByVal Value As Object)
        SaveSetting(Name, Section, Key, Value.ToString)
    End Sub

    Sub WriteSetting(ByVal Key As String, ByVal Value As Object)
        SaveSetting(Name, "Default", Key, Value.ToString)
    End Sub

    Function ReadSetting(ByVal Key As String, Optional ByVal Section As String = "Default", Optional ByVal DefaultValue As Object = "") As Object
        If IsNothing(DefaultValue) Then DefaultValue = ""
        If Section.Trim = "" Then Section = "Default"
        Dim Rez As String = GetSetting(Me.Name, Section, Key, DefaultValue.ToString)
        If Val(Rez).ToString = Rez Then Return Val(Rez)
        If Rez = "True" Then Return True
        If Rez = "False" Then Return False
        If Rez = "" Then Return DefaultValue
        Return Rez
    End Function

    Function ReadSettings(Optional ByVal Section As String = "Default") As Object(,)
        Return GetAllSettings(Me.Name, Section)
    End Function

    Sub DeleteSetting(Optional ByVal Section As String = Nothing, Optional ByVal Key As String = Nothing)
        Try
            Microsoft.VisualBasic.Interaction.DeleteSetting(Me.Name, Section, Key)
        Catch ex As Exception

        End Try        
    End Sub

End Class