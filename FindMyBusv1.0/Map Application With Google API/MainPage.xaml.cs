using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Map_Application_With_Google_API.Resources;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Services;
using System.Runtime.ExceptionServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Expression.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Notification;
using System.Text;
using InTheHand.UI.Popups;

namespace Map_Application_With_Google_API
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string MainUri = "/Html/index.html";
        double l1 = 22.5023525640046;
        double l2 = 88.3297651689591;
        MapLayer mylayer = null;
        MapLayer loclay2 = null;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        string src = string.Empty;
        string dest = string.Empty;
        GeoCoordinate[] geo = new GeoCoordinate[20];
        string test = "";
        string getsrc = "";
        int flag = 0;
        GeoCoordinate find = new GeoCoordinate();
        GeoCoordinate g1 = new GeoCoordinate();
        private MapRoute me2bus;
        private MapLayer myPos;
        private MapLayer stopPos;
        private MapLayer myPosDot;
        private MapLayer stopPosDot;
        private int BusPosition = 0;
        private int myStopPos = 0;


//-------------------------------- Stops the timer on pressing the back key -------------------------------//

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            base.OnBackKeyPress(e);
        }

        

//------------------------------- Getting the values from HOME Xaml Page ---------------------------------//
        
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if(NavigationContext.QueryString.TryGetValue("src", out src))
            {
                if (src != "Unknown")
                {
                    findbusstop(src);
                    flag = 1;
                }
            }

            if (NavigationContext.QueryString.TryGetValue("dest", out dest))
            {
            }
        }

        public MainPage()
        {
            InitializeComponent();
            GeoCoordinate g1 = new GeoCoordinate(l1, l2);
            kmap.Center = g1;
            kmap.ZoomLevel = 13;
            loadfirst();
            busroute();

            GetCoordinate();
            timer2.Interval = new TimeSpan(0, 0, 3);
            timer2.Tick += timer_Tick2;
            timer2.Start();


            timer.Interval = new TimeSpan(0, 0, 15);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick2(object sender, EventArgs e)
        {
            mylocation();
            timer2.Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            buslocation();
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.IsScriptEnabled = true;
            // Add your URL here
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Navigates back in the web browser's navigation stack, not the applications.
        private void BackApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        // Navigates forward in the web browser's navigation stack, not the applications.
        private void ForwardApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
        }

        // Navigates to the initial "home" page.
        private void HomeMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Handle navigation failures.
        private void Browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            MessageBox.Show("Navigation to this page failed, check your internet connection");
        }

        void zoomin(object sender, EventArgs e)
        {
            if (kmap.ZoomLevel < 20)
            {
                kmap.ZoomLevel++;
            }
        }

        void zoomout(object sender, EventArgs e)
        {
            if (kmap.ZoomLevel > 1)
            {
                kmap.ZoomLevel--;
            }
        }


        public void findbusstoptest(string src)
        {
            getsrc = src;
        }
 
