using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImportMapPointAreas
{
    public partial class frmNewArea : Form
    {
        private string _areaName = string.Empty;

        public string AreaName
        {
            get { return _areaName; }
            set { _areaName = value; }
        }

        public frmNewArea()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _areaName = tbAreaName.Text.Trim();
            this.Hide();
        }
    }
}