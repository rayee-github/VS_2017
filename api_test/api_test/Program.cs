using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace api_test
{
    class Program
    {
        public class Data
        {
            public string id { get; set; }

            public string name { get; set; }

            public string age { get; set; }

            public string mail { get; set; }
        }
        static void Gg()
        {
            //後臺client方式GET提交
            HttpClient myHttpClient = new HttpClient();
            //提交當前地址的webapi
            string url = "http://localhost/phptest/index.php/";
            myHttpClient.BaseAddress = new Uri(url);
            //GET提交 返回string 
            HttpResponseMessage response = myHttpClient.GetAsync("get/info").Result;
            string result = "";
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
          
            string jsonData = JsonConvert.SerializeObject(result);
            /*Console.WriteLine(jsonData);
            List<Data> deserializeddata = JsonConvert.DeserializeObject<List<Data>>(result);
            foreach (Data myStringList in deserializeddata)
            {
                Console.WriteLine(myStringList.name);
            }*/

            //post 提交 先建立一個和webapi對應的類
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"name","Name" },
                    {"age","21"},
                    {"mail","1fdsf"}
                 });
            response = myHttpClient.PostAsync("post/info/name='qss'", content).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(result);
        }
        static void Main(string[] args)
        {
            Gg();
            Console.Read();
        }
    }
}
