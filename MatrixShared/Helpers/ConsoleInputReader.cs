namespace MatrixShared.Helpers;

public static class ConsoleInputReader
{
    public static byte ReadSize()
    {
        byte size;
        var sizeString = string.Empty;
        while (!byte.TryParse(sizeString, out size) || size > 250 || size < 1)
        {
            if (!string.IsNullOrEmpty(sizeString))
            {
                Console.WriteLine("Invalid input.");
            }
            Console.Write("Size: ");
            sizeString = Console.ReadLine();
        }
        Console.WriteLine();
        return size;
    }
}
