using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.UserProfiles;

//using SAHL.Common.BusinessModel.Interfaces.Repositories;
//using SAHL.X2.Framework.Interfaces;
//using SAHL.Common.BusinessModel.Interfaces.UI;
//using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.CacheData
{
	/// <summary>
	/// Class allowing us to cache information about the current IPrincipal.
	/// </summary>
	public class SAHLPrincipalCache : SAHL.Common.CacheData.ISAHLPrincipalCache
	{
		/// <summary>
		/// The name of the cache manager used for storing items in the SAHLPrincipalCache.
		/// </summary>
		public const string CacheManagerName = "SAHLPrincipalStore";

		private SAHLPrincipal _principal;
		private PresenterData _presenterData;
		private GlobalData _globalData;
		private UserProfile _userProfile;
		private IDomainMessageCollection _messages;
		private bool _ignoreWarnings;
		private bool? _isAuthenticated = new bool?();
		private object _userToken;
		private int _menuVersion;
		private List<String> _lstRoles;
		private List<int> _featureKeys;
		private List<int> _userOriginationSourceKeys;
		private string _originationSourceKeysStringForQuery;
		private List<RuleExclusionSets> _exclusionSets;
		private CBONodeSetType _currentNodeSetType = CBONodeSetType.CBO;
		private Dictionary<CBONodeSetType, object> _nodeSets;
		private Dictionary<int, object> _dictCboMenus;
		private Dictionary<int, object> _dictContextMenus;
		private object _X2Info;
		private object _X2Provider;
		private static object lockObj = new object();

		/// <summary>
		/// The cache manager doesn't provide us with a list of all keys, so we use this
		/// to keep track of them.
		/// </summary>
		private static List<string> _allUsers = new List<string>();

		public SAHLPrincipalCache()
		{
		}

		private SAHLPrincipalCache(SAHLPrincipal principal)
		{
			_principal = principal;
		}

		public SAHLPrincipal Principal
		{
			get { return _principal; }
		}

		/// <summary>
		/// The list of CBO Menus that the user has access to.
		/// </summary>
		public Dictionary<int, object> CBOMenus
		{
			get
			{
				if (_dictCboMenus == null)
					_dictCboMenus = new Dictionary<int, object>();
				return _dictCboMenus;
			}
		}

		/// <summary>
		/// The list of Context Menus that the user has access to.
		/// </summary>
		public Dictionary<int, object> ContextMenus
		{
			get
			{
				if (_dictContextMenus == null)
					_dictContextMenus = new Dictionary<int, object>();
				return _dictContextMenus;
			}
		}

		/// <summary>
		/// Gets/sets the domain message collection attached to the principal.
		/// </summary>
		public IDomainMessageCollection DomainMessages
		{
			get
			{
				if (null == _messages)
					_messages = new DomainMessageCollection();
				return _messages;
			}
			set
			{
				_messages = value;
			}
		}

		public void SetMessageCollection(IDomainMessageCollection domainMessages)
		{
			this._messages = domainMessages;
		}

		/// <summary>
		/// Gets/sets the list of AD Roles to which the user belongs.
		/// </summary>
		public List<string> Roles
		{
			get
			{
				if (_lstRoles == null)
				{
					_lstRoles = new List<string>();

					DBHelper dbHelper = new DBHelper(Databases.TwoAM);
					string sql = "SELECT DISTINCT ADUserGroup from [2AM].dbo.FeatureGroup (nolock)";

					using (IDataReader rdr = dbHelper.ExecuteReader(sql))
					{
						while (rdr.Read())
						{
							string role = rdr.GetString(0);

							if (_principal.IsInRole(role))
							{
								_lstRoles.Add(role);
							}
						}
					}

					//ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();

					//foreach (string role in lookUps.ADUserGroups)
					//{
					//    if (_principal.IsInRole(role))
					//    {
					//        _lstRoles.Add(role);
					//    }
					//}
				}
				return _lstRoles;
			}
		}

		/// <summary>
		/// Gets a list of all feature keys the user has access to.  This is dependent on the roles existing.
		/// </summary>
		public List<int> FeatureKeys
		{
			get
			{
				if (_featureKeys == null)
				{
					_featureKeys = new List<int>();

					DBHelper dbHelper = new DBHelper(Databases.TwoAM);
					string sql = "SELECT FeatureKey, ADUserGroup from [2AM].dbo.FeatureGroup (nolock)";

					using (IDataReader rdr = dbHelper.ExecuteReader(sql))
					{
						while (rdr.Read())
						{
							int key = rdr.GetInt32(0);
							string role = rdr.GetString(1);

							if (Roles.Contains(role) && !_featureKeys.Contains(key))
								_featureKeys.Add(key);
						}
					}

					//ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
					//foreach (IFeatureGroup featureGroup in lookups.FeatureGroups)
					//{
					//    if (Roles.Contains(featureGroup.ADUserGroup))
					//    {
					//        // allowedFeatureGroups.Add(Lookups.FeatureGroups[i]);
					//        IFeature feature = featureGroup.Feature;
					//        if (feature != null && !_featureKeys.Contains(feature.Key))
					//            _featureKeys.Add(feature.Key);
					//    }
					//}
				}
				return _featureKeys;
			}
		}

		/// <summary>
		/// Gets a list of origination sources to which the principal belongs.
		/// </summary>
		public List<int> UserOriginationSourceKeys
		{
			get
			{
				if (_userOriginationSourceKeys == null)
				{
					//IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
					//_userOriginationSourceKeys = orgStructRepo.GetOrgStructureOriginationSourcesPerADUser(_principal.Identity.Name);

					_userOriginationSourceKeys = new List<int>();

					string s1 = @"with OSTopLevels (OrganisationStructureKey, ParentKey) as (
	                                select OS.OrganisationStructureKey, OS.ParentKey
	                                from [2AM].dbo.OrganisationStructure OS (nolock)
	                                inner join [2AM].dbo.UserOrganisationStructure UOS (nolock) on OS.OrganisationStructureKey = UOS.OrganisationStructureKey
	                                inner join [2AM].dbo.ADUser A (nolock) on A.ADUserKey = UOS.ADUserKey
	                                where A.ADUserName =";
					string s2 = @"
                                    UNION ALL
	                                select OS.OrganisationStructureKey, OS.ParentKey
	                                from [2AM].dbo.OrganisationStructure OS (nolock)
	                                join OSTopLevels on OS.OrganisationStructureKey = OSTopLevels.ParentKey
                                )
                                select distinct OSOS.OriginationSourceKey from OSTopLevels
                                inner join [2AM].dbo.OrganisationStructureOriginationSource OSOS (nolock) on OSOS.OrganisationStructureKey = OSTopLevels.OrganisationStructureKey";

					string sql = String.Format("{0} '{1}' {2}", s1, _principal.Identity.Name, s2);

					DBHelper dbHelper = new DBHelper(Databases.TwoAM);

					using (IDataReader rdr = dbHelper.ExecuteReader(sql))
					{
						while (rdr.Read())
						{
							int key = rdr.GetInt32(0);

							if (!_userOriginationSourceKeys.Contains(key))
								_userOriginationSourceKeys.Add(key);
						}
					}
				}
				return _userOriginationSourceKeys;
			}
		}

		public string OriginationSourceKeysStringForQuery
		{
			get
			{
				if (_originationSourceKeysStringForQuery == null)
				{
					List<string> originationSourceKeys = new List<string>();

					for (int i = 0; i < UserOriginationSourceKeys.Count; i++)
						originationSourceKeys.Add(Convert.ToString(UserOriginationSourceKeys[i]));

					if (originationSourceKeys.Count > 0)
					{
						string keys = String.Join(",", originationSourceKeys.ToArray());
						_originationSourceKeysStringForQuery = keys;
					}
				}

				return _originationSourceKeysStringForQuery;
			}
		}

		/// <summary>
		/// User token that helps us keep track of a user only working in one browser window.
		/// This token stored is a serialized object and is not for general use.
		/// </summary>
		public object UserToken
		{
			get
			{
				return _userToken;
			}
			set
			{
				_userToken = value;
			}
		}

		/// <summary>
		/// Defines which CBONodeSet is selected
		/// </summary>
		public CBONodeSetType CurrentNodeSetType
		{
			get { return _currentNodeSetType; }
			set { _currentNodeSetType = value; }
		}

		/// <summary>
		/// Gets a list of nodesets for the principal.
		/// </summary>
		public Dictionary<CBONodeSetType, object> NodeSets
		{
			get
			{
				if (_nodeSets == null)
				{
					_nodeSets = new Dictionary<CBONodeSetType, object>();
					//_nodeSets.Add(CBONodeSetType.CBO, new CBONodeSet(CBONodeSetType.CBO));
					//_nodeSets.Add(CBONodeSetType.X2, new CBONodeSet(CBONodeSetType.X2));
				}
				return _nodeSets;
			}
		}

		/// <summary>
		///
		/// </summary>
		public int MenuVersion
		{
			get
			{
				return _menuVersion;
			}
			set
			{
				_menuVersion = value;
			}
		}

		/// <summary>
		/// X2Provider inforomation stored against the principal.
		/// </summary>
		public object X2Provider
		{
			get
			{
				return _X2Provider;
			}
			set
			{
				_X2Provider = value;
			}
		}

		/// <summary>
		/// X2 information stored against the principal.
		/// </summary>
		public object X2Info
		{
			get
			{
				return _X2Info;
			}
			set
			{
				_X2Info = value;
			}
		}

		/// <summary>
		/// Gets the data stored for the current presenter.  This will clear out as you move between presenters.
		/// </summary>
		/// <returns></returns>
		public PresenterData GetPresenterData()
		{
			if (_presenterData == null)
				_presenterData = new PresenterData();
			return _presenterData;
		}

		/// <summary>
		/// Gets the global cached collection.
		/// </summary>
		/// <returns></returns>
		public GlobalData GetGlobalData()
		{
			if (_globalData == null)
				_globalData = new GlobalData();
			return _globalData;
		}

		/// <summary>
		/// Holds a list of settings for the principal.
		/// </summary>
		public UserProfile Profile
		{
			get
			{
				if (_userProfile == null)
					_userProfile = new UserProfile();
				return _userProfile;
			}
		}

		/// <summary>
		/// Used to determine if the user is allowed to use SAHL applications by ensuring
		/// they are a member of the organisation structure.
		/// </summary>
		public bool IsAuthenticated
		{
			get
			{
				if (!_isAuthenticated.HasValue)
				{
					string sql = String.Format("select count(*) from [2AM].dbo.UserOrganisationStructure uos (nolock) join [2AM].dbo.ADUser a (nolock) on a.ADUserKey = uos.ADUserKey where a.ADUsername = '{0}'", _principal.Identity.Name);
					DBHelper dbHelper = new DBHelper(Databases.TwoAM);

					object obj = dbHelper.ExecuteScalar(sql);

					if (obj != null && (int)obj > 0)
						_isAuthenticated = new bool?(true);
					else
						_isAuthenticated = new bool?(false);

					//ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
					//IADUser adUser = secRepo.GetADUserByPrincipal(_principal.Identity.Name);
					//if (adUser != null && adUser.UserOrganisationStructure != null && adUser.UserOrganisationStructure.Count > 0)
					//    _isAuthenticated = new bool?(true);
					//else
					//    _isAuthenticated = new bool?(false);
				}
				return _isAuthenticated.Value;
			}
		}

		/// <summary>
		/// Gets/sets whether the user has elected to ignore warnings.
		/// </summary>
		public bool IgnoreWarnings
		{
			get
			{
				return _ignoreWarnings;
			}
			set
			{
				_ignoreWarnings = value;
			}
		}

		/// <summary>
		/// Gets a list of exclusion sets currently applied to the user.
		/// </summary>
		public List<RuleExclusionSets> ExclusionSets
		{
			get
			{
				if (_exclusionSets == null)
					_exclusionSets = new List<RuleExclusionSets>();
				return _exclusionSets;
			}
		}

		/// <summary>
		/// Gets a reference to the cache for a principal using the authenticated user.
		/// If the cache does not exist, it will be created.
		/// </summary>
		/// <param name="principal">The windows principal.</param>
		/// <returns>The cache object.</returns>
		public static SAHLPrincipalCache GetPrincipalCache(SAHLPrincipal principal)
		{
			CacheManager principalStore = CacheFactory.GetCacheManager(SAHLPrincipalCache.CacheManagerName);
			string key = principal.Identity.Name.ToLower();
			lock (lockObj)
			{
				if (!principalStore.Contains(key))
				{
					SAHLPrincipalCache principalCache = new SAHLPrincipalCache(principal);
					principalStore.Add(key, principalCache);
					_allUsers.Add(key);
				}
			}

			// if the principal exists in the cache then we can retrieve the SAHLPrincipalCache object
			return principalStore[key] as SAHLPrincipalCache;
		}

		/// <summary>
		/// Clears ALL principal users from the cache.
		/// </summary>
		public static void RemoveAll()
		{
			while (_allUsers.Count > 0)
				RemovePrincipalCache(_allUsers[0]);
		}

		/// <summary>
		/// Removes a user's cache.
		/// </summary>
		/// <param name="adUserName"></param>
		/// <returns>True if the cache existed and has been cleared, otherwise false.</returns>
		public static bool RemovePrincipalCache(string adUserName)
		{
			CacheManager principalStore = CacheFactory.GetCacheManager(SAHLPrincipalCache.CacheManagerName);
			string key = adUserName.ToLower();
			if (principalStore.Contains(key))
			{
				principalStore.Remove(key);
				_allUsers.Remove(key);
				return true;
			}
			return false;
		}

		/// <summary>
		/// This will form a comma-delimited list of the cached roles for purposes of using in queries
		/// </summary>
		/// <returns></returns>
		public string GetCachedRolesAsStringForQuery(bool IncludeIdentityName, bool IncludeEveryone, bool IncludeQuotes = true)
		{
			string _rolesStringForQuery = string.Empty;
			List<string> roles = new List<string>();

			if (IncludeIdentityName && !String.IsNullOrEmpty(this._principal.Identity.Name))
				roles.Add(IncludeQuotes == true ? String.Format("'{0}'", this._principal.Identity.Name) : this._principal.Identity.Name);

			if (IncludeEveryone)
				roles.Add(IncludeQuotes == true ? "'Everyone'" : "Everyone");

			List<string> temp = new List<string>();

			for (int i = 0; i < this.Roles.Count; i++)
			{
				string r = this.Roles[i].Replace("'", "''").Replace(",", "");

				if (!String.IsNullOrEmpty(r))
					temp.Add(IncludeQuotes == true ? String.Format("'{0}'", r) : r);
			}

			if (temp.Count > 0)
				_rolesStringForQuery = String.Join(",", temp.ToArray());

			if (!String.IsNullOrEmpty(_rolesStringForQuery))
				roles.Add(_rolesStringForQuery);

			if (roles.Count > 0)
			{
				string groups = String.Join(",", roles.ToArray());
				return groups;
			}

			return null;
		}
	}
}