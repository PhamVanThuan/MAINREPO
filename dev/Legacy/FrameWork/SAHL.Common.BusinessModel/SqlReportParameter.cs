using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.WebServices.ReportExecution2005;
using SAHL.Common.WebServices.ReportingServices2010;
using System;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel
{
    public class SqlReportParameter : ISqlReportParameter
    {
        private IReportStatement _reportStatement;
        private SAHL.Common.WebServices.ReportingServices2010.ItemParameter _rp;
        private IReportParameterType _rpt;
        private List<string> _defaultValues;
        private Dictionary<string, object> _validValues;
        private Int32? _parameterLength;
        private Int32 _key;
        private IDomainField _domainField;

        public SqlReportParameter(IReportStatement reportStatement, SAHL.Common.WebServices.ReportingServices2010.ItemParameter reportParameter)
        {
            _reportStatement = reportStatement;
            _rp = reportParameter;
            _parameterLength = 0;
            _key = -1;
            _domainField = null;
        }

        public Int32 Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DomainField
        /// </summary>
        public IDomainField DomainField
        {
            get
            {
                return _domainField;
            }

            set
            {
                _domainField = value;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterName
        /// </summary>
        public String ParameterName
        {
            get { return _rp.Name; }
            set { _rp.Name = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterLength
        /// </summary>
        public Int32? ParameterLength
        {
            get { return 0; }
            set { _parameterLength = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DisplayName
        /// </summary>
        public String DisplayName
        {
            get { return _rp.Prompt; }
            set { _rp.Prompt = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.Required
        /// </summary>
        public Boolean? Required
        {
            get
            {
                bool req = true;

                // There is a bug on sql report designer for datetime parameters.
                // The "Allow blank value" is set to true and the control is disabled so it cannot be changed.
                // A workaround on the report is to chage the data type, uncheck this option and then set back to a datetime.
                // The logic below is just to "bullet-proof" because we prob wont go back and change all the reports.
                switch (_rp.ParameterTypeName)
                {
                    case "DateTime":
                        req = !_rp.Nullable;
                        break;

                    default:
                        req = !(_rp.Nullable || _rp.AllowBlank);
                        break;
                }

                return req;
            }
            set
            {
                if (value.HasValue)
                {
                    _rp.AllowBlank = value.Value;
                    _rp.Nullable = value.Value;
                }
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportParameterType
        /// </summary>
        public IReportParameterType ReportParameterType
        {
            get
            {
                if (_rpt == null)
                {
                    ReportParameterType_DAO rpt;

                    if (_rp.ValidValues != null)
                    {
                        if (_rp.MultiValue == true)
                            rpt = ReportParameterType_DAO.Find((int)ReportParameterTypes.DropDownusingDescription); // checked list box
                        else
                            rpt = ReportParameterType_DAO.Find((int)ReportParameterTypes.DropDown);
                    }
                    else
                    {
                        int paramTypeKey = -1;
                        switch (_rp.ParameterTypeName)
                        {
                            case "Boolean"://0: // Boolean
                                {
                                    paramTypeKey = 1; // boolean
                                    break;
                                }
                            case "DateTime": //1: // DateTime
                                {
                                    paramTypeKey = 2; // DateTime
                                    break;
                                }
                            case "Integer": //2: // Integer
                                {
                                    if (_rp.MultiValue)
                                        paramTypeKey = 8; // Multi Integer Drop Down
                                    else
                                        paramTypeKey = 4; // Integer
                                    break;
                                }
                            case "Float"://3: // Float
                                {
                                    paramTypeKey = 3;
                                    break;
                                }
                            case "String"://4: // String
                                {
                                    if (_rp.MultiValue)
                                        paramTypeKey = 7; // Multi String Drop Down
                                    else
                                        paramTypeKey = 5; // String
                                    break;
                                }
                            default:
                                {
                                    paramTypeKey = 6; // Drop Down
                                    break;
                                }
                        }

                        rpt = ReportParameterType_DAO.Find(paramTypeKey);
                    }

                    _rpt = new ReportParameterType(rpt);
                }

                return _rpt;
            }

            set { _rpt = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportStatement
        /// </summary>
        public IReportStatement ReportStatement
        {
            get
            {
                return _reportStatement;
            }
            set
            {
                _reportStatement = value;
            }
        }

        public Dictionary<string, object> ValidValues
        {
            get
            {
                if (_validValues == null)
                {
                    _validValues = new Dictionary<string, object>();

                    if (_rp.ValidValues != null)
                    {
                        foreach (SAHL.Common.WebServices.ReportingServices2010.ValidValue vv in _rp.ValidValues)
                        {
                            // only add the valid value to the dictionary is it doesnt already exist.
                            if (!_validValues.ContainsKey(vv.Value))
                                _validValues.Add(vv.Value, vv.Label);
                        }
                    }
                }

                return _validValues;
            }

            //set { _validValues = value; }
        }

        public bool IsInternalParameter
        {
            get
            {
                return !_rp.PromptUser;
            }
        }

        public List<string> DefaultValues
        {
            get
            {
                if (_defaultValues == null)
                {
                    _defaultValues = new List<string>();

                    if (_rp.DefaultValues != null)
                    {
                        _defaultValues.AddRange(_rp.DefaultValues);
                    }
                }

                return _defaultValues;
            }
        }
    }
}