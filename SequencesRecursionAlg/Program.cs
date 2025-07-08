using SequencesRecursionAlg.Models;
using System.Numerics;
using System.Text.Json;

internal class Program
{
    /*private static void Main()
    {
        Console.Write("Sequence: ");
        var sequenceStr = Console.ReadLine()!;
        BigInteger[] sequence = [];

        try
        {
            sequence = sequenceStr
                .Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(BigInteger.Parse)
                .ToArray();
        }
        catch { }

        if (sequence.Length <= 0)
        {
            Console.WriteLine("Invalid sequence.");
            Console.ReadLine();
            return;
        }

        var dres = CalculateDerivative(sequence);
        var adres = CalculateAntiderivative(sequence);
        var dres2 = CalculateDerivative(adres);
        var adres2 = CalculateAntiderivative(dres);

        sequenceStr = GetSequenceString(sequence);
        var dresStr = GetSequenceString(dres);
        var adresStr = GetSequenceString(adres);
        var dresStr2 = GetSequenceString(dres2);
        var adresStr2 = GetSequenceString(adres2);

        Console.WriteLine();
        Console.WriteLine(dresStr);
        Console.WriteLine("   <= Derivative   ");
        Console.WriteLine(sequenceStr);
        Console.WriteLine("   Antiderivative =>  ");
        Console.WriteLine(adresStr);

        Console.WriteLine();
        Console.WriteLine(dresStr);
        Console.WriteLine($"   Antiderivative{(adresStr2 == sequenceStr ? " (confirmed)" : string.Empty)} =>  ");
        Console.WriteLine(adresStr2);

        Console.WriteLine();
        Console.WriteLine(dresStr2);
        Console.WriteLine($"   <= Derivative{(dresStr2 == sequenceStr ? " (confirmed)" : string.Empty)}   ");
        Console.WriteLine(adresStr);

        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }*/

    private static void Main()
    {
        Console.Write("Sequence: ");
        var sequenceStr = Console.ReadLine()!;
        BigInteger[] sequence = [];

        try
        {
            sequence = sequenceStr
                .Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(BigInteger.Parse)
                .Take(ApplicationConfiguration.SequenceLength)
                .ToArray();
        }
        catch { }

        if (sequence.Length <= 0)
        {
            Console.WriteLine("Invalid sequence.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Length: {sequence.Length}");

        sequenceStr = GetSequenceString(sequence);
        var dres = new List<BigInteger[]>();
        var adres = new List<BigInteger[]>();

        var t = sequence.ToArray();
        for (var i = 0; i < ApplicationConfiguration.FindSequences; i++)
        {
            t = CalculateDerivative(t);
            dres.Add(t);
        }

        t = sequence.ToArray();
        for (var i = 0; i < ApplicationConfiguration.FindSequences; i++)
        {
            t = CalculateAntiderivative(t);
            adres.Add(t);
        }

        Console.WriteLine();

        for (var i = ApplicationConfiguration.FindSequences - 1; i >= 0; i--)
        {
            var s = GetSequenceString(dres[i]);
            var oeis = GetOeisName(s);
            Console.WriteLine($"{oeis}{s}");
        }

        Console.WriteLine($"{GetOeisName(sequenceStr)}{sequenceStr}");

        for (var i = 0; i < ApplicationConfiguration.FindSequences; i++)
        {
            var s = GetSequenceString(adres[i]);
            var oeis = GetOeisName(s);
            Console.WriteLine($"{oeis}{s}");
        }

        Console.WriteLine();
        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static string GetSequenceString(BigInteger[] sequence) =>
        string.Join(", ", sequence);

    private static BigInteger[] CalculateDerivative(BigInteger[] sequence)
    {
        var len = sequence.Length;
        var res = new BigInteger[len];

        for (var n = 0; n < len; n++)
        {
            BigInteger sum = 0;
            for (var i = 0; i <= n; i++)
            {
                var idx = n - i - 1;
                sum += sequence[i] * (idx >= 0 ? res[idx] : 1);
            }
            res[n] = sum;
        }

        return res;
    }

    private static BigInteger[] CalculateAntiderivative(BigInteger[] sequence)
    {
        var len = sequence.Length;
        var res = new BigInteger[len];

        for (var n = 0; n < len; n++)
        {
            BigInteger sum = 0;
            for (var i = 0; i <= n; i++)
            {
                var idx = n - i - 1;
                sum += -sequence[i] * (idx >= 0 ? res[idx] : -1);
            }
            res[n] = sum;
        }

        return res;
    }

    private static string GetOeisName(string s)
    {
        if (!ApplicationConfiguration.SearchOeis)
            return string.Empty;

        var url = $"https://oeis.org/search?q={Uri.EscapeDataString(s)}&fmt=json";

        using var client = new HttpClient();
        try
        {
            var response = client.GetStringAsync(url).Result;

            var jsonDoc = JsonDocument.Parse(response);
            var result = jsonDoc.RootElement.EnumerateArray().First();

            var number = result.GetProperty("number").ToString();
            return $"A{number.ToString().PadLeft(6, '0')}: ";
        }
        catch
        {
            return "???????: ";
        }
    }
}