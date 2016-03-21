// ***********************************************************************
// Assembly         : XLabs.Sample.WP
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MainPage.xaml.cs" company="XLabs Team">
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

using System.Threading;
using Microsoft.Phone.Controls;
using XLabs.Forms.Extensions;

namespace XLabs.Sample.WP
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            // Sample code to localize the ApplicationBar
            Xamarin.Forms.Forms.Init();

            XLabs.Sample.App.Init();
            Thread.Sleep(2000);
            //Content = XLabs.Sample.App.GetMainPage().ConvertPageToUIElement(this);
            LoadApplication(new XLabs.Sample.App());
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.SetOrientation();
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);

            this.SetOrientation(e.Orientation);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}