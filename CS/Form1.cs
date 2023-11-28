using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;

namespace DxSample {
    public partial class Form1 : Form {
        RepositoryItemTokenEdit tokenEditRep;
        public Form1() {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            this.tokenEditRep = CreateRep();
            this.gridControl.DataSource = TasksRegistry.GetTasks();
            InitUnboundColumn();
        }
        protected RepositoryItemTokenEdit CreateRep() {
            RepositoryItemTokenEdit rep = new RepositoryItemTokenEdit();
            rep.BeginInit();
            rep.Tokens.BeginUpdate();
            rep.AutoHeightMode = TokenEditAutoHeightMode.RestrictedExpand;
            rep.MaxExpandLines = 1;
            try {
                rep.Name = "rep";
                rep.EditValueType = TokenEditValueType.List;
                gridControl.RepositoryItems.Add(rep);
                foreach(Employee emp in EmployeesRegistry.GetEmployees()) {
                    rep.Tokens.AddToken(emp.Name, emp.GetId());
                }
            }
            finally {
                rep.Tokens.EndUpdate();
                rep.EndInit();
            }
            return rep;
        }
        protected void InitUnboundColumn() {
            GridColumn col = new GridColumn() {
                Name = "col",
                Caption = "Unbound Col",
                FieldName = "col",
                Visible = true,
                UnboundType = UnboundColumnType.Object
            };
            col.OptionsColumn.AllowEdit = true;
            gridView.Columns.Add(col);
            gridView.CustomUnboundColumnData += OnGridViewCustomUnboundColumnData;
            col.ColumnEdit = tokenEditRep;
        }

        void OnGridViewCustomUnboundColumnData(object sender, CustomColumnDataEventArgs e) {
            Task task = e.Row as Task;
            if(task == null)
                return;
            if(e.IsGetData) {
                var ids = new List<int>();
                foreach(Employee emp in task.Employees) {
                    int newId = emp.GetId();
                    if(!ids.Contains(newId)) ids.Add(newId);
                }
                e.Value = ids;
            } else {
                var ids = e.Value as List<int>;
                task.Employees.Clear();
                foreach(var id in ids) {
                    Employee emp = EmployeesRegistry.GetEmployee(id);
                    task.AddEmployee(emp);
                }
            }
        }
    }
}
