// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DragContentViewRenderer.cs" company="XLabs Team">
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
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;
//using Microsoft.Expression.Interactivity;
//using System.Windows.Interactivity;
//using Microsoft.Expression.Interactivity.Layout;

[assembly: ExportRenderer(typeof(DragContentView), typeof(DragContentViewRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class DragContentViewRenderer.
    /// </summary>
    public class DragContentViewRenderer : ViewRenderer<DragContentView, Canvas>
    {
        private int idx;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<DragContentView> e)
        {
            foreach (var child in this.Children.OfType<ViewRenderer>())
            {
                RemoveListeners(child);
            }

            base.OnElementChanged(e);

            foreach (var child in this.Children.OfType<ViewRenderer>())
            {
                SetListeners(child);
            }
        }

        private void SetListeners(UIElement element)
        {
            var renderer = element as ViewRenderer;

            if (renderer != null)
            {
                foreach (var child in renderer.Children)
                {
                    SetListeners(child);
                }
            }
            else
            {
                //var property = new MouseDragElementBehavior()
                //{
                //    ConstrainToParentBounds = true
                //};

                
                
                //element.MouseLeftButtonDown += DragContentViewRenderer_MouseLeftButtonDown;
                //element.MouseLeftButtonUp += DragContentViewRenderer_MouseLeftButtonUp;
                element.MouseMove += DragContentViewRenderer_MouseMove;
            }
        }

        private void RemoveListeners(UIElement element)
        {
            var renderer = element as ViewRenderer;

            if (renderer != null)
            {
                foreach (var child in renderer.Children)
                {
                    RemoveListeners(child);
                }
            }
            else
            {
                //element.MouseLeftButtonDown -= DragContentViewRenderer_MouseLeftButtonDown;
                //element.MouseLeftButtonUp -= DragContentViewRenderer_MouseLeftButtonUp;
                element.MouseMove -= DragContentViewRenderer_MouseMove;
            }
        }

        void DragContentViewRenderer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);
            System.Diagnostics.Debug.WriteLine(e);
        }

        void DragContentViewRenderer_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);
            System.Diagnostics.Debug.WriteLine(e);
        }

        void DragContentViewRenderer_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var element = sender as UIElement;

            if (element == null)
            {
                return;
            }

            

            //var position = e.GetPosition(element);

            //element.Arrange(new Rect(position, element.RenderSize));

            //System.Diagnostics.Debug.WriteLine(position);

            Canvas.SetZIndex(element, idx++);
            System.Diagnostics.Debug.WriteLine(sender);
            System.Diagnostics.Debug.WriteLine(e);
        }
    }
}
