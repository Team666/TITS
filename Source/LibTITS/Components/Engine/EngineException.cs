using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TITS.Components.Engine
{
    /// <summary>
    /// Represents an error that occurs within the ZPlay library.
    /// </summary>
    [Serializable]
    public class EngineException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class.
        /// </summary>
        public EngineException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class with the specified error message.
        /// </summary>        
        /// <param name="message">The message that describes the error. See <see cref="ZPlay.GetError"/>.</param>
        public EngineException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class with the specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>        
        /// <param name="message">The message that describes the error. See <see cref="ZPlay.GetError"/>.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.
        /// </param>
        public EngineException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected EngineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
