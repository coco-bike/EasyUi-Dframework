using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ConfigHelper
    {
        public static string GetValue(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                return null;
            }
            return ConfigurationManager.AppSettings[key].ToString().Trim();

        }



        public static bool DeleteAppSettings(string key)
        {
            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            string root = "D:\\用户目录\\我的文档\\Visual Studio 2013\\Projects\\视频后台管理系统\\Sys_VideoManage_Admin";
            filemap.ExeConfigFilename = root + "\\Web.config";//配置文件路径
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            app.Settings.Remove(key);
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }



        public static bool SetValue(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                DeleteAppSettings(key);//如果存在则删除
            }

            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            string root ="D:\\用户目录\\我的文档\\Visual Studio 2013\\Projects\\视频后台管理系统\\Sys_VideoManage_Admin";
            filemap.ExeConfigFilename = root + "\\Web.config";//配置文件路径
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
            //修改key为name的value
            config.AppSettings.Settings.Add(key, value);
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }

        public static bool UpdateValue(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] != null || ConfigurationManager.AppSettings[key] != "")
            {
                return false;
            }
            try
            {
                ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
                string path = Directory.GetCurrentDirectory();
                filemap.ExeConfigFilename = path + "\\Web.config";//配置文件路径
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
                //修改key为name的value
                config.AppSettings.Settings[key].Value = value;
                //在appsetings节点下新增一个add标签，key为age,value为23
                //config.AppSettings.Settings.Add("gender", "男");
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
