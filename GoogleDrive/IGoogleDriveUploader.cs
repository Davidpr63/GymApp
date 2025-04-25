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
        void InitializeDriveService();
        string UploadFile(string filePath);
    }
}
