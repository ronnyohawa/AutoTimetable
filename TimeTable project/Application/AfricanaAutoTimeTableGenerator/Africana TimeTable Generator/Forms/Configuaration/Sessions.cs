﻿using System;
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
    public partial class Sessions : Form
    {
        public Sessions()
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
                    query = "select SessionID [ID], Title,IsActive [Status] from SessionTable";
                }
                else
                {
                    query = "select SessionID [ID], Title,IsActive [Status] from SessionTable where Title like '%" + searchvalue.Trim() + "%'";
                }
                DataTable sessionlist = DatabaseLayer.Retrive(query);
                dgvSession.DataSource = sessionlist;
                if (dgvSession.Rows.Count > 0)
                {
                    dgvSession.Columns[0].Width = 80;
                    dgvSession.Columns[1].Width = 150;
                    dgvSession.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
            catch
            {

                MessageBox.Show("Some unexpected issues occured please try again");
            }
        }

        private void txtSessionTittle_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
       

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if(txtSessionTittle.Text.Trim().Length > 9)
            {
                ep.SetError(txtSessionTittle, "Please Enter Correct Session Title!");
                txtSessionTittle.Focus();
                txtSessionTittle.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from SessionTable where Title ='" + txtSessionTittle.Text.Trim() + "'");
            if(checktitle != null)
            {
                if(checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSessionTittle, "Already Exists!");
                    txtSessionTittle.Focus();
                    txtSessionTittle.SelectAll();
                    return;
                }
            }
            string insertquery = string.Format("Insert into SessionTable(Title,IsActive) values('{0}','{1}')", txtSessionTittle.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if(result == true)
            {
                MessageBox.Show("Save Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Sesssion Details then try again");

            }

        }

        public void ClearForm()
        {
            txtSessionTittle.Clear();
            chkStatus.Checked = false;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        public void EnableComponents()
        {
            dgvSession.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvSession.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }
        private void cmsedit_Click(object sender, EventArgs e)
        {
            if(dgvSession != null)
            {
                if(dgvSession.Rows.Count > 0)
                {
                    if(dgvSession.SelectedRows.Count == 1)
                    {
                        txtSessionTittle.Text = Convert.ToString(dgvSession.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvSession.CurrentRow.Cells[2].Value);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtSessionTittle.Text.Trim().Length > 9)
            {
                ep.SetError(txtSessionTittle, "Please Enter Correct Session Title!");
                txtSessionTittle.Focus();
                txtSessionTittle.SelectAll();
                return;

            }
            DataTable checktitle = DatabaseLayer.Retrive("select * from SessionTable where Title ='" + txtSessionTittle.Text.Trim() + "' and SessionID != '"+ Convert.ToString(dgvSession.CurrentRow.Cells[0].Value)+"'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtSessionTittle, "Already Exists!");
                    txtSessionTittle.Focus();
                    txtSessionTittle.SelectAll();
                    return;
                }
            }
            string Updatequery = string.Format("update SessionTable set Title = '{0}', IsActive = '{1}' where SessionID = '{2}'", txtSessionTittle.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvSession.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(Updatequery);
            if (result == true)
            {
                MessageBox.Show("Update Succesfull");
                DisableComponents();

            }
            else
            {
                MessageBox.Show("Please provide Correct Sesssion Details then try again");

            }
        }

        private void Sessions_Load(object sender, EventArgs e)
        {

        }
    }
}
