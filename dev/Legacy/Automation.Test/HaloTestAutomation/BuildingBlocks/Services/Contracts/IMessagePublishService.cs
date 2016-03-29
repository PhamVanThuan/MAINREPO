namespace BuildingBlocks.Services.Contracts
{
    public interface IMessagePublishService
    {
        void Publish<T>(T message);
    }
}
