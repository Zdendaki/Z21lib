using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

public class RBusDataChangedMessage : Message
{
    public required List<TrackData> Tracks { get; init; }

    [SetsRequiredMembers]
    internal RBusDataChangedMessage(ReadOnlySpan<byte> data) : base(MessageType.LAN_RMBUS_DATACHANGED)
    {
        int group = data[0] == 1 ? 10 : 0;

        Tracks = [];

        for (int i = 1; i < data.Length; i++)
        {
            byte b = data[i];

            Tracks.Add(new TrackData(group + i, 1, b.Bit(0)));
            Tracks.Add(new TrackData(group + i, 2, b.Bit(1)));
            Tracks.Add(new TrackData(group + i, 3, b.Bit(2)));
            Tracks.Add(new TrackData(group + i, 4, b.Bit(3)));
            Tracks.Add(new TrackData(group + i, 5, b.Bit(4)));
            Tracks.Add(new TrackData(group + i, 6, b.Bit(5)));
            Tracks.Add(new TrackData(group + i, 7, b.Bit(6)));
            Tracks.Add(new TrackData(group + i, 8, b.Bit(7)));
        }
    }
}

[method: SetsRequiredMembers]
public readonly struct TrackData(int module, int input, bool occupied)
{
    public required readonly int Module { get; init; } = module;

    public required readonly int Input { get; init; } = input;

    public required readonly bool Occupied { get; init; } = occupied;
}
