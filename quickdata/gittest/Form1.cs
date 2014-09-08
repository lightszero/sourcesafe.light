using GitAutoSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace gittest
{
    public partial class Form1 : Form, IConsole
    {
        public Form1()
        {
            InitializeComponent();
        }
        GitWatcher watcher = new GitWatcher();
        private void Form1_Load(object sender, EventArgs e)
        {
            watcher.Start(this,"cmd","","d:\\gittest\\gpage");
        }

        public void Show(string line)
        {
            if (line.Length == 0) return;
            Action<string,bool> safeshow = (_line,lineend) => 
            {
                if (this.listBox1.Items.Count == 0)
                    this.listBox1.Items.Add("");
                this.listBox1.Items[this.listBox1.Items.Count - 1] = this.listBox1.Items[this.listBox1.Items.Count - 1] + _line;
                if(lineend)
                {
                    this.listBox1.Items.Add("");
                }
            };
            this.BeginInvoke(safeshow, new object[] { line,line[line.Length-1]=='\n' });

        }

        public void ShowErr(string line)
        {
            if (line.Length == 0) return;
            Action<string, bool> safeshow = (_line, lineend) =>
            {
                if (this.listBox2.Items.Count == 0)
                    this.listBox2.Items.Add("");
                this.listBox2.Items[this.listBox2.Items.Count - 1] = this.listBox2.Items[this.listBox2.Items.Count - 1] + _line;
                if (lineend)
                {
                    this.listBox2.Items.Add("");
                }
            };
            this.BeginInvoke(safeshow, new object[] { line, line[line.Length - 1] == '\n' });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            watcher.WriteLine("git push");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //watcher.Stop();
        }
    }
}
