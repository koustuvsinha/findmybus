using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MapApplication
{
    public partial class HomePagePage : PhoneApplicationPage
    {
        public HomePagePage()
        {
            InitializeComponent();

            String[] busstops = { "Unknown","Tollygunge","Bangur Hospital","Rashbehari More","Sarat Bose Road Xing","Basanti Devi College",
                "Gariahat","Bakultala P.O","Narkelbagan","Ruby","Vip Bazar","Science City","Chingrighata","Beliaghata",
                "Hyatt Regency","Ultadanga","Lake Town","Dum Dum Park","Baguihati","Kaikhali","Airport"};

            String[] bus = { "V1", "V2", "V9", "V10" };
            SourceBusStop.ItemsSource = busstops;
            DestinationBusStop.ItemsSource = busstops;
            Buses.ItemsSource = bus;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string src = SourceBusStop.SelectedItem.ToString();
            string dest = DestinationBusStop.SelectedItem.ToString();
            
            NavigationService.Navigate(new Uri("/MainPage.xaml?src="+src+"&dest="+dest, UriKind.Relative));    
            

            ///MessageBox.Show(SourceBusStop.SelectedItem.ToString());
        }
    }
}