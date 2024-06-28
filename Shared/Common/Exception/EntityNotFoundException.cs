namespace DreamWeddingApi.Shared.Common.Exception;

[Serializable]
public class EntityNotFoundException : SystemException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }
}