// ***********************************************************************
// Assembly         : XLabs.Sample.WP
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BasicListRenderer.cs" company="XLabs Team">
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
using System.Windows;
using System.Windows.Markup;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Sample.WP.DynamicListView;
using DataTemplate = System.Windows.DataTemplate;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace XLabs.Sample.WP.DynamicListView
{
	public class BasicListRenderer : DynamicListViewRenderer<object>
	{
		private const string Xaml = @"<DataTemplate
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
					xmlns:Views=""clr-namespace:XLLabs.Sample.WP.DynamicListView;assembly=XLabs.Forms.Sample.WP""
					>
					<Views:BasicListContentControl Content=""{Binding}"">
							<Views:BasicListContentControl.DateTimeTemplate>
								<DataTemplate>
									<Grid>
										<TextBlock Text='{Binding}' FontSize='30' />
									</Grid>
								</DataTemplate>
							</Views:BasicListContentControl.DateTimeTemplate>
							<Views:BasicListContentControl.StringTemplate>
								<DataTemplate>
									<Grid>
										<TextBlock Text='{Binding}' FontSize='24' />
									</Grid>
								</DataTemplate>
							</Views:BasicListContentControl.StringTemplate>
					</Views:BasicListContentControl>        
				</DataTemplate>";

		protected override DataTemplate Template
		{
			get
			{
				return (DataTemplate)XamlReader.Load(Xaml);
			}
		}

		protected override DataTemplate TemplateForItem(object item)
		{
			return base.TemplateForItem(item);
		}
	}

	public class BasicListContentControl : System.Windows.Controls.ContentControl
	{
		public DataTemplate DateTimeTemplate { get; set; }

		public DataTemplate StringTemplate { get; set; }

		public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is DateTime)
			{
				return DateTimeTemplate;
			}

			if (item is string)
			{
				return StringTemplate;
			}

			return null;
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);

			ContentTemplate = SelectTemplate(newContent, this);
		}
	}
}
