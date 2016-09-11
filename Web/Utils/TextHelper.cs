using System.Text;

namespace Web.Utils
{
    public class TextHelper
    {
        #region 截取字符串长度，超过长度时截取指定长度内容并加上后缀字符

        /// <summary>
        /// 截取字符串长度，超过长度时截取指定长度内容并加上后缀字符
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="maxLength">要截取的长度</param>
        /// <param name="endFlag">后缀字符</param>
        /// <returns>返回截取后的字符串内容</returns>
        public static string CutString(string value, int maxLength, string endFlag)
        {
            //如果字符串长度比需要截取的长度小,返回原字符串
            if ((maxLength <= 0) || (Encoding.Default.GetByteCount(value) <= maxLength))
            {
                return value;
            }
            else
            {
                int totalCount = 0;
                int len = value.Length;
                string everyChar;

                StringBuilder sbSubStr = new StringBuilder();

                for (int i = 0; i < len; i++)
                {
                    everyChar = value.Substring(i, 1);

                    //判断每个字符所占的字节数
                    int byteCount = Encoding.Default.GetByteCount(everyChar);

                    if (totalCount + byteCount > maxLength)
                    {
                        break;
                    }
                    else
                    {
                        totalCount += byteCount;
                        sbSubStr.Append(everyChar);
                    }
                }

                return (string.Format("{0}{1}", sbSubStr.ToString(), endFlag));
            }
        }

        #endregion

        #region 截取字符串长度，超过长度时截取指定长度内容并加上省略号

        /// <summary>
        /// 截取字符串长度，超过长度时截取指定长度内容并加上省略号
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="maxLength">要截取的长度</param>
        /// <returns>返回截取后的字符串内容</returns>
        public static string CutString(string value, int maxLength)
        {
            return CutString(value, maxLength, "...");
        }

        #endregion
    }
}