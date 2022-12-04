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
    public partial class Rooms : Form
    {
        public Rooms()
        {
            InitializeComponent();
        }

        private void txtCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public void FillGrid(string searchvalue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchvalue.Trim()))
                {
                    query = "select RoomID [ID], RoomNo, StudentCapacity,IsActive [Status] from RoomsTable";
                }
                else
                {
                    query = "select RoomID [ID], RoomNo, StudentCapacity,IsActive [Status] from RoomsTable where RoomNo like '%" + searchvalue.Trim() + "%'";
                }
                DataTable Roomlist = DatabaseLayer.Retrive(query);
                dgvRooms.DataSource = Roomlist;
                if (dgvRooms.Rows.Count > 0)
                {
                    dgvRooms.Columns[0].Width = 80;
                    dgvRooms.Columns[1].Width = 150;
                    dgvRooms.Columns[2].Width = 150;
                    dgvRooms.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
            catch
            {

                MessageBox.Show("Some unexpected issues occured please try again");
            }
        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }
        private void txtRoomNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtRoomNo.Text.Trim().Length == 0 || txtRoomNo.Text.Trim().Length > 11)
            {
                ep.SetError(txtRoomNo, "Please Enter Correct Room No!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;

            }
            if (txtCapacity.Text.Trim().Length > 11)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Room Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from RoomsTable where RoomNo ='" + txtRoomNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exists!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }

            }
            string insertquery = string.Format("Insert into RoomsTable(RoomNo,StudentCapacity,IsActive) values('{0}','{1}','{2}')", txtRoomNo.Text.ToUpper().Trim(), txtCapacity.Text.ToLower().Trim(), chkStatus.Checked, Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Room Details then try again");

            }
        }
        public void ClearForm()
        {
            txtRoomNo.Clear();
            txtCapacity.Clear();
            chkStatus.Checked = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        public void EnableComponents()
        {
            dgvRooms.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvRooms.Enabled = true;
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
            if (dgvRooms != null)
            {
                if (dgvRooms.Rows.Count > 0)
                {
                    if (dgvRooms.SelectedRows.Count == 1)
                    {
                        txtRoomNo.Text = Convert.ToString(dgvRooms.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvRooms.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvRooms.CurrentRow.Cells[3].Value);
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
            if (txtRoomNo.Text.Trim().Length == 0 || txtRoomNo.Text.Trim().Length > 11)
            {
                ep.SetError(txtRoomNo, "Please Enter Correct Room No!");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;

            }
            if (txtCapacity.Text.Trim().Length > 11)
            {
                ep.SetError(txtCapacity, "Please Enter Correct Room Capacity!");
                txtCapacity.Focus();
                txtCapacity.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from RoomsTable where RoomNo ='" + txtRoomNo.Text.ToUpper().Trim() + "' and RoomID != '" + Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value) + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exists!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }
            string Updatequery = string.Format("update RoomsTable set RoomNo = '{0}', StudentCapacity = '{1}', IsActive = '{3}' where RoomID = '{3}'", txtRoomNo.Text.ToUpper().Trim(), txtCapacity.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvRooms.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(Updatequery);
            if (result == true)
            {
                MessageBox.Show("Update Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Room Details then try again");

            }
        }


    }
}
