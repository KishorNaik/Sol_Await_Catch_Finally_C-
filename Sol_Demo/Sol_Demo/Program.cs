using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sol_Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Demo demo = new Demo();
            string response = await demo.MakeRequestAsync();
        }
    }

    public static class Logs
    {
        public static Task WriteLog(string logMessage)
        {
            return Task.Factory.StartNew(() =>
            {
                Debug.WriteLine($"Info : {logMessage}");
            });


        }
    }

    public static class LogError
    {
        public static Task WriteErrorLog(string errorMessage)
        {
            return Task.Factory.StartNew(() =>
            {

                Debug.WriteLine($"Error : {errorMessage}");

            });


        }
    }

    public class Demo
    {
        public async Task<String> MakeRequestAsync()
        {
            HttpClient client = new HttpClient();
            await Logs.WriteLog("HttpClient instance init");
            try
            {
                var responseText = await client.GetStringAsync("https://docs.microsoft.com/en-us/dotnet/about/");
                await Logs.WriteLog($"Response : {responseText}");

                return responseText;
            }
            catch (Exception ex) when (ex.Message.Contains("301"))
            {
                await LogError.WriteErrorLog(ex.Message);
                return "Site Moved";
            }
            catch (Exception ex) when (ex.Message.Contains("401"))
            {
                await LogError.WriteErrorLog(ex.Message);
                return "Bad Request";
            }
            finally
            {
                await Logs.WriteLog("Make request method end.");
                client.Dispose();
            }

        }
    }
}
