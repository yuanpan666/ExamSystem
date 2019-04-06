using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReadExcelDll;

namespace ExamSystem
{
    public partial class Main : Form
    {
        private string _FileName;
        private List<TiMuItem> TimuList;
        private List<TiMuItem> TimuWrongList;
        static int i = -1;
        static int nR, nW;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ConfirmBtn.Visible = false;
            this.NextBtn.Visible = false;
            this.label1.Text = "题目总数：";
            this.label2.Text = "答对数量：";
            this.label3.Text = "打错数量：";
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
            HideCheckAndRadio();

        }
        private void HideCheckAndRadio()
        {
            this.checkBoxA.Visible = false;
            this.checkBoxB.Visible = false;
            this.checkBoxC.Visible = false;
            this.checkBoxD.Visible = false;
            this.checkBoxE.Visible = false;
            this.checkBoxF.Visible = false;

            this.checkBoxA.Checked = false;
            this.checkBoxB.Checked = false;
            this.checkBoxC.Checked = false;
            this.checkBoxD.Checked = false;
            this.checkBoxE.Checked = false;
            this.checkBoxF.Checked = false;
            //this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
            this.radioButtonA.Visible = false;
            this.radioButtonA.Checked = false;
            this.radioButtonB.Visible = false;
            this.radioButtonB.Checked = false;
            this.radioButtonC.Visible = false;
            this.radioButtonC.Checked = false;
            this.radioButtonD.Visible = false;
            this.radioButtonD.Checked = false;

            this.CorrectPictureBox.Visible = false;
            this.WrongPictureBox.Visible = false;

            this.ContentRichTextBox.Text = "";
            this.RightAnsTextBox.Text = "";
        }
        //打开excel的题库，将其转换成list格式，并初始化两个列表
        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
           
