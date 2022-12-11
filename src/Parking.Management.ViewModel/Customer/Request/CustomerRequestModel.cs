using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Customer.Request;

public class CustomerAddRequestModel
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Address { get; set; }
}

public class CustomerUpdateRequestModel: CustomerAddRequestModel
{
}

#region --- Utilities ---
public class CustomerFilterRequestModel: PagingFilterRequest
{
}
#endregion