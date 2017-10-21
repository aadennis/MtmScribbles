using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleApp2 {

    public static class JsonHelper {
        public static string FormatJson(string json) {
            return JValue.Parse(json).ToString(Formatting.Indented);
        }
    }
    class Program {
        static void Main(string[] args) {
            GetTfsProjects();
            Console.WriteLine("Done. Press a key");
            Console.ReadKey();
        }

        // https://docs.microsoft.com/en-us/vsts/integrate/get-started/rest/basics
        // https://docs.microsoft.com/en-us/vsts/integrate/get-started/client-libraries/samples
        // https://docs.microsoft.com/en-us/vsts/integrate/quickstarts/work-item-quickstart
        public static async void GetTfsProjects() {
            try {
                var personalAccessToken = ConfigurationManager.AppSettings["PAT"];
                // Note that tfsServer has a format something like this: "joe.visualstudio.com"
                var tfsServer = ConfigurationManager.AppSettings["TfsServer"];

                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.Encoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", personalAccessToken))));

                    using (var response = client.GetAsync(
                                $"{tfsServer}/DefaultCollection/_apis/projects").Result) {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(JsonHelper.FormatJson(responseBody));

                    }
                    using (var response2 = client.GetAsync(
                                $"{tfsServer}/DefaultCollection/ARM Basic Templates/_apis/test/plans?api-version=1.0").Result) {
                        response2.EnsureSuccessStatusCode();
                        var responseBody2 = await response2.Content.ReadAsStringAsync();
                        Console.WriteLine(JsonHelper.FormatJson(responseBody2));

                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
