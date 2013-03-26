using System;
namespace ProUpgradeEditor.Common
{
    public interface ISpecializedMessageList
    {
        
        int GetIndexFromTick(int itemTick);
        GuitarMessageType MessageType { get; }
        string ToString();
    }
}
