using Mintcode.Dianna.Model;
using Mintcode.Dianna.Model.ConstModel;
using Mintcode.Dianna.Model.DataBaseModel;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mintcode.Dianna.Utility
{
    public class CommonUtility
    {

        public static string UserKey = "zeus-2013perfect";//加密用户名和密码
        public static byte[] Useriv = { 5, 4, 0xF, 7, 9, 0xC, 1, 0xB, 3, 0x5B, 0xD, 0x17, 1, 0xA, 6, 8 };

        public static string ServerAuthTokenKey = "zeusxiacheng2013";//加密服务端AuthToken
        public static byte[] ServerAuthTokeniv = { 0xC, 1, 0xB, 3, 0x5B, 0xD, 5, 4, 0xF, 7, 9, 0x17, 1, 0xA, 6, 8 };

        public static string ClientAuthTokenKey = "zeusmint2013code";//加密客户端AuthToken
        public static byte[] ClientAuthTokeniv = { 0x38, 0x31, 0x37, 0x34, 0x36, 0x33, 0x35, 0x33, 0x32, 0x31, 0x34, 0x38, 0x37, 0x36, 0x35, 0x32 };
        public static string Encrypt(string toEncrypt, string key, byte[] ivbyte)
        {
            if (key.Length != 16)
            {
                return "";
            }
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = Encoding.UTF8.GetBytes(key);
            rDel.IV = ivbyte;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        public static string Decrypt(string toDecrypt, string key, byte[] ivbyte)
        {
            if (key.Length != 16)
            {
                return "";
            }
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = Encoding.UTF8.GetBytes(key);
            rDel.IV = ivbyte;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <returns></returns>
        public static long CreateTimestamp()
        {
            return CreateTimestamp(System.DateTime.UtcNow);
        }

        /// <summary>
        /// 根据给定时间返回时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long CreateTimestamp(DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long totalSeconds = Convert.ToInt64((dateTime - epoch).TotalMilliseconds);
            return totalSeconds;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime CreateDateTime(long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(timestamp);
        }
        /// <summary>
        ///返回授权token
        /// </summary>
        /// <returns></returns>
        public static string GenerateAuthToken()
        {
            string guid = Guid.NewGuid().ToString();
            string date = DateTime.Now.ToString("yyMMdd");
            return guid.Substring(1, 1) + guid.Substring(3, 1) + guid.Substring(5, 1) +
                                guid.Substring(7, 1) + guid.Substring(9, 1)
                                + date.Substring(0, 1) + date.Substring(2, 1) + date.Substring(4, 1) +
                                guid.Substring(0, 1) + guid.Substring(2, 1) + guid.Substring(4, 1) +
                                guid.Substring(6, 1) + guid.Substring(8, 1)
                                + date.Substring(1, 1) + date.Substring(3, 1) + date.Substring(5, 1);
        }

        /// <summary>
        /// 解析授权token
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static string AnalyzeAuthToken(string authToken)
        {
            //token的偶数位前五位 + 当前时间的奇数位前三位 + token的奇数位前五位 + 当前时间的偶数位前三位
            return authToken.Substring(3, 5) + authToken.Substring(0, 3) + authToken.Substring(11, 5) + authToken.Substring(8, 3);
        }

        /// <summary>
        /// 生成服务端token并且加密
        /// </summary>
        /// <param name="authtoken">服务端的token</param>
        /// <returns></returns>
        public static string EncryptAuthToken(string authtoken = "")
        {
            if (string.IsNullOrWhiteSpace(authtoken))
            {
                authtoken = GenerateAuthToken();
            }
            return Encrypt(authtoken, ServerAuthTokenKey, ServerAuthTokeniv);
        }

        /// <summary>
        /// 解密服务端token并且客户端加密
        /// </summary>
        /// <param name="authtoken">服务端的加密token</param>
        /// <returns></returns>
        public static string DecryptServerAuthTokenAndEncryptClient(string authtoken)
        {
            //解密服务端token
            string decryptauthtoken = Decrypt(authtoken, ServerAuthTokenKey, ServerAuthTokeniv);
            //客户端加密token
            return Encrypt(GenerateClientAuthToken(decryptauthtoken), ClientAuthTokenKey, ClientAuthTokeniv);
        }


        /// <summary>
        /// 解密客户端token
        /// </summary>
        /// <param name="authtoken"></param>
        /// <returns></returns>
        public static string DecryptClientAuthToken(string authtoken)
        {
            string decryptauthtoken = Decrypt(authtoken, ClientAuthTokenKey, ClientAuthTokeniv);
            return AnalyzeAuthToken(decryptauthtoken);
        }


        /// <summary>
        /// 生成客户端token
        /// </summary>
        /// <returns></returns>
        public static string GenerateClientAuthToken(string authToken)
        {
            //当前时间的奇数位前三位 + token的偶数位前五位 + 当前时间的偶数位前三位+ token的奇数位前五位。
            return authToken.Substring(5, 3) + authToken.Substring(0, 5) + authToken.Substring(13, 3) + authToken.Substring(8, 5);
        }
        /// <summary>
        /// 客户端登陆解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string DecryptClientLogin(string token)
        {
            return CommonUtility.Decrypt(token, UserKey, Useriv);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string Md5Hash(string sender)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sender));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return sBuilder.ToString().ToUpper();
            }
        }

        /// <summary>
        /// 根据版本 生成不同的下线专题
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        public static LoginState GenerateOffLineState(string os)
        {
            return string.IsNullOrWhiteSpace(os) ?
                LoginState.OffLine : os.Equals(OsTypeHelper.IPhone) ? LoginState.Backend : LoginState.OffLine;
        }


        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="folderPath">文件目录</param>
        public static void CreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="saveName"></param>
        /// <param name="filedata"></param>
        ///  <returns></returns>
        public static string CreateAttachment(string loginName, string saveName, byte[] filedata)
        {
            string path = string.Format("{0}/{1}/{2}/", StaticModel.AttachPath, DateTime.Now.ToString("yy-MM-dd"), loginName);
            string folderPath = CreateRealPath(path);
            //创建目录
            CommonUtility.CreateDirectory(folderPath);
            //1.服务器生成附件
            File.WriteAllBytes(folderPath + saveName, filedata);
            //返回相对目录
            return folderPath + saveName;
        }
        /// <summary>
        /// 获取真实的目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateRealPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath("~" + path);
        }

        /// <summary>
        /// 获取配置的主机地址
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetConfigHostUrl(MessageType? type)
        {
            if (type.HasValue && StaticModel.ConfigHostUrl.ContainsKey(type.Value))
            {
                return StaticModel.ConfigHostUrl[type.Value];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
