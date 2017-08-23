namespace PDF_Binder
{
    using Doc.DocManager.Interfaces;
    using ExplorerLib;
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using ViewModels.Base;

    public class AppCommands : ModelBase
    {
        private static AppCommands _instance = null;

        static public AppCommands Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppCommands();

                return _instance;
            }
        }

        #region OpenCommand
        /// <summary>
        /// Open a file persistence with dialog and user interaction.
        /// </summary>
        public void OnOpen(string typeOfDocument
                        , string defaultPath
                        , string defaultFallbackPath
                        , IFileManager docManager
                        , IExplorer explorer
                        , IList<IFileItem> pathCollection
                            )
        {
            try
            {
                IFileFilterEntries fileEntries = null;

                // Get filter strings for document specific filters or all filters
                // depending on whether type of document is set to a key or not.
                fileEntries = docManager.GetFileFilterEntries(typeOfDocument);

                var pathColl =  explorer.FileOpenMultipleFiles( fileEntries.GetFilterString()
                                                              , defaultPath
                                                              , defaultFallbackPath);

                if (pathColl != null)
                {
                    foreach (string fileName in pathColl)
                    {
                        pathCollection.Add(docManager.GetFileItem(fileName));
                    }
                }
            }
            catch (Exception exp)
            {
                var msg = GetService<IContentDialogService>().MsgBox;

                //logger.Error(exp.Message, exp);
                msg.Show(exp, "Unexpected Error",
                         MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton);
            }
        }
        #endregion OnOpen

        #region File System
        /// <summary>
        /// Opens the given file in an associated application in Windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnOpenInWindowsCommand(string fileName)
        {
            try
            {
                Process.Start(new ProcessStartInfo(fileName));
            }
            catch (System.Exception ex)
            {
                var msg = GetService<IContentDialogService>().MsgBox;

                msg.Show(string.Format(CultureInfo.CurrentCulture, "{0}\n'{1}'.", ex.Message, (fileName == null ? string.Empty : fileName)),
                         "Error Finding Resource", MsgBoxButtons.OK, MsgBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the folder in which this document is stored in the Windows Explorer.
        /// </summary>
        public void OnOpenContainingFolderCommand(string filePath)
        {
            var msg = GetService<IContentDialogService>().MsgBox;

            try
            {
                if (System.IO.File.Exists(filePath) == true)
                {
                    // combine the arguments together it doesn't matter if there is a space after ','
                    string argument = @"/select, " + filePath;

                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
                else
                {
                    string parentDir = System.IO.Directory.GetParent(filePath).FullName;

                    if (System.IO.Directory.Exists(parentDir) == false)
                        msg.Show(string.Format(CultureInfo.CurrentCulture
                                , "Cannot find: {0}"
                                , parentDir)
                                , "Error finding directory"
                                , MsgBoxButtons.OK
                                , MsgBoxImage.Error);
                    else
                    {
                        string argument = @"/select, " + parentDir;

                        System.Diagnostics.Process.Start("EXPLORER.EXE", argument);
                    }
                }
            }
            catch (System.Exception ex)
            {
                msg.Show(string.Format(CultureInfo.CurrentCulture, "{0}\n'{1}'.", ex.Message, (filePath == null ? string.Empty : filePath))
                        , "Error finding directory"
                        , MsgBoxButtons.OK, MsgBoxImage.Error);
            }
        }

        /// <summary>
        /// Copies the given string into the Windows Clipboard.
        /// </summary>
        /// <param name="filePath"></param>
        public void OnCopyFullPathtoClipboardCommand(string filePath)
        {
            try
            {
                System.Windows.Clipboard.SetText(filePath);
            }
            catch
            {
            }
        }
        #endregion File System
    }
}
