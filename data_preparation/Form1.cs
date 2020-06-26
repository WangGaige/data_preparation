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

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace data_preparation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }
        private string merging(string filename) {
            string output=null;
            string file_name = filename.Substring(filename.LastIndexOf('\\') + 1);
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                try
                {
                    string str=null;
                    int line_seq = 0;
                    while ((str = sr.ReadLine()) != null)
                    {
                        if (str.ToCharArray()[0] == ' ')
                        {
                            line_seq++;
                            output = output + file_name+" "+ line_seq + " "+ str + "\r\n";
                        }
                    }
                    sr.Close();
                    fs.Close();
                    return output;
                }
                catch (Exception)
                {
                    throw;
                }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            //设置打开对话框的初始目录，默认目录为exe运行文件所在的路径
            ofd.InitialDirectory = Application.StartupPath;
            //设置打开对话框的标题
            ofd.Title = "请选择要打开的文件";
            //设置打开对话框可以多选
            ofd.Multiselect = true;
            //设置对话框打开的文件类型
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            //设置文件对话框当前选定的筛选器的索引
            ofd.FilterIndex = 2;
            //设置对话框是否记忆之前打开的目录
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                //获取用户选择的文件完整路径
                string[] filePath = ofd.FileNames;
                //获取对话框中所选文件的文件名和扩展名，文件名不包括路径
                string[] fileName = ofd.SafeFileNames;

                int idx = filePath[0].LastIndexOf('\\');
                int parallel_thread = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filePath.Count();
                Task.Factory.StartNew(() =>
                {
                    string content = "";
                    for (int i = 0; i < filePath.Count(); i++)
                    {
                        
                        //OutLog("用户选择的文件目录为:" + item);
                        string file_name = filePath[i].Substring(filePath[i].LastIndexOf('\\') + 1);
                        string file_dir = filePath[i];
                        
                        //label1.Text = label1.Text + file_name + "开始！" + "\r\n";
                        this.Invoke((Action)(() =>
                        {
                            richTextBox1.Text = richTextBox1.Text + file_name + "开始！";
                            
                        }));
                        
                        content = content+merging(file_dir);

                        this.Invoke((Action)(() =>
                        {
                            progressBar1.Value = i + 1;
                            richTextBox1.Text = richTextBox1.Text+ " 已完成！" + "\r\n";
                            richTextBox1.Focus();
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            richTextBox1.ScrollToCaret();
                            

                        }));
                    }
                    using (StreamWriter writer = new StreamWriter((filePath[0].Substring(0, idx) + "\\connected.txt"), true))
                    {
                        writer.Write(content);
                    }
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("全部提取完成");
                        button1.Enabled = true;
                    }));
                });
                
            }
        }
    }
}
