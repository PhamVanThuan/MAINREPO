using System;
using System.Collections.Generic;
using System.Text;
using X2DomainService.Interface.WTFSimpleTest;
using System.Security.Permissions;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Configuration;
using SAHL.X2.Common;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using SAHL.X2.Common.DataAccess;

namespace WTFSimpleTestClient
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WTFSimpleTest
    {
        static IWTFSimpleTest wtfCS = null;
        static WTFSimpleTest()
        {
            try
            {
                if (wtfCS == null)
                {
                    BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                    BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                    serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                    IDictionary props = new Hashtable();
                    props["port"] = 0;
                    string s = Guid.NewGuid().ToString();
                    props["name"] = s;
                    props["typeFilterLevel"] = TypeFilterLevel.Full;
                    TcpChannel chan = new TcpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(chan);

                    string IP = ConfigurationSettings.AppSettings["DomainServiceIP"];
                    string URL = string.Format("tcp://{0}:10000/WTFSimpleTest", IP);
                    wtfCS = (IWTFSimpleTest)Activator.GetObject(typeof(IWTFSimpleTest), URL);
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogError(ex.ToString());
            }
        }

        //public static bool Test()
        //{
        //    bool mc = wtfCS.Test();
        //    return mc;
        //}

        //public static IX2ReturnData GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out string ADUserName, int OfferRoleTypeKey, int ApplicationKey)
        //{
        //    IX2ReturnData ret = wtfCS.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out ADUserName, OfferRoleTypeKey, ApplicationKey);
        //    return ret;
        //}

        public static IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey)
        {
            IX2ReturnData mc = wtfCS.WTFRoundRobinAssignForGivenOrgStructure(Tran, out AssignedUser, DynamicRole, GenericKey, OrgStructureKey);
            return mc;
        }

        //public static IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, List<int> OrgStructureKeys)
        //{
        //    IX2ReturnData mc = wtfCS.RoundRobinAssignForGivenOrgStructure(Tran, out AssignedUser, DynamicRole, GenericKey, OrgStructureKeys);
        //    return mc;
        //}
    }
}
