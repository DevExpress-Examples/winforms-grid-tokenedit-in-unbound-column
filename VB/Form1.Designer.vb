Namespace DxSample
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.gridControl = New DevExpress.XtraGrid.GridControl()
            Me.gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
            DirectCast(Me.gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' gridControl
            ' 
            Me.gridControl.Cursor = System.Windows.Forms.Cursors.Default
            Me.gridControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.gridControl.Location = New System.Drawing.Point(0, 0)
            Me.gridControl.MainView = Me.gridView
            Me.gridControl.Name = "gridControl"
            Me.gridControl.Size = New System.Drawing.Size(840, 452)
            Me.gridControl.TabIndex = 2
            Me.gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gridView})
            ' 
            ' gridView
            ' 
            Me.gridView.GridControl = Me.gridControl
            Me.gridView.Name = "gridView"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(840, 452)
            Me.Controls.Add(Me.gridControl)
            Me.Name = "Form1"
            Me.Text = "Form1"
            DirectCast(Me.gridControl, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private gridControl As DevExpress.XtraGrid.GridControl
        Private gridView As DevExpress.XtraGrid.Views.Grid.GridView

    End Class
End Namespace

