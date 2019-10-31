using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Main.Model
{
    public class InternalLoginModel
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public long TimeTick { get; set; }

        public string Language { get; set; }

        public bool IsHideTopMenu { get; set; }

        public string SystemLogin { get; set; }

        public string AfterLogInSite { get; set; }

        public string AfterLoginQuery { get; set; }

        public string BrowserSize { get; set; }
    }
}
