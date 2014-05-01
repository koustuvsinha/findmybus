using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Map_Application_With_Google_API
{
    public partial class splash : PhoneApplicationPage
    {
        public splash()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         NavigationService.Navigate(new Uri("/HomePage1.xaml", UriKind.Relative));              
        }

        private void fareButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/fare.xaml", UriKind.Relative)); 
        }
    }
}