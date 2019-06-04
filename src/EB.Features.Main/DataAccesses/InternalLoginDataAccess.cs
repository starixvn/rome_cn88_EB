namespace EB.Features.Main.DataAccesses
{
    using System;
    using EB.Features.Main.Criteria.InternalLogin;
    using Fanex.Data;

    public interface InternalLoginDataAccess
    {
        DateTime GetDBTime(GetDBTimeCriteria criteria);
    }

    public class InternalLoginDataAccessImpl : InternalLoginDataAccess
    {
        public DateTime GetDBTime(GetDBTimeCriteria criteria)
        {
            DateTime loginTime;

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria);
                var output = objectDb.GetOuputParams();

                loginTime = Fanex.Common.Converter.ToDateTime(output["logintime"]);
            }

            return loginTime;
        }
    }
}