           // string strOdbcConn;
            if(OpenFileDlg.ShowDialog() == DialogResult.OK)
            {
                _FileName = OpenFileDlg.FileName;
            }
            if(!string.IsNullOrEmpty(_FileName))
            {
                if(TimuList == null)
                {
                    TimuList = new List<TiMuItem>();
                }
                else
                {
                    TimuList.Clear();
                }

                if(TimuWrongList == null)
                {
                    TimuWrongList = new List<TiMuItem>();
                }
                else
                {
                    TimuWrongList.Clear();
                }

                //选择题库的按钮禁用
                this.OpenFileBtn.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                ReadExcel readExcel = new ReadExcel();
                string[,] readResult = readExcel.OpenExcel(_FileName, "A", "D");
                readResult.GetLength(0);

                for(int i=0; i< readResult.GetLength(0);i++)
                {
                    TiMuItem item = new TiMuItem();
                    item.XuHao = int.Parse(readResult[i,0]);
                    item.Type = readResult[i,1];
                    item.Content = readResult[i,2];
                    item.Answer = readResult[i,3];
                    TimuList.Add(item);                   
                }

                

                /*
                 * int[,,,] arr = new int[9, 8, 7, 6];
                    arr.Rank;//返回4
                    arr.GetLength(0);//返回9
                    arr.GetLength(1);//返回8
                    arr.GetLength(2);//返回7
                    arr.GetLength(3);//返回6 
                    arr.GetUpperBound(0)+1;//返回9
                    arr.Length;//返回3024

                 */

                //strOdbcConn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source={0}; Extended Properties=Excel 8.0;", _FileName);
                //using (OleDbConnection OleDb = new OleDbConnection(strOdbcConn))
                //{
                //    OleDb.Open();
                //    //"select * from [Sheet1$] where 类型='多选题'"
                //    OleDbCommand oleCommmand = new OleDbCommand("select * from [Sheet1$];", OleDb);
                //    OleDbDataReader dr = oleCommmand.ExecuteReader();
                //    while (dr.Read())
                //    {
                //        TiMuItem item = new TiMuItem();
                //        item.XuHao = int.Parse(dr[0].ToString());
                //        item.Type = dr[1].ToString();
                //        item.Content = dr[2].ToString();
                //        item.Answer = dr[3].ToString();
                //        TimuList.Add(item);
                       
                //    }
                //    dr.Close();
                   
                //}
               this.groupBox1.Visible = true;
      
                this.label1.Text = "题目总数：" + TimuList.Count.ToString();
                this.Cursor = Cursors.Default;         
                this.NextBtn.Visible = true;
         
            }
        }

        //点击开始或下一题按钮时，根据题目类型将答案类型显示出来
        private void NextBtn_Click(object sender, EventArgs e)
        {
            if(this.ConfirmBtn.Visible == false)
            {
                this.ConfirmBtn.Visible = true;
            }
            HideCheckAndRadio();
            
            i++;
            if(i == TimuList.Count)
            {
                if(MessageBox.Show("您已经练习完一遍，是否进行错题练习","提示",MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    TimuList.Clear();
                    foreach(TiMuItem p in TimuWrongList)
                    {
                        TimuList.Add(p);
                    }
                    TimuWrongList.Clear();
                    this.label1.Text = "题目总数：" + TimuList.Count.ToString();
                }
                else
                {
                    MessageBox.Show("请选择新题库");
                    this.OpenFileBtn.Enabled = true;
                    this.label1.Text = "";
                    this.groupBox1.Visible = false;
                }
                //恢复初始状态 情况题目显示栏 隐藏图标  i变为-1
                i = -1;
                nR = nW = 0;
                
                this.label2.Text = "";
                this.label3.Text = "";

                this.NextBtn.Text = "开始";
                return;
            }
            
            TiMuItem item = TimuList[i];
            this.ContentRichTextBox.Text ="第"+(i+1).ToString()+"题\r\n"+ item.Content;
            if(item.Type == "单选题")
            {
                ShowRadioBtn();
            }
            else
            {
                ShowCheckBtn();
            }
            
            this.NextBtn.Text = "下一题";
            this.NextBtn.Enabled = false;
            this.ConfirmBtn.Enabled = true;
        }


        //确认答案，并显示错误和正确图标，
        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            
            this.NextBtn.Enabled = true;
            this.ConfirmBtn.Enabled = false;
            string ans;
            TiMuItem item = TimuList[i];

            //对单选题的结果进行判断，错题进入错题列表
            if (item.Type == "单选题")
            {
                if(radioButtonA.Checked)
                {
                    ans = "A";
                }
                else if(radioButtonB.Checked)
                {
                    ans = "B";
                }
                else if (radioButtonC.Checked)
                {
                    ans = "C";
                }
                else
                {
                    ans = "D";
                }
                if(item.Answer.IndexOf(ans)>-1)
                {
                    this.CorrectPictureBox.Visible = true;
                    nR++;
                    this.label2.Text = "答对数量：" + nR.ToString();
                }
                else
                {
                    TiMuItem wrongItem = new TiMuItem();
                    wrongItem = item;
                    TimuWrongList.Add(wrongItem);
                    nW++;
                    this.label3.Text = "答错数量：" + nW.ToString();
                    this.WrongPictureBox.Visible = true;
                    this.RightAnsTextBox.Text = "正确答案是：" + item.Answer;
                }
               
            }//单选题判断结束

            //对多选题的结果进行判断

            else
            {
                ans = "";
                if (checkBoxA.Checked)
                {
                    ans = "A";
                }
                if (checkBoxB.Checked)
                {
                    ans += "B";
                }
                if (checkBoxC.Checked)
                {
                    ans += "C";
                }
                if (checkBoxD.Checked)
                {
                    ans += "D";
                }
                if (checkBoxE.Checked)
                {
                    ans += "E";
                }
                if (checkBoxF.Checked)
                {
                    ans += "F";
                }
                if(item.Answer.Equals(ans))
                {
                    this.CorrectPictureBox.Visible = true;
                    nR++;
                    this.label2.Text = "答对数量：" + nR.ToString();
                }
                else
                {
                    TiMuItem wrongItem = new TiMuItem();
                    wrongItem = item;
                    TimuWrongList.Add(wrongItem);
                    nW++;
                    this.label3.Text = "答错数量：" + nW.ToString();
                    this.WrongPictureBox.Visible = true;
                    this.RightAnsTextBox.Text = "正确答案是：" + item.Answer;
                }                
            }
            //多选题判断结束
        }
        private void ShowCheckBtn()
        {
            this.checkBoxA.Visible = true;
            this.checkBoxB.Visible = true;
            this.checkBoxC.Visible = true;
            this.checkBoxD.Visible = true;
            this.checkBoxE.Visible = true;
            this.checkBoxF.Visible = true;
        }
        private void ShowRadioBtn()
        {
            this.radioButtonA.Visible = true;
            this.radioButtonB.Visible = true;
            this.radioButtonC.Visible = true;
            this.radioButtonD.Visible = true;
            this.groupBox2.Visible = true;
        }      
    }
}
