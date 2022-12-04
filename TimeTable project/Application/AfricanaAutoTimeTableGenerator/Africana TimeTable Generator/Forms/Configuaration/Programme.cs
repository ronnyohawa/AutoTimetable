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
    public partial class Programme : Form
    {
        public Programme()
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
                    query = "select ProgramID [ID], Name[Program],IsActive [Status] from ProgramTable";
                }
                else
                {
                    query = "select ProgramID [ID], Name[Program],IsActive [Status] from ProgramTable where Name like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Programlist = DatabaseLayer.Retrive(query);
                dgvProgram.DataSource = Programlist;
                if (dgvProgram.Rows.Count > 0)
                {
                    dgvProgram.Columns[0].Width = 80;
                    dgvProgram.Columns[1].Width = 150;
                    dgvProgram.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    
                }
            }
            catch
            {

                MessageBox.Show("Some unexpected issues occured please try again");
            }
        }

        private void Program_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void txtProgramName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtProgramName.Text.Trim().Length > 11)
            {
                ep.SetError(txtProgramName, "Please Enter Correct Program Name!");
                txtProgramName.Focus();
                txtProgramName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from ProgramTable where Name ='" + txtProgramName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramName, "Already Exists!");
                    txtProgramName.Focus();
                    txtProgramName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("Insert into ProgramTable(Name,IsActive) values('{0}','{1}')", txtProgramName.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Program Details then try again");

            }
        }
        public void ClearForm()
        {
            txtProgramName.Clear();
            chkStatus.Checked = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        public void EnableComponents()
        {
            dgvProgram.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvProgram.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvProgram != null)
            {
                if (dgvProgram.Rows.Count > 0)
                {
                    if (dgvProgram.SelectedRows.Count == 1)
                    {
                        txtProgramName.Text = Convert.ToString(dgvProgram.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgram.CurrentRow.Cells[2].Value);
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
            if (txtProgramName.Text.Trim().Length > 9)
            {
                ep.SetError(txtProgramName, "Please Enter Correct Program Name!");
                txtProgramName.Focus();
                txtProgramName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from ProgramTable where Name ='" + txtProgramName.Text.Trim() + "' and ProgramID != '" + Convert.ToString(dgvProgram.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtProgramName, "Already Exists!");
                    txtProgramName.Focus();
                    txtProgramName.SelectAll();
                    return;
                }
            }
            string Updatequery = string.Format("update ProgramTable set Name = '{0}', IsActive = '{1}' where ProgramID = '{2}'", txtProgramName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvProgram.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(Updatequery);
            if (result == true)
            {
                MessageBox.Show("Update Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Program Details then try again");

            }
        }
    }
}
