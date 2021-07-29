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
                        //userDefault
                        else if ((matchSysKey = Regex.Match(expression, @"^\w+$")).Success)
                        {

                        }
                        else
                            throw new Exception($"无法识别 {expression}");

                    }
            }
            //ts.Push(sr.BaseStream.Position);

            ln++;
        }

        void HandleSK(SysKeyEnum sk)
        {

        }
        SysKeyEnum GetSysKey(string inner, out string[] para)
        {
            SysKeyEnum _keyEnum = SysKeyEnum.Understand;
            para = null;
            string[] keyPars = inner.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (keyPars.Length == 0)
                return _keyEnum;

            string key = keyPars[0];
            key = key.ToUpper();

            if (key == "FOR")
            {
                if (keyPars.Length == 2)
                {
                    _keyEnum = SysKeyEnum.FOR;
                    para = new string[] { keyPars[1] };

                }
            }
            else if (key == "IF")
            {
                if (keyPars.Length == 2)
                {
                    _keyEnum = SysKeyEnum.IF;
                    para = new string[] { keyPars[1] };

                }
            }
            else if (key == "ELSE")
            {
                if (keyPars.Length == 1)
                {
                    _keyEnum = SysKeyEnum.ELSE;
                }
                else if (keyPars.Length == 3 && keyPars[1].ToUpper()=="IF")
                {
                    _keyEnum = SysKeyEnum.ELSEIF;
                    para = new string[] { keyPars[2] };
                }
            }
            else if(key=="END" && keyPars.Length == 1)
            {
                _keyEnum = SysKeyEnum.END;
            }
            else if (keyPars.Length==1)
            {
                _keyEnum = SysKeyEnum.UserDefault;
                para = new string[] { keyPars[0] };
            }
            if (!CheckSysKey(para))
            {
                _keyEnum = SysKeyEnum.Understand;
                para = null;
            }
            return _keyEnum;
        }
        bool CheckSysKey(string word)
        {
            return Regex.IsMatch(word,@"^(FOR|IF|ELSE|ELSEIF|END|AND)$",RegexOptions.IgnoreCase);
        }
        bool CheckSysKey(string[] words)
        {
            if (words == null)
                return true;
            var has=words.Where(s => CheckSysKey(s)).ToList();
            if (has .Count>0)
                return false;
            return true;
        }
        void ReadString(string txt)
        {

        }
        SysKeyEnum AnalysisSysKey(string skSourceStr,out string[] paras)
        {
            paras = null;
            SysKeyEnum sk = SysKeyEnum.Understand;


            return sk;
        }
        


        public static bool CheckModeFile(string modeFile, out string error)
        {
            error = null;
            return true;
        }
        public enum SysKeyEnum
        {
            IF,
            FOR,
            END,
            ELSE,
            ELSEIF,
            Reserved,
            Understand,
            UserDefault
        }
    }

}
