// ***********************************************************************
// Assembly         : XLabs.Sample.WP
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="LabsUrlMapper.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Windows.Navigation;

namespace XLabs.Sample.WP
{
    internal class LabsUrlMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            var tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());

            if (tempUri.Contains("xamarin.forms.labs"))
            {
                //string URI = string.Format("/MainPage.xaml");
                return new Uri("/MainPage.xaml", UriKind.Relative);
            }

            return uri;
        }
    }
}
