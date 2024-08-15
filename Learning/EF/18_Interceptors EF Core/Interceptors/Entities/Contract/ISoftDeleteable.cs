
namespace Interceptors.Entities.Contract
{
    public interface ISoftDeleteable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedAt { get; set; }

        void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }

        void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
