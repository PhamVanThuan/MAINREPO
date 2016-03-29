namespace SAHL.Core.Validation
{
    public enum ValidationActionType
    {
        Unspecified,
        IsNotValidatable, //object, e.g. string
        IsValidatable, //object
        IsEnumerable, //IEnumerable<Address>
        IsEnumerableOfKeyValuePair, //IEnumerable<IKeyValuePair<Key, Value>> e.g. Dictionary<Key, Value>
    }
}