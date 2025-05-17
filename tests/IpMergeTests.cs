using ipmerge;

namespace tests;

public class IpMergeTests
{
    [Theory]
    [InlineData("192.168.1.0/24 NetA", "192.168.1.128/25 NetB", "192.168.1.0/24 NetA")]
    [InlineData("11.0.0.0/8 Label1", "11.0.0.0/9 Label2", "11.0.0.0/8 Label1")]
    [InlineData("100.64.0.0/10 A", "100.64.0.0/11 B", "100.64.0.0/10 A")]
    public void MergesOverlappingRangesCorrectly(string line1, string line2, string expected)
    {
        var input = new List<string> { line1, line2 };
        var parsed = input.Select(IpMerge.ParseLineToLabeledRange).ToList();
        var merged = IpMerge.MergeLabeledRanges(parsed);

        var output = merged.Select(r => {
            var (cidr, baseIp) = IpMerge.GetMinimalCidr(new IpRange(r.Start, r.End));
            return $"{IpMerge.ToDottedDecimal(baseIp)}/{cidr} {r.Label}";
        }).ToList();

        Assert.Single(output);
        Assert.Equal(expected, output[0]);
    }

    [Fact]
    public void HandlesNonOverlappingCorrectly()
    {
        
        var input = new List<string> {
            "192.168.1.0/25 Net1",
            "192.168.1.129/25 Net2"
        };
        var parsed = input.Select(IpMerge.ParseLineToLabeledRange).ToList();
        var merged = IpMerge.MergeLabeledRanges(parsed);

        Assert.Equal(2, merged.Count);
    }
}