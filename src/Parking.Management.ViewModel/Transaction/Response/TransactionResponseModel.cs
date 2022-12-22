using Parking.Management.ViewModel.Common.Enum;

namespace Parking.Management.ViewModel.Transaction.Response;

public class TransactionResponseModel
{
    public TransactionTypes Type { get; set; }
    
    public DateTime Time { get; set; }
    
    public double Amount { get; set; }
    
    public double NewestBalance { get; set; }
    
    public string Description { get; set; }
}