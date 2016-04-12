using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace tt_medley.Functions
{
    public class BackGroundServerTimer : IRegisteredObject
    {
         private Timer _timer;
        
        public BackGroundServerTimer()
        {
            StartTimer();
        }
        private void StartTimer()
        {
            int hours = 24;
            int repeatEvery = hours * 60 * 60 * 1000;
            //_timer = new Timer(CheckPushNotifications, null, 0, 2000); // repeatEvery);
        }
        private void CheckPushNotifications(object state)
        {
            //Código
            string URL = "https://tt-medley-podcast.azure-mobile.net/tables/TodoItem";
            string urlParameters = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6MH0.eyJleHAiOjE0NTc3ODc5MzAuMDE5LCJpc3MiOiJ1cm46bWljcm9zb2Z0OndpbmRvd3MtYXp1cmU6enVtbyIsInZlciI6MSwiYXVkIjoiQ3VzdG9tIiwidWlkIjoiQ3VzdG9tOkUxMzJGNDIzLTRCQjItNDJBQy05ODZBLUE5Rjk2NDk1RDlEOSJ9.AnSUognQXmiR8YQq3bBpK2cc0e3WtcC5Ytsn97u7oKQ");

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                
            }
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}