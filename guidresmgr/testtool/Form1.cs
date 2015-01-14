using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        SHA1 sha1 = new SHA1Managed();
        MD5 md5 = MD5.Create();

        string BytesToString(byte[] bytes)
        {
            string str = "";
            foreach (var b in bytes)
            {
                str += b.ToString("X02");
            }
            return str;
        }
        string hash1;
        string hash2;
        string hash3;
        byte[] filedata;
        private void buttonSetFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    listBox1.Items.Clear();
                    textFile.Text = f.FileName;
                    filedata = System.IO.File.ReadAllBytes(f.FileName);

                    hash1 = CRC.CalculateDigest(filedata, 0, (uint)filedata.Length).ToString("X08");
                    hash2 = BytesToString(md5.ComputeHash(filedata, 0, filedata.Length));
                    hash3 = BytesToString(sha1.ComputeHash(filedata, 0, filedata.Length));
                    listBox1.Items.Add(hash1);
                    listBox1.Items.Add(hash2);
                    listBox1.Items.Add(hash3);
                }
                catch (Exception err)
                {
                    listBox1.Items.Add("error:");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        class mywc : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                var req = base.GetWebRequest(address);
                req.Timeout = 3000;
                return req;
            }
        }
        mywc wc = new mywc();

        string fileid;
        string filename;
        private void GetFileName_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + textCall1.Text + "?shash=" + hash1 + "&mhash=" + hash2 + "&lhash=" + hash3 + "&length=" + filedata.Length;
                string str = wc.DownloadString(url);
                var json = MyJson.Parse(str);
                filename = null;
                filename = json.asDict()["filename"].AsString();
                textBox2.Text = textUrl.Text + json.asDict()["path"].AsString();
                textBox6.Text = str;
                textTag.Text = filename;
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + textCall2.Text + "?filename=" + filename;
                string str = wc.DownloadString(url);
                textBox6.Text = str;
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + textCall3.Text + "?filename=" + filename;
                byte[] binstr = wc.UploadData(url, filedata);
                string str = System.Text.Encoding.UTF8.GetString(binstr);
                textBox6.Text = str;
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }

        int userid = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + "login.php" + "?username=" + textUser.Text + "&password=" + textPassword.Text;
                string str = wc.DownloadString(url);
                var json = MyJson.Parse(str);
                userid = json.asDict()["userid"].AsInt();
                textUserID.Text = userid.ToString();
                textBox6.Text = str;
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + "userinfo.php" + "?username=" + textUser.Text;
                string str = wc.DownloadString(url);
                var json = MyJson.Parse(str);

                StringBuilder sb = new StringBuilder();
                json.ConvertToStringWithFormat(sb,0);
                textBox6.Text = sb.ToString();
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox1.Text = textUrl.Text + "updatejson.php" + "?username=" + textUser.Text + "&password=" + textPassword.Text
                    + "&tag=" + textTag.Text;
                string str = wc.DownloadString(url);
                textBox6.Text = str;
            }
            catch (Exception err)
            {
                textBox6.Text = err.ToString();
            }
        }
    }
    class CRC
    {
        public static readonly uint[] Table;

        static CRC()
        {
            Table = new uint[256];
            const uint kPoly = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                uint r = i;
                for (int j = 0; j < 8; j++)
                    if ((r & 1) != 0)
                        r = (r >> 1) ^ kPoly;
                    else
                        r >>= 1;
                Table[i] = r;
            }
        }

        uint _value = 0xFFFFFFFF;

        public void Init() { _value = 0xFFFFFFFF; }

        public void UpdateByte(byte b)
        {
            _value = Table[(((byte)(_value)) ^ b)] ^ (_value >> 8);
        }

        public void Update(byte[] data, uint offset, uint size)
        {
            for (uint i = 0; i < size; i++)
                _value = Table[((byte)_value) ^ data[offset + i]] ^ (_value >> 8);

        }

        public uint GetDigest() { return _value ^ 0xFFFFFFFF; }

        public static uint CalculateDigest(byte[] data, uint offset, uint size)
        {
            CRC crc = new CRC();
            // crc.Init();
            crc.Update(data, offset, size);
            return crc.GetDigest();
        }

        static bool VerifyDigest(uint digest, byte[] data, uint offset, uint size)
        {
            return (CalculateDigest(data, offset, size) == digest);
        }
    }
}
