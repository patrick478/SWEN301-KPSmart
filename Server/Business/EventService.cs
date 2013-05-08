using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    /// <summary>
    /// This Service only allows you to see how many events have been processed at certain times.
    /// </summary>
    public class EventService
    {
        private CurrentState state;
        private EventDataHelper dataHelper;
        
        public EventService(CurrentState state)
        {
            dataHelper = new EventDataHelper();
            this.state = state;

            if (!state.EventsInitialised())
            {
                int currentNumberOfEvents = dataHelper.GetNumberOfEvents();
                state.SetNumberOfEvents(currentNumberOfEvents);
            }
        }

        /// <summary>
        /// Returns the number of events that have been processed.
        /// </summary>
        /// <returns></returns>
        public int GetTotalNumberOfEvents()
        {
            return state.NumberOfEvents;
        }

        /// <summary>
        /// Returns the number of events that were processed before the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetTotalNumberOfEvents(DateTime time)
        {
            return dataHelper.GetNumberOfEvents(time);
        }

    }
}
