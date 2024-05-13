using Ipz_client.Models;
using Ipz_client.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ipz_client
{
    public static class ServerRequest
    {
        public static async Task<ApiResponse> SendAsync(string url, object? data, string requestType)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CurrentUser.AccessToken);
                    HttpResponseMessage response;

                    if (requestType.ToLower() == "post") 
                    {
                        response = await client.PostAsync(url, content);
                    }
                    else if(requestType.ToLower() == "get")
                    {
                        response = await client.GetAsync(url);
                    }
                    else if (requestType.ToLower() == "put")
                    {
                        response = await client.PutAsync(url, content);
                    }
                    else if (requestType.ToLower() == "delete")
                    {
                        response = await client.DeleteAsync(url);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid request type");
                    }
                    
                    var jsonApiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse>(jsonApiResponse);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
