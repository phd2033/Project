using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace App_Code
{
    public class UploaderHelper
    {
        private const string STORAGE_SUBFOLDER_VIRTUAL = "temp/";
        private const string STORAGE_SUBFOLDER = @"\temp\";
        
        /// <summary>
        /// Calculates the full path for an uploaded file in the server 
        /// </summary>
        /// <param name="server">Server context</param>
        /// <param name="filePath">Path of the file (ex) @"\temp\" </param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>The full path to the file in the server</returns>
        public static string GetServerPath(HttpServerUtility server, string fileName)
        {
            return server.MapPath(Path.Combine(STORAGE_SUBFOLDER, Path.GetFileName(fileName)));
        }

        /// <summary>
        /// Calculates the full path for an uploaded file in the server 
        /// </summary>
        /// <param name="server">Server context</param>
        /// <param name="filePath">Path of the file (ex) @"\temp\" </param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>The full path to the file in the server</returns>
        public static string GetServerPath(HttpServerUtility server, string filePath, string fileName)
        {
            return server.MapPath(Path.Combine(filePath, Path.GetFileName(fileName)));            
        }

        /// <summary>
        /// Calculates the full path for an uploaded file in the server 
        /// </summary>
        /// <param name="server">Server context</param>
        /// <param name="filePath">Path of the file (ex) @"\temp\" </param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>The full path to the file in the server</returns>
        public static string GetServerPath(HttpServerUtility server, string filePath, string fileName, out string file_Guid)
        {
            file_Guid = Guid.NewGuid().ToString();
            return server.MapPath(Path.Combine(filePath, Path.GetFileName(file_Guid)));
        }
        

        /// <summary>
        /// Calculates the URL of the uploaded file in the server
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="handlerName">Name of the handler calling this method</param>
        /// <param name="fileName">Name of the method</param>
        /// <returns>The url for the file, to be accessed through internet</returns>
        public static string GetUploadedFileUrl(HttpContext context, string handlerName, string fileName)
        {
            return context.Request.Url.AbsoluteUri.Replace(handlerName, context.Request.Params["sub_folder"].Replace("\\\\","").Replace('\\', '/') + fileName);
        }

        /// <summary>
        /// Process small parts and save them to disk when they are images
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="partCount"></param>
        /// <param name="partNumber"></param>
        /// <returns>true if the file was processed ok</returns>
        public static bool ProcessPart(HttpContext context, Stream stream, string serverFileName, int partCount, int partNumber)
        {
            int length = (int)stream.Length;
            if (serverFileName.LastIndexOf("\\") >0)
            {
                string filepath = serverFileName.Substring(0, serverFileName.LastIndexOf("\\"));

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
            }

            // if it's the first part
            if (partNumber == 1)
            {                
                FileStream fileStream = File.Open(serverFileName, FileMode.Create);

                byte[] header = new byte[100];
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);

                for (int i = 0; i < 100; i++ )
                {
                    header[i] = buffer[i];
                    buffer[i] = new byte();
                }

                fileStream.Write(buffer, 0, length);
                fileStream.Close();
                //if (partCount != partNumber)
                //{
                //    // not the last one, save temporarilly
                //    fileStream.Close();
                //}
                //else
                //{
                //    CleanStream(fileStream, serverFileName);
                //    return false;
                //}
            }
            else
            {
                // put the stream in a buffer
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);

                // if it isn't the last part
                if (partCount != partNumber)
                {
                    // read the temporal file saved in the server and append the new part
                    FileStream storedFile = File.Open(serverFileName, FileMode.Append);
                    storedFile.Write(buffer, 0, buffer.Length);
                    storedFile.Close();
                }
                else
                {
                    // this is the last part 
                    // read the temporal file saved in the server and append the new part
                    FileStream storedFile = File.Open(serverFileName, FileMode.Open);
                    storedFile.Position = storedFile.Length;
                    storedFile.Write(buffer, 0, buffer.Length);
                    storedFile.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// Process small parts and save them to disk when they are images
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="partCount"></param>
        /// <param name="partNumber"></param>
        /// <returns>true if the file was processed ok</returns>
        public static bool ProcessPart(HttpContext context, Stream stream, string serverFileName, int partCount, int partNumber, ref byte[] header)
        {
            int length = (int)stream.Length;
            
            if (serverFileName.LastIndexOf("\\") > 0)
            {
                string filepath = serverFileName.Substring(0, serverFileName.LastIndexOf("\\"));

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
            }

            // if it's the first part
            if (partNumber == 1)
            {
                FileStream fileStream = File.Open(serverFileName, FileMode.Create);

                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);

                for (int i = 0; i < 128; i++)
                {
                    header[i] = buffer[i];
                    buffer[i] = new byte();
                }

                fileStream.Write(buffer, 0, length);
                fileStream.Close();
                //if (partCount != partNumber)
                //{
                //    // not the last one, save temporarilly
                //    fileStream.Close();
                //}
                //else
                //{
                //    CleanStream(fileStream, serverFileName);
                //    return false;
                //}
            }
            else
            {
                // put the stream in a buffer
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);

                // if it isn't the last part
                if (partCount != partNumber)
                {
                    // read the temporal file saved in the server and append the new part
                    FileStream storedFile = File.Open(serverFileName, FileMode.Append);
                    storedFile.Write(buffer, 0, buffer.Length);
                    storedFile.Close();
                }
                else
                {
                    // this is the last part 
                    // read the temporal file saved in the server and append the new part
                    FileStream storedFile = File.Open(serverFileName, FileMode.Open);
                    storedFile.Position = storedFile.Length;
                    storedFile.Write(buffer, 0, buffer.Length);
                    storedFile.Close();
                }
            }

            return true;
        }


        /// <summary>
        /// Writes an error in the context' response.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="message">Error message to write</param>
        public static void WriteError(HttpContext context, string message)
        {
            context.Response.StatusCode = 500;
            context.Response.Write(message);
        }

        /// <summary>
        /// Clean an openend stream, then closes it, and then remove the corresponding file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="serverFileName"></param>
        public static void CleanStream(FileStream fileStream, string serverFileName)
        {
            int lenght = (int)fileStream.Length;
            fileStream.Position = 0;
            fileStream.Write(new byte[lenght], 0, lenght);
            fileStream.Close();

            File.Delete(serverFileName);
        }


    }
}