using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class KillData
    {
        public List<string> ClearedSpawners;
        public KillData()
        {
            ClearedSpawners = new List<string>();
        }
    }
}