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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigikabuRework.UI.Frames
{
    /// <summary>
    /// Interaktionslogik für Speiseplan.xaml
    /// </summary>
    public partial class Speiseplan : Page, IDisposable
    {
        MainViewModel mvm;

        public Speiseplan()
        {
            mvm = (MainViewModel)FindResource("mvm");
            mvm.GetSpeiseplan();
            InitializeComponent();
        }

        public void Dispose()
        {
           
        }
    }
}
