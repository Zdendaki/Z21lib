using System.Diagnostics.CodeAnalysis;

namespace Z21lib.Structs
{
    [method: SetsRequiredMembers]
    public readonly struct Z21Info(string ip, int port)
    {
        public required string IP { readonly get; init; } = ip;

        public required int Port { readonly get; init; } = port;
    }
}
