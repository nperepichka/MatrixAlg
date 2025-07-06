internal class Program
{
    /*private static void Main()
    {
        Console.Write("Sequence: ");
        var sequenceStr = Console.ReadLine()!;
        int[] sequence = [];

        try
        {
            sequence = sequenceStr
                .Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
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
        int[] sequence = [];

        try
        {
            sequence = sequenceStr
                .Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }
        catch { }

        if (sequence.Length <= 0)
        {
            Console.WriteLine("Invalid sequence.");
            Console.ReadLine();
            return;
        }

        sequenceStr = GetSequenceString(sequence);
        var dres = new List<int[]>();
        var adres = new List<int[]>();

        var t = sequence.ToArray();
        for (var i = 0; i < 10; i++)
        {
            t = CalculateDerivative(t);
            dres.Add(t);
        }

        t = sequence.ToArray();
        for (var i = 0; i < 10; i++)
        {
            t = CalculateAntiderivative(t);
            adres.Add(t);
        }

        Console.WriteLine();

        for (var i = 9; i >=0; i--)
        {
            Console.WriteLine(GetSequenceString(dres[i]));
        }
        Console.WriteLine(sequenceStr);
        for (var i = 0; i < 10; i++)
        {
            Console.WriteLine(GetSequenceString(adres[i]));
        }

        Console.WriteLine();
        Console.WriteLine("Done. Press <Enter> to exit...");
        Console.ReadLine();
    }

    private static string GetSequenceString(int[] sequence) =>
        string.Join(", ", sequence);

    private static int[] CalculateDerivative(int[] sequence)
    {
        var len = sequence.Length;
        var res = new int[len];

        for (var n = 0; n < len; n++)
        {
            var sum = 0;
            for (var i = 0; i <= n; i++)
            {
                var idx = n - i - 1;
                sum += sequence[i] * (idx >= 0 ? res[idx] : 1);
            }
            res[n] = sum;
        }

        return res;
    }

    private static int[] CalculateAntiderivative(int[] sequence)
    {
        var len = sequence.Length;
        var res = new int[len];

        for (var n = 0; n < len; n++)
        {
            var sum = 0;
            for (var i = 0; i <= n; i++)
            {
                var idx = n - i - 1;
                sum += -sequence[i] * (idx >= 0 ? res[idx] : -1);
            }
            res[n] = sum;
        }

        return res;
    }
}