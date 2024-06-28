using System.Runtime.CompilerServices;

namespace DreamWeddingApi.AuthorizationServer.Data;

public static class ModuleInitializer
{

    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}