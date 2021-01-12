<%@ WebHandler Language="C#" Class="FileUploadHandler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Code;

    /// <summary>
    /// FileUploadHandler의 요약 설명입니다.
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // get custom parameters
                string parameters = context.Request.Params["parameters"];

                // get the uploaded file, and calculates the full path to save it in the server
                HttpPostedFile file = context.Request.Files[0];

                // get parts parameters (used to upload a file broken into small parts)
                int partCount = int.Parse(context.Request.Params["partCount"]);
                int partNumber = int.Parse(context.Request.Params["partNumber"]);
                string filePath = context.Request.Params["sub_folder"];

                //string serverFileName = UploaderHelper.GetServerPath(context.Server, filePath, file.FileName);
                string file_Guid = context.Request.Params["file_Guid"];
                string serverFileName = UploaderHelper.GetServerPath(context.Server, filePath, file_Guid);
                //string serverFileName = UploaderHelper.GetServerPath(context.Server, filePath, file.FileName);


                byte[] header = new byte[128];
                // process this new small part
                if (UploaderHelper.ProcessPart(context, file.InputStream, serverFileName, partCount, partNumber, ref header))
                {
                    string url = UploaderHelper.GetUploadedFileUrl(context, "FileUploadHandler.ashx", file_Guid);
                    string strHeader = System.Text.Encoding.Unicode.GetString(header);
                    if (partNumber == 1)
                    {
                        context.Response.Write(strHeader + "|" + file_Guid);
                    }
                }
                else
                {
                    UploaderHelper.WriteError(context, "Couldn't upload the file. Please verify it's a correct file!");
                }
            }
            catch (Exception exc)
            {
                UploaderHelper.WriteError(context, exc.Message);
                context.Response.End();
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
