using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Slakever.DriveConsole
{
    class Program
    {
        static string[] Scopes = {
            DriveService.Scope.DriveReadonly,
            DriveService.Scope.DriveFile
        };

        static async Task Main(string[] args)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SlakeverConsole"
            });

            //ListFile(service);
            const string uploadedFile = "GN6KNFX6V_20200212.txt";
            await UploadFile(service, uploadedFile);
        }

        static async Task UploadFile(DriveService service, string filePath)
        {
            using (var toUploadStream = File.OpenRead(filePath))
            {
                var uploadProgress = await service.Files.Create(
                    new Google.Apis.Drive.v3.Data.File
                    {
                        Name = Path.GetFileName(filePath),
                    },
                    toUploadStream,
                    "text/plain").UploadAsync();
                //TODO: not work yet

                Console.WriteLine("BytesSent: " + uploadProgress.BytesSent);
                Console.WriteLine("Status: " + uploadProgress.Status);
                Console.WriteLine("Exception: " + uploadProgress.Exception.ToString());
            }
        }

        private static void ListFile(DriveService service)
        {
            var listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            var files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
        }
    }
}
