using System;
using System.Collections.Generic;
using System.Text;

namespace Apteryx.StackExChange.Redis.Extend
{
    public class RemoveResult
    {
        public RemoveResult(long deletedCount) { this.DeletedCount = deletedCount; }
        public long DeletedCount { get; }
    }
}
