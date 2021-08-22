using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class NonPickedUpLoot : SerializableDictionary<string, LootPieceData>
    {
    }
}