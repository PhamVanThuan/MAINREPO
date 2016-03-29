namespace SAHL.X2Engine2.Providers
{
    public interface ISerializationProvider
    {
        string Serialize<T>(T instanceToSerialize);

        T Deserialize<T>(string serializedObject);
    }
}