using API_with_Postgres.Common.Enums;

namespace API_with_Postgres.Common.Contract
{
    public class MapItem
    {
        public Type Type { get; private set; }
        public DataRetrieveType DataRetrieveType { get; private set; }
        public string PropertyName { get; private set; }

        public MapItem(Type type, DataRetrieveType dataRetrieveType, string propertyName)
        {
            Type = type;
            DataRetrieveType = dataRetrieveType;
            PropertyName = propertyName;
        }
    }
}
