using Parking.Management.ViewModel.Common.Request;

namespace Parking.Management.ViewModel.Customer.Request;

public class CustomerAddRequestModel
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
}

public class CustomerUpdateRequestModel: CustomerAddRequestModel
{
}

public class CustomerWalletDepositRequestModel
{
    public double Amount { get; set; }
    
    public string Description { get; set; }
}

public class CustomerWalletWithdrawRequestModel
{
    public double Amount { get; set; }
    
    public string Description { get; set; }
}


#region --- Utilities ---
public class CustomerFilterRequestModel: PagingFilterRequest
{
}
#endregion