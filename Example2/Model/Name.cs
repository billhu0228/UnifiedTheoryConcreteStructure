
/********************作者：黄昏前黎明后************************************
*   作者：黄昏前黎明后
*   CLR版本：4.0.30319.42000
*   创建时间：2018-04-01 13:52:52
*   命名空间：Example1
*   唯一标识：617472f7-cdf9-49f9-afa4-7655f1f5774a
*   机器名称：HLPC
*   联系人邮箱：hl@cn-bi.com
* 
*   描述说明：
*
*   修改历史：
*
*
*****************************************************************/
namespace Example2.Test
{
    public class Name
    {
        #region 字段
        string _userName;
        string _companyName;
        #endregion

        #region 属性
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }
        #endregion
    }
}
