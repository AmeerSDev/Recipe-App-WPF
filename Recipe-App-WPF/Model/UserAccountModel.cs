using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Model
{
    public class UserAccountModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public SecureString Password{ get; set; }
    }
}
