using log4net;

namespace API_with_Postgres.Common.Logging
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
