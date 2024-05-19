using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TamOtomatikBlisterMakinesi2
{
	public partial class Form3 : Form
	{
		Form1 form1;
		public Form3(Form1 form)
		{
			InitializeComponent();
			this.form1 = form;
		}
		private void numberControl(TextBox textBox, KeyPressEventArgs e)
		{

			if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
			{
				e.Handled = true;
			}

		}

		private void txtSicil_KeyPress(object sender, KeyPressEventArgs e)
		{
			numberControl(txtSicil, e);
		}

		private void btnProductionEnd_Click(object sender, EventArgs e)
		{
			
			if (txtSicil.Text!="")
			{
				this.form1.sicilNo = txtSicil.Text;
				this.form1.isBitirEmri = true;
				this.Close();
			}
		}

		private void Form3_FormClosing(object sender, FormClosingEventArgs e)
		{
		
		}
	}
}
