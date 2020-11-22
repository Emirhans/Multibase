using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.IO;

namespace a1q_hub
{
    public partial class LoginForm : Form
    {
        sql sqldata = new sql();
        Point mouseDownPoint = Point.Empty;
        DataTable table = new DataTable();
        private int sayac = 0;
        int t = 1;
        public int id;
       

        public LoginForm()
        {
            InitializeComponent();
        }
      
        private void LoginForm_Load(object sender, EventArgs e)
        {
            
        }

        public bool InternetKontrol()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                kontrol_client.Close();
                return true;
            }
            catch (Exception)
            {             
                return false;
            }
        }

        private void ButtonEnter_Click(object sender, EventArgs e)
        {
            string userdata = txtUsername.Text;
            bool kontrol = InternetKontrol();
            if (txtUsername.Text == "" || txtPass.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "A1Q-HUB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (kontrol == false)
            {
                MessageBox.Show("Please check your connection!", "A1Q-HUB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AllocConsole();
                TextWriter writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                Console.SetOut(writer);
                Console.WriteLine("A1Q-HUB Console System");
                Console.WriteLine("Checking your information...");

                while (t < 4)//giriş hatalıysa 4 kere daha deniyor
                {
                    try
                    {
                        bool loginform = sqldata.login(txtUsername.Text, txtPass.Text);
                        if (loginform==false)
                        {
                            Console.WriteLine("Username Or Password Are Invalid");
                            t = 5;
                            sayac = 0;
                            timer2.Start();
                        }
                        else
                        {                                              
                            Console.WriteLine("Login Successfully");                            
                            sayac = 0;
                            t = 5;
                            timer1.Start();
                           
                        }            
                    }
                    catch
                    {
                        Console.WriteLine("500: Server connection failed!");
                        Console.WriteLine("Connection retrying - " + t.ToString());
                        t++;
                    }
                }
                if (t == 4)
                {
                    FreeConsole();
                    Console.SetIn(new StreamReader(Console.OpenStandardInput()));
                    lbl_message.Text = "500: Server connection failed!";
                    t = 0;
                }             
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
            if (sayac == 2)
            {
                t = 0;
                lbl_message.Text = "";
                sayac = 0;
                try
                {
                    Console.Clear();
                }
                catch { }
                FreeConsole();
                Console.SetIn(new StreamReader(Console.OpenStandardInput()));
                timer1.Stop();
               
                this.Hide();
                
                MainForm mf = new MainForm();
                mf.userdata = txtUsername.Text;           
                mf.Show();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac++;
            if (sayac == 2)
            {                                                    
                FreeConsole();
                Console.SetIn(new StreamReader(Console.OpenStandardInput()));              
                timer2.Stop();
                lbl_message.Text = "Username Or Password Are Invalid.Please try again.";
                t = 0;
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            DialogResult exit = new DialogResult();
            exit = MessageBox.Show("Are you sure want to exit?","A1Q-HUB",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(exit==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void lblChangePass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = new Point(e.X, e.Y);
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint.IsEmpty)
                return;
            Form f = sender as Form;
            f.Location = new Point(f.Location.X + (e.X - mouseDownPoint.X), f.Location.Y + (e.Y - mouseDownPoint.Y));
        }
    }
}