//-------------------------------------------Locates the bus----------------------------------------------//

        
        public void buslocation()
        {
            String url = "http://findmybus.herokuapp.com/location/all?route=v1&bus=1";
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(url), UriKind.Relative);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(nowcompletedstring);
        }
        public void nowcompletedstring(object sender, DownloadStringCompletedEventArgs e)
        {
            double dis = 0.0;
            GeoCoordinate geotest = new GeoCoordinate();
            kmap.Layers.Remove(mylayer);
            kmap.Layers.Remove(loclay2);
            var myData = e.Result;
            var json = JsonConvert.DeserializeObject<Mappresent>(myData);
            double lat2 = json.lat;
            double lon2 = json.lon;
            GeoCoordinate g1 = new GeoCoordinate(lat2, lon2);

            Microsoft.Phone.Maps.Toolkit.Pushpin push = new Microsoft.Phone.Maps.Toolkit.Pushpin();
            push.Content = "Your bus is here";
            push.FontSize = 18;
            push.GeoCoordinate = g1;
            push.Opacity = 0.7;
            push.Background = new SolidColorBrush(Colors.Green);
            push.Foreground = new SolidColorBrush(Colors.White);
            MapOverlay myoverlay = new MapOverlay();
            myoverlay.Content = push;
            myoverlay.GeoCoordinate = g1;
            myoverlay.PositionOrigin = new Point(0,1);
            mylayer = new MapLayer();
            mylayer.Add(myoverlay);
            kmap.Layers.Add(mylayer);

            var myImage = new BitmapImage(new Uri("/Assets/bus.png", UriKind.Relative));
            var image = new Image();
            image.Width = 30;
            image.Height = 30;
            image.Opacity = 50;

            image.Source = myImage;
            MapOverlay myLocationOverlay2 = new MapOverlay();
            myLocationOverlay2.Content = image;
            myLocationOverlay2.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay2.GeoCoordinate = g1;
            loclay2 = new MapLayer();
            loclay2.Add(myLocationOverlay2);
            kmap.Layers.Add(loclay2);
                      


//------------------------ Calculating distance between the bus and the Bus stop !!-----------------------//



            //test = text1.Text;
            //test = test.Replace("Nearest Bus Stop : ", "");
            //String[] busstops = new String[20];
            //busstops[0] = "Tollygunge";
            //busstops[1] = "Bangur Hospital";
            //busstops[2] = "Rashbehari More";
            //busstops[3] = "Sarat Bose Road Xing";
            //busstops[4] = "Basanti Devi College";
            //busstops[5] = "Gariahat";
            //busstops[6] = "Bakultala P.O";
            //busstops[7] = "Narkelbagan";
            //busstops[8] = "Ruby";
            //busstops[9] = "Vip Bazar";
            //busstops[10] = "Panchannagram";
            //busstops[11] = "Science City";
            //busstops[12] = "Chingrighata";
            //busstops[13] = "Beliaghata";
            //busstops[14] = "Hyatt Regency";
            //busstops[15] = "Ultadanga";
            //busstops[16] = "Lake Town";
            //busstops[17] = "Dum Dum Park";
            //busstops[18] = "Baguihati";
            //busstops[19] = "Kaikhali";

            //for (int k = 0; k < 20; k++)
            //{
            //    if (busstops[k].Equals(test))
            //    {
            //        geotest = getgeoCoords(k);
            //    }
            //}

            //var query = new RouteQuery();
            //query.Waypoints = new[]
            //{
            //    g1,
            //    geotest,
            //};

            //query.TravelMode = TravelMode.Driving;
            //var result = await query.GetRouteAsync();
            //Route myroute = result;
            //dis = myroute.LengthInMeters;
            //query.Dispose();

            //if(dis<300)
            //{
            //    MessageBox.Show("Your Bus has almost arrived !!", "", MessageBoxButton.OK);
            //    notify();              
            //}

            String url = "http://findmybus.herokuapp.com/routes?route=v1&lat=" + lat2 + "&lon=" + lon2;
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(url), UriKind.Relative);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnServerResult);
            
        }

        public void OnServerResult(object sender, DownloadStringCompletedEventArgs e)
        {
            var myData = e.Result;
            var json = JsonConvert.DeserializeObject<MapData>(myData);
            BusPosition = json.index;
            if (BusPosition > myStopPos)
            {
                //MessageBox.Show("Sorry, Bus has passed!");
            }
            else if (BusPosition == myStopPos)
            {
                MessageBox.Show("The Bus is nearby! Lookout for it.");
                //notification to display when the bus is nearby
            }
            else
            {
                //MessageBox.Show("Please wait for the bus");
            }

            MessageBox.Show("Nearest Bus Stop Index = " + BusPosition + ", myStop = " + myStopPos);
        }



        public GeoCoordinate getgeoCoords(int k)
        {
            geo[0] = new GeoCoordinate(22.4946412329567, 88.3455178639135); // Tollygunge
            geo[1] = new GeoCoordinate(22.4999830886328, 88.3450915643186); // Bangur Hospital
            geo[2] = new GeoCoordinate(22.5170895753413, 88.3462961913435); // Rashbehari More
            geo[3] = new GeoCoordinate(22.5173833122021, 88.3521443299742); // Sarat Bose Road Xing
            geo[4] = new GeoCoordinate(22.5188118870097, 88.3600069631226); // Basanti Devi College
            geo[5] = new GeoCoordinate(22.5198945865004, 88.3657370808621); // Gariahat
            geo[6] = new GeoCoordinate(22.5195788000248, 88.3765461665984); // Bakultala P.O
            geo[7] = new GeoCoordinate(22.5135787198967, 88.4000526723251); // Narkelbagan
            geo[8] = new GeoCoordinate(22.5142554336946, 88.4015340379907); // Ruby
            geo[9] = new GeoCoordinate(22.5254692843552, 88.3958326617964); // Vip Bazar
            geo[10] = new GeoCoordinate(22.5288723488812, 88.3952234565451); // Panchannagram
            geo[11] = new GeoCoordinate(22.541757285409, 88.398245731789);  // Science City
            geo[12] = new GeoCoordinate(22.5588059833443, 88.4106826918831); // Chingrighata
            geo[13] = new GeoCoordinate(22.5623387392234, 88.4080292566803); // Beliaghata
            geo[14] = new GeoCoordinate(22.5717790214662, 88.4034386509914); // Hyatt Regency
            geo[15] = new GeoCoordinate(22.5898610686113, 88.393166763794); // Ultadanga
            geo[16] = new GeoCoordinate(22.6000509021716, 88.4068897446306); // Lake Town
            geo[17] = new GeoCoordinate(22.6031167221805, 88.4187243912121); // Dum Dum Park
            geo[18] = new GeoCoordinate(22.610301093592, 88.4278974632185); // Baguihati
            geo[19] = new GeoCoordinate(22.6268905385358, 88.4332694485994); // Kaikhali

            return geo[k];
        }



