using System.Linq;
using System.Collections;
using LynxMobileData;

namespace LynxWebApi.Controllers
{
    public class SiteTypesController : BaseController
    {  
        public class SiteTypes
        {
            public int SiteTypeId { get; set; }
            public string SiteTypeName { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class SiteCode
        {
            string Code { get; set; }
            string Name { get; set; }
        }

        public IEnumerable Get()
        {
            var dataTable = from sitetypes in entities.SiteTypes
                            join sitecodes in entities.SiteCodes
                                on sitetypes.SiteTypeID equals sitecodes.SiteTypeID into temp
                            from sitecodes in temp.DefaultIfEmpty()
                            select new SiteTypes
                            {
                                SiteTypeId = sitetypes.SiteTypeID,
                                SiteTypeName = sitetypes.Description,
                                Code = sitecodes.Code,
                                Name = sitecodes.Name
                            };

            var result =  from b in dataTable
                        group b by new { b.SiteTypeId,b.SiteTypeName} into g
                        select new 
                        {
                            SiteTypeId = g.Key.SiteTypeId,
                            SiteTypeName = g.Key.SiteTypeName,
                            Orders =
                                from item in g
                                select new { item.Name, item.Code }
                        };
           
            return result;

        }
    }
}
