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
    public partial class Days : Form
    {
        public Days()
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
                    query = "select DayID [ID], Name,IsActive [Status] from DayTable";
                }
                else
                {
                    query = "select DayID [ID], Name,IsActive [Status] from DayTable where Name like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Daylist = DatabaseLayer.Retrive(query);
                dgvDay.DataSource = Daylist;
                if (dgvDay.Rows.Count > 0)
                {
                    dgvDay.Columns[0].Width = 80;
                    dgvDay.Columns[1].Width = 150;
                    dgvDay.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
            catch
            {

                MessageBox.Show("Some unexpected issues occured please try again");
            }
        }


        private void txtDayName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Days_Load(object sender, EventArgs e)
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
            if (txtDayName.Text.Trim().Length > 11)
            {
                ep.SetError(txtDayName, "Please Enter Correct Day!");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from DayTable where Name ='" + txtDayName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exists!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("Insert into DayTable(Name,IsActive) values('{0}','{1}')", txtDayName.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Day Details then try again");

            }
        }
        public void ClearForm()
        {
            txtDayName.Clear();
            chkStatus.Checked = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        public void EnableComponents()
        {
            dgvDay.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvDay.Enabled = true;
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
            if (dgvDay != null)
            {
                if (dgvDay.Rows.Count > 0)
                {
                    if (dgvDay.SelectedRows.Count == 1)
                    {
                        txtDayName.Text = Convert.ToString(dgvDay.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvDay.CurrentRow.Cells[2].Value);
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
            if (txtDayName.Text.Trim().Length > 9)
            {
                ep.SetError(txtDayName, "Please Enter Correct Day Name!");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from DayTable where Name ='" + txtDayName.Text.Trim() + "' and DayID != '" + Convert.ToString(dgvDay.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exists!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }
            }
            string Updatequery = string.Format("update DayTable set Name = '{0}', IsActive = '{1}' where DayID = '{2}'", txtDayName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvDay.CurrentRow.Cells[0].Value));
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
