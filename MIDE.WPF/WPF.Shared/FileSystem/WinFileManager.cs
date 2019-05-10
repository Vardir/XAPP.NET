﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using MIDE.FileSystem;
using MIDE.Application;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace MIDE.WPF.FileSystem
{
    public class WinFileManager : FileManager
    {
        public override void Write(string data, string path) => File.WriteAllText(path, data);
        public override void Write(string[] data, string path) => File.WriteAllLines(path, data);
        public override void Serialize(object data, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                AppKernel.Instance.AppLogger.PushError(ex, this, "Can not serialize data");
            }
        }

        public override bool Exists(string path) => File.Exists(path) || Directory.Exists(path);
        public override string MapPath(string path)
        {
            throw new NotImplementedException();
        }
        public override string TryRead(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            return File.ReadAllText(filePath);
        }
        public override string ExtractName(string path)
        {
            if (File.Exists(path))
                return Path.GetFileName(path);
            else if (Directory.Exists(path))
                return Path.GetDirectoryName(path);
            return null;
        }
        public override string Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            else if (Directory.Exists(path))
            {
                if (Directory.EnumerateFileSystemEntries(path).Any())
                    return "Can not delete directory - it is not empty";
                Directory.Delete(path);
            }
            return null;
        }
        public override string MakeFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return null;
            }
            else
                return "Duplicate folder name";
        }
        public override string MakeFile(string path, string templatePath)
        {
            if (File.Exists(path))
                return "Duplicate file name";
            try
            {
                if (templatePath != null)
                    File.Copy(templatePath, path);
                else
                    File.Create(path);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
        public override string Combine(params string[] paths) => Path.Combine(paths);
        public override string ReadOrCreate(string filePath, string defaultContent = "")
        {
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(defaultContent);
                    fs.Write(bytes, 0, bytes.Length);
                    return defaultContent;
                }
            }
            return File.ReadAllText(filePath);
        }
        public override IEnumerable<string> EnumerateFiles(string directory, string filter = null)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException($"Directory not found [{directory}]");
            return Directory.EnumerateFiles(directory, filter);
        }
        public override IEnumerable<(string prop, string val)> ExtractProperties(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                yield return ("Attributes", file.Attributes.ToString());
                yield return ("Creation time UTC", file.CreationTimeUtc.ToString());
                yield return ("Parent directory", file.DirectoryName);
                yield return ("Extension", file.Extension);
                yield return ("Is readonly", file.IsReadOnly.ToString());
                yield return ("Last access time UTC", file.LastAccessTimeUtc.ToString());
                yield return ("Last write time UTC", file.LastWriteTimeUtc.ToString());
                yield return ("Length", file.Length.ToString());
                yield break;
            }

            var directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                yield return ("Attributes", directory.Attributes.ToString());
                yield return ("Creation time UTC", directory.CreationTimeUtc.ToString());
                yield return ("Parent directory", directory.Parent.FullName);
                yield return ("Last access time UTC", directory.LastAccessTimeUtc.ToString());
                yield return ("Last write time UTC", directory.LastWriteTimeUtc.ToString());
                yield break;
            }

            throw new ArgumentException("The given path is invalid", nameof(path));
        }
    }
}