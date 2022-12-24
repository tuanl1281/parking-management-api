using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Reflection;
using System.Net.Http.Headers;
using Parking.Management.Service.Utilities;
using Parking.Management.ViewModel.Common.Attribute;
using Parking.Management.ViewModel.Common.Enum;

namespace Parking.Management.Service.Core.Common;

public interface IHttpClientService
{
    Task<T> SendAsync<T>(string url, string method, object? parameter, object? payload);

    Task<T> GetAsync<T>(string url);

    Task<T> PostAsync<T>(string url, object payload, ContentTypes contentType = ContentTypes.Json);

    Task<T> PutAsync<T>(string url, object payload, ContentTypes contentType = ContentTypes.Json);

    Task<T> DeleteAsync<T>(string url);
}

public class HttpClientService: IHttpClientService
{
    private readonly HttpClient _client;

    public HttpClientService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<T> SendAsync<T>(string url, string method, object? parameter, object? payload)
    {
        var urlBuilder = url;
        if (!parameter.Equals(null))
            urlBuilder = BuildUrl(url, parameter);

        var payloadBuilder = payload;
        if (payloadBuilder.Equals(null))
            payloadBuilder = new { };

        switch (method)
        {
            case "POST":
            {
                return await PostAsync<T>(urlBuilder, payloadBuilder);
            }
            case "PUT":
            {
                return await PutAsync<T>(urlBuilder, payloadBuilder);
            }
            case "DELETE":
            {
                return await DeleteAsync<T>(urlBuilder);
            }
            default:
            {
                return await GetAsync<T>(urlBuilder);
            }
        }
    }
    
    public async Task<T> GetAsync<T>(string url)
    {
        /* Request */
        var response = await _client.GetAsync(url);
        /* Deserialize */
        var responseText = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(responseText);
        /* Return */
        return data;
    }
    
    public async Task<T> PostAsync<T>(string url, object payload, ContentTypes contentType = ContentTypes.Json)
    {
        /* Prepare */
        dynamic httpContent = null;
        switch (contentType)
        {
            case ContentTypes.Json:
            {
                var payloadString = JsonConvert.SerializeObject(payload);
                httpContent = new StringContent(payloadString, Encoding.UTF8, "application/json");
                
                break;
            }
            case ContentTypes.FormUrlEncoded:
            {
                httpContent = BuildFormUrlEncodedContent(payload);
                
                break;
            }
            case ContentTypes.MultipartFormData:
            {
                httpContent = buildMultipartFormDataContent(payload);
                
                break;
            }
        }
        /* Request */
        var response = await _client.PostAsync(url, httpContent);
        /* Deserialize */
        var responseText = await response.Content.ReadAsStringAsync();
        if (typeof(T).FullName == "System.String")
            return responseText;
        
        var data = JsonConvert.DeserializeObject<T>(responseText);
        /* Return */
        return data;
    }
    
    public async Task<T> PutAsync<T>(string url, object payload, ContentTypes contentType = ContentTypes.Json)
    {
        /* Prepare */
        dynamic httpContent = null;
        switch (contentType)
        {
            case ContentTypes.Json:
            {
                var payloadString = JsonConvert.SerializeObject(payload);
                httpContent = new StringContent(payloadString, Encoding.UTF8, "application/json");
                
                break;
            }
            case ContentTypes.FormUrlEncoded:
            {
                httpContent = BuildFormUrlEncodedContent(payload);
                
                break;
            }
            case ContentTypes.MultipartFormData:
            {
                httpContent = buildMultipartFormDataContent(payload);
                
                break;
            }
        }
        /* Request */
        var response = await _client.PutAsync(url, httpContent);
        /* Deserialize */
        var responseText = await response.Content.ReadAsStringAsync();
        if (typeof(T).FullName == "System.String")
            return responseText;

        var data = JsonConvert.DeserializeObject<T>(responseText);
        /* Return */
        return data;
    }
    
    public async Task<T> DeleteAsync<T>(string url)
    {
        /* Request */
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        /* Deserialize */
        var responseText = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(responseText);
        /* Return */
        return data;
    }
    
    #region --- Utilities ---
    private string BuildUrl(string url, object parameter)
    {
        var parameters = new Dictionary<string, string>();
        foreach (PropertyInfo props in parameter.GetType().GetProperties())
            parameters.Add(props.Name.Camelize(), props.GetValue(parameter, null).ToString());

        return new Uri(QueryHelpers.AddQueryString(url, parameters)).ToString();
    }
    
    private FormUrlEncodedContent BuildFormUrlEncodedContent(object payload)
    {
        var result = new List<KeyValuePair<string, string>>();
        foreach (PropertyInfo props in payload.GetType().GetProperties())
        {
            try
            {
                var key = props.GetCustomAttribute<ObjectPropertyAttribute>().ToString() ?? props?.Name.Camelize() ?? String.Empty;
                result.Add(new KeyValuePair<string, string>(key, props.GetValue(payload, null).ToString()));
            }
            catch (Exception)
            {
                // Ignored
            }
        }
        /* Return */
        return new FormUrlEncodedContent(result);
    }
    
    private MultipartFormDataContent buildMultipartFormDataContent(object payload)
    {
        var result = new MultipartFormDataContent();
        foreach (PropertyInfo props in payload.GetType().GetProperties())
        {
            try
            {
                var key = props.GetCustomAttribute<ObjectPropertyAttribute>().ToString() ?? props?.Name.Camelize() ?? String.Empty;
                /* Build */
                if (props.PropertyType == typeof(IFormFile))
                {
                    var file = (IFormFile) props.GetValue(payload);
                    var memoryContent = new StreamContent(file.OpenReadStream());
                    memoryContent.Headers.ContentType = new MediaTypeHeaderValue($"image/{Path.GetExtension(file.FileName)}");

                    result.Add(memoryContent, name: key, file.FileName);  
                }
                else
                {
                    result.Add(new StringContent(props.GetValue(payload, null).ToString()), name: key);
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }
        /* Return */
        return result;
    }
    #endregion
}