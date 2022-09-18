Public Class Main

    Public error3v3 As Boolean = False
    Public error5v As Boolean = False
    Public error12v As Boolean = False
    Dim random As New Random

    Private Sub L1_Click(sender As Object, e As EventArgs) Handles L1.Click
        switchIt(TB1)
    End Sub

    Private Sub L2_Click(sender As Object, e As EventArgs) Handles L2.Click
        switchIt(TB2)
    End Sub

    Private Sub L3_Click(sender As Object, e As EventArgs) Handles L3.Click
        switchIt(TB3)
    End Sub

    Private Sub L4_Click(sender As Object, e As EventArgs) Handles L4.Click
        switchIt(TB4)
    End Sub

    Private Sub L5_Click(sender As Object, e As EventArgs) Handles L5.Click
        switchIt(TB5)
    End Sub

    Private Sub L6_Click(sender As Object, e As EventArgs) Handles L6.Click
        switchIt(TB6)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Keys.D1 Then
            switchIt(TB1)
        End If
        If e.KeyValue = Keys.D2 Then
            switchIt(TB2)
        End If
        If e.KeyValue = Keys.D3 Then
            switchIt(TB3)
        End If
        If e.KeyValue = Keys.D4 Then
            switchIt(TB4)
        End If
        If e.KeyValue = Keys.D5 Then
            switchIt(TB5)
        End If
        If e.KeyValue = Keys.D6 Then
            switchIt(TB6)
        End If
        If e.KeyValue = Keys.R Then
            moveIt()
        End If
    End Sub

    Function switchIt(ding)
        If ding.Text = "OK" Then
            ding.Text = "ERROR"
            ding.BackColor = Color.FromName("Red")
            Select Case ding.Name
                Case "TB1"
                    error3v3 = True
                Case "TB2"
                    error5v = True
                Case "TB3"
                    error12v = True
            End Select
        ElseIf ding.Text = "ERROR" Then
            ding.Text = "OK"
            ding.BackColor = Color.FromName("LawnGreen")
            Select Case ding.Name
                Case "TB1"
                    error3v3 = False

                Case "TB2"
                    error5v = False

                Case "TB3"
                    error12v = False
            End Select
        End If
    End Function

    Function moveIt()
        If (Screen.AllScreens.Length > 1) Then
            Dim minHeight = Screen.AllScreens(0).Bounds.Height
            Dim minWidth = Screen.AllScreens(0).Bounds.Width
            Dim lastHeight = Screen.AllScreens(1).Bounds.Height - Me.Size.Height
            Dim lastWidth = (Screen.AllScreens(0).Bounds.Width + Screen.AllScreens(1).Bounds.Width) - Me.Size.Width
            Me.Location = New Point(random.Next(minWidth, lastWidth), random.Next(0, lastHeight))
        Else
            Dim lastHeight = Screen.PrimaryScreen.Bounds.Height - Me.Size.Height
            Dim lastWidth = Screen.PrimaryScreen.Bounds.Width - Me.Size.Width
            Me.Location = New Point(random.Next(0, lastWidth), random.Next(0, lastHeight))
        End If
    End Function

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles L0.Click

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim rand As New Random()
        TB0.Text = TB0.Text + 1

        If error3v3 Then
            TB1V.Text = Math.Round(rand.NextDouble() * 2.3, 2)
        Else
            TB1V.Text = 3.2 + Math.Round(rand.NextDouble() * 0.1, 2)
        End If

        If error5v Then
            TB2V.Text = Math.Round(rand.NextDouble() * 4.0, 2)
        Else
            TB2V.Text = 4.9 + Math.Round(rand.NextDouble() * 0.1, 2)
        End If

        If error12v Then
            TB3V.Text = Math.Round(rand.NextDouble() * 11.0, 2)
        Else
            TB3V.Text = 11.9 + Math.Round(rand.NextDouble() * 0.1, 2)
        End If
    End Sub

    Private Sub TB0_TextChanged(sender As Object, e As EventArgs) Handles TB0.TextChanged

    End Sub


End Class
