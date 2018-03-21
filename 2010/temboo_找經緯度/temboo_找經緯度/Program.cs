using System;
using Temboo.Core;
using Temboo.Library.Google.Geocoding;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate the Choreo, using a previously instantiated TembooSession object, eg:
            TembooSession session = new TembooSession("wqeasd", "myFirstApp", "lFo4hEJT4Jvv6migVdTfMuhMrzEIHDEz");
            GeocodeByAddress geocodeByAddressChoreo = new GeocodeByAddress(session);

            // Set inputs
            geocodeByAddressChoreo.setAddress("雲林縣虎尾鎮公安路25號");

            // Execute Choreo
            GeocodeByAddressResultSet geocodeByAddressResults = geocodeByAddressChoreo.execute();

            // Print results
            Console.WriteLine(geocodeByAddressResults.Latitude);
            Console.WriteLine(geocodeByAddressResults.Longitude);
            //Console.WriteLine(geocodeByAddressResults.Response);
            Console.ReadLine();
        }
    }
}