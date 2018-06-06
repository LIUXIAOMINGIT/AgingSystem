using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace PTool
{
    public class IniReader
    {
        private string _fileName;

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          int nDefault,// 如果 Key 值没有找到，则返回缺省的值是多少
          string lpFileName
          );

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          string lpDefault,// 如果 Key 值没有找到，则返回缺省的字符串的地址
          StringBuilder lpReturnedString,// 返回字符串的缓冲区地址
          int nSize,// 缓冲区的长度
          string lpFileName
          );

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(
          string lpAppName,// 指向包含 Section 名称的字符串地址
          string lpKeyName,// 指向包含 Key 名称的字符串地址
          string lpString,// 要写的字符串地址
          string lpFileName
          );

        public IniReader(string filename)
        {
            _fileName = filename;
        }

        public int GetInt(string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, _fileName);
        }

        public string GetString(string section, string key, string def)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, _fileName);
            return temp.ToString();
        }

    }
}