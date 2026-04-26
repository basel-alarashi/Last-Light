namespace LastLight.Systems
{
    /// <summary>
    /// Implemented by anything the player can gather from.
    /// </summary>
    public interface IGatherable
    {
        bool IsDepleted { get; }
        ResourceData Data { get; }
        int Gather();
    }
}