using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace Event_Hubs_Real_Time_Data_Ingestion
{
    class Program
    {
        static EventHubClient hubClient;
        static Random random = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("Generate Item Wise Ordered Qty\n");

            hubClient = EventHubClient.CreateFromConnectionString(ConfigurationManager.AppSettings["eventHubConnectionString"], ConfigurationManager.AppSettings["EventHubName"]);

            // Randomly create instances of the store actions, such as add view remove and checkout a product, 
            // convert it into a JSON string and sends to the IoT Hub.
            GenerateRandomProductSalesQty();
            Console.ReadLine();
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public static void GenerateRandomProductSalesQty()
        {



            var ProductNames = new List<string>() { "Product1", "Product-2", "Product-3", "Product-4", "Product-5",
                                                "Product-6", "Product-7", "Product-8" };

            var Qty = new List<int>() { 95, 12, 9, 51, 31, 72, 82, 40 };

            // load call types into list
            var SalePrice = new List<double>() { 110.10, 222.22, 303.04, 424.50, 556.35, 676.24, 771.47, 99.52 };
            var ShippingPrice = new List<double>() { 56.10, 34.22, 2.40, 99.50, 23.35, 64.24, 98.47, 56.25 };
            while (true)
            {
                Int32 RandomNo = RandomNumber(1, 8);
                // randomize data
                var ProductID = RandomNo;
                var ProductName = ProductNames[RandomNo].ToString();// GetRandomEventNum(ProductNames.Count, ProductNamesWeight);

                decimal SellPrice = Convert.ToDecimal(SalePrice[RandomNo]);
                decimal ShippingPrices = Convert.ToDecimal(ShippingPrice[RandomNo]);
                Int32 Qtys = Qty[RandomNo];
                // construct call detail record
                var RandomProductSalesDetailss = new RandomProductSalesDetails();
                RandomProductSalesDetailss = new RandomProductSalesDetails
                {
                    ProductID = ProductID,
                    ProductName = ProductName,
                    ShippingCost = ShippingPrices,
                    SellPrice = SellPrice,
                    SellQty = Qtys,
                    SaleDate = DateTime.UtcNow

                };
                SendMessageToEventHub(RandomProductSalesDetailss);
                TimeOfDayWait();

            }

        }
        private static async void SendMessageToEventHub(RandomProductSalesDetails RandomProductSalesDetails)
        {
            // Serialize the Call Detail Record into JSON and send to event hub
            try
            {
                var messagestring = JsonConvert.SerializeObject(RandomProductSalesDetails, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                EventData data = new EventData(Encoding.UTF8.GetBytes(messagestring));
                hubClient.Send(data);

                Console.WriteLine("Sent message: {0} at time {1}.", messagestring.ToString(), DateTime.UtcNow.ToString("yyyyMMdd hh:mm:ss"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(DateTime.Now.ToString() + " > Exception: " + exception.ToString());
            }
        }
        private static void TimeOfDayWait()
        {

            // simulate quiet/busy times of the day by sleeping the thread

            TimeZoneInfo userZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

            int hour = TimeZoneInfo.ConvertTime(DateTime.UtcNow, userZone).Hour;

            if (hour >= 22 || hour <= 5)
            {
                Thread.Sleep(900);
            }
            else if (hour >= 6 && hour <= 8)
            {
                Thread.Sleep(400);
            }
            else if (hour >= 9 && hour <= 11)
            {
                Thread.Sleep(300);
            }
            else if (hour >= 12 && hour <= 14)
            {
                Thread.Sleep(200);
            }
            else if (hour >= 15 && hour <= 19)
            {
                Thread.Sleep(400);
            }
            else
            {
                Thread.Sleep(500);
            }

        }


    }
}
