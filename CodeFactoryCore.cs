using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Codu
{
    public class CodeFactoryCore
    {
        string flagPattern = @"\{\{\s*(\w+(\s+\w+)*)\s*\}\}";

        //一行一行读
        public void Create(string fileOutput, string modeFile, JObject data)
        {
            FileInfo fi = new FileInfo(modeFile);
            FileInfo fiOutput = new FileInfo(fileOutput);
            using
                StreamReader sr = new StreamReader(fi.OpenRead());
            StreamWriter sw = new StreamWriter(fiOutput.OpenWrite());


            Stack<SysKeyEnum> stackSysKey = new Stack<SysKeyEnum>();
            JObject currentData = data;
            int ln = 1;
            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                    break;
                var matchs = Regex.Matches(s, flagPattern, RegexOptions.IgnoreCase);
                if (matchs.Count == 0)
                {
                    sw.WriteLine(s);
                }
                else
                    foreach (Match match in matchs)
                    {
                        #region  =====
                        /*
                        string keyString = Regex.Replace(match.Groups[1].Value, @"\s+", " ");
                        string[] keys = keyString.Split(' ');
                        if (keys.Length == 2)
                        {
                            string key = keys[0];
                            switch (key.ToUpper())
                            {
                                case "END": { }; break;
                                default:
                                    {

                                    };
                                    break;
                            }
                            
                        }
                        else if(keys.Length==1)
                        {
                            string systemKey = keys[0];
                            switch (systemKey.ToUpper())
                            {
                                case "IF":
                                default:
                                    throw new Exception($"未知的系统关键字{keys[0]}");
                                    break;
                            }
                        }
                        */
                        #endregion

                        string expression = match.Groups[1].Value;
                        Match matchSysKey;
                        //for
                        if ((matchSysKey = Regex.Match(expression, @"\s+for\s+", RegexOptions.IgnoreCase)).Success)
                        {
                            //string para=Regex.Replace(expression,matchSysKey.Value,"").Trim();
                            Match para = Regex.Match(expression, Regex.Escape(matchSysKey.Value) +
                                @"\s*(\w+)\s*");
                            if (!para.Success)
                                throw new Exception($"关键字[for]缺少条件");
                            string paraStr = para.Groups[1].Value;
                            stackSysKey.Push(SysKeyEnum.FOR);
                        }
                        //else if
                        else if ((matchSysKey = Regex.Match(expression, @"\s+else\s+if\s+", RegexOptions.IgnoreCase)).Success)
                        {

                        }
                        //if
                        else if ((matchSysKey = Regex.Match(expression, @"\s+if\s+", RegexOptions.IgnoreCase)).Success)
                        {

                        }
                        //else
                        else if ((matchSysKey = Regex.Match(expression, @"\s+else\s+", RegexOptions.IgnoreCase)).Success)
                        {

                        }
                        //end
                        else if ((matchSysKey = Regex.Match(expression, @"\s+end\s+", RegexOptions.IgnoreCase)).Success)
                        {

                        }

                    }
            }
            //ts.Push(sr.BaseStream.Position);

            ln++;
        }

        void HandleIF()
        {

        }



        public static bool CheckModeFile(string modeFile, out string error)
        {
            error = null;
            return true;
        }
        enum SysKeyEnum
        {
            FOR,
            IF,
            ELSE,
            ELSEIF,
            END,

        }
    }

}
