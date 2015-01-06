using System;

namespace Abp.Events.Bus
{
    /// <summary>
    /// Implements <see cref="IEventData"/> and provides a base for event data classes.
    /// </summary>
    [Serializable]
    public abstract class EventData : IEventData
    {
        /// <summary>
        /// The time when the event occured.
        /// </summary>
        public DateTime EventTimeUtc { get; set; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// </summary>
        public object EventSource { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected EventData()
        {
            this.EventTimeUtc = DateTime.UtcNow;
        }
    }
}