using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.GoogleDrive
{
    public interface IGoogleDriveUploader
    {
        void UploadFile(string filename);
    }
}
