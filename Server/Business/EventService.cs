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
                if (currentNumberOfEvents != 0)
                {
                    state.SetNumberOfEvents(currentNumberOfEvents);
                }
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


        /// <summary>
        /// Returns a DateTime object which is equal to the time of the first event.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTimeOfFirstEvent()
        {
            //return dataHelper.GetDateTimeOfFirstEvent();
            return DateTime.Now - new TimeSpan(4, 12, 31, 13);
        }

    }
}
