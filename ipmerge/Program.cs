using System;
using System.IO;
using System.Linq;
using ipmerge;
public class Program
{
  public static void Main(string[] args)
  {

    if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
    {
      Console.WriteLine("""
                        Usage: ipmerge [file]

                        Merges overlapping IP ranges with labels.

                        Arguments:
                          file      Optional path to input file. If not provided, reads from stdin.

                        Input Format:
                          Each line: IP/CIDR Label
                          Example:   192.168.0.0/24 Office LAN

                        Output Format:
                          Merged IP/CIDR Label

                        Example:
                          cat input.txt | ./ipmerge
                          ./ipmerge input.txt
                        """);
      return;
    }

    var lines = args.Length > 0
      ? File.ReadAllLines(args[0])
      : IpMerge.ReadLinesFromStdin();

// Then process as before:
    var input = lines.Select(IpMerge.ParseLineToLabeledRange).ToList();
    var merged = IpMerge.MergeLabeledRanges(input);

    foreach (var r in merged)
    {
      var (cidr, baseIp) = IpMerge.GetMinimalCidr(new IpRange(r.Start, r.End));
      Console.WriteLine($"{IpMerge.ToDottedDecimal(baseIp)}/{cidr} {r.Label}");
    }
  }
}