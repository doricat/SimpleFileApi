using System.Threading.Tasks;

namespace Domain.Seedwork
{
    public interface IIdentityGenerator<T>
    {
        Task<T> GenerateAsync();
    }
}