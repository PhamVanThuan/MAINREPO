using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Managers.Security
{
    public class SecurityDataManager : ISecurityDataManager
    {
        private IDbFactory dbFactory;
        public SecurityDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesBranchIdExist(Guid id)
        {
            bool result = false;
            DoesBranchIdExistQuery query = new DoesBranchIdExistQuery(id);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var branch = db.SelectOne<BranchDataModel>(query);
                if (branch != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public void AddBranch(string branchName, bool isActive, Guid suburbId, string branchCode)
        {
            BranchDataModel branch = new BranchDataModel(CombGuid.Instance.Generate(), branchName, suburbId,isActive, branchCode);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<BranchDataModel>(branch);
                db.Complete();
            }
        }

        public void ChangeBranchDetails(Guid id, string branchName, bool isActive, Guid suburbId)
        {
            ChangeBranchDetailsQuery query = new ChangeBranchDetailsQuery(id, branchName, isActive, suburbId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<BranchDataModel>(query);
                db.Complete();
            }
        }

        public void LinkUserToBranch(Guid userId, Guid branchId)
        {
            UserBranchDataModel userBranchQuery = new UserBranchDataModel(CombGuid.Instance.Generate(), userId, branchId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<UserBranchDataModel>(userBranchQuery);
                db.Complete();
            }
        }

        public void UpdateUserToBranchLink(Guid userId, Guid branchId)
        {
            UpdateUserToBranchLinkQuery userBranchQuery = new UpdateUserToBranchLinkQuery(userId, branchId);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<UserBranchDataModel>(userBranchQuery);
                db.Complete();
            }
        }

        public bool HasUsersBranchChanged(Guid userId, Guid branchId)
        {
            bool result = true;
            HasUsersBranchChangedQuery query = new HasUsersBranchChangedQuery(userId,branchId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var branch = db.SelectOne<UserBranchDataModel>(query);
                if (branch != null)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool DoesUserBelongToAnyBranches(Guid userId)
        {
            bool result = false;
            DoesUserBelongToAnyBranchesQuery query = new DoesUserBelongToAnyBranchesQuery(userId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var branch = db.SelectOne<UserBranchDataModel>(query);
                if (branch != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public RoleDataModel GetRoleByName(string roleName)
        {           
            GetRoleByName query = new GetRoleByName(roleName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<RoleDataModel>(query);               
            }           
        }

        public bool DoesUserBelongToBranch(Guid userId, Guid branchId)
        {
            var query = new DoesUserBelongToBranchQuery(userId, branchId);

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(query) != null;
            }
        }

        public BranchDataModel GetBranchByBranchCode(string branchCode)
        {
            var query = new GetBranchByBranchCodeQuery(branchCode);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<BranchDataModel>(query);
            }
        }

        public BranchDataModel GetBranchForUser(Guid userId)
        {
            var query = new GetBranchForUserQuery(userId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<BranchDataModel>(query);
            }
        }
    }
}