using System.Collections.Generic;

namespace ApiServer.Model.Model.Nodes
{
    public class UserRoleCheckedIds
    {
        public int UserId { get; set; }

        public List<int> CheckedIds { get; set; }
    }
}
