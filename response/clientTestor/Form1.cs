using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clientTestor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        class Mywc : System.Net.WebClient
        {
            public Mywc()
            {
                this.Encoding = System.Text.Encoding.UTF8;

            }
            protected override System.Net.WebRequest GetWebRequest(Uri address)
            {
                var req = base.GetWebRequest(address);
                req.Timeout = 3000;
                return req;
            }
        }
        System.Net.WebClient wc = new Mywc();
        class Log
        {
            public Log(string desc, string recive)
            {
                this.desc = desc;
                this.recive = recive;
            }
            public string desc;
            public string recive;
            public override string ToString()
            {
                return desc + ":" + recive;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            try
            {
                string returnstr = wc.DownloadString(url + "?c=ping");
                listBox1.Items.Add(new Log("ping result", returnstr));
            }
            catch (Exception err)
            {
                listBox1.Items.Add(new Log("http err", err.ToString()));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log l = listBox1.SelectedItem as Log;
            if (l != null)
            {
                textBox2.Text = l.recive;
            }
        }
        class GameItem
        {
            public string name;
            public string desc;
            public string admin;
            public override string ToString()
            {
                return name + "|" + admin;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            string url = textBox1.Text;
            try
            {
                string returnstr = wc.DownloadString(url + "?c=play");
                listBox1.Items.Add(new Log("get gamelist result", returnstr));
                var json = MyJson.Parse(returnstr);
                if (json.GetDictItem("status").AsInt() == 0)
                {
                    var list = json.GetDictItem("list").AsList();
                    foreach (var item in list)
                    {
                        GameItem i = new GameItem();
                        i.name = item.GetDictItem("name").AsString();
                        i.desc = item.GetDictItem("desc").AsString();
                        i.admin = item.GetDictItem("admin").AsString();
                        listBox2.Items.Add(i);
                    }
                }
            }
            catch (Exception err)
            {
                listBox1.Items.Add(new Log("http err", err.ToString()));
            }
        }
        System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1Managed();

        string mytoken;
        private void button3_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            GameItem item = listBox2.SelectedItem as GameItem;
            if (item == null)
            {
                MessageBox.Show("选中一个Game");
                return;
            }
            try
            {
                byte[] sb = System.Text.Encoding.UTF8.GetBytes(textBox4.Text);
                byte[] wt = sha1.ComputeHash(sb);

                string code = Convert.ToBase64String(sb, 0, sb.Length);
                byte[] br = wc.DownloadData(url + "?c=login&g=" + item.name + "&u=" + textBox3.Text + "&p=" + code);
                string returnstr = System.Text.Encoding.UTF8.GetString(br);
                listBox1.Items.Add(new Log("login result", returnstr));
                var json = MyJson.Parse(returnstr);
                if (json.GetDictItem("status").AsInt() == 0)
                {
                    mytoken = json.GetDictItem("token").AsString();
                    this.Text = "mytoken:" + mytoken;
                    textBox6.Text = mytoken;
                }
            }
            catch (Exception err)
            {
                listBox1.Items.Add(new Log("http err", err.ToString()));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            try
            {
                byte[] br = wc.DownloadData(url + "?c=rpc&s=" + Uri.EscapeDataString(textBox5.Text));
                string returnstr = System.Text.Encoding.UTF8.GetString(br);
                listBox1.Items.Add(new Log("ping result", returnstr));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            catch (Exception err)
            {
                listBox1.Items.Add(new Log("http err", err.ToString()));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
