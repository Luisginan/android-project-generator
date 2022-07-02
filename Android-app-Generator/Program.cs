using System;
using System.IO;
using NGE_Tool_Lib;

namespace Android_App_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            StartGenerateApp();
        }

        private static void StartGenerateApp()
        {
            var fileMaker = new NgFileMaker();
            var config = new Config();

            PrintHeader();
            if (fileMaker.Exists(new FileInfo("config.xml")))
            {
                config = fileMaker.ReadConfig<Config>("config.xml");
            }
            else
            {
                config.TemplatePath = @"D:\Working Folder\DotNet\Template Project";
                config.OutputPath = @"D:\Working Folder\DotNet\output";
                fileMaker.WriteConfig(config, "config.xml");

                Environment.ExitCode = -1;
                return;
            }

            DoTask(config, fileMaker);
            PrintFooter();
        }

        private static void DoTask(Config config, NgFileMaker fileMaker)
        {
            fileMaker.DeleteFolder(new DirectoryInfo(config.OutputPath));
            fileMaker.CopyFolder(config.TemplatePath, config.OutputPath);

            JustReplaceContent(config, fileMaker);
            JustRenameFileName(config, fileMaker);
            JustCopyFile(config, fileMaker);
            JustRenameFolder(config, fileMaker);
        }

        private static void JustReplaceContent(Config config, NgFileMaker fileMaker)
        {
            var s = new StyleUtil();
            s.WriteTitle("Replace Contents");
            foreach (var x in config.ReplaceContentList)
            {
                var content = fileMaker.ReadFromFile(config.OutputPath + @"\" + x.SourcePath);
                foreach (var variable in config.VariableList)
                {
                    content = content.Replace($"$${variable.Name}$$", variable.Value);
                }

                fileMaker.WriteToFile(config.OutputPath + @"\" + x.SourcePath, content);
            }
        }

        private static void JustRenameFileName(Config config, NgFileMaker fileMaker)
        {
            var s = new StyleUtil();
            s.WriteTitle("Rename Files");
            foreach (var x in config.RenameFileList)
            {
                var oldSource = config.OutputPath + @"\" + x.FilePath;
                var file = new FileInfo(oldSource);
                var newFile = file.Name;

                foreach (var variable in config.VariableList)
                {
                    newFile = newFile.Replace($"@{variable.Name}@", variable.Value);
                }

                if (file.Directory == null) continue;
                var sourceToReplace = file.Directory.FullName + @"\" + newFile;
                fileMaker.RenameFile(oldSource, sourceToReplace);
            }
        }

        private static void JustCopyFile(Config config, NgFileMaker fileMaker)
        {
            var s = new StyleUtil();
            s.WriteTitle("Copy Files");
            foreach (var x in config.CopyFileList)
            {
                fileMaker.CopyFile(new FileInfo(x.SourcePath),
                    new FileInfo(config.OutputPath + @"\" + x.DestinationPath), false);
            }
        }

        private static void JustRenameFolder(Config config, NgFileMaker fileMaker)
        {
            var s = new StyleUtil();
            s.WriteTitle("Rename Folders");
            foreach (var x in config.RenameFolderList)
            {
                var dir = new DirectoryInfo(config.OutputPath + @"\" + x.SourcePath);
                var pathOld = dir.FullName;
                var pathNew = dir.Name;

                foreach (var variable in config.VariableList)
                {
                    pathNew = pathNew.Replace($"@{variable.Name}@", variable.Value);
                }

                var newDir = dir.Parent + @"/" + pathNew;

                fileMaker.RenameFolder(new DirectoryInfo(pathOld), new DirectoryInfo(newDir));
            }
        }

        private static void PrintHeader()
        {
            var s = new StyleUtil();
            s.WriteTitle("Starting Generate Android apps");
        }

        private static void PrintFooter()
        {
            var style = new StyleUtil();
            style.WriteTitle("Finish Generate Android apps");
        }
    }
}