<%@ WebHandler Language="C#" Class="FileDownHandler" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using App_Code;

    /// <summary>
    /// FileDownHandler의 요약 설명입니다.
    /// </summary>
    public class FileDownHandler : IHttpHandler
    {

        private string HandlerName = "FileDownHandler.ashx";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string tARGETSOURCE = HttpUtility.UrlDecode(context.Request.Params["TARGETSOURCE"]);
                string fileDirectory = HttpUtility.UrlDecode(context.Request.Params["FILEDIR"]);
                string fileName = HttpUtility.UrlDecode(context.Request.Params["FILENAME"]);
                string fileHeader = HttpUtility.UrlDecode(context.Request.Params["FILEHEADER"]);

                string filePath = UploaderHelper.GetServerPath(context.Server, @"TemporaryFiles\\" + fileDirectory + "\\", fileName);
                string TargetFilePath = context.Server.MapPath(tARGETSOURCE);


                if (filePath.LastIndexOf("\\") > 0)
                {
                    string Dirpath = filePath.Substring(0, filePath.LastIndexOf("\\"));

                    if (!Directory.Exists(Dirpath))
                    {
                        Directory.CreateDirectory(Dirpath);
                    }
                }

                if (File.Exists(TargetFilePath))
                {
                    byte[] buffer;

                    using (FileStream fileStream = File.Open(TargetFilePath, FileMode.Open, FileAccess.Read))
                    {
                        buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Count());
                        fileStream.Close();
                    }

                    byte[] header = System.Text.Encoding.Unicode.GetBytes(fileHeader);

                    for (int i = 0; i < header.Length; i++)
                    {
                        buffer[i] = header[i];
                    }

                    using (FileStream tmpfileStream = File.Create(filePath))
                    {
                        tmpfileStream.Write(buffer, 0, buffer.Length);
                        tmpfileStream.Close();
                    }
                }

                string Url = context.Request.Url.AbsoluteUri.Substring(0, context.Request.Url.AbsoluteUri.IndexOf(HandlerName))
                             + "TemporaryFiles/" + fileDirectory.Replace("\\", "/") + "/" + fileName;

                context.Response.Write(Url);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
