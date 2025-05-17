using System;
using System.Collections.Generic;
using System.Linq;

namespace ipmerge;
public record struct IpRange(uint Start, uint End);
public record struct LabeledRange(uint Start, uint End, string Label);

public static class IpMerge
{
    public static IEnumerable<string> ReadLinesFromStdin()
    {
        while (Console.ReadLine() is { } line)
        {
            if (!string.IsNullOrWhiteSpace(line))
                yield return line;
        }
    }
    public static LabeledRange ParseLineToLabeledRange(string line)
    {
        var (label, ip, cidr) = IpMerge.ParseLine(line);
        var mask = IpMerge.MaskFromCidr(cidr);
        var start = ip & mask;
        var end = ip | ~mask;
        return new LabeledRange(start, end, label);
    }

    public static (int Cidr, uint BaseIp) GetMinimalCidr(IpRange range)
    {
        for (var cidr = 0; cidr <= 32; cidr++)
        {
            var mask = MaskFromCidr(cidr);
            if ((range.Start & mask) == range.Start &&
                (range.Start | ~mask) >= range.End)
                return (cidr, range.Start);
        }
        return (32, range.Start); // fallback
    }

    private static (string Label, uint IP, int CIDR) ParseLine(string line)
    {
        var spaceIdx = line.IndexOf(' ');
        var cidrIdx = line.IndexOf('/');

        var ipStr = line[..cidrIdx];
        var label = line[(spaceIdx + 1)..];
        var cidr = int.Parse(line[(cidrIdx + 1)..spaceIdx]);
        var ipParts = ipStr.Split('.').Select(byte.Parse).ToArray();

        var ip = ((uint)ipParts[0] << 24) | ((uint)ipParts[1] << 16) |
                 ((uint)ipParts[2] << 8) | ipParts[3];

        return (label, ip, cidr);
    }

    public static List<LabeledRange> MergeLabeledRanges(List<LabeledRange> ranges)
    {
        ranges.Sort((a, b) => a.Start.CompareTo(b.Start));
        List<LabeledRange> mergeLabeledRanges = [];
        var current = ranges[0];

        for (var i = 1; i < ranges.Count; i++)
        {
            var next = ranges[i];
            if (current.End >= next.Start)
            {
                current = new LabeledRange(
                    current.Start,
                    Math.Max(current.End, next.End),
                    current.Label // keep first label
                );
            }
            else
            {
                mergeLabeledRanges.Add(current);
                current = next;
            }
        }
        mergeLabeledRanges.Add(current);
        return mergeLabeledRanges;
    }

    private static uint MaskFromCidr(int cidr) => cidr == 0 ? 0 : 0xFFFFFFFF << (32 - cidr);

    public static string ToDottedDecimal(uint ip) =>
        $"{(ip >> 24) & 0xFF}.{(ip >> 16) & 0xFF}.{(ip >> 8) & 0xFF}.{ip & 0xFF}";

}