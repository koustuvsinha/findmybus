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
namespace Map_Application_With_Google_API
{
    public static class BusRoute
    {
        static MapRoute maproute;
        static public MapRoute BusRoutefinder()
        {
            busroute();
            return maproute;
        }

        static public void busroute()
        {
            String url = "http://findmybus.herokuapp.com/routes/show?route=v1";
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(url), UriKind.Relative);
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }

        static private async void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
                maproute = new MapRoute(myroute);
                maproute.Color = Colors.Red;
               // kmap.AddRoute(maproute);
                query.Dispose();
            }
        }
    }

}
