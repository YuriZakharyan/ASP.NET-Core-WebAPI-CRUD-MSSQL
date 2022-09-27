using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.MiddleWares
{
    public static class LoggingMethods
    {
        public static async void Logging(string methodName, string requestBody, string? Exeption, string? MSG_Error)
        {
            string path = @"D:\LogInfo.txt";

            try
            {
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //    
                //}

                ///await using (StreamWriter sw = File.CreateText(path))
                ///{
                ///    //Add some text to file
                ///    sw.WriteLine($"Method Name: {methodName}");
                ///    sw.WriteLine($"RequestBody: {requestBody}");
                ///}

                ///var s = HttpContext.Current.Request.UserHostName;


                await using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("->");
                    sw.WriteLine($"Method Name: {methodName}");
                    sw.WriteLine($"RequestBody: {requestBody}");
                    sw.WriteLine($"Status code: {MSG_Error}");
                    sw.WriteLine($"Request Time:    {DateTime.Now}");       //mejdunarodni jam
                    sw.WriteLine("<-");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exeption: " + ex.ToString());
            }
            finally
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    sr.Close();
                }
            }
        }
    }
}