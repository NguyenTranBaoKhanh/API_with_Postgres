using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace API_with_Postgres.Common.Configuration
{
    public sealed class BaseConfiguration
    {
        #region Private Static Members

        private static BaseConfiguration _instance;
        private static readonly object ConfigLock = new object();

        #endregion


        #region Private Members

        private readonly ConcurrentDictionary<string, int> IntConfig = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentDictionary<string, long> LongConfig = new ConcurrentDictionary<string, long>();
        private readonly ConcurrentDictionary<string, string> StrConfig = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, bool> BoolConfig = new ConcurrentDictionary<string, bool>();
        private readonly ConcurrentDictionary<string, double> DoubleConfig = new ConcurrentDictionary<string, double>();
        private readonly ConcurrentDictionary<string, decimal> DecimalConfig = new ConcurrentDictionary<string, decimal>();
        private readonly ConcurrentDictionary<string, float> FloatConfig = new ConcurrentDictionary<string, float>();
        private readonly ConcurrentDictionary<string, Guid> GuidConfig = new ConcurrentDictionary<string, Guid>();
        private IConfigurationRoot _configRoot;
        private const string AppSettingJson = "appsettings.json";
        private const string AppSettingNode = "ApplicationSettings";
        private bool _hasOverwrite = false;

        #endregion


        #region Constructor

        private BaseConfiguration()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), AppSettingJson);
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(path, true)
                .AddEnvironmentVariables();

            _configRoot = configurationBuilder.Build();
        }

        #endregion


        #region Public Static Methods

        public static BaseConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (ConfigLock)
                    {
                        _instance = new BaseConfiguration();
                    }
                }

                return _instance;
            }
        }

        #endregion


        #region Public Members

        public void OverwriteEnvConfig(IConfigurationRoot overwriteConfig)
        {
            if (_hasOverwrite) 
                return;

            _configRoot = overwriteConfig;
            _hasOverwrite = true;
        }

        public Guid ReadConfigureGuidValue(string config, string defaultValue = "")
        {
            if (GuidConfig.TryGetValue(config, out var guidValue))
            {
                return guidValue;
            }

            Guid tryVal;

            if (Guid.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) )
            {
                // Do nothing
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultValue) && Guid.TryParse(defaultValue, out tryVal))
                    return tryVal;

                tryVal = Guid.Empty;
            }

            lock (ConfigLock)
            {
                if (!GuidConfig.ContainsKey(config))
                {
                    GuidConfig.TryAdd(config, tryVal);
                }
            }

            return GuidConfig[config];
        }

        public int ReadConfigureIntValue(string config, int defaultValue = 0)
        {
            if (IntConfig.TryGetValue(config, out var intValue))
            {
                return intValue;
            }

            int tryVal = 0;

            if (int.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) && tryVal > 0)
            {
                // Do nothing
            }
            else
            {
                tryVal = defaultValue;
            }

            lock (ConfigLock)
            {
                if (!IntConfig.ContainsKey(config))
                {
                    IntConfig.TryAdd(config, tryVal);
                }
            }


            return IntConfig[config];
        }

        public long ReadConfigureLongValue(string config, long defaultValue = 0)
        {
            if (LongConfig.TryGetValue(config, out var longValue))
            {
                return longValue;
            }

            long tryVal = 0;

            if (long.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) && tryVal > 0)
            {
                // Do nothing
            }
            else
            {
                tryVal = defaultValue;
            }

            lock (ConfigLock)
            {
                if (!IntConfig.ContainsKey(config))
                {
                    LongConfig.TryAdd(config, tryVal);
                }
            }


            return LongConfig[config];
        }
        
        public decimal ReadConfigureDecimalValue(string config, decimal defaultValue = 0)
        {
            if (DecimalConfig.TryGetValue(config, out var decValue))
            {
                return decValue;
            }

            decimal tryVal = 0M;

            if (decimal.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) && tryVal > 0)
            {
                // Do nothing
            }
            else
            {
                tryVal = defaultValue;
            }

            lock (ConfigLock)
            {
                if (!DecimalConfig.ContainsKey(config))
                {
                    DecimalConfig.TryAdd(config, tryVal);
                }
            }

            return DecimalConfig[config];
        }

        public double ReadConfigureDoubleValue(string config, double defaultValue = 0)
        {
            if (DoubleConfig.TryGetValue(config, out var doubleValue))
            {
                return doubleValue;
            }

            double tryVal = 0;

            if (double.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) && tryVal > 0)
            {
                // Do nothing
            }
            else
            {
                tryVal = defaultValue;
            }

            lock (ConfigLock)
            {
                if (!DoubleConfig.ContainsKey(config))
                {
                    DoubleConfig.TryAdd(config, tryVal);
                }
            }

            return DoubleConfig[config];
        }

        public float ReadConfigureFloatValue(string config, float defaultValue = 0)
        {
            if (FloatConfig.TryGetValue(config, out var floatValue))
            {
                return floatValue;
            }

            float tryVal = 0;

            if (float.TryParse(_configRoot.GetSection(AppSettingNode)[config], out tryVal) && tryVal > 0)
            {
                // Do nothing
            }
            else
            {
                tryVal = defaultValue;
            }

            lock (ConfigLock)
            {
                if (!FloatConfig.ContainsKey(config))
                {
                    FloatConfig.TryAdd(config, tryVal);
                }
            }

            return FloatConfig[config];
        }

        public string ReadConfigureStringValue(string config, string defaultValue = "")
        {
            if (StrConfig.TryGetValue(config, out var valueString))
            {
                return valueString;
            }

            string strVal;

            if (!string.IsNullOrEmpty(_configRoot.GetSection(AppSettingNode)[config]))
            {
                strVal = _configRoot.GetSection(AppSettingNode)[config];
            }
            else
            {
                return defaultValue;
            }

            lock (ConfigLock)
            {
                if (!StrConfig.ContainsKey(config))
                {
                    StrConfig.TryAdd(config, strVal);
                }
            }


            return StrConfig[config];
        }

        public bool ReadConfigureBoolValue(string config, bool defaultValue = false)
        {
            if (BoolConfig.TryGetValue(config, out var boolValue))
            {
                return boolValue;
            }

            bool boolVal;

            if (!string.IsNullOrEmpty(_configRoot.GetSection(AppSettingNode)[config]) &&
                bool.TryParse(_configRoot.GetSection(AppSettingNode)[config], out boolVal))
            {
                // do nothing
            }
            else
            {
                return defaultValue;
            }

            lock (ConfigLock)
            {
                if (!BoolConfig.ContainsKey(config))
                {
                    BoolConfig.TryAdd(config, boolVal);
                }
            }

            return BoolConfig[config];
        }

        public string ReadConnectionStrValue(string config)
        {
            return _configRoot.GetConnectionString(config);
        }

        public IConfigurationSection GetSection(string sectionName)
        {
            return _configRoot.GetSection(sectionName);
        }

        #endregion
    }
}
