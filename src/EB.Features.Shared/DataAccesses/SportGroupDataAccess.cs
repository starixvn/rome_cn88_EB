namespace EB.Features.Shared.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Shared.Model;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface SportGroupDataAccess
    {
        IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName, Func<int, string> getSportGroupDisplayNameEnglish);
    }

    public class SportGroupDataAccessImpl : SportGroupDataAccess
    {
        public IEnumerable<SportGroup> GetSportGroups(ICriteria criteria, Func<int, string> getSportGroupDisplayName, Func<int, string> getSportGroupDisplayNameEnglish)
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
                                displaySportIds: Converter.ToString2(reader["DisplaySport"]).Split(',').Select(int.Parse),
                                getSportGroupDisplayName: getSportGroupDisplayName,
                                getSportGroupDisplayNameEnglish: getSportGroupDisplayNameEnglish);

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
