using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SPS.Web.Common
{
    public static class StateTranslator
    {
        public static string GetStateName(string stateAcronym)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/StatesNames.txt");

            using (FileStream fileStream = File.OpenRead(path))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] separatedLine = line.Split(';');
                        string state = separatedLine[1].ToLower();

                        if (state == stateAcronym.ToLower())
                        {
                            return separatedLine[0];
                        }
                    }
                }
            }

            return null;
        }
    }
}