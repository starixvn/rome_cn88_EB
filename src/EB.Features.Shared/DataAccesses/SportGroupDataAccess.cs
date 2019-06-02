using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Shared.Model;
using Fanex.Common;
using Fanex.Data;
using Fanex.Data.Repository;

namespace EB.Features.Shared.DataAccesses
{
    public interface SportGroupDataAccess
    {
        IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName);
    }

    public class SportGroupDataAccessImpl : SportGroupDataAccess
    {
        public IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName)
        {
            var groups = new List<SportGroup>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var group = new SportGroup(
                                id: Converter.ToInt(reader["SportTypeGroup"]),
                                productId: Converter.ToInt(reader["ProductID"]),
                                displaySportIds: Converter.ToString2(reader["DisplaySport"]).Split(',').Select(int.Parse),                                
                                getSportGroupDisplayName: getSportGroupDisplayName);

                            groups.Add(group);
                        }

                        reader.Close();
                    }
                }
            }

            return groups;
        }
    }
}
