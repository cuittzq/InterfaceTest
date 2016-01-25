// ***********************************************************************
// <copyright file="CommonBusiness.cs" company="四川全球通">
// Copyright (c) 四川全球通. All rights reserved.</copyright>
// Assembly         : InterfaceTest.Common
// Author            : 谭志强
// Created          : 2016/1/25 11:44:41
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace InterfaceTest.Common
{
    public class CommonBusiness
    {
        /// <summary>
        /// 校验参数
        /// </summary>
        /// <param name="inparamsDic">入参</param>
        /// <returns></returns>
        public static string GetTop_sign(Dictionary<string, string> inparamsDic)
        {
            List<string> paramlist = new List<string>();
            foreach (var item in inparamsDic)
            {
                if (item.Key.ToLower() != "top_sign")
                {
                    paramlist.Add(item.Key + item.Value);
                }
            }
            paramlist = paramlist.OrderBy(p => p).ToList();
            string top_sign = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join(string.Empty, paramlist.ToArray()), "SHA1").ToUpper();
            return top_sign;
        }
    }
}