//--------------------------------- Fetching the Bus Route from the Server --------------------------------//
        
        
        public void busroute()
        {
            String url = "http://findmybus.herokuapp.com/routes/show?route=v1";
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(url), UriKind.Relative);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }

        private async void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var query = new RouteQuery();
            var data = e.Result;
            var json = JsonConvert.DeserializeObject<Maproute>(data);
            if (json.elements.Count > 0)
            {
                int i = 0;
                GeoCoordinate[] geo = new GeoCoordinate[json.elements.Count];
                String[] busstops = new String[json.elements.Count];
                while (i < json.elements.Count)
                {
                    geo[i] = new GeoCoordinate(json.elements[i].lat, json.elements[i].lon);
                    i++;
                }
                query.Waypoints = geo;
                query.TravelMode = TravelMode.Driving;
                var result = await query.GetRouteAsync();
                Route myroute = result;
                var maproute = new MapRoute(myroute);
                maproute.Color = Colors.Red;
                kmap.AddRoute(maproute);
                query.Dispose();
            }
        }

        public async void busrouteprovided()
        {
            GeoCoordinate[] geocords = new GeoCoordinate[21];
            var query = new RouteQuery();
            query.Waypoints = new GeoCoordinate[]
            {
              new GeoCoordinate(22.4946412329567,88.3455178639135), // Tollygunge
              new GeoCoordinate(22.4999830886328, 88.3450915643186),// Bangur Hospital
              new GeoCoordinate(22.5170895753413, 88.3462961913435), // Rashbehari More
              new GeoCoordinate(22.51738265698964, 88.3523637605948), // Sarat Bose Road Xing
              new GeoCoordinate(22.5188118870097, 88.3600069631226), // Basanti Devi College
              new GeoCoordinate(22.5198945865004, 88.3657370808621), // Gariahat
              new GeoCoordinate(22.5195788000248, 88.3765461665984), // Bakultala P.O
              new GeoCoordinate(22.5135787198967, 88.4000526723251), // Narkelbagan
              new GeoCoordinate(22.5139424594816, 88.401602802297), // Ruby
              new GeoCoordinate(22.5254692843552, 88.3958326617964), // Vip Bazar
              new GeoCoordinate(22.5276830738962,88.3949543528643),
              new GeoCoordinate(22.5279701507772,88.3949643141571),
              new GeoCoordinate(22.5282682684453,88.3949922057769),
              new GeoCoordinate(22.5284743741112,88.3950340432066),
              new GeoCoordinate(22.5288755431862,88.3951356483931),
              new GeoCoordinate(22.529302474133,88.3952412380968),
              new GeoCoordinate(22.5297588471342,88.3953587813519),
              new GeoCoordinate(22.5301140073377,88.3954524175043),
              new GeoCoordinate(22.5305482952574,88.3955341001052),
              new GeoCoordinate(22.541757285409, 88.398245731789),  // Science City
              new GeoCoordinate(22.5586679662571, 88.4108906215633), // Chingrighata
              new GeoCoordinate(22.5620540608442, 88.4082246754306), // Beliaghata
              new GeoCoordinate(22.5711483685549, 88.4038818437594), // Hyatt Regency
              new GeoCoordinate(22.5921224653173, 88.3944552201765), // Ultadanga
              new GeoCoordinate(22.6000509021716, 88.4068897446306), // Lake Town
              new GeoCoordinate(22.6031167221805, 88.4187243912121), // Dum Dum Park
              new GeoCoordinate(22.610301093592, 88.4278974632185), // Baguihati
              new GeoCoordinate(22.6276967999729, 88.4334427274113), // Kaikhali
            };

            query.TravelMode = TravelMode.Driving;
            var result = await query.GetRouteAsync();
            Route myroute = result;
            var maproute = new MapRoute(myroute);
            maproute.Color = Colors.Green;
            kmap.AddRoute(maproute);
            query.Dispose();
    }
        private void loadfirst()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.5;

            ApplicationBarIconButton abb = new ApplicationBarIconButton(new Uri("\\Assets\\zoomin.png", UriKind.Relative));
            abb.Text = "Zoom In";
            abb.Click += zoomin;
            ApplicationBar.Buttons.Add(abb);

            ApplicationBarIconButton abb1 = new ApplicationBarIconButton(new Uri("\\Assets\\zoomout.png", UriKind.Relative));
            abb1.Text = "Zoom out";
            abb1.Click += zoomout;
            ApplicationBar.Buttons.Add(abb1);
        }




