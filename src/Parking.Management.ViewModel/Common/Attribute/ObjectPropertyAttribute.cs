namespace Parking.Management.ViewModel.Common.Attribute;

public class ObjectPropertyAttribute: System.Attribute
{
    private readonly string _name;

    public ObjectPropertyAttribute(string name)
    {
        _name = name;
    }

    public override string ToString() => _name;
}