using InterfaceTest.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace InterfaceTest.UI
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 显示页面信息委托
        /// </summary>
        private Action<string> showMsgHandler = null;


        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示需要显示的页面提示信息的事件实现
        /// </summary>
        /// <param name="msg">页面提示信息</param>
        private void ShowMessge_ShowMsg(string msg)
        {
            this.Invoke(this.showMsgHandler, msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string requsturl = this.textBox1.Text.Trim();
            string requstPost = this.richTextBox1.Text.Trim();
            if (this.checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(requstPost))
                {

                }
                else
                {
                    Dictionary<string, object> paramDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(requstPost);
                    if (paramDic != null)
                    {
                        paramDic = paramDic.OrderBy(o => o.Key).ThenBy(o => o.Value).ToDictionary(p => p.Key, k => k.Value);
                    }

                }

            }

            if (!requsturl.StartsWith("http://"))
            {
                requsturl = "http://" + requsturl;
            }
            MainProcess mainProcess = new MainProcess(requsturl, requstPost);
            mainProcess.Exetue();
        }

        /// <summary>
        /// 具体页面显示
        /// </summary>
        /// <param name="msg">页面显示内容</param>
        private void PrintMsg(string msg)
        {
            this.richTextBox2.Text = msg;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 处理底层到页面显示的事件
            ShowAction.ShowMsgEvent += new Action<string>(this.ShowMessge_ShowMsg);
            // 页面显示委托
            this.showMsgHandler = new Action<string>(this.PrintMsg);
        }
    }
}