//---------------------------------------- Gets the location of the User ----------------------------------//



        public void mylocation()
        {
            //Geolocator mygeolocator = new Geolocator();
            //Geoposition mygeoposition = await mygeolocator.GetGeopositionAsync();
            //Geocoordinate mygeocoordinate = mygeoposition.Coordinate;
            //GeoCoordinate mygeoCoordinate = CoordinateConverter.ConvertGeocoordinate(mygeocoordinate);
            //ShowLocation(mygeoCoordinate);
            //double lat1 = mygeoCoordinate.Latitude;
            //double lon1 = mygeoCoordinate.Longitude;
            //
            kmap.Layers.Remove(myPos);
            kmap.Layers.Remove(myPosDot);
            GeoCoordinate g1 = new GeoCoordinate(l1,l2);
            Microsoft.Phone.Maps.Toolkit.Pushpin push2 = new Microsoft.Phone.Maps.Toolkit.Pushpin();
            push2.Content = "      You       ";
            push2.FontSize = 18;
            push2.GeoCoordinate = g1;
            push2.Opacity = 0.7;
            push2.Background = new SolidColorBrush(Colors.Green);
            push2.Foreground = new SolidColorBrush(Colors.White);

            MapOverlay myoverlay2 = new MapOverlay();
            myoverlay2.Content = push2;
            myoverlay2.GeoCoordinate = g1;
            myoverlay2.PositionOrigin = new Point(0, 1);
            myPos = new MapLayer();
            myPos.Add(myoverlay2);
            kmap.Layers.Add(myPos);
            myPosDot = ShowLocation(g1);

            if(flag==0)
            {
                String url = "http://findmybus.herokuapp.com/routes?route=v1&lat=" + l1 + "&lon=" + l2;
                WebClient wc = new WebClient();
                wc.DownloadStringAsync(new Uri(url), UriKind.Relative);
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(opencompletedstring);
            }
        }

        public async void opencompletedstring(object sender, DownloadStringCompletedEventArgs e)
        {
            var myData = e.Result;
            var json = JsonConvert.DeserializeObject<MapData>(myData);
            double lat2 = json.lat;
            double lon2 = json.lon;
            int i = json.index;
            double distance = json.distance;
            double dis = distance / 1000;

            GeoCoordinate g1 = new GeoCoordinate(l1, l2);
            GeoCoordinate g2 = new GeoCoordinate(lat2, lon2);

            String[] busstops = new String[20];
            busstops[0] = "Tollygunge";
            busstops[1] = "Bangur Hospital";
            busstops[2] = "Rashbehari More";
            busstops[3] = "Sarat Bose Road Xing";
            busstops[4] = "Basanti Devi College";
            busstops[5] = "Gariahat";
            busstops[6] = "Bakultala P.O";
            busstops[7] = "Narkelbagan";
            busstops[8] = "Ruby";
            busstops[9] = "Vip Bazar";
            busstops[10] = "Panchannagram";
            busstops[11] = "Science City";
            busstops[12] = "Chingrighata";
            busstops[13] = "Beliaghata";
            busstops[14] = "Hyatt Regency";
            busstops[15] = "Ultadanga";
            busstops[16] = "Lake Town";
            busstops[17] = "Dum Dum Park";
            busstops[18] = "Baguihati";
            busstops[19] = "Kaikhali";

            text1.Text = "Nearest Bus Stop : " + busstops[i - 1];
            text1.IsReadOnly = true;
            text1.Opacity = 1;

            text2.Text = dis + " Km";
            text2.Opacity = 1;
            text2.IsReadOnly = true;

            var query = new RouteQuery();
            query.Waypoints = new[]
            {
                g1,
                g2,
            };

            if(me2bus!=null) kmap.RemoveRoute(me2bus);
            query.TravelMode = TravelMode.Driving;
            var result = await query.GetRouteAsync();
            Route myroute = result;
            me2bus = new MapRoute(myroute);
            me2bus.Color = Colors.Orange;
            kmap.AddRoute(me2bus);
            query.Dispose();

            kmap.Layers.Remove(stopPos);
            kmap.Layers.Remove(stopPosDot);

            Microsoft.Phone.Maps.Toolkit.Pushpin push3 = new Microsoft.Phone.Maps.Toolkit.Pushpin();
            push3.Content = "     Bus-Stop   ";
            push3.FontSize = 18;
            push3.GeoCoordinate = g2;
            push3.Opacity = 0.7;
            push3.Background = new SolidColorBrush(Colors.Green);
            push3.Foreground = new SolidColorBrush(Colors.White);

            MapOverlay myoverlay3 = new MapOverlay();
            myoverlay3.Content = push3;
            myoverlay3.GeoCoordinate = g2;
            myoverlay3.PositionOrigin = new Point(0,1);
            stopPos = new MapLayer();
            stopPos.Add(myoverlay3);
            kmap.Layers.Add(stopPos);
            stopPosDot = ShowLocation(g2);
        }
      


