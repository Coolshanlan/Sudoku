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
using System.Diagnostics;

namespace 讀你的數
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TextBox[] tbx;
        List<int>[] block = new List<int>[9];
        List<int[]> ans = new List<int[]>();
        bool cool = false;
        bool threaddone = false;
        bool Computer(int[] s)
        {
            List<int>[] AllHave = new List<int>[81];
            int Min = 10, simnum = 10;
            int[] ss = new int[81];
            bool donegame = false;
            for (int i = 0; i < 81; i++)
                ss[i] = s[i];
            while (checkonly(ref ss, ref AllHave)) ;//檢查唯一解
            for (int i = 0; i < 81; i++)
            {
                int value = AllHave[i].Count;
                if (value < Min)
                {
                    Min = value;
                    simnum = i;
                }
            }
            if (Min == 9 && ss[simnum] !=0)
            {
                //for (int i = 0; i < 81; i++)
                    //tbx[i].Text = ss[i];
                ans.Add(ss);
                if (cool)
                {
                    for (int i = 0; i < 81; i++)
                        tbx[i].Text = ans[ans.Count-1][i].ToString();
                }
                return true;
            }
            else if (Min == 0) return false;
            else
            {
                for (int i = 0; i < Min; i++)
                {
                    ss[simnum] = AllHave[simnum][i];
                    if (Computer(ss)) donegame = true;
                }
                return donegame;
            }
        }
        List<int> question = new List<int>(); 
        void readtxt()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            FileStream fs = new FileStream(op.FileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string ss = sr.ReadToEnd();
            ss = ss.Replace("\r\n",",");
            string[] s = ss.Split(',');
            for(int i = 0; i < s.Count(); i++)
            {
                if(s[i] != "0")
                {
                    tbx[i].Text = s[i];
                    question.Add(i);
                }
            }
        }
        void Addtxb()//Textbox建立
        {
            tbx = new TextBox[81];
            int ytimes = 0;
            for (int i = 0; i < 9; i++)
            {
                int xtimes = 0;
                for (int ii = 0; ii < 9; ii++)
                {
                    int x, y;
                    tbx[i * 9 + ii] = new TextBox();
                    tbx[i * 9 + ii].Size = new Size(40, 40);
                    tbx[i * 9 + ii].Multiline = true;
                    tbx[i * 9 + ii].TextAlign = HorizontalAlignment.Center;
                    tbx[i * 9 + ii].Font = new Font("標楷體", 27);
                    tbx[i * 9 + ii].MaxLength = 1;
                    x = 135 + ii * 40 + xtimes * 5;
                    y = 100 + 40 * i + ytimes * 5;
                    if ((ii + 1) % 3 == 0) xtimes++;
                    tbx[i * 9 + ii].Location = new Point(x, y);
                    this.Controls.Add(tbx[i * 9 + ii]);
                }
                if ((i + 1) % 3 == 0) ytimes++;
            }
        }
        void AddBlock()//區塊分類
        {
            for(int i = 0; i < 9; i++)
                block[i] = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for(int ii = 0; ii < 3; ii++)
                {
                    int tt = 0;
                    block[tt++].Add(i*9+ii);
                    block[tt++].Add(i * 9 + 3 + ii);
                    block[tt++].Add(i * 9 + 6 + ii);
                    block[tt++].Add(i * 9 + 27 + ii);
                    block[tt++].Add(i * 9 + 30 + ii);
                    block[tt++].Add(i * 9 + 33 + ii);
                    block[tt++].Add(i * 9 + 54 + ii);
                    block[tt++].Add(i * 9 + 57 + ii);
                    block[tt++].Add(i * 9 + 60 + ii);
                }
            }
        }
        bool checkdone()//檢查重複
        {
            List<int> ans = new List<int>();
            bool repect = false;
            for (int i = 0; i < 9; i++)
            {
                ans = new List<int>();//判斷列
                for (int ii = 0; ii < 9; ii++)
                {
                    foreach (var a in ans)
                        if (tbx[i * 9 + ii].Text == a.ToString())
                            repect = true;
                    ans.Add(int.Parse(tbx[i * 9 + ii].Text));
                }
                ans = new List<int>();//判斷區塊
                for (int ii = 0; ii < 9; ii++)
                {
                    foreach (var a in ans)
                        if (a.ToString() == tbx[block[i][ii]].Text)
                            repect = true;
                    ans.Add(int.Parse(tbx[block[i][ii]].Text));
                }
                ans = new List<int>();//判斷行
                for (int ii = 0; ii < 9; ii++)
                {
                    foreach (var a in ans)
                        if (tbx[ii * 9 + i].Text == a.ToString())
                            repect = true;
                    ans.Add(int.Parse(tbx[ii * 9 + i].Text));
                }
            }
            return repect;
        }
        bool checkonly(ref int[] txt , ref List<int>[] AllHave)//確認是否有唯一解
        {
            #region 宣告
            List<int>[] onlyRow = new List<int>[9];
            List<int>[] onlyColumn = new List<int>[9];
            List<int>[] onlyBlock = new List<int>[9];
            #endregion
            #region 泛型初始化
            for (int i = 0; i < 9; i++)
            {
                onlyRow[i] = new List<int>();
                onlyColumn[i] = new List<int>();
                onlyBlock[i] = new List<int>();
            }
            #endregion
            #region 將1~9加入個點的集合中
            for (int i = 0; i < 81; i++)
            {
                AllHave[i] = new List<int>();
                for (int ii = 1; ii < 10; ii++)
                    AllHave[i].Add(ii);
            }
            #endregion
            #region 將行、列、區塊所包含的元素加入泛型中
            check_row_num(ref onlyRow , txt);
            check_column_num(ref onlyColumn ,txt);
            check_block_num(ref onlyBlock ,txt);
            #endregion
            #region 進行唯一解檢查，將以包含元素移除
            for (int i = 0; i < 9; i++)
                for(int ii = 0; ii < 9; ii++)
                {
                    if (txt[i * 9 + ii] != 0) continue;
                    int[] have = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    for (int iii = 0; iii < onlyRow[i].Count; iii++)//列元素移除
                        AllHave[i * 9 + ii].Remove(onlyRow[i][iii]);
                    for (int iii = 0; iii < onlyColumn[ii].Count; iii++)//區塊元素移除
                        AllHave[i * 9 + ii].Remove(onlyColumn[ii][iii]);
                    for (int iii = 0; iii < onlyBlock[ii / 3 + (i / 3) *3].Count; iii++)//行元素移除
                        AllHave[i * 9 + ii].Remove(onlyBlock[ii / 3 + (i / 3) * 3][iii]);
                    if (AllHave[i * 9 + ii].Count == 1)
                    {
                        txt[i * 9 + ii] = AllHave[i * 9 + ii][0];
                        return true;
                    }
                }
            #endregion
            #region 行列範圍性判斷
            for (int i = 0; i < 9; i++)
            { 
                for (int ii = 0; ii < 9; ii++)
                { 
                    if (txt[i * 9 + ii] == 0)
                    {
                        List<int> AllHaveremove = new List<int>();
                        foreach (var a in AllHave[i * 9 + ii])
                            AllHaveremove.Add(a);
                        for (int j = 0; j < 9; j++)
                            if (txt[i * 9 + j] == 0 && j != ii)
                                foreach (int a in AllHave[i * 9 + ii])
                                    AllHaveremove.Remove(a);
                        if (AllHaveremove.Count == 1)
                        {
                            txt[i * 9 + ii] = AllHaveremove[0];
                            return true;
                        }
                    }
                    if (txt[ii * 9 + i] == 0)
                    {
                        List<int> AllHaveremove = new List<int>();
                        foreach (var a in AllHave[ii * 9 + i])
                            AllHaveremove.Add(a);
                        for (int j = 0; j < 9; j++)
                            if (txt[j * 9 + i] == 0 && j != ii)
                                foreach (int a in AllHave[j * 9 + i])
                                    AllHaveremove.Remove(a);
                        if (AllHaveremove.Count == 1)
                        {
                            txt[ii * 9 + i] = AllHaveremove[0];
                            return true;
                        }
                    }
                }
            }
            #endregion
            return false;
        }
        void check_row_num(ref List<int>[] onlyRow , int[] txt)//確認列包含數字
        {
            for(int i = 0; i < 9; i++)
                for(int ii = 0; ii < 9; ii++)
                    if (txt[i * 9 + ii] !=0)
                        onlyRow[i].Add(txt[i * 9 + ii]);
        }
        void check_column_num(ref List<int>[] onlyColumn , int[] txt)//確認行包含數字
        {
            for (int i = 0; i < 9; i++)
                for (int ii = 0; ii < 9; ii++)
                {
                    if (txt[ii * 9 + i] != 0)
                        onlyColumn[i].Add(txt[ii * 9 + i]);
                }
        }
        void check_block_num(ref List<int>[] onlyBlock , int[] txt)//確認區塊包含數字
        {
            for(int i = 0; i < 9; i++)
                foreach(int a in block[i])
                    if (txt[a] != 0)
                        onlyBlock[i].Add(txt[a]);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Addtxb();
            AddBlock();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkdone()) MessageBox.Show("錯了歐");
            else
            {
                comboBox1.Items.Clear();
                for(int i = 1; i <= ans.Count; i++)
                    comboBox1.Items.Add("第 " +i+" 種");
                comboBox1.SelectedIndex = 0;
                label2.Text = "共 " + ans.Count + " 種答案";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int[] s = new int[81];
            for (int i = 0; i < 81; i++)
                s[i] = tbx[i].Text==""?0 : int.Parse(tbx[i].Text);
            if (cool)
            {
                button3.Enabled = false;
                Task.Run(() =>
                {
                    Computer(s);
                    threaddone = true;
                });
                Task.Run(() =>
                {
                    while (!threaddone) ;
                    button1.PerformClick();
                    button3.Enabled = true;
                });
                Task.Run(() =>
                {
                    while (!threaddone) label2.Text = "共 " + ans.Count + " 種答案";
                });
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Reset();
                sw.Start();
                Computer(s);
                sw.Stop();
                Task.Run(() =>
                {
                    MessageBox.Show("計算秒數為：" + sw.Elapsed.TotalMilliseconds + "毫秒");
                });
                for (int i = 0; i < 81; i++) tbx[i].Text = ans[0][i].ToString();

                button1.PerformClick();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            foreach (TextBox a in tbx)
                a.Text = "";
            ans.Clear();
            comboBox1.Items.Clear();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // var aa = ans.GroupBy(x => x).Select(x => x.ToList()).ToList();
            //MessageBox.Show(aa.Count.ToString());
            for (int i = 0; i < 81; i++)
                tbx[i].Text = ans[/*int.Parse(comboBox1.Text.Replace("第 ", "").Replace(" 種", "")) - 1*/comboBox1.SelectedIndex][i].ToString();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = true;
        }
        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
            button4.ForeColor = Color.White;
        }
        private void button4_MouseLeave(object sender, EventArgs e)
        {
            if(!cool)
            button4.ForeColor = Color.Black;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            threaddone = false;
            cool = cool == true ? false : true;
            button4.ForeColor = cool == true ? Color.White : Color.Black;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            readtxt();
        }
    }
}
