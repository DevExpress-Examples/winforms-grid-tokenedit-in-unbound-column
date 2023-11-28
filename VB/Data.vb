Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq

Namespace DxSample

    Public Class Task
        Implements INotifyPropertyChanged

        Private nameField As String

        Private employeesField As IList

        Public Sub New(ByVal name As String)
            nameField = name
            employeesField = New List(Of Employee)()
        End Sub

        Public Property Name As String
            Get
                Return nameField
            End Get

            Set(ByVal value As String)
                If Equals(Name, value) Then Return
                nameField = value
                RaisePropertyChanged("Name")
            End Set
        End Property

        Public Sub AddEmployee(ByVal emp As Employee)
            employeesField.Add(emp)
        End Sub

        Public ReadOnly Property Employees As IList
            Get
                Return employeesField
            End Get
        End Property

        Public Sub ClearEmployees()
            Employees.Clear()
        End Sub

        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

    Public Class Employee

        Private nameField As String

        Private id As Integer

        Public Sub New(ByVal name As String)
            nameField = name
            id = IdGenerator.GetId()
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Friend Function GetId() As Integer
            Return id
        End Function

        Public ReadOnly Property Name As String
            Get
                Return nameField
            End Get
        End Property
    End Class

    Public Class IdGenerator

        Private val As Integer

        Protected Sub New()
            val = 0
        End Sub

        Protected Function GetIdInt() As Integer
            Return Math.Min(Threading.Interlocked.Increment(val), val - 1)
        End Function

        Private Shared instance As IdGenerator = New IdGenerator()

        Public Shared Function GetId() As Integer
            Return instance.GetIdInt()
        End Function
    End Class

    Public Class EmployeesRegistry

        Private employees As IList(Of Employee)

        Public Sub New()
            employees = LoadEmployees()
        End Sub

        Private Function LoadEmployees() As IList(Of Employee)
            Dim list As BindingList(Of Employee) = New BindingList(Of Employee)()
            list.Add(New Employee("John"))
            list.Add(New Employee("Mark"))
            list.Add(New Employee("Paul"))
            list.Add(New Employee("David"))
            list.Add(New Employee("Cindy"))
            list.Add(New Employee("Charlie"))
            list.Add(New Employee("James"))
            Return list
        End Function

        Private Shared registry As EmployeesRegistry = New EmployeesRegistry()

        Public Shared Function GetEmployees() As IList(Of Employee)
            Return registry.employees
        End Function

        Public Shared Function GetEmployee(ByVal id As Integer) As Employee
            Return registry.employees.FirstOrDefault(Function(emp) emp.GetId() = id)
        End Function

        Public Shared ReadOnly Property EmployeeCount As Integer
            Get
                Return registry.employees.Count
            End Get
        End Property
    End Class

    Public Class TasksRegistry

        Private tasks As IList(Of Task)

        Public Sub New()
            tasks = LoadTasks()
        End Sub

        Private Shared rd As Random = New Random()

        Protected Function LoadTasks() As IList(Of Task)
            Dim list As List(Of Task) = New List(Of Task)()
            For i As Integer = 0 To 20 - 1
                Dim task As Task = New Task(String.Format("Task {0}", i.ToString()))
                For n As Integer = 0 To 3 - 1
                    Dim id As Integer = rd.Next(0, EmployeesRegistry.EmployeeCount)
                    task.AddEmployee(EmployeesRegistry.GetEmployee(id))
                Next

                list.Add(task)
            Next

            Return list
        End Function

        Private Shared instance As TasksRegistry = New TasksRegistry()

        Public Shared Function GetTasks() As IList(Of Task)
            Return instance.tasks
        End Function
    End Class
End Namespace
