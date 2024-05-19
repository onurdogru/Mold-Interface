using iasWs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TamOtomatikBlisterMakinesi2
{
	public partial class Form1 : Form
	{
		SqlConnection sqlConnection;
		SqlCommand sqlCommand;
		SqlDataReader sqlDataReader;
		SqlDataAdapter sqlDataAdapter;
		static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		string modelFolderPath = desktopPath + @"\Debug";
		string modelFilePath = desktopPath + @"\Debug\Modeller.ini";
		string sicilFilePath = desktopPath + @"\Debug\Siciller.ini";
		string connection;
		INIKaydet ini;
		bool boolReadStatus, boolIsEmri;
		int readLoopCounter = 0, countModel = 0, selectedModel = 0, dropDownHeight=1;
		Thread threadRead,threadWriteBool,threadReadString, threadWriteString;
		WsConnector iasWebServis;
		public string sicilNo;
		public bool isBitirEmri;
		List<string> sicilList;

		public Form1()
		{
			InitializeComponent();
		}
		public class INIKaydet
		{
			[DllImport("kernel32")]
			private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

			[DllImport("kernel32")]
			private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

			public INIKaydet(string dosyaYolu)
			{
				DOSYAYOLU = dosyaYolu;
			}
			private string DOSYAYOLU = String.Empty;
			public string Varsayilan { get; set; }
			public string Oku(string bolum, string ayaradi)
			{
				Varsayilan = Varsayilan ?? string.Empty;
				StringBuilder StrBuild = new StringBuilder(256);
				GetPrivateProfileString(bolum, ayaradi, Varsayilan, StrBuild, 255, DOSYAYOLU);
				return StrBuild.ToString();
			}
			public long Yaz(string bolum, string ayaradi, string deger)
			{
				return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
			}
			public long Sil(string bolum, string ayaradi, string deger)
			{
				return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
			Control.CheckForIllegalCrossThreadCalls = false;
			cmbBxWorkingMod.SelectedIndex = 0;
			ini = new INIKaydet(modelFilePath);
			getModelName();
			getModelSetting();
			getModelSelectedData();
			//dropDownHeight = cmbBxModelSelect.DropDownHeight;
			
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2connectionStart", true));
			threadWriteBool.Start();
			Thread.Sleep(100);

		
			if (dataAccessProccess())
				btnDBStatus.BackColor = Color.Green;
			

			if (nxCompoletBoolRead("k2connectionOk") && boolReadStatus == false)
			{
				btnStatus.BackColor = Color.ForestGreen;
				txtBxUnit.Text = nxCompoletStringRead("k2Adet");
				threadRead = new Thread(readPlcData);
				threadRead.Start();
			}
			else
			{
				btnStatus.BackColor = Color.Red;
			}
			
		}
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Setting.Default.heatingTime = txtBxHeatingTime.Text;
			Setting.Default.vacuumTime = txtBxVacuumTime.Text;
			Setting.Default.vacuumAfter = txtBxVacuumAfter.Text;
			Setting.Default.coolingTime = txtCoolingTime.Text;
			Setting.Default.txtPrdOrder = txtPrdOrder.Text;
			Setting.Default.txtUProductionModel = txtUProductionModel.Text;
			Setting.Default.txtUProductionUnit = txtUProductionUnit.Text;
			Setting.Default.Save();
			
			try
			{
			//	sqlConnection.Close();
				threadWriteBool = new Thread(() => nxCompoletBoolWrite("connectionStart", false));
				threadWriteBool.Start();
				threadRead.Abort();
				Thread.Sleep(100);
			}
			catch (Exception)
			{
			}
			Environment.Exit(1);
		}

		private bool dataAccessProccess()
		{

			string server = @"192.168.10.22";
			string database = "ALP802";
			string user = "otomasyon";
			string pass = "123KUM*";
			String connection = @"Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + user + ";Password=" + pass;
			sqlConnection = new SqlConnection(connection);

			try
			{
			    iasWebServis = new WsConnector();
				iasWebServis.SetParameters("", "CANIAS", "192.168.10.21:27499", "ALP802", "ARGBLISTER", "Blister123*");
				this.connection = connection;
				sqlConnection.Open();
				sqlConnection.Close();
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show( "Veritabanı bağlantısı kurulamadı !");
				return false;
			}

		}


		private void readPlcData()
		{
		
			while (true)
			{
				Thread.Sleep(100);
				if (readLoopCounter == 10)
				{
					readLoopCounter = 0;

					/***********************HARDWARE**********************************/
					if (nxCompoletBoolRead("k2Donanim[" + 0 + "]") && !boolReadStatus)
					{
						btnTopPressUp.BackColor = Color.Green;
					}
					else
					{
						btnTopPressUp.BackColor = Color.FromArgb(41,53,65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 1 + "]") && !boolReadStatus)
					{
						btnTopPressDown.BackColor = Color.Green;
					}
					else
					{
						btnTopPressDown.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 2 + "]") && !boolReadStatus)
					{
						btnBottomPressUp.BackColor = Color.Green;
					}
					else
					{
						btnBottomPressUp.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 3 + "]") && !boolReadStatus)
					{
						btnBottomPressDown.BackColor = Color.Green;
					}
					else
					{
						btnBottomPressDown.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 4 + "]") && !boolReadStatus)
					{
						btnElavotorUp.BackColor = Color.Green;
					}
					else
					{
						btnElavotorUp.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 5 + "]") && !boolReadStatus)
					{
						btnElavotorDown.BackColor = Color.Green;
					}
					else
					{
						btnElavotorDown.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 6 + "]") && !boolReadStatus)
					{
						btnPressingUp.BackColor = Color.Green;
					}
					else
					{
						btnPressingUp.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 7 + "]") && !boolReadStatus)
					{
						btnPressingDown.BackColor = Color.Green;
					}
					else
					{
						btnPressingDown.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 8 + "]") && !boolReadStatus)
					{
						btnDoorSensor.BackColor = Color.Green;
					}
					else
					{
						btnDoorSensor.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 9 + "]") && !boolReadStatus)
					{
						btnElavotorDoorSensor.BackColor = Color.Green;
					}
					else
					{
						btnElavotorDoorSensor.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 12 + "]") && !boolReadStatus)
					{
						btnHomeOk.BackColor = Color.Green;
					}
					else
					{
						btnHomeOk.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 11 + "]") && !boolReadStatus)
					{
						btnResistanceTooHot.BackColor = Color.Green;
					}
					else
					{
						btnResistanceTooHot.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 10 + "]") && !boolReadStatus)
					{
						btnStop.BackColor = Color.Green;
					}
					else
					{
						btnStop.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 13 + "]") && !boolReadStatus)
					{
						btnRawMaterial.BackColor = Color.Green;
					}
					else
					{
						btnRawMaterial.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 14 + "]") && !boolReadStatus)
					{
						btnExecutiveEngineMalfunction.BackColor = Color.Green;
					}
					else
					{
						btnExecutiveEngineMalfunction.BackColor = Color.FromArgb(41, 53, 65);
					}
					if (nxCompoletBoolRead("k2Donanim[" + 15 + "]") && !boolReadStatus)
					{
						btnResistorMotorFault.BackColor = Color.Green;
					}
					else
					{
						btnResistorMotorFault.BackColor = Color.FromArgb(41, 53, 65);
					}
					/*********************************************************/
					if (nxCompoletBoolRead("k2Oku"))
					{
						txtBxUnit.Text = nxCompoletStringRead("k2Adet");
						txtBxCycleTime.Text = nxCompoletStringRead("k2Sure");
					}
					if (nxCompoletStringRead("k2Uretim")=="0")
					{
						lblProductionStatus.Text = "Üretim Yok";
					}
					else if (nxCompoletStringRead("k2Uretim") == "1")
					{
						lblProductionStatus.Text = "Üretim Yapılıyor...";
					}
					else if (nxCompoletStringRead("k2Uretim") == "2")
					{
						lblProductionStatus.Text = "Üretim Durduruldu !";
					}
					else if (nxCompoletStringRead("k2Uretim") == "3")
					{
						lblProductionStatus.Text = "Lift Doldu !";
					}
					else if (nxCompoletStringRead("k2Uretim") == "4")
					{
						lblProductionStatus.Text = "Üretim Bitiriliyor...";
					}
					else if (nxCompoletStringRead("k2Uretim") == "5")
					{
						lblProductionStatus.Text = "Ham Madde Yok !";
					}
					else if (nxCompoletStringRead("k2Uretim") == "6")
					{
						lblProductionStatus.Text = "İstenilen Adet Üretildi !";
					}
					if (nxCompoletBoolRead("k2Sistem") && !boolReadStatus)
					{
						txtPrdOrder.ReadOnly = true;
						//btnGetPrdOrder.Enabled = false;
						boolIsEmri = false;
						btnResetProductionUnit.Enabled = false;
						cmbBxModelSelect.Enabled = false;
						//cmbBxModelSelect.DropDownHeight = 50;
					}
					else
					{
						txtPrdOrder.ReadOnly = false;
						//btnGetPrdOrder.Enabled = true;
						boolIsEmri = true;
						btnResetProductionUnit.Enabled = true;
						cmbBxModelSelect.Enabled = true;//true
						//cmbBxModelSelect.DropDownHeight = 106;
					}
					if (nxCompoletBoolRead("k2Sicaklik") && !boolReadStatus)
					{
						lblHeatingStatus.Text = "Açık";
					}
					else
					{
						lblHeatingStatus.Text = "Kapalı";
					}
				}
				readLoopCounter++;
			}
		}

		/*******************************BUTTON FONKSİYONLARI************************************/
		private void lblMin_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void lblExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void lblMin_MouseHover(object sender, EventArgs e)
		{
			lblMin.ForeColor = Color.Red;
		}

		private void lblMin_MouseLeave(object sender, EventArgs e)
		{
			lblMin.ForeColor = Color.White;
		}

		private void lblExit_MouseHover(object sender, EventArgs e)
		{
			lblExit.ForeColor = Color.Red;
		}

		private void lblExit_MouseLeave(object sender, EventArgs e)
		{
			lblExit.ForeColor = Color.White;

		}
		private void btnHome_MouseDown(object sender, MouseEventArgs e)
		{
			btnHome.ForeColor = Color.FromArgb(255, 180, 0);
		}

		private void btnHome_MouseUp(object sender, MouseEventArgs e)
		{
			btnHome.ForeColor = Color.White;
		}

		private void btnHome_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Home", true));
			threadWriteBool.Start();
		}


		private void btnModelSelectSave_MouseDown(object sender, MouseEventArgs e)
		{
			btnModelSelectSave.BackColor = Color.Green;
		}

		private void btnModelSelectSave_MouseUp(object sender, MouseEventArgs e)
		{
			btnModelSelectSave.BackColor = Color.ForestGreen;
		}

		private void btnSelectModeNewRecord_MouseDown(object sender, MouseEventArgs e)
		{
			btnSelectModeNewRecord.ForeColor = Color.FromArgb(255, 180, 0);
		}

		private void btnSelectModeNewRecord_MouseUp(object sender, MouseEventArgs e)
		{
			btnSelectModeNewRecord.ForeColor = Color.White;
		}

		private void btnModelSettingsSend_MouseDown(object sender, MouseEventArgs e)
		{
			btnModelSettingsSend.BackColor = Color.Navy;
		}

		private void btnModelSettingsSend_MouseUp(object sender, MouseEventArgs e)
		{
			btnModelSettingsSend.BackColor = Color.MidnightBlue;
		}
		/** Combobox ve textboxların textlerini temizlyen ve kontrol eden fonksiyon*/
		private void textClear(ComboBox comboBox, params TextBox[] textBoxes)
		{
			foreach (var textBox in textBoxes)
			{
				textBox.Text = "";
			}
			comboBox.Text = "";
		}
		private bool textEmptyControl(ComboBox comboBox, params TextBox[] textBoxes)
		{
			bool flag = false;
			foreach (var textBox in textBoxes)
			{
				if (textBox.Text == "")
					flag = true;
			}
			if (comboBox.Text == "")
				flag = true;
			return flag;
		}
	
		/*******************************MODEL FONKSİYONLARI************************************/
		private void getModelName()
		{
			if(Directory.Exists(modelFolderPath) && File.Exists(modelFilePath))
			{
				cmbBxModelSelect.Items.Clear();
				int modelCount = Int16.Parse(ini.Oku("Model", "Model Count").Trim());
				for (int i = 1; i <= modelCount; i++)
				{
					cmbBxModelSelect.Items.Add(ini.Oku("Model" + i.ToString(), "Model Name"));
				}
			}
		}
		private void getModelData(int index) 
		{
			cmbBxModelSelect.Text = ini.Oku("Model"+(index+1).ToString(), "Model Name").Trim();
			txtBxRunningDistance.Text= ini.Oku("Model" + (index + 1).ToString(), "Running Distance").Trim();
			txtBxMoldNumber.Text = ini.Oku("Model" + (index + 1).ToString(), "Mold Number").Trim();
		}

		private void getModelSetting()
		{
			txtBxHeatingTime.Text = Setting.Default.heatingTime;
			txtBxVacuumTime.Text = Setting.Default.vacuumTime;
			txtBxVacuumAfter.Text = Setting.Default.vacuumAfter;
			txtCoolingTime.Text = Setting.Default.coolingTime;
			txtPrdOrder.Text = Setting.Default.txtPrdOrder;
			txtUProductionModel.Text = Setting.Default.txtUProductionModel;
			txtUProductionUnit.Text = Setting.Default.txtUProductionUnit;
		}
		private void getModelSelectedData()
		{
			cmbBxModelSelect.SelectedIndex = Setting.Default.modelCmbBxIndex;
		}
		private void cmbBxModelSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			getModelData(cmbBxModelSelect.SelectedIndex);
			selectedModel = cmbBxModelSelect.SelectedIndex + 1;
			Setting.Default.modelCmbBxIndex = Int16.Parse(cmbBxModelSelect.SelectedIndex.ToString());
			Setting.Default.Save();

			if (File.Exists(modelFilePath))
				countModel = Int16.Parse(ini.Oku("Model", "Model Count").Trim());
			btnModelSelectSave.Enabled = true;
			btnModelSelectSend.Enabled = true;
			txtBxModelName.Text = cmbBxModelSelect.Text;
		}
	
		private void btnSelectModeNewRecord_Click(object sender, EventArgs e)
		{
			textClear(cmbBxModelSelect, txtBxRunningDistance, txtBxMoldNumber);
			selectedModel = cmbBxModelSelect.Items.Count + 1;
			countModel = cmbBxModelSelect.Items.Count+1;
			btnModelSelectSave.Enabled = true;
		}
		private void btnModelSelectSave_Click(object sender, EventArgs e)
		{
			modelSave("Kayıt işlemi başarılı !");
		}

		private void btnModelSelectSend_Click(object sender, EventArgs e)
		{
			if (cmbBxModelSelect.Text!="" && txtBxRunningDistance.Text!="" && txtBxMoldNumber.Text!="")
			{
				if (nxCompoletBoolWrite("k2RecipeSend",true))
				{
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2Model", cmbBxModelSelect.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2IasModel", txtUProductionModel.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2Uretilecek", txtUProductionUnit.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2YurutmeMesafe", txtBxRunningDistance.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2KalipSayi", txtBxMoldNumber.Text));
					threadWriteString.Start();
					modelSave("Gönderme işlemi başarılı !");
					Thread.Sleep(250);
					threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2RecipeSend", false));
					threadWriteBool.Start();
					if (cmbBxModelSelect.Text != txtUProductionModel.Text)
					{
						lblModelNameMessage.Text = "Model adları eşlemiyor !";
					}
					else
					{
						lblModelNameMessage.Text = "";
					}
				}
			}
			else
			{
				MessageBox.Show("Lütfen boşlukları doldurunuz !");
			}
		}
		private void modelSave(string message)
		{
			if (!textEmptyControl(cmbBxModelSelect, txtBxRunningDistance, txtBxMoldNumber))
			{

				if (!Directory.Exists(modelFolderPath))
					Directory.CreateDirectory(modelFolderPath);

				if (!File.Exists(modelFilePath))
				{
					ini.Yaz("Model", "Model Count", "0");
				}
				//countModel = Int16.Parse(ini.Oku("Model", "Model Count").Trim());
				ini.Yaz("Model" + (selectedModel).ToString(), "Model Name", cmbBxModelSelect.Text.Trim());
				ini.Yaz("Model" + (selectedModel).ToString(), "Running Distance", txtBxRunningDistance.Text.Trim());
				ini.Yaz("Model" + (selectedModel).ToString(), "Mold Number", txtBxMoldNumber.Text.Trim());
				ini.Yaz("Model", "Model Count", (countModel).ToString());
				getModelName();
				MessageBox.Show(message);

			}
			else
			{
				MessageBox.Show("Lütfen boşlukları doldurunuz !");
			}
		}

		private void btnModelSettingsSend_Click(object sender, EventArgs e)
		{
			if (txtBxHeatingTime.Text != "" && txtBxVacuumTime.Text != "" && txtBxVacuumAfter.Text != "" && txtCoolingTime.Text != "")
			{
				if (nxCompoletBoolWrite("k2Veri", true))
				{
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2IsitmaSure", txtBxHeatingTime.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2VakumSure", txtBxVacuumTime.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2VakumSonraSure", txtBxVacuumAfter.Text));
					threadWriteString.Start();
					threadWriteString = new Thread(() => nxCompoletStringWrite("k2SisirmeSure", txtCoolingTime.Text));
					threadWriteString.Start(); ;
					Thread.Sleep(1000);

					threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Veri", false));
					threadWriteBool.Start();
					MessageBox.Show("Veriler gönderildi !");


				}
			}
			else
			{
				MessageBox.Show("Lütfen boşlukları doldurunuz !");
			}


		}
		private void btnResetProductionUnit_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sifirla", true));
			threadWriteBool.Start();
			Thread.Sleep(100);
			txtBxUnit.Text = "0";
		}

		private void btnCut_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2KesimYap", true));
			threadWriteBool.Start();
		}

		/****** IAS KISMI*************/
		bool flag;
		private bool getSicilNum(string sicilNum)
		{
			flag = false;
			if (Directory.Exists(modelFolderPath) && File.Exists(sicilFilePath))
			{
				ini = new INIKaydet(sicilFilePath);
				string sicil = ini.Oku("Sicil", "Sicil_Number").Trim();
				for (int i = 0; i < sicil.Split(',').Length; i++)
				{
					if (sicil.Split(',')[i]==sicilNum)
					{
						flag = true;
					}
					
				}
				return flag;
			}
			else
			{
				return false;
			}
			
		}
		private void btnGetPrdOrder_Click(object sender, EventArgs e)
		{
			try
			{
				if (false)/*Blister ile ilgili iş emrilerinin listesini çekip form2 ye gönderdiğimiz kodlar.*/
				{
					sqlCommand = new SqlCommand("Select PRDORDER,MATERIAL,cast (QUANTITY as int) as QUANTITY from IASPRDORDER where MATERIAL Like 'BLS%'  order by PRDORDER", sqlConnection);
					sqlDataAdapter = new SqlDataAdapter(sqlCommand);
					DataTable dataTable = new DataTable();
					sqlDataAdapter.Fill(dataTable);
					Form2 form = new Form2(dataTable, this);
					form.ShowDialog();
					btnDBStatus.BackColor = Color.Green;
				}
			 else if(true)
				{
					
					DataTable dataTable = new DataTable();
					dataTable = iasWebServis.IsEmriSorgula("01", "20", "BLISTER1", "EL", "2%", "%BLS%", DateTime.Now, "");
					Form2 form = new Form2(dataTable, this);
					form.ShowDialog();
					btnDBStatus.BackColor = Color.Green;
				}
				else/*Blister ile ilgili iş emrileri tek tek çekebileceğimiz kodlar.*/
				{
					if (txtPrdOrder.Text.Length > 0)
					{
						sqlCommand = new SqlCommand("Select MATERIAL,cast (QUANTITY as int) as QUANTITY from IASPRDORDER where  PRDORDER='" + txtPrdOrder.Text + "' and MATERIAL Like 'BLS%'", sqlConnection);

						sqlDataReader = sqlCommand.ExecuteReader();

						if (sqlDataReader.Read())
						{
							txtUProductionModel.Text = (string)sqlDataReader["MATERIAL"];
							txtUProductionUnit.Text = sqlDataReader["QUANTITY"].ToString();
						}
						else
						{
							txtUProductionModel.Text = "";
							txtUProductionUnit.Text = "";
							MessageBox.Show("İş emri mevcut değildir !");
						}

						sqlDataReader.Close();
					}
					else
					{
						MessageBox.Show("İş emrini boş bırakmayınız");
					}
				}

			}
			catch (Exception ex)
			{
				btnDBStatus.BackColor = Color.Red;
				MessageBox.Show(ex.ToString() + " Veritabanı bağlantısı sağlanamadı !");
			}
		}

		private void btnProductionEnd_Click(object sender, EventArgs e)
		{
			Form3 form3 = new Form3(this);
			form3.ShowDialog();
			if (isBitirEmri)
			{
				if (txtPrdOrder.Text != "" && txtBxUnit.Text != "" && sicilNo != "")
				{
					if (getSicilNum(sicilNo))
					{
						string[] RetVal = { "0", "İşlem Yapılamadı!" };
						try
						{
							RetVal = iasWebServis.IsBitisOnay("01", "20", "EL", txtPrdOrder.Text, Int16.Parse(txtBxUnit.Text), "BLISTER1", "55", "55", "", "0", DateTime.Now, sicilNo, "", "");
						}
						catch (Exception)
						{

							RetVal[0] = "0";
						}

						Form4 form4 = new Form4(RetVal[0]);
						form4.ShowDialog();

						if (RetVal[0] != "1")
						{
							btnDBStatus.BackColor = Color.Red;
							txtUProductionUnit.Text = (int.Parse(txtUProductionUnit.Text) - int.Parse(txtBxUnit.Text)).ToString();
							threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Bitir", true));
							threadWriteBool.Start();
						}
						else
						{
							DataTable dataTable = new DataTable();
							dataTable = iasWebServis.IsEmriSorgula("01", "20", "BLISTER1", "EL", txtPrdOrder.Text, "%BLS%", DateTime.Now, "");
							foreach (DataRow row in dataTable.Rows)
							{
								txtUProductionUnit.Text = Convert.ToString(Int16.Parse(row["QUANTITY"].ToString().Split('.')[0]) - Int16.Parse(row["DELIVERED"].ToString().Split('.')[0]));
							}
							
							btnDBStatus.BackColor = Color.Green;
							threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Bitir", true));
							threadWriteBool.Start();
						}
						threadWriteString = new Thread(() => nxCompoletStringWrite("k2Uretilecek", txtUProductionUnit.Text));
						threadWriteString.Start();

					}
					else
					{
						MessageBox.Show("Yetkisiz sicil numarası !");
					}

				}
				else
				{
					MessageBox.Show("Lütfen eksik bilgileri doldurunuz");
				}

			}

			isBitirEmri = false;


		}
		/*******************************BUTTON FONKSİYONLARI************************************/
		private void cmbBxWorkingMod_SelectedIndexChanged(object sender, EventArgs e)
		{
			threadWriteString = new Thread(() => nxCompoletStringWrite("k2Mod", cmbBxWorkingMod.SelectedIndex.ToString()));
			threadWriteString.Start();
		}

		private void btnElavotorMotorUp_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AsansorAsagi", true));
			threadWriteBool.Start();

		}
		private void btnElavotorMotorUp_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AsansorAsagi", false));
			threadWriteBool.Start();
		}

		private void btnElavotorMotorDown_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AsansorYukari", true));
			threadWriteBool.Start();
		}

		private void btnElavotorMotorDown_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AsansorYukari", false));
			threadWriteBool.Start();
		}

		/************************************************************************************/


		private void btnBakeryDown_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2RezistansBackward", true));
			threadWriteBool.Start();
		}

		private void btnBakeryDown_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2RezistansBackward", false));
			threadWriteBool.Start();
		}

		private void btnBakeryUp_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2RezistansForward", true));
			threadWriteBool.Start();
		}

		private void btnBakeryUp_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2RezistansForward", false));
			threadWriteBool.Start();
		}



		/************************************************************************************/
		private void bntExecutiveMotorLeft_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2YurutmeBackward", true));
			threadWriteBool.Start();
		}

		private void bntExecutiveMotorLeft_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2YurutmeBackward", false));
			threadWriteBool.Start();
		}

		private void bntExecutiveMotorRight_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2YurutmeForward", true));
			threadWriteBool.Start();
		}

		private void bntExecutiveMotorRight_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2YurutmeForward", false));
			threadWriteBool.Start();
		}


		/************************************************************************************/

		private void btnTopPressingPistonDown_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2UstBaskiAsagi", true));
			threadWriteBool.Start();
		}

		private void btnTopPressingPistonDown_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2UstBaskiAsagi", false));
			threadWriteBool.Start();
		}

		private void btnTopPressingPistonUp_MouseDown(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2UstBaskiYukari", true));
			threadWriteBool.Start();
		}

		private void btnTopPressingPistonUp_MouseUp(object sender, MouseEventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2UstBaskiYukari", false));
			threadWriteBool.Start();
		}

		/************************************************************************************/

		private void btnBottomPressingPistonDown_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AltBaski", false));
			threadWriteBool.Start();
		}

		private void btnBottomPressingPistonUp_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2AltBaski", true));
			threadWriteBool.Start();
		}

		private void btnPressingPistonDown_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sikistirma", false));
			threadWriteBool.Start();
		}

		private void btnPressingPistonUp_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sikistirma", true));
			threadWriteBool.Start();
		}

		/************************************************************************************/

		private void btnVacuumStop_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Vakum", false));
			threadWriteBool.Start();
		}

		private void btnVacuumStart_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Vakum", true));
			threadWriteBool.Start();
		}

		private void btnMoldCoolingStop_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sisirme", false));
			threadWriteBool.Start();
		}

		private void btnMoldCoolingStart_Click(object sender, EventArgs e)
		{
			threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sisirme", true));
			threadWriteBool.Start();
		}


		/************************************************************************************/
		/**Textboxlar için sayı, aralık ve nokta kontrolü yapan fonskiyonlar*/ 
		private void txtBxRunningDistance_KeyPress(object sender, KeyPressEventArgs e)
		{
			numberControl(txtBxRunningDistance, e);
		}

		private void txtBxRunningDistance_Leave(object sender, EventArgs e)
		{
			minMaxControl(txtBxRunningDistance, 0, 800);
		}

		private void txtBxMoldNumber_KeyPress(object sender, KeyPressEventArgs e)
		{
			numberControl(txtBxMoldNumber, e);
		}

		private void txtBxMoldNumber_Leave(object sender, EventArgs e)
		{
			minMaxControl(txtBxMoldNumber, 1, 5);
		}


		/************************************************************************************/

		private void txtBxHeatingTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			dotControl(txtBxHeatingTime, e);
		}
		private void txtBxHeatingTime_Leave(object sender, EventArgs e)
		{
			txtBxHeatingTime.Text = txtBxHeatingTime.Text.Replace(",", ".");
			minMaxControl(txtBxHeatingTime, 0, 60);
		}

		private void txtBxVacuumTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			dotControl(txtBxVacuumTime, e);
		}

		private void txtBxVacuumTime_Leave(object sender, EventArgs e)
		{
			txtBxVacuumTime.Text = txtBxVacuumTime.Text.Replace(",", ".");
			minMaxControl(txtBxVacuumTime, 0, 20);
		}

		private void txtBxVacuumAfter_KeyPress(object sender, KeyPressEventArgs e)
		{
			dotControl(txtBxVacuumAfter, e);
		}

		private void txtBxVacuumAfter_Leave(object sender, EventArgs e)
		{
			txtBxVacuumAfter.Text = txtBxVacuumAfter.Text.Replace(",", ".");
			minMaxControl(txtBxVacuumAfter, 0, 20);
		}

		private void txtCoolingTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			dotControl(txtCoolingTime, e);
		}

		private void txtCoolingTime_Leave(object sender, EventArgs e)
		{
			txtCoolingTime.Text = txtCoolingTime.Text.Replace(",", ".");
			minMaxControl(txtCoolingTime, 0, 99);
		}


		/************************************************************************************/
		private void minMaxControl(TextBox textBox, double min, double max)
		{
			if (textBox.Text != "")
			{
				bool isNumeric = float.TryParse(textBox.Text, out _);
				if (isNumeric && double.Parse(textBox.Text.Replace(".", ",")) <= max && double.Parse(textBox.Text.Replace(".", ",")) >= min)
				{

				}
				else
				{
					MessageBox.Show(min.ToString() + " - " + max.ToString() + " değer aralığında giriniz");
					textBox.Text = "";
				}
			}
		}

		private void dotControl(TextBox textBox, KeyPressEventArgs e)
		{
			int countDot = 0;
			for (int i = 0; i < textBox.Text.Length; i++)
			{
				if (textBox.Text.Contains(".") || textBox.Text.Contains(","))
				{
					countDot++;
				}
			}

			if ((e.KeyChar == '.' || e.KeyChar == ',') && countDot <= 0)
			{
			}
			else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
			{
				e.Handled = true;
			}

		}
		private void numberControl(TextBox textBox, KeyPressEventArgs e)
		{

			 if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
			{
				e.Handled = true;
			}

		}

		/*******************************PLC FONKSİYONLARI************************************/
		public bool nxCompoletBoolRead(string variable)  //NX READ
		{
			try
			{
				boolReadStatus = false;
				bool staticValue = Convert.ToBoolean(nxCompolet1.ReadVariable(variable));
				return staticValue;
			}
			catch
			{
				boolReadStatus = true;
				return false;
			}
		}

		public bool nxCompoletBoolWrite(string variable, bool value)  //NX WRITE
		{
			try
			{
				nxCompolet1.WriteVariable(variable, value);
				return true;
			}
			catch
			{
				return false;
			}
		}
		public string nxCompoletStringRead(string variable)  //NX STRING
		{
			try
			{
				string staticStringValue = Convert.ToString(nxCompolet1.ReadVariable(variable));
				return staticStringValue;
			}
			catch (Exception e)
			{
				return "error";
			}

		}
		public bool nxCompoletStringWrite(string variable, string value)  //NX STRING
		{
			try
			{
				nxCompolet1.WriteVariable(variable, value);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

}
