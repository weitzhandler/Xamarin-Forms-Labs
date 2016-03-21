// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTextCellRenderer.cs" company="XLabs Team">
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

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTextCell), typeof(ExtendedTextCellRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTextCellRenderer.
	/// </summary>
	public class ExtendedTextCellRenderer : TextCellRenderer
	{
		// TODO: Complete Cell customizations

		//public override UITableViewCell GetCell(Cell item, UITableView tv)
		//{
		//    var extendedCell = (ExtendedTextCell)item;
		//    var cell = base.GetCell (item, tv);
		//    if (cell != null) {
		//        cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor ();
		//        cell.SeparatorInset = new UIEdgeInsets ((float)extendedCell.SeparatorPadding.Top, (float)extendedCell.SeparatorPadding.Left,
		//            (float)extendedCell.SeparatorPadding.Bottom, (float)extendedCell.SeparatorPadding.Right);

		//        if (extendedCell.ShowDisclousure) {
		//            cell.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;
		//            if (!string.IsNullOrEmpty (extendedCell.DisclousureImage)) {
		//                var detailDisclosureButton = UIButton.FromType (UIButtonType.Custom);
		//                detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Normal);
		//                detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Selected);

		//                detailDisclosureButton.Frame = new RectangleF (0f, 0f, 30f, 30f);
		//                detailDisclosureButton.TouchUpInside += (object sender, EventArgs e) => {
		//                    var index = tv.IndexPathForCell (cell);
		//                    tv.SelectRow (index, true, UITableViewScrollPosition.None);
		//                    tv.Source.AccessoryButtonTapped (tv, index);
		//                };
		//                cell.AccessoryView = detailDisclosureButton;
		//            }
		//        }
		//    }

		//    if(!extendedCell.ShowSeparator)
		//        tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;

		//    tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();

		//    return cell;
		//}
	}
}

