using System;
using System.Collections.Generic;
using System.Text;
using X2DomainService.Interface.X2EngineTest;
using System.Security.Permissions;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;


namespace X2EngineTestClient
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class X2EngineTest
    {
        static IX2EngineTest x2eCS = null;
        static X2EngineTest()
        {
            try
            {
                if (x2eCS == null)
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
                    string URL = string.Format("tcp://{0}:10000/X2EngineTest", IP);
                    x2eCS = (IX2EngineTest)Activator.GetObject(typeof(IX2EngineTest), URL);
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogError(ex.ToString());
            }
        }

        public static IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey)
        {
            IX2ReturnData mc = x2eCS.WTFRoundRobinAssignForGivenOrgStructure(Tran, out AssignedUser, DynamicRole, GenericKey, OrgStructureKey);
            return mc;
        }
    }
}
