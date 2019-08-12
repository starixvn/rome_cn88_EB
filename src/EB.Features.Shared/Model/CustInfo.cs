namespace EB.Features.Shared.Model
{
    public class CustInfo
    {
        public CustInfo(
            int custId,
            string custName,
            byte roleId)
        {
            CustId = custId;
            CustName = custName;
            this.RoleId = roleId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte RoleId { get; }
    }
}
