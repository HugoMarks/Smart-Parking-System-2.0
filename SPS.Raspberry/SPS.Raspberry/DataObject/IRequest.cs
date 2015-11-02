namespace SPS.Raspberry.DataObject
{
    /// <summary>
    /// Interface for all the server requests.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// When overriden in a derived class, serializes the current object into a JSON string.
        /// </summary>
        /// <returns>The serialized object, as a JSON string.</returns>
        string Serialize();
    }
}
