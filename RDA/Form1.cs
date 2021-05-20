using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace RDA
{
    public partial class Form1 : Form
    {
        List<float>[] C;
        List<int> C_num;
        List<string> D;
        int U;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dtg.DataSource = null;
            dtg.Refresh();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                
                txtfilePath.Text = dlg.FileName;
                binDataCSV(txtfilePath.Text);
                textBox1.Text = U.ToString();
                textBox2.Text = C.Length.ToString();
                this.Text = dlg.FileName;
            }
        }

        private string showM(int[,] M)
        {
            string stringM="";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    stringM += M[i, j] + ",";
                }
                stringM = stringM.Remove(stringM.Length-1,1);
                stringM += "\n";
            }
                return stringM;
        }
        private void binDataCSV(string filename)
        {
            DataTable dt=new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filename);
            U = lines.Length-1;
            C = new List<float>[lines[0].Split(',').Length-2];
            C_num = new List<int>();
            for (int i = 0; i < C.Length; i++)
            {
                C[i] = new List<float>();
                C_num.Add(i);
            }

            D = new List<string>();
            if (lines.Length > 0)
            {
                //first line
                string firstLine=lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headWord));
                }
                //for data
                for (int r = 1; r < lines.Length; r++)
                {
                    string[] dataWord = lines[r].Split(',');
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    int columnIndex = 0;
                    foreach (string headWord in headerLabels)
                    {
                        dr[headWord] = dataWord[columnIndex++];
                    }
                    for (int i = 1; i < dataWord.Length-1; i++)
                        C[i - 1].Add(float.Parse(dataWord[i]));
                    D.Add(dataWord[dataWord.Length - 1]);
                }
            }
            if (dt.Rows.Count > 1)
            {
                dtg.DataSource = dt;
                dtg.AutoResizeColumns();
            }
        }
        private void btnM_Click(object sender, EventArgs e)
        {
            F_IFS ids = new F_IFS();
            rtbO.Text = ids.IDS_F_DAR(U, C,C_num,D);
            textBox3.Text = ids.tg;
        }
         
    }
}
