﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

///抽奖小程序
///
namespace Lottery
{
    public partial class FormPrize : Form
    {

        private string[] persons;

        //用一个变量也可以，但是这样更简单
        private int p1;
        private int p2;
        private int p3;
        private int p4;
        private int p5;

        private int len;

        //滚动方向：从上至下
        private bool bTop2Bottom;

        //检查重名使用
        private Dictionary<string, int> dictName = null;

        private string titleSoftName = "公司年会抽奖程序";
        private string titleWait = "正在抽奖，请倒计时";
        private string fileName = "人员名单.txt";

        private bool bError;

        public FormPrize()
        {
            InitializeComponent();
        }

        private void FormPrize_Load(object sender, EventArgs e)
        {
            this.lblWait.Text = titleSoftName;
            bTop2Bottom = true;
            this.timer1.Interval = 110;//中间值
            this.cmbPrize.SelectedIndex = 1;
            InitData();
        }

        private void InitData()
        {
            LoadFromFile();
            if (bError)
            {
                this.btnStart.Enabled = false;
                this.btnCheckRepeat.Enabled = false;
                return;
            }
            Random rnd = new Random();
            int index = rnd.Next(len);  // 创建一个数字是0~len-1之间的
            InitScrollIndex(index);

            this.lbl5.Text = persons[p5];
            this.lbl4.Text = persons[p4];
            this.lbl3.Text = persons[p3];
            this.lbl2.Text = persons[p2];
            this.lbl1.Text = persons[p1];

            string line = string.Format("{0} 初始化奖池，重新开始抽奖，共 {1} 人", DateTime.Now.ToString(), len);
            SaveLotteryList("  ");
            SaveLotteryList("  ");
            SaveLotteryList(line);
            SaveLotteryList("  ");
        }

        private void LoadFromFile()
        {
            dictName = new Dictionary<string, int>();
            persons = null;

            List<string> list = new List<string>();
            try
            {
                //TODO:这里可以做成浏览文件方式，不要写死
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("GB2312"));
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = string.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    string personName = str.Trim();
                    if (!string.IsNullOrEmpty(personName))
                    {
                        //添加前先判断是否已添加过
                        if (list.Contains(personName))
                        {
                            if (dictName.ContainsKey(personName))
                            {
                                dictName[personName] = dictName[personName] + 1;
                            }
                            else
                            {
                                dictName[personName] = 2;
                            }
                        }
                        list.Add(personName);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {
                bError = true;
                MessageBox.Show("初始化奖池失败，请检查是否存在: 人员名单.txt");
                return;
            }
            finally
            {
            }
            persons = list.ToArray();
            len = persons.Length;
            if (len < 6)
            {
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = false;
                MessageBox.Show("名单中的姓名太少，至少应有6人");
            }
        }


        /// <summary>
        /// 初始化下标
        /// </summary>
        private void InitScrollIndex(int p3init)
        {
            if (p3init >= len)
                p3init -= len;
            else if (p3init < 0)
                p3init += len;

            p3 = p3init;

            p2 = p3 - 1;
            if (p2 < 0)
                p2 += len;

            p1 = p3 - 2;
            if (p1 < 0)
                p1 += len;

            p4 = p3 + 1;
            if (p4 >= len)
                p4 -= len;

            p5 = p3 + 2;
            if (p5 >= len)
                p5 -= len;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bTop2Bottom)
            {
                p1--;
                p2--;
                p3--;
                p4--;
                p5--;
                if (p1 < 0)
                {
                    p1 = len - 1;
                }
                if (p2 < 0)
                {
                    p2 = len - 1;
                }
                if (p3 < 0)
                {
                    p3 = len - 1;
                }
                if (p4 < 0)
                {
                    p4 = len - 1;
                }
                if (p5 < 0)
                {
                    p5 = len - 1;
                }
            }
            else
            {
                p1++;
                p2++;
                p3++;
                p4++;
                p5++;
                if (p1 >= len)
                {
                    p1 = 0;
                }
                if (p2 >= len)
                {
                    p2 = 0;
                }
                if (p3 >= len)
                {
                    p3 = 0;
                }
                if (p4 >= len)
                {
                    p4 = 0;
                }
                if (p5 >= len)
                {
                    p5 = 0;
                }
            }
            this.lbl5.Text = persons[p5];
            this.lbl4.Text = persons[p4];
            this.lbl3.Text = persons[p3];
            this.lbl2.Text = persons[p2];
            this.lbl1.Text = persons[p1];
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (bError)
            {
                MessageBox.Show("没有找到人员名单.txt，如果没有则新建该文件，并输入员工姓名即可！");
            }
            this.timer1.Start();
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.lblResult.Text = null;
            this.lblResult.Hide();
            this.lblWait.Text = titleWait;
            this.lblWait.Show();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            string result = string.Format("恭喜 {0} 获得{1}！", persons[p3], this.cmbPrize.SelectedItem.ToString());
            this.lblResult.Text = result;
            this.lblResult.Show();
            this.lblWait.Hide();

            string line = string.Format("{0} {1} 获得 {2}", DateTime.Now.ToString(), persons[p3], this.cmbPrize.SelectedItem.ToString());
            SaveLotteryList(line);

            //去除已经中奖者，防止重复中奖
            List<String> listtmp = new List<string>(persons);
            listtmp.Remove(persons[p3]);
            persons = listtmp.ToArray();
            //string sname = String.Join(" ", persons);
            //MessageBox.Show(sname);

            len = persons.Length;
            if (len < 6)
            {
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = false;
                MessageBox.Show("名单中的姓名太少，至少应有6人");
            }
            //删除一位之后调整索引
            if (bTop2Bottom)
            {   
                InitScrollIndex(p3);
            }
            else
            {
                InitScrollIndex(p3-1);
            }
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            bTop2Bottom = !bTop2Bottom;
        }

        private void btnInitPool_Click(object sender, EventArgs e)
        {
            InitData();
            MessageBox.Show("初始化奖池完成，共" + len + "人");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //频率：10ms - 210ms
            this.timer1.Interval = 210 - this.trackBar1.Value * 20;
        }

        private void btnCheckRepeat_Click(object sender, EventArgs e)
        {
            if (null != dictName && dictName.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("以下姓名存在重复，请修改(建议增加部门名称)\n");
                foreach (KeyValuePair<string, int> item in dictName)
                {
                    sb.Append(item.Key + " " + item.Value + "个");
                    sb.Append("\n");
                }
                MessageBox.Show(sb.ToString());
            }
            else
            {
                MessageBox.Show("恭喜，没有重名！");
            }
        }

        private void toolStripMenuItemAuthor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("作者：Veipin\n编写日期：2020年1月9日");
        }

        private void toolStripMenuItemDoc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("将员工姓名录入人员名单.txt中即可，每个名字占一行\n如果有相同姓名建议名字后面增加部门");
        }

        private void toolStripMenuItemSoftUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("已是最新版本");
        }

        private void SaveLotteryList(string line)
        {
            //System.IO.File.WriteAllText(@"中奖名单.txt", line); // 覆盖
            
            // 追加
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"中奖名单.txt", true))
            {
                file.WriteLine(line);
            }   
        }

    }
}