//------------------------------ Finding the bus stop if the Source is provided ---------------------------//
        
        
        public async void findbusstop(string str)
        {
            //MessageBox.Show(str);
            String[] busstops = new String[20];
            busstops[0] = "Tollygunge";
            busstops[1] = "Bangur Hospital";
            busstops[2] = "Rashbehari More";
            busstops[3] = "Sarat Bose Road Xing";
            busstops[4] = "Basanti Devi College";
            busstops[5] = "Gariahat";
            busstops[6] = "Bakultala P.O";
            busstops[7] = "Narkelbagan";
            busstops[8] = "Ruby";
            busstops[9] = "Vip Bazar";
            busstops[10] = "Panchannagram";
            busstops[11] = "Science City";
            busstops[12] = "Chingrighata";
            busstops[13] = "Beliaghata";
            busstops[14] = "Hyatt Regency";
            busstops[15] = "Ultadanga";
            busstops[16] = "Lake Town";
            busstops[17] = "Dum Dum Park";
            busstops[18] = "Baguihati";
            busstops[19] = "Kaikhali";

            for(int a = 0; a < 20; a++)
            {
                if(busstops[a].Equals(str))
                {
                    find = getgeoCoords(a);
                }
            }

            GetCoordinate();
            GeoCoordinate g1 = new GeoCoordinate(l1,l2);

            var query = new RouteQuery();
            query.Waypoints = new[]
            {
                g1,
                find,
            };

            query.TravelMode = TravelMode.Walking;
            var result = await query.GetRouteAsync();
            Route myroute = result;
            double dis = myroute.LengthInMeters;
            dis = dis / 1000;
            if (me2bus != null) kmap.RemoveRoute(me2bus);
            me2bus = new MapRoute(myroute);
            me2bus.Color = Colors.Orange;
            kmap.AddRoute(me2bus);
            query.Dispose();

            text1.Text = "Nearest Bus Stop : " + str;
            text1.IsReadOnly = true;
            text1.Opacity = 1;

            text2.Text = dis + " Km";
            text2.Opacity = 1;
            text2.IsReadOnly = true;

            Microsoft.Phone.Maps.Toolkit.Pushpin push3 = new Microsoft.Phone.Maps.Toolkit.Pushpin();
            push3.Content = "     Bus-Stop   ";
            push3.FontSize = 18;
            push3.GeoCoordinate = find;
            push3.Opacity = 0.7;
            push3.Background = new SolidColorBrush(Colors.Green);
            push3.Foreground = new SolidColorBrush(Colors.White);

            kmap.Layers.Remove(stopPos);
            kmap.Layers.Remove(stopPosDot);

            MapOverlay myoverlay3 = new MapOverlay();
            myoverlay3.Content = push3;
            myoverlay3.GeoCoordinate = find;
            myoverlay3.PositionOrigin = new Point(0, 1);
            stopPos = new MapLayer();
            stopPos.Add(myoverlay3);
            kmap.Layers.Add(stopPos);
            stopPosDot = ShowLocation(find);
        }


