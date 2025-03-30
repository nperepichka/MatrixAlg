namespace MatrixShared.Helpers;

public static class ConsoleInputReader
{
    public static byte ReadValue(string name = "Size", byte max = 250, bool allowZero = false)
    {
        byte size;
        var sizeString = string.Empty;
        while (!byte.TryParse(sizeString, out size) || size > max || size < (allowZero ? 0 : 1))
        {
            if (!string.IsNullOrEmpty(sizeString))
            {
                Console.WriteLine("Invalid input.");
            }
            Console.Write($"{name}: ");
            sizeString = Console.ReadLine();
        }
        Console.WriteLine();
        return size;
    }
}
