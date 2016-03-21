// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DynamicListViewRenderer.cs" company="XLabs Team">
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

using System.Linq;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using Xamarin.Forms.Platform.WinPhone;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class DynamicListViewRenderer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DynamicListViewRenderer<T> : ViewRenderer<DynamicListView<T>, LongListSelector>
	{
		/// <summary>
		/// The _table view
		/// </summary>
		private LongListSelector _tableView;

		private System.Windows.DataTemplate _dateTemplate;

		/// <summary>
		/// The xaml
		/// </summary>
		private const string XAML = @"<DataTemplate
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
					<Grid>
						<TextBlock Text='{Binding}' FontSize='40' />
					</Grid>        
				</DataTemplate>";

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
		{
			base.OnElementChanged(e);

			if (_tableView == null)
			{
				//var source = new DataSource(this);
				//this.tableView.ItemTemplate = new DataSource(this).ContentTemplate;
				
				_tableView = new LongListSelector();
				_tableView.SelectionChanged += (sender, args) =>
					{
						foreach (var item in args.AddedItems.OfType<T>())
						{
							Element.InvokeItemSelectedEvent(this, item);
						}
					};

				_tableView.ItemTemplate = Template;

				SetNativeControl(_tableView);
			}

			Unbind(e.OldElement);
			Bind(e.NewElement);
		}

		/// <summary>
		/// Gets the template.
		/// </summary>
		/// <value>The template.</value>
		protected virtual System.Windows.DataTemplate Template
		{
			get
			{
				return _dateTemplate ?? (_dateTemplate = (System.Windows.DataTemplate)XamlReader.Load(XAML));
			}
			set { _dateTemplate = value; }
		}

		/// <summary>
		/// Templates for item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>System.Windows.DataTemplate.</returns>
		protected virtual System.Windows.DataTemplate TemplateForItem(object item)
		{
			return Template;
		}

		/// <summary>
		/// Unbinds the specified old element.
		/// </summary>
		/// <param name="oldElement">The old element.</param>
		private void Unbind(DynamicListView<T> oldElement)
		{
			if (oldElement != null)
			{
				oldElement.PropertyChanged -= ElementPropertyChanged;
			}

			_tableView.ItemsSource = null;
		}

		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(DynamicListView<T> newElement)
		{
			if (newElement != null)
			{
				_tableView.ItemsSource = Element.Data;
				newElement.PropertyChanged += ElementPropertyChanged;
			}
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Data")
			{
				_tableView.ItemsSource = Element.Data;
			}
		}

		/// <summary>
		/// Class DataSource.
		/// </summary>
		public class DataSource : System.Windows.Controls.ContentControl
		{
			/// <summary>
			/// The _parent
			/// </summary>
			private readonly DynamicListViewRenderer<T> _parent;

			/// <summary>
			/// Initializes a new instance of the <see cref="DataSource"/> class.
			/// </summary>
			/// <param name="parent">The parent.</param>
			public DataSource(DynamicListViewRenderer<T> parent)
			{
				_parent = parent;
			}

			/// <summary>
			/// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes.
			/// </summary>
			/// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
			/// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
			protected override void OnContentChanged(object oldContent, object newContent)
			{
				base.OnContentChanged(oldContent, newContent);

				if (newContent != null && (oldContent == null || oldContent.GetType() != newContent.GetType()))
				{
					ContentTemplate = _parent.TemplateForItem(newContent);
				}
			}
		}

		/// <summary>
		/// Class MyDataTemplate.
		/// </summary>
		public class MyDataTemplate : System.Windows.DataTemplate
		{
			
		}
	}
}
