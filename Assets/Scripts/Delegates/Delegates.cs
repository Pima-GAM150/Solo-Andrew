using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EraseGame.Delegates
{
    public delegate void BarSpawn(HorizontalBar bar);

    public delegate void BlockEvent(BreakableBlock block);

    public delegate void ColorEvent(Color color);
}