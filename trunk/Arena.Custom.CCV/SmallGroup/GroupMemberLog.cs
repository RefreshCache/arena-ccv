using System;
using System.Collections.Generic;
using System.Text;

using Arena.Core;
using Arena.SmallGroup;

namespace Arena.Custom.CCV.SmallGroup
{
    public class GroupMemberLog : GroupMember
    {
        public GroupMemberLog()
            : base()
        {
        }

        public GroupMemberLog(GroupMember groupMember)
        {
            base.PersonID = groupMember.PersonID;
            base.GroupID = groupMember.GroupID;
            base.Role = groupMember.Role;
        }

        public void SaveGroupMemberLog(Group group)
        {
            new Arena.Custom.CCV.DataLayer.SmallGroup.GroupData().SaveGroupMemberLog(
                this.PersonID,
                group.GroupCluster.ClusterTypeID,
                group.GroupID,
                group.Title,
                group.LeaderID,
                this.Role.LookupID);
        }
    }
}
