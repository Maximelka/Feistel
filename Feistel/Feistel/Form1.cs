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

namespace Feistel
{
    public partial class Form1 : Form
    {
        private Feistel feistel;
        private List<string> FileStr = new List<string>();

        public Form1()
        {

            InitializeComponent();
        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (OFD.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = OFD.FileName;
            //чтение из файла
            FileStr = new List<string>();
            StreamReader sr = new StreamReader(filename);
            string line;
            while ((line = sr.ReadLine()) != null)
                FileStr.Add(line);
            sr.Close();
            ListAdd(FileStr, 1);
        }

        private void buttonEncryption_Click(object sender, EventArgs e)
        {
            Cription(true);
        }

        private void ListAdd(List<string> FileStr, int i)
        {
            switch (i)
            {
                case 1:
                    for (int j = 0; j < FileStr.Count; j++)
                        listBox1.Items.Add(FileStr[j]);
                    break;
                case 2:
                    for (int j = 0; j < FileStr.Count; j++)
                        listBox2.Items.Add(FileStr[j]);
                    break;
            }
        }

        private void Cription(bool crypt)
        {
            if (FileStr.Count == 0)
            {
                MessageBox.Show("Выберете файл");
                return;
            }
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (SFD.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = SFD.FileName;
            if (textBoxKey.Text == "")
            {
                MessageBox.Show("Введите ключ");
                return;
            }
            feistel = new Feistel(textBoxKey.Text, FileStr, crypt);
            List<string> fileStr = feistel.Cryption();
            ListAdd(fileStr, 2);
            StreamWriter sw = new StreamWriter(filename);
            for (int j = 0; j < fileStr.Count; j++)
                sw.WriteLine(fileStr[j]);
            sw.Close();
        }

        private void buttonDecryption_Click(object sender, EventArgs e)
        {
            Cription(false);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }
    }
}
