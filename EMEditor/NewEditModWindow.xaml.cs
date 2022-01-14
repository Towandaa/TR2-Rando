﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EMEditor
{
    /// <summary>
    /// Interaction logic for NewEditModWindow.xaml
    /// </summary>
    public partial class NewEditModWindow : Window
    {
        public NewEditModWindow()
        {
            InitializeComponent();
        }

        public NewEditModWindow(object SelectedMod)
        {
            this.DataContext = SelectedMod;
            InitializeComponent();
        }
    }
}
