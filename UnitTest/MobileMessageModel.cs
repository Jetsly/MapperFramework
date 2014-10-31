using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.Dianna.Model
{
    public class MobileMessageModel
    {
        /// <summary>
        /// From用户名
        /// </summary>
        public string fromLoginName { get; set; }

        /// <summary>
        /// To用户名
        /// </summary>
        public string toLoginName { get; set; }

        /// <summary>
        /// 成员列表（推送到 RabbitMQ 时，请转换为 string）
        /// </summary>
        //public IEnumerable<user_infovalueModel> memberList { get; set; }
        public object memberList { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 客户端消息ID
        /// </summary>
        public long clientMsgId { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public long msgId { get; set; }

        /// <summary>
        /// 群的信息
        /// </summary>
        public string info { get; set; }

        /// <summary>
        /// 发送消息时间
        /// </summary>
        public long createDate { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType type { get; set; }
    }
}
