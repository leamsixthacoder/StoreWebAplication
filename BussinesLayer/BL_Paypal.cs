using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using EntityLayer.Paypal;

namespace BussinesLayer
{
    public  class BL_Paypal
    {
        private string urlpaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private string clientid = ConfigurationManager.AppSettings["ClientId"];
        private string secret = ConfigurationManager.AppSettings["Secret"];

        public async Task<Response_Paypal<Response_Checkout>> CreateRequest(Checkout_Order order)
        {
            Response_Paypal<Response_Checkout> response_Paypal = new Response_Paypal<Response_Checkout>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{client}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var json = JsonConvert.SerializeObject(order);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders",data);

                response_Paypal.Status = response.IsSuccessStatusCode;

                if(response.IsSuccessStatusCode)
                {
                    string jsonresponse = response.Content.ReadAsStringAsync().Result;
                    
                    Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonresponse);
                    response_Paypal.Response = checkout;
                }
                return response_Paypal;
            }
        }

        public async Task<Response_Paypal<Response_Capture>> ApprovePayment(string token)
        {
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlpaypal);

                var authToken = Encoding.ASCII.GetBytes($"{client}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                response_Paypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string jsonresponse = response.Content.ReadAsStringAsync().Result;

                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonresponse);
                    response_Paypal.Response = capture;
                }
                return response_Paypal;
            }
        }
    }
}