//---------------------------------------- Acts as a Marker on the Map ------------------------------------//
        
        
        
        private MapLayer ShowLocation(GeoCoordinate g1)
        {
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Green);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 0.7;

            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = g1;

            MapLayer loclay = new MapLayer();
            loclay.Add(myLocationOverlay);
            kmap.Layers.Add(loclay);
            return loclay;
        }
       
        public void notify()
        {
            ShellToast st = new ShellToast();
            st.Title = "Find My Bus :";
            st.Content = "Your Bus has almost arrived !!";
            st.Show();
        }

        private void GetCoordinate()
        {
            var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
            {
                MovementThreshold = 1
            };
            watcher.PositionChanged += this.watcher_PositionChanged;
            watcher.Start();
        }
        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var pos = e.Position.Location;
            l1 = pos.Latitude;
            l2 = pos.Longitude;

            //eraseMap();
            flag = 0;
            mylocation();
        }

        private void eraseMap()
        {
            if (me2bus != null || myPos != null || stopPos != null || myPosDot != null || stopPosDot != null)
            {
                kmap.RemoveRoute(me2bus);
                kmap.Layers.Remove(myPos);
                kmap.Layers.Remove(stopPos);
                kmap.Layers.Remove(myPosDot);
                kmap.Layers.Remove(stopPosDot);
            }
        }

        }

   public class MapData
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public int index { get; set; }
        public double distance { get; set; }
    }

    public class Mappresent
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Maproute
    {
        public List<Mapelements> elements { get; set; }
    }
     public class Mapelements
     {
         public double lat { get; set; }
         public double lon { get; set; }
         public int route_id { get; set; }
         public String stop_name { get; set; }
     }

}