using Nutanix.VmmSDK.Model.Vmm.V4.Ahv.Config;
using NLog;
using Nutanix.VmmSDK.Api;
using Nutanix.VmmSDK.Client;

namespace Nutanix.Example
{
    class MyApp
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static string vmExtId = "f6b4333b-72d2-489f-41d6-20d1080fd14f";
        private static Configuration _config = new Configuration()
        {
            Username = "admin",
            Password = "Nutanix.123",
            Host = "10.15.4.16",
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
            GetVmApiResponse getVmResponse = vmApi.GetVmById(vmExtId);
            string eTagHeader = ApiClient.GetEtag(getVmResponse);
            Dictionary<string, object> opts = new Dictionary<string, object>();
            opts["If-Match"] = eTagHeader;
            PowerOnVmApiResponse resp = vmApi.PowerOnVm(vmExtId, opts);

        }

        private static void testGetVm()
        {
            ApiClient client = new ApiClient(_config);
            VmApi vmApi = new VmApi(client);
            GetVmApiResponse vm = vmApi.GetVmById(vmExtId);
            Vm vmData = (VmmSDK.Model.Vmm.V4.Ahv.Config.Vm)vm.Data;
            Console.WriteLine("vmName:"+vmData.Name);

        }

        private static void testVMList()
        {

            ApiClient client = new ApiClient(_config);
            VmApi vmApi = new VmApi(client);
            try
            {
                ListVmsApiResponse result = vmApi.ListVms();
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

            string clusterUuid = "00064033-3c2a-6c5d-6886-0cc47a9b09b3";
            Vm vm = new Vm();
            vm.Name = "ATestVMByMyApp2";

            vm.Cluster = new ClusterReference(clusterUuid);
            // Vm object initializations here...
            CreateVmApiResponse createVmResponse = vmApi.CreateVm(vm);
        }
    }
}
