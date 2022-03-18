using TestPaymob.Repos.Interface;
using TestPaymob.Dtos;
using Newtonsoft.Json;
using TestPaymob.Models;

namespace TestPaymob.Repos.implementation
{
    public class PayMobRepo : IPAyMobRepo
    {
        private readonly IConfiguration _configuration;

        public PayMobRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> Purchase(Product product)
        {
            try
            {
                string token = await GetToken(_configuration.GetSection("ApiKeys").GetSection("PayMobApiKey").Value);
                string orderIid = await CreateOrder(product, token);
                return await GetPaymentKeys(orderIid, token, product.Price);
                
            }
            catch (Exception ex )
            {

                return ex.Message;
            }
        }

        private async Task <string> GetToken(string apiKey)
        {
           
            try
            {
                GetTokenRequestDto tokenRequestDto = new GetTokenRequestDto
                {
                    api_key = apiKey
                };


                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                string baseURL = "https://accept.paymob.com/api/";
                string apiURL = "auth/tokens";
                HttpClient webApiClient = new HttpClient(handler);
                webApiClient.BaseAddress = new Uri(baseURL);
                webApiClient.DefaultRequestHeaders.Clear();
                webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));


                HttpResponseMessage response = await webApiClient.PostAsJsonAsync(apiURL, tokenRequestDto);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var deserialized = JsonConvert.DeserializeObject<GetTokenResponseDto>(jsonString);

                if ((int)response.StatusCode == 201)
                    if(deserialized != null)
                    return deserialized.token;
                return "Error Please Contact Technical Spport!!";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
       
        private  async Task<string> CreateOrder(Product product, string authToken)
        {
            try
            {
                List<ItemDto> items = new List<ItemDto>();
                items.Add(new ItemDto {
                    amount_cents=product.Price*100,
                     description=product.Description,
                     name=product.Name,
                     quantity =1
                });
                Random rand = new Random();
                CreateOrderRequestDto createOrderRequestDto = new CreateOrderRequestDto
                {
                    auth_token = authToken,
                    amount_cents = product.Price * 100,
                    currency = "EGP",
                    items = items,
                    merchant_order_id = rand.Next(1, 999999),
                    terminal_id= "23772"


                };


                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                string baseURL = "https://accept.paymob.com/api/";
                string apiURL = "ecommerce/orders";
                HttpClient webApiClient = new HttpClient(handler);
                webApiClient.BaseAddress = new Uri(baseURL);
                webApiClient.DefaultRequestHeaders.Clear();
                webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));


                HttpResponseMessage response = await webApiClient.PostAsJsonAsync(apiURL, createOrderRequestDto);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var deserialized = JsonConvert.DeserializeObject<CreateOrderResponseDto>(jsonString);

                if ((int)response.StatusCode == 201)
                    if (deserialized != null&&deserialized.id!=null)
                        return deserialized.id;
                return "Error Please Contact Technical Spport!!";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }


        private async Task<string> GetPaymentKeys(string orderId, string token, double amount_cents)
        {

            try
            {
                BillingData billingData = new BillingData
                {
                    apartment = "803",
                    email = "claudette09@exa.com",
                    floor = "42",
                    first_name = "Clifford",
                    street = "Ethan Land",
                    building = "8028",
                    phone_number = "+86(8)9135210487",
                    shipping_method = "PKG",
                    country = "CR",
                    postal_code = "01898",
                    city = "Jaskolskiburgh",
                    last_name = "Nicolas",
                    state = "Utah"

                };
                GetPaymentKeysRequestDto getPaymentKeysRequestDto = new GetPaymentKeysRequestDto
                {
                    billing_data = billingData,
                    amount_cents = amount_cents*100,
                    auth_token= token,
                    order_id=orderId,
                    currency= "EGP",
                    integration_id= "1914636"
                };


                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                string baseURL = "https://accept.paymob.com/api/";
                string apiURL = "acceptance/payment_keys";
                HttpClient webApiClient = new HttpClient(handler);
                webApiClient.BaseAddress = new Uri(baseURL);
                webApiClient.DefaultRequestHeaders.Clear();
                webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));


                HttpResponseMessage response = await webApiClient.PostAsJsonAsync(apiURL, getPaymentKeysRequestDto);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var deserialized = JsonConvert.DeserializeObject<GetPaymentKeysResponseDto>(jsonString);

                if ((int)response.StatusCode == 201)
                    if (deserialized != null&& deserialized.token!=null)
                        return deserialized.token;
                return "Error Please Contact Technical Spport!!";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }




    }
}
