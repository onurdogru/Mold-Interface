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
using TamOtomatikBlisterMakinesi2.Properties;

namespace TamOtomatikBlisterMakinesi2
{
	public partial class Form4 : Form
	{
		string result = "";
		string message = "";
		public Form4(string result)
		{
			InitializeComponent();
			this.result = result;
		}
		private void Form4_Load(object sender, EventArgs e)
		{
			if (result== "1")
			{
				message = "İş Emri Tamamlandı !";
				pictureBox1.BackgroundImage = (Image)Resources.complate;
			}
			else
			{
				message = "İş Emri Tamamlanamadı !";
				pictureBox1.BackgroundImage = (Image)Resources.cross;
			}
			lblMessage.Text = this.message;
		}
		private void numberControl(TextBox textBox, KeyPressEventArgs e)
		{

			if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
			{
				e.Handled = true;
			}

		}

	

		private void Form3_FormClosing(object sender, FormClosingEventArgs e)
		{
		
		}

	
	}
}
