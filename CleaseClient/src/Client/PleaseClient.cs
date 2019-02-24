namespace CleaseSolution
{
    using System;
    using System.Collections.Generic;
    using RestSharp;

    public class PleaseClient : IPleaseClient
    {
        private readonly RestClient client;

        public PleaseClient(string url)
        {
            this.client = new RestClient(url);
        }

        public List<Hardware> GetHardwareList()
        {
            var getRequest = new RestRequest("/list", Method.GET);
            var response = this.client.Execute(getRequest);
            var content = response.Content;
            var hardwareList = content.FromJson<List<Hardware>>(new List<Hardware>());
            return hardwareList;
        }

        public List<Hardware> GetHardwareList(string platform)
        {
            var getRequest = new RestRequest($"/listbyplatform/{platform}", Method.GET);
            var response = this.client.Execute(getRequest);
            var content = response.Content;
            var hardwareList = content.FromJson<List<Hardware>>(new List<Hardware>());
            return hardwareList;
        }

        public List<Hardware> GetLeasedHardwareList()
        {
            var getRequest = new RestRequest("/listleased", Method.GET);
            var response = this.client.Execute(getRequest);
            var content = response.Content;
            var hardwareList = content.FromJson<List<Hardware>>(new List<Hardware>());
            return hardwareList;
        }

        public bool AddHardware(Hardware hardware)
        {
            var putRequest = new RestRequest("/add", Method.PUT);
            putRequest.AddJsonBody(hardware.ToJson());
            var response = this.client.Execute(putRequest);
            var content = response.Content;
            return response.IsSuccessful;
        }

        public bool LeaseHardware(Platform platform, int duration)
        {
            var dict = new Dictionary<string, object>
            {
                ["platform"] = platform,
                ["lease_duration"] = duration
            };
            var postRequest = new RestRequest("/lease", Method.POST);
            postRequest.AddJsonBody(dict.ToJson());
            var response = this.client.Execute(postRequest);
            var content = response.Content;
            return response.IsSuccessful;
        }
    }
}