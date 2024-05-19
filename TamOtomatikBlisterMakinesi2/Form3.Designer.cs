
namespace TamOtomatikBlisterMakinesi2
{
	partial class Form3
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtSicil = new System.Windows.Forms.TextBox();
			this.btnProductionEnd = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(53)))), ((int)(((byte)(65)))));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.3127F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.03704F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.74074F));
			this.tableLayoutPanel1.Controls.Add(this.txtSicil, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnProductionEnd, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.89552F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.35821F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.85075F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 201);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// txtSicil
			// 
			this.txtSicil.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtSicil.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.txtSicil.Location = new System.Drawing.Point(123, 45);
			this.txtSicil.Name = "txtSicil";
			this.txtSicil.Size = new System.Drawing.Size(328, 28);
			this.txtSicil.TabIndex = 1;
			this.txtSicil.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSicil_KeyPress);
			// 
			// btnProductionEnd
			// 
			this.btnProductionEnd.BackColor = System.Drawing.Color.DarkRed;
			this.btnProductionEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnProductionEnd.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnProductionEnd.FlatAppearance.BorderSize = 3;
			this.btnProductionEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnProductionEnd.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.btnProductionEnd.ForeColor = System.Drawing.Color.White;
			this.btnProductionEnd.Location = new System.Drawing.Point(123, 102);
			this.btnProductionEnd.Name = "btnProductionEnd";
			this.btnProductionEnd.Size = new System.Drawing.Size(328, 40);
			this.btnProductionEnd.TabIndex = 21;
			this.btnProductionEnd.Text = "İş Bitir !";
			this.btnProductionEnd.UseVisualStyleBackColor = false;
			this.btnProductionEnd.Click += new System.EventHandler(this.btnProductionEnd_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(3, 47);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 23);
			this.label1.TabIndex = 22;
			this.label1.Text = "Sicil No";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(540, 201);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Form3";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IAS İŞ EMİRLERİ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtSicil;
		private System.Windows.Forms.Button btnProductionEnd;
		private System.Windows.Forms.Label label1;
	}
}