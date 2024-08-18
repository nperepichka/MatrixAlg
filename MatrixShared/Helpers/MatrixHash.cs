namespace MatrixShared.Helpers;

public static class MatrixHash
{
    public static string GetHash(this bool[,] matrix)
    {
        var flatArray = new char[matrix.Length];

        var i = 0;
        foreach (var element in matrix)
        {
            flatArray[i] = element ? '1' : '0';
            i++;
        }

        return new string(flatArray);
    }
}
