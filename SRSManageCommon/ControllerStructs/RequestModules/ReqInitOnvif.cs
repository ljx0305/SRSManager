#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SRSManageCommon.ControllerStructs.RequestModules
{
    /// <summary>
    /// 初始化onvif设备的请求结构
    /// </summary>
    [Serializable]
    public class ReqInitOnvif
    {
        private List<string> _ipAddrArray = new List<string>();
        private string _ipAddrs = null!;
        private string? _password;
        private string? _username;

        /// <summary>
        /// ip 地址串，多个ip 地址用空格隔开
        /// </summary>
        public string IpAddrs
        {
            get => _ipAddrs;
            set => _ipAddrs = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? Username
        {
            get => _username;
            set => _username = value;
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password
        {
            get => _password;
            set => _password = value;
        }

        [JsonIgnore]

        /// <summary>
        /// 初始化时不用传，此字段为内部使用
        /// </summary>
        public List<string> IpAddrArray
        {
            get => _ipAddrArray;
            set => _ipAddrArray = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 从ip地址串中获取ip 地址列表
        /// </summary>
        public void GetIpArray()
        {
            if (!string.IsNullOrEmpty(IpAddrs))
            {
                _ipAddrArray = Regex.Split(_ipAddrs, @"[\s]+").ToList();
                // _ipAddrArray = _ipAddrs.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
    }
}