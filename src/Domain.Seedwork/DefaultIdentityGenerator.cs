using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Domain.Seedwork
{
    public class DefaultIdentityGenerator : IIdentityGenerator<long>
    {
        private int _i;

        public DefaultIdentityGenerator(IOptionsMonitor<LocalIdentityGeneratorOptions> generatorOptions)
        {
            GeneratorOptions = generatorOptions.CurrentValue;
            if (GeneratorOptions.MachineTag > 31 || GeneratorOptions.MachineTag < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public LocalIdentityGeneratorOptions GeneratorOptions { get; }

        public Task<long> GenerateAsync()
        {
            var ticks = DateTime.UtcNow.Ticks & 0xFFFFFFFF;
            Interlocked.CompareExchange(ref _i, -1, ushort.MaxValue);
            var seq = Interlocked.Increment(ref _i);
            var value = ((long)GeneratorOptions.MachineTag << 48) + ((long)seq << 32) + ticks;
            return Task.FromResult(value);
        }
    }

    public class LocalIdentityGeneratorOptions
    {
        public int MachineTag { get; set; }
    }
}