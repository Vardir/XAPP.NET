﻿using System.Windows;

namespace MIDE.WPFApp
{
    public class DialogWindowViewModel : BaseWindowViewModel
    {
        public DialogWindowViewModel(Window window) : base(window)
        {
            WindowMinimumWidth = 290;
            WindowMinimumHeight = 180;
            InnerContentPadding = new Thickness(2);
        }
    }
}