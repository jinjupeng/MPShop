using ApiServer.Model.Model.DataTree;
using ApiServer.Model.Model.ViewModel;
using System.Collections.Generic;

namespace ApiServer.Model.Model.Nodes
{
    public class SysOrgNode : SysOrg, IDataTree<SysOrgNode, long>
    {
        public List<SysOrgNode> Children { get; set; }
        public long GetId()
        {
            return id;
        }

        public long GetParentId()
        {
            return orgPid;
        }


    }
}
