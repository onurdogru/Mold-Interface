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
	public partial class Form2 : Form
	{
		Form1 form1;
		public Form2(DataTable dataTable,Form1 form)
		{
			InitializeComponent();
			dataGridView1.DataSource = dataTable;
			this.form1 = form;
		}

		private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			form1.txtPrdOrder.Text=dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
			form1.txtUProductionModel.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			form1.txtUProductionUnit.Text =Convert.ToString(Int16.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Split('.')[0]) - Int16.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString().Split('.')[0]));
		}
	}
}
