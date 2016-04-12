using InterfaceTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace InterfaceTest.Business
{
    public class MainProcess
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        private string url = string.Empty;

        /// <summary>
        /// post数据
        /// </summary>
        private string postContent = string.Empty;

        /// <summary>
        /// 执行线程
        /// </summary>
        private Thread exetueThread = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postContent"></param>
        public MainProcess(string url, string postContent)
        {
            this.url = url;
            this.postContent = postContent;
            this.exetueThread = new Thread(Run);
            this.exetueThread.IsBackground = true;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public void Exetue()
        {
            this.exetueThread.Start();
        }

        private void Run()
        {
            DateTime startTime = DateTime.Now;
            string responce = string.Empty;
            ShowAction.ShowMsg("请求中。。。。");
            if (string.IsNullOrEmpty(this.url))
            {
                responce = "URL错误";
                ShowAction.ShowMsg(responce);
                return;
            }

            if (string.IsNullOrEmpty(this.postContent))
            {
                responce = HttpHelper.HttpGet(this.url);
            }
            else
            {
                responce = HttpHelper.HttpPost(this.url, this.postContent);
            }
            DateTime endTime = DateTime.Now;

            ShowAction.ShowMsg(string.Format("本次接口耗时{0} 响应：{1}", (endTime - startTime).TotalMilliseconds, responce));
        }
    }
}
