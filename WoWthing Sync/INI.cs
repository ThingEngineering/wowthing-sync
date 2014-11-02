using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WoWthing_Sync
{
    class INI
    {
        private string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
            string key,string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public INI(string path)
        {
            this.path = path;
        }
        
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.path);
        }
        
        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }
}
