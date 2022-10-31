using NdiCommander;

namespace NdiPresetSelector;


internal static class StaticFactory
{
    static StaticFactory()
    {
        _ndiCommanderFactory = new NdiCommanderFactory();
    }


    public static ISettingsRepository NewSettingsRepository()
    {
        return new SettingsRepository();
    }


    public static IGlobalHotKeyManager NewGlobalHotKeyManager()
    {
        return new GlobalHotKeyManager();
    }


    public static INdiWatcher NewNdiWatcher()
    {
        return _ndiCommanderFactory.NewNdiWatcher();
    }


    private static INdiCommanderFactory _ndiCommanderFactory;
}