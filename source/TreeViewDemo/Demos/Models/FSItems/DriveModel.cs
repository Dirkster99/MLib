namespace TreeViewDemo.Demos.Models.FSItems
{
    using FileSystemModels.Models.FSItems.Base;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Threading.Tasks;

    public class DriveModel : Base.FileSystemModel
    {
        #region fields
        PathModel _Model;
        #endregion fields

        #region constructors
        /// <summary>
        /// Parameterized class  constructor
        /// </summary>
        /// <param name="model"></param>
        [SecuritySafeCritical]
        public DriveModel(PathModel model)
          : base(model)
        {
            _Model = new PathModel(model);
        }
        #endregion constructors

        #region properties
        public long AvailableFreeSpace
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.AvailableFreeSpace : 0);
            }
        }

        public string DriveFormat
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.DriveFormat : "(unknown)");
            }
        }

        ////    public DriveType DriveType
        ////    {
        ////      get
        ////      {
        ////        return this.mDrive.DriveType;
        ////      }
        ////    }

        public bool Exists
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.RootDirectory.Exists : false);
            }
        }

        public bool IsReady
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.IsReady : false);
            }
        }

        public long TotalFreeSpace
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.TotalFreeSpace : 0);
            }
        }

        public long TotalSize
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.TotalSize : 0);
            }
        }

        public string VolumeLabel
        {
            get
            {
                var drive = GetDriveInfo();
                return (drive != null ? drive.VolumeLabel : "(unknown)");
            }
        }
        #endregion properties

        #region methods
        public static IEnumerable<PathModel> GetLogicalDrives()
        {
            foreach (var item in Environment.GetLogicalDrives())
                yield return new PathModel(item, FSItemType.LogicalDrive);
        }

        public static Task<IEnumerable<PathModel>> GetLogicalDrivesAsync()
        {
            return Task.Run(() => { return GetLogicalDrives(); });
        }

        private DriveInfo GetDriveInfo()
        {
            try
            {
                return new DriveInfo(_Model.Path);
            }
            catch (Exception)
            {

            }

            return null;
        }
        #endregion methods
    }
}
