using IriTech_Installer.models;
using NLog;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace IriTech_Installer.api
{
    class ApiService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static String scannerUrl = "http://127.0.0.1:11141/rd";
        private static String apiUrl = "https://us-central1-biometric-scanner.cloudfunctions.net/api";

        public static async Task<DeviceInfo> GetDeviceInfo()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(scannerUrl),
            };

            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Xml,
                Resource = "info"
            };

            DeviceInfo deviceInfo = null;

            try
            {
                IRestResponse<DeviceInfo> response = await client.ExecuteTaskAsync<DeviceInfo>(request);
                deviceInfo = response.Data;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }

            return deviceInfo;
        }

        public static async Task<bool> IsDeviceRegistered(String serialNumber)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(apiUrl)
            };

            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json,
                Resource = "auth/device-verify",
            };

            // Attach Request Data
            request.AddBody(new
            {
                token = "kOrlyZZFUwBUVIlyFrQZAAaXjMeC40wRiC5rNXSmODEvSFKwUNeQOkwmLMMp5CY",
                brandCode = "IRITECH",
                serialNumber = serialNumber
            });

            bool isRegistered = false;

            try
            {
                IRestResponse response = await client.ExecuteTaskAsync(request);
                isRegistered = response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }

            return isRegistered;
        }

        public static async Task<PidData> CaptureRetina()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(scannerUrl),
            };

            var request = new RestRequest
            {
                Method = Method.POST,
                Resource = "capture",
                UseDefaultCredentials = true
            };

            // Attach Request Body
            String body = "<PidOptions ver=\"1.0\"><Opts fCount=\"\" fType=\"\" iCount=\"1\" iType=\"0\" pCount=\"\" pType=\"\"";
            body += " format=\"0\" pidVer=\"2.0\" timeout=\"20000\" otp=\"\" wadh=\"\" posh=\"UNKNOWN\" env=\"S\"/>";
            body += "<CustOpts><Param name=\"enable_preview\" value=\"yes\" /></CustOpts></PidOptions>";
            request.AddParameter("text/xml", body, ParameterType.RequestBody);

            PidData captureData = null;

            try
            {
                IRestResponse<PidData> response = await client.ExecuteTaskAsync<PidData>(request);
                captureData = response.Data;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }

            return captureData;
        }
    }
}
