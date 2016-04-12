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
using InterfaceTest.Common;

namespace InterfaceTest.UI
{
    public partial class InterfaceTest : Form
    {
        /// <summary>
        /// 显示页面信息委托
        /// </summary>
        private Action<string> showMsgHandler = null;


        public InterfaceTest()
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
            if (string.IsNullOrEmpty(requsturl))
            {
                this.richTextBox1.Text = "请求地址不能为空";
                return;
            }
            if (this.checkBox1.Checked)
            {
                Dictionary<string, string> paramDic = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(requstPost))
                {
                    requsturl = this.GetTopsignUrl(requsturl);
                    this.textBox1.Text = requsturl;
                }
                else
                {
                    paramDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(requstPost);
                    if (paramDic.Count > 0)
                    {
                        paramDic = paramDic.OrderBy(o => o.Key).ThenBy(o => o.Value).ToDictionary(p => p.Key, k => k.Value);
                        if (paramDic.ContainsKey("top_sign"))
                        {
                            paramDic["top_sign"] = CommonBusiness.GetTop_sign(paramDic);
                        }
                        else
                        {
                            paramDic.Add("top_sign", CommonBusiness.GetTop_sign(paramDic));
                        }

                        requstPost = JsonConvert.SerializeObject(paramDic);
                        this.richTextBox1.Text = requstPost;
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


        /// <summary>
        /// 加上校验参数
        /// </summary>
        /// <param name="requstUrl"></param>
        /// <returns></returns>
        private string GetTopsignUrl(string requstUrl)
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();

            string[] urlparams = requstUrl.Split('?');
            string result = requstUrl;
            if (urlparams != null)
            {
                result = urlparams[0];
                urlparams = urlparams[1].Split('&');
                if (urlparams != null)
                {
                    foreach (var item in urlparams)
                    {
                        if (item != "top_sign")
                        {
                            paramDic.Add(item.Split('=')[0], item.Split('=')[1]);
                        }
                    }
                }

                if (paramDic.Count > 0)
                {
                    paramDic = paramDic.OrderBy(o => o.Key).ThenBy(o => o.Value).ToDictionary(p => p.Key, k => k.Value);
                    paramDic.Add("top_sign", CommonBusiness.GetTop_sign(paramDic));
                    result = result + "?";

                    foreach (var item in paramDic)
                    {
                        result += item.Key + "=" + item.Value + "&";
                    }

                    result = result.Remove(result.Length - 1, 1);
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TC_FlightQueryServiceReference.TC_FlightQueryServiceNewSoapClient TC_FlightQueryServiceNew = new TC_FlightQueryServiceReference.TC_FlightQueryServiceNewSoapClient();

            // TC_FlightQueryServiceReference.FlightQueryCondition flightQueryCondition = JsonConvert.DeserializeObject<TC_FlightQueryServiceReference.FlightQueryCondition>(this.richTextBox1.Text);
            // http://www.ly.com/flight/flight-book1.aspx?para=0*URC*YIN*2016-04-12****all*all&key=5CEDAE2DF4B4C07097206D9F2C6771BC&TCAlianceCode=&filterlist=&fqdMark=2
            TC_FlightQueryServiceReference.FlightQueryCondition flightQueryCondition = new TC_FlightQueryServiceReference.FlightQueryCondition();
            flightQueryCondition.OrgAirportCode = "URC";
            flightQueryCondition.ArrivalAirportCode = "YIN";

            flightQueryCondition.TakeoffBegDate = DateTime.Parse("2016-04-12");
            flightQueryCondition.TakeoffEndDate = DateTime.Parse("2016-04-13");

            TC_FlightQueryServiceReference.FlightQueryResult flightQueryResult = TC_FlightQueryServiceNew.TCQueryOneWayFlights(TC_FlightQueryServiceReference.TCFlatType.B2C_Cooperation, flightQueryCondition);
            if (flightQueryResult != null)
            {

                foreach (var fFlight in flightQueryResult.IBEFlights)
                {
                    // flightQueryResult.IBEFlights[1].EngineRoomColl[5].ForeProfit
                    if (fFlight.AirCompanyCode == "GS")
                    {
                        foreach (var roomColl in fFlight.EngineRoomColl)
                        {
                            if (roomColl.ForeProfit > 0)
                            {
                                PrintMsg(JsonConvert.SerializeObject(roomColl));
                            }
                        }
                    }
                }

                PrintMsg(JsonConvert.SerializeObject(flightQueryResult));
            }

        }
    }
}
