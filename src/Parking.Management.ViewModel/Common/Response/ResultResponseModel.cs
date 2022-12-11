using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Parking.Management.ViewModel.Common.Response;

public class ResultResponseModel
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; }

    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public dynamic Data { get; set; }

    public ResultResponseModel()
    {
    }
    
    public ResultResponseModel(bool succeed)
    {
        Succeed = succeed;
    }
    
    public ResultResponseModel(bool succeed, object data)
    {
        Succeed = succeed;
        Data = data;
    }
    
    public ResultResponseModel(bool succeed, object data, string message)
    {
        Succeed = succeed;
        Data = data;
        Message = message;
    }
    
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}

public class ResultResponseModel<T>
{
    [JsonProperty("succeed")]
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; }

    [JsonProperty("message")]
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public T Data { get; set; }

    public ResultResponseModel()
    {
    }
    
    public ResultResponseModel(T data)
    {
        Data = data;
    }
    
    public ResultResponseModel(T data, string message)
    {
        Data = data;
        Message = message;
    }
    
    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
}