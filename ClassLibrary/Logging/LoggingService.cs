using log4net;

namespace ClassLibrary.Logging
{
    public class LoggingService
    {
        #region Public Properties

        protected ILog GetLogger { get; }

        #endregion


        #region Constructor

        public LoggingService(Type concreteType)
        {
            GetLogger = LogManager.GetLogger(concreteType);
        }

        #endregion
    }
}
