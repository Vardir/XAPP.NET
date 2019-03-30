﻿using System;
using System.Collections.Generic;

namespace MIDE.Application.Logging
{
    /// <summary>
    /// A logging service to register application events based on their logging levels
    /// </summary>
    public class Logger
    {
        private DateTime TimeNow => UseUtcTime ? DateTime.UtcNow : DateTime.Now;
        private LinkedList<ApplicationEvent> events;

        /// <summary>
        /// A flag to indicate whether to use UTC time for registering events
        /// </summary>
        public bool UseUtcTime { get; }
        /// <summary>
        /// A flag to indicate whether to skip fatal errors if <seealso cref="LoggingLevel.FATAL"/> not set to <see cref="Levels"/>
        /// </summary>
        public bool SkipFatalEvents { get; }
        /// <summary>
        /// A set of flags to filter out incoming events
        /// </summary>
        public LoggingLevel Levels { get; }
        /// <summary>
        /// Count of all events stored in collection
        /// </summary>
        public int EventsCount => events.Count;

        public event EventHandler<FatalEvent> FatalEventRegistered;

        public Logger (LoggingLevel levels, bool useUtcTime, bool skipFatals)
        {
            Levels = levels;
            UseUtcTime = useUtcTime;
            SkipFatalEvents = skipFatals;
            events = new LinkedList<ApplicationEvent>();
        }

        /// <summary>
        /// Adds the given event into collection
        /// </summary>
        /// <param name="applicationEvent"></param>
        public void PushEvent(ApplicationEvent applicationEvent)
        {
            if (applicationEvent == null)
            {
                PushArgumentError(nameof(applicationEvent), "Can't register empty event");
                return;
            }
            if (!Levels.Has(applicationEvent.Level))
                return;
            events.AddLast(applicationEvent);
        }
        /// <summary>
        /// Adds an informational event into collection
        /// </summary>
        /// <param name="message"></param>
        public void PushInfo(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                PushArgumentError(nameof(message), "Can't add info event with empty message");
                return;
            }
            if (!Levels.Has(LoggingLevel.INFO))
                return;
            events.AddLast(new InfoEvent(message, TimeNow));
        }
        /// <summary>
        /// Adds a debug event into collection based on context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public void PushDebug(object context, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                PushArgumentError(nameof(message), "Can't add debug event with empty message");
                return;
            }
            if (!Levels.Has(LoggingLevel.DEBUG))
                return;
            events.AddLast(new DebugEvent(context, message, TimeNow));
        }
        /// <summary>
        /// Adds a warning event into collection
        /// </summary>
        /// <param name="message"></param>
        public void PushWarning(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                PushArgumentError(nameof(message), "Can't add warning event with empty message");
                return;
            }
            if (!Levels.Has(LoggingLevel.WARN))
                return;
            events.AddLast(new WarnEvent(message));
        }
        /// <summary>
        /// Adds an error event with exception instance
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public void PushError(Exception exception, object context, string message = "")
        {
            if (exception == null)
            {
                PushArgumentError(nameof(exception), "Can't add error event without exception instance");
                return;
            }
            if (!Levels.Has(LoggingLevel.ERROR))
                return;
            events.AddLast(new ErrorEvent(exception, context, message, TimeNow));
        }
        /// <summary>
        /// Adds a fatal error event with description of problem occur
        /// </summary>
        /// <param name="message"></param>
        public void PushFatal(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                PushArgumentError(nameof(message), "Can't add fatal error event with empty message");
                return;
            }
            if (SkipFatalEvents && !Levels.Has(LoggingLevel.FATAL))
                return;
            FatalEvent fatalEvent = new FatalEvent(message, TimeNow);
            events.AddLast(fatalEvent);
            FatalEventRegistered?.Invoke(this, fatalEvent);
        }

        /// <summary>
        /// Returns all events based on the given levels
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationEvent> Pull(LoggingLevel levels = LoggingLevel.ALL)
        {
            foreach (ApplicationEvent appEvent in events)
            {
                if (levels.Has(appEvent.Level))
                    yield return appEvent;
            }
        }

        private void PushArgumentError(string argumentName, string message)
        {
            Exception exception = new ArgumentNullException(argumentName);
            PushError(exception, this, message);
        }
    }
}