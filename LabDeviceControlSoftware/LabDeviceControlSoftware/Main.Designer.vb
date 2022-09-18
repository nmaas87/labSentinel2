<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TB1 = New System.Windows.Forms.TextBox()
        Me.L1 = New System.Windows.Forms.Label()
        Me.TB2 = New System.Windows.Forms.TextBox()
        Me.L2 = New System.Windows.Forms.Label()
        Me.TB3 = New System.Windows.Forms.TextBox()
        Me.L3 = New System.Windows.Forms.Label()
        Me.TB6 = New System.Windows.Forms.TextBox()
        Me.TB5 = New System.Windows.Forms.TextBox()
        Me.L6 = New System.Windows.Forms.Label()
        Me.TB4 = New System.Windows.Forms.TextBox()
        Me.L5 = New System.Windows.Forms.Label()
        Me.L4 = New System.Windows.Forms.Label()
        Me.L0 = New System.Windows.Forms.Label()
        Me.TB0 = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TB1V = New System.Windows.Forms.TextBox()
        Me.TB2V = New System.Windows.Forms.TextBox()
        Me.TB3V = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TB7 = New System.Windows.Forms.TextBox()
        Me.TB8 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TB1
        '
        Me.TB1.BackColor = System.Drawing.Color.LawnGreen
        Me.TB1.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB1.Location = New System.Drawing.Point(387, 63)
        Me.TB1.Name = "TB1"
        Me.TB1.ReadOnly = True
        Me.TB1.Size = New System.Drawing.Size(385, 47)
        Me.TB1.TabIndex = 3
        Me.TB1.TabStop = False
        Me.TB1.Text = "OK"
        '
        'L1
        '
        Me.L1.AutoSize = True
        Me.L1.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L1.Location = New System.Drawing.Point(12, 66)
        Me.L1.Name = "L1"
        Me.L1.Size = New System.Drawing.Size(97, 39)
        Me.L1.TabIndex = 2
        Me.L1.Text = "3.3 V"
        '
        'TB2
        '
        Me.TB2.BackColor = System.Drawing.Color.LawnGreen
        Me.TB2.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB2.Location = New System.Drawing.Point(387, 116)
        Me.TB2.Name = "TB2"
        Me.TB2.ReadOnly = True
        Me.TB2.Size = New System.Drawing.Size(385, 47)
        Me.TB2.TabIndex = 5
        Me.TB2.TabStop = False
        Me.TB2.Text = "OK"
        '
        'L2
        '
        Me.L2.AutoSize = True
        Me.L2.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L2.Location = New System.Drawing.Point(12, 119)
        Me.L2.Name = "L2"
        Me.L2.Size = New System.Drawing.Size(68, 39)
        Me.L2.TabIndex = 4
        Me.L2.Text = "5 V"
        '
        'TB3
        '
        Me.TB3.BackColor = System.Drawing.Color.LawnGreen
        Me.TB3.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB3.Location = New System.Drawing.Point(387, 169)
        Me.TB3.Name = "TB3"
        Me.TB3.ReadOnly = True
        Me.TB3.Size = New System.Drawing.Size(385, 47)
        Me.TB3.TabIndex = 7
        Me.TB3.TabStop = False
        Me.TB3.Text = "OK"
        '
        'L3
        '
        Me.L3.AutoSize = True
        Me.L3.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L3.Location = New System.Drawing.Point(12, 172)
        Me.L3.Name = "L3"
        Me.L3.Size = New System.Drawing.Size(87, 39)
        Me.L3.TabIndex = 6
        Me.L3.Text = "12 V"
        '
        'TB6
        '
        Me.TB6.BackColor = System.Drawing.Color.LawnGreen
        Me.TB6.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB6.Location = New System.Drawing.Point(182, 329)
        Me.TB6.Name = "TB6"
        Me.TB6.ReadOnly = True
        Me.TB6.Size = New System.Drawing.Size(590, 47)
        Me.TB6.TabIndex = 29
        Me.TB6.TabStop = False
        Me.TB6.Text = "OK"
        '
        'TB5
        '
        Me.TB5.BackColor = System.Drawing.Color.LawnGreen
        Me.TB5.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB5.Location = New System.Drawing.Point(182, 276)
        Me.TB5.Name = "TB5"
        Me.TB5.ReadOnly = True
        Me.TB5.Size = New System.Drawing.Size(590, 47)
        Me.TB5.TabIndex = 28
        Me.TB5.TabStop = False
        Me.TB5.Text = "OK"
        '
        'L6
        '
        Me.L6.AutoSize = True
        Me.L6.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L6.Location = New System.Drawing.Point(14, 332)
        Me.L6.Name = "L6"
        Me.L6.Size = New System.Drawing.Size(85, 39)
        Me.L6.TabIndex = 27
        Me.L6.Text = "O.T."
        '
        'TB4
        '
        Me.TB4.BackColor = System.Drawing.Color.LawnGreen
        Me.TB4.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB4.Location = New System.Drawing.Point(182, 222)
        Me.TB4.Name = "TB4"
        Me.TB4.ReadOnly = True
        Me.TB4.Size = New System.Drawing.Size(590, 47)
        Me.TB4.TabIndex = 26
        Me.TB4.TabStop = False
        Me.TB4.Text = "OK"
        '
        'L5
        '
        Me.L5.AutoSize = True
        Me.L5.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L5.Location = New System.Drawing.Point(12, 279)
        Me.L5.Name = "L5"
        Me.L5.Size = New System.Drawing.Size(87, 39)
        Me.L5.TabIndex = 25
        Me.L5.Text = "O.V."
        '
        'L4
        '
        Me.L4.AutoSize = True
        Me.L4.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L4.Location = New System.Drawing.Point(12, 225)
        Me.L4.Name = "L4"
        Me.L4.Size = New System.Drawing.Size(89, 39)
        Me.L4.TabIndex = 23
        Me.L4.Text = "O.C."
        '
        'L0
        '
        Me.L0.AutoSize = True
        Me.L0.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L0.Location = New System.Drawing.Point(12, 15)
        Me.L0.Name = "L0"
        Me.L0.Size = New System.Drawing.Size(132, 39)
        Me.L0.TabIndex = 30
        Me.L0.Text = "SeqCnt"
        '
        'TB0
        '
        Me.TB0.BackColor = System.Drawing.Color.White
        Me.TB0.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB0.Location = New System.Drawing.Point(182, 12)
        Me.TB0.Name = "TB0"
        Me.TB0.ReadOnly = True
        Me.TB0.Size = New System.Drawing.Size(590, 47)
        Me.TB0.TabIndex = 31
        Me.TB0.TabStop = False
        Me.TB0.Text = "0"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'TB1V
        '
        Me.TB1V.BackColor = System.Drawing.Color.White
        Me.TB1V.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB1V.Location = New System.Drawing.Point(182, 63)
        Me.TB1V.Name = "TB1V"
        Me.TB1V.ReadOnly = True
        Me.TB1V.Size = New System.Drawing.Size(199, 47)
        Me.TB1V.TabIndex = 32
        Me.TB1V.TabStop = False
        Me.TB1V.Text = "0"
        '
        'TB2V
        '
        Me.TB2V.BackColor = System.Drawing.Color.White
        Me.TB2V.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB2V.Location = New System.Drawing.Point(182, 116)
        Me.TB2V.Name = "TB2V"
        Me.TB2V.ReadOnly = True
        Me.TB2V.Size = New System.Drawing.Size(199, 47)
        Me.TB2V.TabIndex = 33
        Me.TB2V.TabStop = False
        Me.TB2V.Text = "0"
        '
        'TB3V
        '
        Me.TB3V.BackColor = System.Drawing.Color.White
        Me.TB3V.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB3V.Location = New System.Drawing.Point(182, 169)
        Me.TB3V.Name = "TB3V"
        Me.TB3V.ReadOnly = True
        Me.TB3V.Size = New System.Drawing.Size(199, 47)
        Me.TB3V.TabIndex = 34
        Me.TB3V.TabStop = False
        Me.TB3V.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 385)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 39)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "IP"
        '
        'TB7
        '
        Me.TB7.BackColor = System.Drawing.Color.White
        Me.TB7.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB7.Location = New System.Drawing.Point(182, 382)
        Me.TB7.Name = "TB7"
        Me.TB7.ReadOnly = True
        Me.TB7.Size = New System.Drawing.Size(590, 47)
        Me.TB7.TabIndex = 37
        Me.TB7.TabStop = False
        Me.TB7.Text = "192.168.0.9"
        '
        'TB8
        '
        Me.TB8.BackColor = System.Drawing.Color.White
        Me.TB8.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB8.Location = New System.Drawing.Point(182, 435)
        Me.TB8.Name = "TB8"
        Me.TB8.ReadOnly = True
        Me.TB8.Size = New System.Drawing.Size(590, 47)
        Me.TB8.TabIndex = 39
        Me.TB8.TabStop = False
        Me.TB8.Text = "manager"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 438)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(170, 39)
        Me.Label1.TabIndex = 38
        Me.Label1.Text = "Password"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.TB8)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TB7)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TB3V)
        Me.Controls.Add(Me.TB2V)
        Me.Controls.Add(Me.TB1V)
        Me.Controls.Add(Me.TB0)
        Me.Controls.Add(Me.L0)
        Me.Controls.Add(Me.TB6)
        Me.Controls.Add(Me.TB5)
        Me.Controls.Add(Me.L6)
        Me.Controls.Add(Me.TB4)
        Me.Controls.Add(Me.L5)
        Me.Controls.Add(Me.L4)
        Me.Controls.Add(Me.TB3)
        Me.Controls.Add(Me.L3)
        Me.Controls.Add(Me.TB2)
        Me.Controls.Add(Me.L2)
        Me.Controls.Add(Me.TB1)
        Me.Controls.Add(Me.L1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main"
        Me.ShowIcon = False
        Me.Text = "Lab PSU Events"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TB1 As TextBox
    Friend WithEvents L1 As Label
    Friend WithEvents TB2 As TextBox
    Friend WithEvents L2 As Label
    Friend WithEvents TB3 As TextBox
    Friend WithEvents L3 As Label
    Friend WithEvents TB6 As TextBox
    Friend WithEvents TB5 As TextBox
    Friend WithEvents L6 As Label
    Friend WithEvents TB4 As TextBox
    Friend WithEvents L5 As Label
    Friend WithEvents L4 As Label
    Friend WithEvents L0 As Label
    Friend WithEvents TB0 As TextBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents TB1V As TextBox
    Friend WithEvents TB2V As TextBox
    Friend WithEvents TB3V As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TB7 As TextBox
    Friend WithEvents TB8 As TextBox
    Friend WithEvents Label1 As Label
End Class
