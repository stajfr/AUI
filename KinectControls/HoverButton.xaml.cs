﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace KinectControls
{
    /// <summary>
    /// Interaction logic for HoverButton.xaml
    /// </summary>
    public partial class HoverButton : UserControl
    {

        #region Fields

        //animation related
        private Duration hoverDuration = new Duration(new TimeSpan(0, 0, 2));
        private Duration reverseDuration = new Duration(new TimeSpan(0, 0, 1));
        private DoubleAnimation maskAnimation;
        private bool isHovering = false;

        #endregion

        #region Properties

        public int HoverTime
        {
            set { hoverDuration = new Duration(new TimeSpan(0, 0, value)); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)this.GetValue(BackgroundColorProperty); }
            set { this.SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            "BackgroundColor", typeof(Brush), typeof(HoverButton), new PropertyMetadata(Brushes.Transparent));

        public Brush HoverColor
        {
            get { return (Brush)this.GetValue(HoverColorProperty); }
            set { this.SetValue(HoverColorProperty, value); }
        }
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(
            "HoverColor", typeof(Brush), typeof(HoverButton), new PropertyMetadata(Brushes.White));

        public Brush TextColor
        {
            get { return (Brush)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Brush), typeof(HoverButton), new PropertyMetadata(Brushes.White));

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(HoverButton), new PropertyMetadata(""));

        public int TextSize
        {
            get { return (int)this.GetValue(TextSizeProperty); }
            set { this.SetValue(TextSizeProperty, value); }
        }
        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register(
            "TextSize", typeof(int), typeof(HoverButton), new PropertyMetadata((int)36));

        public string Image
        {
            get { return (string)this.GetValue(ImageProperty); }
            set { this.SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(string), typeof(HoverButton), new PropertyMetadata(""));

        #endregion

        #region Events

        public delegate void ClickHandler(object sender, EventArgs e);
        public event ClickHandler Click;

        #endregion

        #region Animation and Event HelperMethods

        private void StartHovering()
        {
            double maxFillHeight = this.ActualHeight;

            if (!isHovering && IsEnabled)
            {
                isHovering = true;
                maskAnimation = new DoubleAnimation(Mask.ActualHeight, maxFillHeight, hoverDuration);
                maskAnimation.Completed += new EventHandler(maskAnimation_Completed);
                Mask.BeginAnimation(Canvas.HeightProperty, maskAnimation);
            }
        }

        private void StopHovering()
        {
            if (isHovering)
            {
                isHovering = false;
                maskAnimation.Completed -= maskAnimation_Completed;
                maskAnimation = new DoubleAnimation(Mask.ActualHeight, 0, reverseDuration);
                Mask.BeginAnimation(Canvas.HeightProperty, maskAnimation);
            }
        }

        void maskAnimation_Completed(object sender, EventArgs e)
        {
            isHovering = false;
            if (Click != null)
                Click(this, e);
            Mask.BeginAnimation(Canvas.HeightProperty, null);
        }

        public bool Check(double cursorX, double cursorZdistance)
        {
            if (IsCursorInButton(cursorX) && IsCursorInDistance(cursorZdistance))
            {
                this.StartHovering();
                return true;
            }
            else
            {
                this.StopHovering();
                return false;
            }
        }

        private bool IsCursorInDistance(double CursorZdistance)
        {
            try
            {
                bool isIndistance = false;
                if (CursorZdistance < 2)
                {
                    isIndistance= true;  
                }
                return isIndistance;
            }
            catch
            {
                return false;
            }
        }

        private bool IsCursorInButton(double cursorX)
        {
            try
            {
                Point buttonTopLeft = this.PointToScreen(new Point());
                double buttonLeft = buttonTopLeft.X;
                double buttonRight = buttonLeft + this.ActualWidth;

                if (cursorX < buttonLeft || cursorX > buttonRight)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public HoverButton()
        {
            InitializeComponent();
            this.DataContext = this;

          
           
        }
        public void setbackground(String path)
        {
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative)); //"/Images/cacke.jpg"
            Background = img;
        }
    }
}
