using VmmSDK.Client;
using VmmSDK.Api;
using VmmSDK.Model.Vmm.V4.Ahv.Config;
using NLog;

namespace Nutanix.Example
{
    class MyApp
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static string vmExtId = "ReplaceWithVmUUIDToGet";
        private static Configuration _config = new Configuration()
        {
            Username = "admin",
            Password = "ReplaceWithAdminPassword",
            Host = "ReplaceWithPCHostName",
            Port = 9440,
            Debugging = true,
            VerifySsl = false,
            MaxRetryAttempts = 3000,
            Timeout = 30000
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            testPowerOnVm();
            testGetVm();
            testVMCreate();
            testVMList();

            Console.WriteLine("Done");
        }

        private static void testPowerOnVm()
        {
            ApiClient client = new ApiClient(_config);
            VmApi vmApi = new VmApi(client);
            GetVmResponse getVmResponse = vmApi.GetVmByExtId(vmExtId);
            string eTagHeader = ApiClient.GetEtag(getVmResponse);
            Dictionary<string, object> opts = new Dictionary<string, object>();
            opts["If-Match"] = eTagHeader;
            PowerOnVmResponse resp = vmApi.PowerOnVm(vmExtId, opts);

        }

        private static void testGetVm()
        {
            ApiClient client = new ApiClient(_config);
            VmApi vmApi = new VmApi(client);
            GetVmResponse vm = vmApi.GetVmByExtId(vmExtId);
            Vm vmData = (VmmSDK.Model.Vmm.V4.Ahv.Config.Vm)vm.Data;
            Console.WriteLine("vmName:"+vmData.Name);

        }

        private static void testVMList()
        {

            ApiClient client = new ApiClient(_config);
            VmApi vmApi = new VmApi(client);
            try
            {
                ListVmsResponse result = vmApi.ListVms();
                Console.WriteLine(result.ToJson());
                List<Vm> data = (List<Vm>)result.Data;

                foreach (var d in data)
                {
                    Console.WriteLine(d.Name);
                    foreach (var n in d.Nics)
                    {
                        Console.WriteLine("\t" + n.NetworkInfo.NicType);

                    }
                    Console.WriteLine("");
                }

                //_logger.Info(result.ToJson());
            }
            catch (Exception e)
            {
                _logger.Info("Exception when calling vmApi.ListVms: " + e.Message);
            }



        }

        private static void testVMCreate()
        {
            ApiClient client = new ApiClient(_config);

            VmApi vmApi = new VmApi(client);

            string clusterUuid = "ReplaceWithPCClusterUUID";
            Vm vm = new Vm();
            vm.Name = "TestVMByMyApp";

            vm.Cluster = new ClusterReference(clusterUuid);
            // Vm object initializations here...
            CreateVmResponse createVmResponse = vmApi.CreateVm(vm);
        }
    }
}
