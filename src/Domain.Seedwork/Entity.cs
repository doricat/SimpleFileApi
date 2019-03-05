using System;
using System.Threading.Tasks;

namespace Domain.Seedwork
{
    public abstract class Entity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual async Task GenerateIdAsync(IIdentityGenerator<TKey> generator)
        {
            Id = await generator.GenerateAsync();
        }
    }
}