Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Data
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base

Namespace DxSample

    Public Partial Class Form1
        Inherits Form

        Private tokenEditRep As RepositoryItemTokenEdit

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)
            tokenEditRep = CreateRep()
            gridControl.DataSource = TasksRegistry.GetTasks()
            InitUnboundColumn()
        End Sub

        Protected Function CreateRep() As RepositoryItemTokenEdit
            Dim rep As RepositoryItemTokenEdit = New RepositoryItemTokenEdit()
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
                Next
            Finally
                rep.Tokens.EndUpdate()
                rep.EndInit()
            End Try

            Return rep
        End Function

        Protected Sub InitUnboundColumn()
            Dim col As GridColumn = New GridColumn() With {.Name = "col", .Caption = "Unbound Col", .FieldName = "col", .Visible = True, .UnboundType = UnboundColumnType.Object}
            col.OptionsColumn.AllowEdit = True
            gridView.Columns.Add(col)
            AddHandler gridView.CustomUnboundColumnData, AddressOf OnGridViewCustomUnboundColumnData
            col.ColumnEdit = tokenEditRep
        End Sub

        Private Sub OnGridViewCustomUnboundColumnData(ByVal sender As Object, ByVal e As CustomColumnDataEventArgs)
            Dim task As Task = TryCast(e.Row, Task)
            If task Is Nothing Then Return
            If e.IsGetData Then
                Dim ids As List(Of Integer) = New List(Of Integer)()
                For Each emp As Employee In task.Employees
                    Dim newId As Integer = emp.GetId()
                    If Not ids.Contains(newId) Then ids.Add(newId)
                Next

                e.Value = ids
            End If
        End Sub

        Private Sub OnTokenEditSelectedItemsChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)
            If update Then Return
            Dim edit As TokenEdit = TryCast(sender, TokenEdit)
            If edit Is Nothing Then Return
            Dim task As Task = CType(gridView.GetFocusedRow(), Task)
            UpdateTaskEmployees(edit, task)
        End Sub

        Private update As Boolean = False

        Private Sub UpdateTaskEmployees(ByVal edit As TokenEdit, ByVal task As Task)
            If update Then Return
            update = True
            Try
                DoUpdateTaskEmployees(edit, task)
                gridView.UpdateCurrentRow()
            Finally
                update = False
            End Try
        End Sub

        Private Sub DoUpdateTaskEmployees(ByVal edit As TokenEdit, ByVal task As Task)
            task.ClearEmployees()
            For Each id As Integer In CType(edit.EditValue, IList)
                Dim emp As Employee = EmployeesRegistry.GetEmployee(id)
                task.AddEmployee(emp)
            Next
        End Sub
    End Class
End Namespace
