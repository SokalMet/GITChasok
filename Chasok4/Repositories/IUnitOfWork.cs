namespace Chasok4.Repositories
{
    public interface IUnitOfWork
    {
        MessageRepository Message { get; }
        UserRepository User { get; }
        UserMessageRepository UserMessage { get; }

        void Dispose();
        void Dispose(bool disposing);
        void Save();
    }
}