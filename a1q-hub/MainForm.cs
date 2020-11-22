using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace a1q_hub
{
    public partial class MainForm : Form
    {
        public string userdata;
        public string folder;
        public string main_folder = "history";
        public string p_name;
        public string file_path;
        FileInfo[] rgFiles;

        public MainForm()
        {
            InitializeComponent();
        }

        sql mssql = new sql();
        LoginForm lgn = new LoginForm();
        private void summary(string first_path, string txt_value)
        {

            FileStream fs = new FileStream(first_path, FileMode.Open, FileAccess.Read);

            StreamReader sw = new StreamReader(fs);

            string yazi = sw.ReadLine();
            string[] stringSeparators = new string[] { "\r\n" };
            string[] b = txt_value.Split(stringSeparators, StringSplitOptions.None);

            while (yazi != null)
            {

                foreach (string item in b)
                {
                    if (yazi != item)
                    {
                        textBox1.Text = "";

                        file_path = rgFiles[listBox3.SelectedIndex].Directory.ToString() + "\\" + listBox3.SelectedItem.ToString();
                        FileStream fs1 = new FileStream(file_path, FileMode.Open, FileAccess.Read);

                        StreamReader sw1 = new StreamReader(fs1);

                        string yazi1 = sw1.ReadLine();

                        while (yazi1 !=null)
                        {
                            textBox1.ForeColor = Color.Red;
                            textBox1.Text += yazi1 + "\r\n";
                            yazi1 = sw1.ReadLine();
                        }
                        sw1.Close();
                        fs1.Close();
                    }
                    yazi = sw.ReadLine();
                    if (yazi == null) { break; }
                }

            }
            sw.Close();
            fs.Close();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
           
            userdata = "admin";
            lbl_name.Text = userdata.ToString();
            mssql.cmd("Users", "userName='" + userdata.ToString() + "'");
            SqlCommand command;
            SqlConnection connect = new SqlConnection("Data Source=multibase.database.windows.net;Initial Catalog=MultibaseDB;Persist Security Info=False;User ID=mbadmin; Password=170403yE!; Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");
            command = new SqlCommand("SELECT*FROM Projects WHERE projectID=2" , connect);
            connect.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["ProjectName"]);
            }
            connect.Close();

        }
        private void pb_pp_Click(object sender, EventArgs e)
        {
            OpenFileDialog img = new OpenFileDialog();
            img.Title = "Choose Image File";
            img.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            img.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg";
            img.Multiselect = false;
            if (img.ShowDialog() == DialogResult.OK)
            {
                pb_pp.Image = new Bitmap(img.FileName);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void copy(string Prmt1, string prmt2, bool prmt3)
        {
            DirectoryInfo DrInf = new DirectoryInfo(Prmt1);
            DirectoryInfo[] DrInfLst = DrInf.GetDirectories();
            if (!Directory.Exists(prmt2))
            {
                Directory.CreateDirectory(prmt2);
            }

            FileInfo[] dosya = DrInf.GetFiles();
            string path1 = "";
            foreach (FileInfo FFF in dosya)
            {
                path1 = Path.Combine(prmt2, FFF.Name);
                FFF.CopyTo(path1, false);
            }
            if (true)
            {
                foreach (DirectoryInfo bilgi in DrInfLst)
                {
                    path1 = Path.Combine(prmt2, bilgi.Name);
                    copy(bilgi.FullName, path1, true);

                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            lbl_project_name.Text = comboBox1.SelectedItem.ToString();
            mssql.cmd("Projects", "ProjectName='" + comboBox1.SelectedItem.ToString() + "'");
            folder = mssql.read("LocationUrl");
            DirectoryInfo di = new DirectoryInfo(folder);
            rgFiles = di.GetFiles("*.txt", SearchOption.AllDirectories);
            timer1.Start();
            timer1.Interval = 4000;
            string path2 = "history\\" + comboBox1.SelectedItem.ToString() + " " + DateTime.Now.ToString(("yyyy-MM-dd HH-mm-ss"));

            if (!File.Exists(path2))
                Directory.CreateDirectory(path2);

            copy(folder, path2 + "\\", true);
            for (int i = 0; i < rgFiles.Length; i++)
            {
                listBox3.Items.Add(rgFiles[i]);
            }

        }
   
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";

            file_path = rgFiles[listBox3.SelectedIndex].Directory.ToString() + "\\" + listBox3.SelectedItem.ToString();
            FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read);

            StreamReader sw = new StreamReader(fs);

            string yazi = sw.ReadLine();

            while (yazi != null)
            {
                textBox1.Text += yazi + "\r\n";
                yazi = sw.ReadLine();
            }
            sw.Close();
            fs.Close();
        } 

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            LoginForm lgn = new LoginForm();
            lgn.Show();
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            summary(file_path, textBox1.Text);          
        }
    }
}