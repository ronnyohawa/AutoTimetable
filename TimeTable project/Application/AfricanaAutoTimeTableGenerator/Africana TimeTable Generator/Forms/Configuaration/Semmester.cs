using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Africana_TimeTable_Generator.Forms.Configuaration
{
    public partial class Semmester : Form
    {
        public Semmester()
        {
            InitializeComponent();
        }

        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select TermID [ID], TermName,IsActive [Status] from TermTable";
                }
                else
                {
                    query = "select TermID [ID], TermName,IsActive [Status] from TermTable where TermName like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Semesterlist = DatabaseLayer.Retrive(query);
                dgvSemester.DataSource = Semesterlist;
                if (dgvSemester.Rows.Count > 0)
                {
                    dgvSemester.Columns[0].Width = 80;
                    dgvSemester.Columns[1].Width = 150;
                    dgvSemester.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    
                }
            }
            catch
            {

                MessageBox.Show("Some unexpected issues occured please try again");
            }
        }

        private void Semmester_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSemesterName.Text.Trim().Length > 11)
            {
                ep.SetError(txtSemesterName, "Please Enter Correct Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from TermTable where TermName ='" + txtSemesterName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exists!");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("Insert into TermTable(TermName,IsActive) values('{0}','{1}')", txtSemesterName.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Semester Details then try again");

            }
        }
        public void ClearForm()
        {
            txtSemesterName.Clear();
            chkStatus.Checked = false;
        }
        public void EnableComponents()
        {
            dgvSemester.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvSemester.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvSemester != null)
            {
                if (dgvSemester.Rows.Count > 0)
                {
                    if (dgvSemester.SelectedRows.Count == 1)
                    {
                        txtSemesterName.Text = Convert.ToString(dgvSemester.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvSemester.CurrentRow.Cells[2].Value);
                        EnableComponents();
                    }
                    else
                    {
                        MessageBox.Show("Please select one record");
                    }
                }
                else
                {
                    MessageBox.Show("List is empty");
                }
            }
            else
            {
                MessageBox.Show("List is Empty");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSemesterName.Text.Trim().Length > 9)
            {
                ep.SetError(txtSemesterName, "Please Enter Correct Semester Name!");
                txtSemesterName.Focus();
                txtSemesterName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from TermTable where TermName ='" + txtSemesterName.Text.Trim() + "' and TermID != '" + Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSemesterName, "Already Exists!");
                    txtSemesterName.Focus();
                    txtSemesterName.SelectAll();
                    return;
                }
            }
            string Updatequery = string.Format("update TermTable set TermName = '{0}', IsActive = '{1}' where TermID = '{2}'", txtSemesterName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvSemester.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(Updatequery);
            if (result == true)
            {
                MessageBox.Show("Update Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Semester Details then try again");

            }
        }

        private void txtSemesterName_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
