using RapChieuPhim.Controllers;
using System;
using System.Timers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RapChieuPhim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static readonly Timer _sTimer = new Timer();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SetTimer();
        }
        private static void SetTimer()
        {

            _sTimer.Enabled = true;
            _sTimer.Interval = 10;
            _sTimer.Elapsed += sTimer_Elapsed;
            _sTimer.Start();
        }
        static async void sTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _sTimer.Interval = TimeSpan.FromHours(24).TotalMilliseconds;
            if (DateTime.Now.Date >= Convert.ToDateTime("06/04/2020"))
            {
                var hc = new PhimsController();
                hc.XoaLichChieu();
            }
        }
    }
}
