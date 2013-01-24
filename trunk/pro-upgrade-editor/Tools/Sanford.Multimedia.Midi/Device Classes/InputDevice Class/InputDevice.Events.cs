using System;
using System.Threading;

namespace Sanford.Multimedia.Midi
{
    public partial class InputDevice
    {
        public event EventHandler<ChannelMessageEventArgs> ChannelMessageReceived;

        public event EventHandler<SysExMessageEventArgs> SysExMessageReceived;

        public event EventHandler<SysCommonMessageEventArgs> SysCommonMessageReceived;

        public event EventHandler<SysRealtimeMessageEventArgs> SysRealtimeMessageReceived;

        public event EventHandler<InvalidShortMessageEventArgs> InvalidShortMessageReceived;

        public event EventHandler<InvalidSysExMessageEventArgs> InvalidSysExMessageReceived;

        protected virtual void OnChannelMessageReceived(ChannelMessageEventArgs e)
        {
            EventHandler<ChannelMessageEventArgs> handler = ChannelMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysExMessageReceived(SysExMessageEventArgs e)
        {
            EventHandler<SysExMessageEventArgs> handler = SysExMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysCommonMessageReceived(SysCommonMessageEventArgs e)
        {
            EventHandler<SysCommonMessageEventArgs> handler = SysCommonMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysRealtimeMessageReceived(SysRealtimeMessageEventArgs e)
        {
            EventHandler<SysRealtimeMessageEventArgs> handler = SysRealtimeMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnInvalidShortMessageReceived(InvalidShortMessageEventArgs e)
        {
            EventHandler<InvalidShortMessageEventArgs> handler = InvalidShortMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnInvalidSysExMessageReceived(InvalidSysExMessageEventArgs e)
        {
            EventHandler<InvalidSysExMessageEventArgs> handler = InvalidSysExMessageReceived;
            if (context == null)
                context = SynchronizationContext.Current;
            if (handler != null && context != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }
    }
}
