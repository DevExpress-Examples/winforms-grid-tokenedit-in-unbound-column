Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Data
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base

Namespace DxSample
    Partial Public Class Form1
        Inherits Form

        Private tokenEditRep As RepositoryItemTokenEdit
        Public Sub New()
            InitializeComponent()
        End Sub
        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)
            Me.tokenEditRep = CreateRep()
            Me.gridControl.DataSource = TasksRegistry.GetTasks()
            InitUnboundColumn()
        End Sub
        Protected Function CreateRep() As RepositoryItemTokenEdit
            Dim rep As New RepositoryItemTokenEdit()
            rep.BeginInit()
            rep.Tokens.BeginUpdate()
            AddHandler rep.SelectedItemsChanged, AddressOf OnTokenEditSelectedItemsChanged
            rep.AutoHeightMode = TokenEditAutoHeightMode.RestrictedExpand
            rep.MaxExpandLines = 1
            Try
                rep.Name = "rep"
                rep.EditValueType = TokenEditValueType.List
                gridControl.RepositoryItems.Add(rep)
                For Each emp As Employee In EmployeesRegistry.GetEmployees()
                    rep.Tokens.AddToken(emp.Name, emp.GetId())
                Next emp
            Finally
                rep.Tokens.EndUpdate()
                rep.EndInit()
            End Try
            Return rep
        End Function
        Protected Sub InitUnboundColumn()
            Dim col As New GridColumn() With {.Name = "col", .Caption = "Unbound Col", .FieldName = "col", .Visible = True, .UnboundType = UnboundColumnType.Object}
            col.OptionsColumn.AllowEdit = True
            gridView.Columns.Add(col)
            AddHandler gridView.CustomUnboundColumnData, AddressOf OnGridViewCustomUnboundColumnData
            col.ColumnEdit = tokenEditRep
        End Sub

        Private Sub OnGridViewCustomUnboundColumnData(ByVal sender As Object, ByVal e As CustomColumnDataEventArgs)
            Dim task As Task = TryCast(e.Row, Task)
            If task Is Nothing Then
                Return
            End If
            If e.IsGetData Then
                Dim ids As New List(Of Integer)()
                For Each emp As Employee In task.Employees
                    Dim newId As Integer = emp.GetId()
                    If Not ids.Contains(newId) Then
                        ids.Add(newId)
                    End If
                Next emp
                e.Value = ids
            End If
        End Sub
        Private Sub OnTokenEditSelectedItemsChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)
            If update_Renamed Then
                Return
            End If
            Dim edit As TokenEdit = TryCast(sender, TokenEdit)
            If edit Is Nothing Then
                Return
            End If
            Dim task As Task = CType(gridView.GetFocusedRow(), Task)
            UpdateTaskEmployees(edit, task)
        End Sub


        Private update_Renamed As Boolean = False
        Private Sub UpdateTaskEmployees(ByVal edit As TokenEdit, ByVal task As Task)
            If update_Renamed Then
                Return
            End If
            Me.update_Renamed = True
            Try
                DoUpdateTaskEmployees(edit, task)
                gridView.UpdateCurrentRow()
            Finally
                Me.update_Renamed = False
            End Try
        End Sub
        Private Sub DoUpdateTaskEmployees(ByVal edit As TokenEdit, ByVal task As Task)
            task.ClearEmployees()
            For Each id As Integer In DirectCast(edit.EditValue, IList)
                Dim emp As Employee = EmployeesRegistry.GetEmployee(id)
                task.AddEmployee(emp)
            Next id
        End Sub
    End Class
End Namespace
