using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PHPDecode
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length == 0) 
            {
                return;
            }
            if (!File.Exists(args[0]))                  //檔案不存在就88
            {
                return;
            }

            //string encodingName = Encoding.ASCII.EncodingName;
            string encodingName = Encoding.ASCII.EncodingName;
            FileInfo fi = new FileInfo(args[0]);
            string outputName = fi.FullName.Replace(fi.Extension,".txt");
            fi = null;

            StreamReader sr = new StreamReader(args[0]); //讀取檔案
            
            string line1 = sr.ReadLine();
            string line2 = sr.ReadLine();
            string line3 = sr.ReadLine();

            sr.Close();
            sr.Dispose();

            int indexStart = line2.IndexOf("O0O0000O0('");
            Console.WriteLine("開始位置: " + indexStart);
            int indexEnd = line2.IndexOf("'))");
            Console.WriteLine("結束位置: " + indexEnd);
            
            string S_Code = line2.Substring(indexStart, indexEnd - indexStart).Replace("O0O0000O0('","");
            Console.WriteLine("截取解密內容: \n" + S_Code);

            //string S_Decode = DecodeBase64(Encoding.ASCII.HeaderName,S_Code);
            string S_Decode = DecodeBase64(encodingName, S_Code);
            Console.WriteLine("第一次Base64解密:\n" + S_Decode);

            int keyStartindex = S_Decode.IndexOf("),'");
            Console.WriteLine("密匙開始: " + keyStartindex);

            int KeyEndIndex = S_Decode.IndexOf("',");
            Console.WriteLine("密匙結束: " + KeyEndIndex);

            string key = S_Decode.Substring(keyStartindex, KeyEndIndex - keyStartindex).Replace("),'", "");
            Console.WriteLine("截取密匙: " + key);

            int codeLengthEndIndex = S_Decode.IndexOf("),");

            bool loop = true;
            int index = codeLengthEndIndex;
            while (loop) 
            {
                index--;
                if (index == 0) 
                {
                    loop = false;
                }
                else if (S_Decode[index].Equals(',')) 
                {
                    loop = false;
                }
            }

            int codeLengthStartIndex = index;

            string Slength = S_Decode.Substring(codeLengthStartIndex,codeLengthEndIndex - codeLengthStartIndex).Replace(",","");
            
//            Console.WriteLine("Code Length : " + Slength);
            int iLength = int.Parse(Slength);
            Console.WriteLine("密文長度 : " + iLength);

            string code = line3.Substring(iLength);
            Console.WriteLine("密文內容 : " + code);

            Console.WriteLine("解密內容：");
            string output = "";
            //string output = "<?php\n";
            string strEncryptedData = DecodeBase64(encodingName, strtr(code, key));
            output = output + strEncryptedData;
            //output = output + "\n?>";
            output = output.Replace("\\'","'");
            Console.WriteLine(output);

            Console.ReadLine();
            
            StreamWriter sw = new StreamWriter(outputName,false,Encoding.Unicode);
            sw.WriteLine(output);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static string strtr(string Secret, string decode_key)
        {
            string replaceCode = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            int length = replaceCode.Length - 1;
            StringBuilder sb = new StringBuilder();
            bool replaced = false;
            for (int i = 0; i < Secret.Length; i++) //讀取每一個Secret Code
            {
                for (int j = 0; j < decode_key.Length; j++)
                {
                    if ((Secret[i] == decode_key[j]) && (j <= length)) 
                    {
                        //Console.WriteLine("Bingo");
                        sb.Append(replaceCode[j]);
                        replaced = true;
                        break;
                    }
                }
                if (!replaced)
                {
                    sb.Append(Secret[i]);
                }
                else 
                {
                    replaced = false;
                }
            }

            return sb.ToString();
        }
        

        /// <summary> 
        /// 將字串使用base64演算法解密 
        /// </summary> 
        /// <param name="code_type">編碼類型</param> 
        /// <param name="code">已用base64演算法加密的字串</param> 
        /// <returns>解密後的字串</returns> 
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code); //將2進制編碼轉換為8位元不帶正負號的整數陣列. 
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes); //將指定位元組陣列中的一個位元組序列解碼為一個字串。 
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

    }
}
