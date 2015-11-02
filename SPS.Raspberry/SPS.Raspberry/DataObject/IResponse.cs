namespace SPS.Raspberry.DataObject
{
    /// <summary>
    /// Interface for all server responses.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// When overriden in a derived class, deserializes the specified JSON string into the current object.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        void Deserialize(string json);
    }
}
