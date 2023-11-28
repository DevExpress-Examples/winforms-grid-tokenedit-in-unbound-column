using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DxSample {
    public class Task : INotifyPropertyChanged {
        string name;
        IList employees;
        public Task(string name) {
            this.name = name;
            this.employees = new List<Employee>();
        }
        public string Name {
            get { return name; }
            set {
                if(Name == value)
                    return;
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public void AddEmployee(Employee emp) {
            this.employees.Add(emp);
        }
        public IList Employees {
            get { return employees; }
        }
        public void ClearEmployees() {
            Employees.Clear();
        }

        protected void RaisePropertyChanged(string propertyName) {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Employee {
        string name;
        int id;
        public Employee(string name) {
            this.name = name;
            this.id = IdGenerator.GetId();
        }

        public override string ToString() {
            return Name;
        }
        internal int GetId() {
            return id;
        }
        public string Name { get { return name; } }
    }

    public class IdGenerator {
        int val;
        protected IdGenerator() {
            this.val = 0;
        }
        protected int GetIdInt() { return val++; }
        
        static IdGenerator instance = new IdGenerator();
        public static int GetId() {
            return instance.GetIdInt();
        }
    }
    
    public class EmployeesRegistry {
        IList<Employee> employees;
        public EmployeesRegistry() {
            this.employees = LoadEmployees();
        }
        IList<Employee> LoadEmployees() {
            BindingList<Employee> list = new BindingList<Employee>();
            list.Add(new Employee("John"));
            list.Add(new Employee("Mark"));
            list.Add(new Employee("Paul"));
            list.Add(new Employee("David"));
            list.Add(new Employee("Cindy"));
            list.Add(new Employee("Charlie"));
            list.Add(new Employee("James"));
            return list;
        }
        static EmployeesRegistry registry = new EmployeesRegistry();
        public static IList<Employee> GetEmployees() {
            return registry.employees;
        }
        public static Employee GetEmployee(int id) {
            return registry.employees.FirstOrDefault(emp => emp.GetId() == id);
        }
        public static int EmployeeCount {
            get { return registry.employees.Count; }
        }
    }

    public class TasksRegistry {
        IList<Task> tasks;
        public TasksRegistry() {
            this.tasks = LoadTasks();
        }
        static Random rd = new Random();
        protected IList<Task> LoadTasks() {
            List<Task> list = new List<Task>();
            for(int i = 0; i < 20; i++) {
                Task task = new Task(string.Format("Task {0}", i.ToString()));
                for(int n = 0; n < 3; n++) {
                    int id = rd.Next(0, EmployeesRegistry.EmployeeCount);
                    task.AddEmployee(EmployeesRegistry.GetEmployee(id));
                }
                list.Add(task);
            }
            return list;
        }
        static TasksRegistry instance = new TasksRegistry();

        public static IList<Task> GetTasks() { return instance.tasks; }
    }
}
