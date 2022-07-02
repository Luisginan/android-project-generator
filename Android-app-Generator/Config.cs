using System;
using System.Collections.Generic;

namespace Android_App_Generator
{
    public class Config
    {
        public List<Variable> VariableList = new List<Variable>();
        public string TemplatePath { get; set; }
        public string OutputPath { get; set; }

        public List<ReplaceContent> ReplaceContentList { get; set; } = new List<ReplaceContent>();
        public List<RenameFile> RenameFileList { get; set; } = new List<RenameFile>();
        public List<CopyFile> CopyFileList { get; set; } = new List<CopyFile>();
        public List<RenameFolder> RenameFolderList { get; set; } = new List<RenameFolder>();
    }

    public class Variable
    {
        public String Name { get; set; } = "";
        public String Value { get; set; } = "";
    }

    public class CopyFile
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }

    public class CopyFolder
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }

    public class RenameFile
    {
        public string FilePath { get; set; }
    }

    public class RenameFolder
    {
        public string SourcePath { get; set; }
    }

    public class ReplaceContent
    {
        public string SourcePath { get; set; }
    }
}