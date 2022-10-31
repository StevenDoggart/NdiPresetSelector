namespace NdiCommander;


/// <inheritdoc cref="INdiCommanderFactory"/>
public class NdiCommanderFactory : INdiCommanderFactory, INdiSourceFactory
{
    public INdiWatcher NewNdiWatcher()
    {
        return new NdiWatcher(this);
    }


    INdiSource INdiSourceFactory.NewNdiSource(string receiverName, string sourceName)
    {
        return new NdiSource(receiverName, sourceName);
    }
}


/// <summary>
///     A factory for creating objects from the NDI Commander library
/// </summary>
public interface INdiCommanderFactory
{
    public INdiWatcher NewNdiWatcher();
}


internal interface INdiSourceFactory
{
    public INdiSource NewNdiSource(string receiverName, string sourceName);
}