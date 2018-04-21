using System;

namespace Xan.StellarisModTools.Release
{
    public class ReleaseCreatorEventArgs : EventArgs
    {
        public ReleaseCreatorEventArgs(string Message)
        {
            this.Message = Message;
        }

        public string Message
        {
            get;
            set;
        }
    }
}
