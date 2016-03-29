using System;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RuleInfo : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class RuleAssemblyTag : System.Attribute
    {
        public RuleAssemblyTag()
        {
        }
    }

    /// <summary>
    /// Param Types
    /// Int32 == 9
    /// bool == 3
    /// DateTime = 5
    /// String = 12
    /// Double == 7
    /// Decimal == 6
    /// Int16 == 17
    /// Int64 == 1
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RuleParameterTag : Attribute
    {
        private List<RuleParamObj> _Params;

        public List<RuleParamObj> Params { get { return _Params; } }

        public RuleParameterTag(string[] param)
        {
            _Params = new List<RuleParamObj>();
            foreach (string s in param)
            {
                string[] ss = s.Split(',');
                RuleParamObj r = new RuleParamObj(ss[0], ss[1], Convert.ToInt32(ss[2]));
                _Params.Add(r);
            }
        }
    }

    public class RuleParamObj
    {
        public string _Name;

        public string Name { get { return _Name; } }

        public string _Value;

        public string Value { get { return _Value; } }

        public int _ParameterType;

        public int ParameterType { get { return _ParameterType; } }

        public RuleParamObj(string Name, string Value, int ParamType)
        {
            this._Name = Name;
            this._Value = Value;
            this._ParameterType = ParamType;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class RuleDBTag : Attribute
    {
        public string _Name;
        public string _Desc;
        public string _DLL;
        public string _ActualType;
        public bool _Enforce = true;

        public RuleDBTag(string Name, string Desc, string DLL, string Type)
        {
            this._Name = Name;
            this._Desc = Desc;
            this._DLL = DLL;
            this._ActualType = Type;
        }

        public RuleDBTag(string Name, string Desc, string DLL, string Type, bool Enforce)
        {
            this._Name = Name;
            this._Desc = Desc;
            this._DLL = DLL;
            this._ActualType = Type;
            this._Enforce = Enforce;
        }

        public string Name { get { return _Name; } }

        public string Desc { get { return _Desc; } }

        public string DLL { get { return _DLL; } }

        public string ActualType { get { return _ActualType; } }

        public bool Enforce { get { return _Enforce; } }
    }

    public interface IRuleService
    {
        /// <summary>
        /// Runs a set of rules for a given domain object.
        /// </summary>
        /// <param name="dmc"></param>
        /// <param name="rulesToRun">Complete list of rules to run</param>
        /// <param name="parameters"></param>
        /// <remarks>Internally, this will check the principal to see if there are any rules to be excluded - any excluded rules on the principal will NOT be run even if they are passed in here.</remarks>
        void ExecuteRuleSet(IDomainMessageCollection dmc, List<string> rulesToRun, params object[] parameters);

        /// <summary>
        /// Runs a named set of rules.
        /// </summary>
        /// <param name="dmc"></param>
        /// <param name="ruleSetNameToRun"></param>
        /// <param name="parameters"></param>
        void ExecuteRuleSet(IDomainMessageCollection dmc, string ruleSetNameToRun, params object[] parameters);

        /// <summary>
        /// Executes a single rule on a list of params
        /// </summary>
        /// <param name="dmc"><see href="IDomainMessageCollection">IDomainMessageCollection</see></param>
        /// <param name="ruleName">The name of the rule to run. Must match the class name that implements the rule</param>
        /// <param name="parameters">Params needed to execute the rule (SHOULD BE THE OBJECT YOU ARE VALIDATING)</param>
        int ExecuteRule(IDomainMessageCollection dmc, string ruleName, params object[] parameters);

        /// <summary>
        /// Gets/sets whether the rule service is actually running rules.  This is for
        /// use when testing, as the rules do not need to be run as part of every test
        /// assembly.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Tells the service to reload the exclusion set from the SAHLPrincipalCache.
        /// </summary>
        void ReloadExclusions();

		IBusinessRule CreateRule(Type type);
    }
}