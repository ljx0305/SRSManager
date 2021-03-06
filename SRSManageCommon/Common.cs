#nullable enable
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace SrsManageCommon
{
    /// <summary>
    /// 客户端类型
    /// </summary>
    [Serializable]
    public enum ClientType
    {
        Monitor,
        User,
    }

    /// <summary>
    /// 摄像头类型
    /// </summary>
    [Serializable]
    public enum MonitorType
    {
        Onvif,
        GBT28181,
        Webcast,
        Unknow,
    }

    public static class Common
    {
        public static string WorkPath = Environment.CurrentDirectory + "/";
        public static SystemConfig SystemConfig = null!;
        public static Object LockDbObjForOnlineClient = new object();
        public static Object LockDbObjForDvrVideo = new object();
        public static Object LockDbObjForStreamDvrPlan = new object();
        public static Object LockDbObjForHeartbeat = new object();
        public static readonly string LogPath = WorkPath + "logs/";

        static Common()
        {
            Directory.CreateDirectory(LogPath);
        }


        /// <summary>
        /// 获取两个时间差的毫秒数
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public static long GetTimeGoneMilliseconds(DateTime starttime,DateTime endtime)
        {
            TimeSpan ts = endtime.Subtract(starttime);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 结束自己
        /// </summary>
        public static void KillSelf()
        {
            LogWriter.WriteLog("因异常结束进程...");
            string fileName= Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            var ret = GetProcessPid(fileName);
            if (ret > 0)
            {
                KillProcess(ret);
            }
        }
        public static void KillProcess(int pid)
        {
            string cmd = "kill -9 " + pid.ToString();
            LinuxShell.Run(cmd, 1000);
        }
        /// <summary>
        /// 获取pid
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetProcessPid(string processName)
        {
            string cmd = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                cmd = "ps -aux |grep " + processName + "|grep -v grep|awk \'{print $2}\'";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd = "ps -A |grep " + processName + "|grep -v grep|awk \'{print $1}\'";
            }

            LinuxShell.Run(cmd, 1000, out string std, out string err);
            if (string.IsNullOrEmpty(std) && string.IsNullOrEmpty(err))
            {
                return -1;
            }

            int pid = -1;
            if (!string.IsNullOrEmpty(std))
            {
                int.TryParse(std, out pid);
            }
            if (!string.IsNullOrEmpty(err))
            {
                int.TryParse(err, out pid);
            }
            return pid;
        }
        public static string? GetIngestRtspMonitorUrlIpAddress(string url)
        {
            try
            {
                Uri link = new Uri(url);
                return link.Host;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 检测是否为ip 地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIpAddr(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string? AddDoubleQuotation(string s)
        {
            return "\"" + s + "\"";
        }

        public static string? RemoveDoubleQuotation(string s)
        {
            return s.Replace("\"", "").Replace("{", "").Replace("}", "");
        }


        /// <summary>
        /// 生成guid
        /// </summary>
        /// <returns></returns>
        public static string? CreateUuid()
        {
            return Guid.NewGuid().ToString("D");
        }

        /// <summary>
        /// 是否为GUID
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsUuidByError(string strSrc)
        {
            if (String.IsNullOrEmpty(strSrc))
            {
                return false;
            }

            bool _result = false;
            try
            {
                Guid _t = new Guid(strSrc);
                _result = true;
            }
            catch
            {
            }

            return _result;
        }
    }
}