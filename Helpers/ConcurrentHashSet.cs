namespace MatrixAlg.Helpers
{
    internal class ConcurrentHashSet<T> : IDisposable
    {
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);
        private readonly HashSet<T> HashSet = [];

        public void Add(T item, Action successAction)
        {
            try
            {
                Lock.EnterWriteLock();
                if (HashSet.Add(item))
                {
                    successAction();
                }
            }
            finally
            {
                if (Lock.IsWriteLockHeld)
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public void Dispose()
        {
            HashSet.Clear();
            Lock?.Dispose();
        }
    }
}
