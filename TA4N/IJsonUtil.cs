namespace TA4N
{
    public interface IJsonUtil
    {
        string SerializeObject(object value);
        T DeserializeObject<T>(string contentBody);
    }
}
