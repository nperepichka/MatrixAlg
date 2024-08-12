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

    public bool IsInitialized
    {
        get
        {
            return Y.HasValue;
        }
    }

    public void SetY(byte? y)
    {
        Y = y;
    }

    public Point Clone()
    {
        return new Point(X, Y, Z);
    }

    public void Print()
    {
        Console.WriteLine(Y.HasValue ? $"({X}, {Y.Value}, {Z})" : $"({X}, ?, {Z})");
    }
}
