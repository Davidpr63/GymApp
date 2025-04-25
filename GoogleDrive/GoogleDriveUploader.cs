using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;
using File = Google.Apis.Drive.v3.Data.File;
using System.Windows.Media.Animation;
using System.Net;

namespace GymApp.GoogleDrive
{
    public class GoogleDriveUploader : IGoogleDriveUploader
    {

        private string credentialsPath = "credentials.json";
        private string folderId = "1jMydKZS8_blMNbQ7HDy8YU7rJavKxCtO";
       
        public GoogleDriveUploader()
        {
              
        }

     
        public async Task UploadFile(string filename)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GymApp"
            });

            string fileId = GetFileIdByName(filename);
            if (string.IsNullOrEmpty(fileId))
            {
                 
                return;  
            }
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = filename,
                //Parents = new List<string> { folderId}
            };
            FilesResource.UpdateMediaUpload request;
            using (var stream = new FileStream("C:/GymDatabase/" + filename, FileMode.Open))
            {
                request = service.Files.Update(fileMetadata, fileId, stream, "application/json");
                request.Fields = "id";  
                await request.UploadAsync();  
            }

            var uploadedFile = request.ResponseBody;
            
        }
        public string GetFileIdByName(string fileName)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GymApp"
            });
          
            var request = service.Files.List();

        
            request.Q = $"name = '{fileName}'";   

          
            request.Fields = "files(id, name)";

             
            var result = request.Execute();

             
            var file = result.Files.FirstOrDefault();

            return file?.Id;  
        }
    }
}
