using Newtonsoft.Json;

namespace Parking.Management.ViewModel.Common.Response;

public class PagingWithStatisticResponseModel
{
    [JsonProperty("totalCounts")]
    public int TotalCounts { get; set; } = 0;

    [JsonProperty("data")]
    public dynamic Data { get; set; }

    [JsonProperty("statistic")]
    public dynamic Statistic { get; set; }

    public PagingWithStatisticResponseModel()
    {
    }

    public PagingWithStatisticResponseModel(object statistic, object data, int totalCounts)
    {
        this.Statistic = statistic;
        this.Data = data;
        this.TotalCounts = totalCounts;
    }
}

public class PagingWithStatisticResponseModel<TSEntity, TPEntity>
{
    [JsonProperty("totalCounts")]
    public int TotalCounts { get; set; } = 0;

    [JsonProperty("data")]
    public List<TPEntity> Data { get; set; } = new();

    [JsonProperty("statistic")]
    public TSEntity Statistic { get; set; }
    
    public PagingWithStatisticResponseModel()
    {
    }

    public PagingWithStatisticResponseModel(TSEntity statistic, List<TPEntity> data, int totalCounts)
    {
        this.Statistic = statistic;
        this.Data = data;
        this.TotalCounts = totalCounts;
    }
}
