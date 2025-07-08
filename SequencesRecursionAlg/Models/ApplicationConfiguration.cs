using MatrixShared.Helpers;

namespace SequencesRecursionAlg.Models;

public static class ApplicationConfiguration
{
    public static readonly bool SearchOeis = ConfigurationHelper.GetFlag(nameof(SearchOeis), false);
    public static readonly int FindSequences = ConfigurationHelper.GetValue(nameof(FindSequences), 10);
    public static readonly int SequenceLength = ConfigurationHelper.GetValue(nameof(SequenceLength), 20);
}
