using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Main.Params
{
    public class ChangePasswordParam
    {
        public int UserId { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmedNewPassword { get; set; }

        public string OldPassword { get; set; }
    }
}
