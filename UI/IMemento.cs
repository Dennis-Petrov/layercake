using System;

namespace LayerCake.UI
{
    internal interface IMemento : IDisposable
    {
        void Restore();
    }
}
