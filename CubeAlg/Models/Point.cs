namespace CubeAlg.Models;

internal class Point
{
    public Point(byte x, byte z)
    {
        X = x;
        Z = z;
        Y = null;
    }

    public Point(byte x, byte? y, byte z)
    {
        X = x;
        Z = z;
        Y = y;
    }

    public byte X { get; init; }
    public byte? Y { get; private set; }
    public byte Z { get; init; }

    public bool SetY(byte? y)
    {
        if (Y.HasValue != y.HasValue)
        {
            Y = y;
            return true;
        }

        return false;
    }

    public Point Clone()
    {
        return new Point(X, Y, Z);
    }

    public void Print()
    {
        Console.WriteLine($"({X}, {Y}, {Z})");
    }
}
