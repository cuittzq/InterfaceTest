using InterfaceTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postContent"></param>
        public MainProcess(string url, string postContent)
        {
            this.url = url;
            this.postContent = postContent;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public void Exetue()
        {
            string responce = string.Empty;
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

            ShowAction.ShowMsg(responce);
        }
    }
}
