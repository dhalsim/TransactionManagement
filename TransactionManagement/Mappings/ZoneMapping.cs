using FluentNHibernate.Mapping;
using TransactionManagement.Entities;

namespace TransactionManagement.Mappings
{
    public class ZoneMapping : ClassMap<Zone>
    {
        public ZoneMapping()
        {
            Id(x => x.ZoneId);
            Map(x => x.CityId);
            Table("Zone");
        }
    }
}