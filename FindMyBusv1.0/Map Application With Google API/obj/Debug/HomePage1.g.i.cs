﻿#pragma checksum "C:\Users\Kritartha\Desktop\findmybus\FindMyBusv1.0\Map Application With Google API\HomePage1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0E8F056DAFF56D7B9C976F20C7E30674"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MapApplication {
    
    
    public partial class HomePagePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.ListPicker Buses;
        
        internal Microsoft.Phone.Controls.ListPicker SourceBusStop;
        
        internal Microsoft.Phone.Controls.ListPicker DestinationBusStop;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Map%20Application%20With%20Google%20API;component/HomePage1.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Buses = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("Buses")));
            this.SourceBusStop = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("SourceBusStop")));
            this.DestinationBusStop = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("DestinationBusStop")));
        }
    }
}

