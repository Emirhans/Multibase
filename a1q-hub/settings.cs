using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace a1q_hub
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            string file = @"set.txt";
            if (File.Exists(file))
            {
                File.Delete(file);
            }          
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(txt_server.Text);
            sw.WriteLine(txt_database.Text);
            sw.WriteLine(txt_id.Text);
            sw.WriteLine(txt_svpass.Text);
            sw.Flush();
            fs.Close();
            MessageBox.Show("Settings saved.","Successfuly",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        private void settings_Load(object sender, EventArgs e)
        {
            mysql data = new mysql();
            data.read();
            txt_server.Text = data.datas[0];
            txt_database.Text = data.datas[1];
            txt_id.Text = data.datas[2];           
            txt_svpass.Text = data.datas[3];
        }
    }
}
