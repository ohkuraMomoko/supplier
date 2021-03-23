using SupplierPlatform.Enums;
using SupplierPlatform.Helps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SupplierPlatform.Extensions
{
    public static class StringExtension
    {
        public static int? TryToInt(this string str)
        {
            int outPut;
            if (int.TryParse(str, out outPut))
                return outPut;

            return null;
        }
        /// <summary>
        /// Base64 解碼
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(this string code)
        {
            string decode = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                byte[] bytes = System.Convert.FromBase64String(code);
                decode = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
            }

            return decode;
        }

        /// <summary>
        /// Base64 編碼
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncodeBase64(this string text)
        {
            string encode = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                encode = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
            }

            return encode;
        }

        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SHA256(this string data)
        {
            using (System.Security.Cryptography.SHA256 mySHA256 = System.Security.Cryptography.SHA256.Create())
            {
                try
                {
                    // Compute the hash of the fileStream.
                    byte[] hashValue = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(data));
                    return Convert.ToBase64String(hashValue);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 檢核是否符合指定規則
        /// </summary>
        /// <param name="code">檢核傳入值</param>
        /// <param name="regexEnum">檢核規則 Enum</param>
        /// <param name="digi">限制位數</param>
        /// <returns>檢核是否通過</returns>
        public static bool Regex(this string code, RegexEnum regexEnum, int digi = 0)
        {
            if (regexEnum != RegexEnum.Phone)
            {
                if (code.Length != digi)
                {
                    return false;
                }
            }

            Regex regex = new Regex(EnumAttributeHelper.GetEnumDescription(regexEnum));

            return regex.IsMatch(code);
        }

        /// <summary>
        /// ConvertLatestCaseStatus
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ConvertLatestCaseStatus(this string status)
        {
            switch (status)
            {
                case "001": //尚未操作
                    return "1";

                case "002":  //審核中
                    return "2";

                case "006":  //婉拒(未通過)
                    return "3";

                case "021":  //取消案件
                    return "4";

                case "003":  //已核准未請款
                    return "5";

                case "004":  //請款中
                    return "6";

                case "005":  //已撥款
                    return "7";

                case "024":  //已核准處理中
                    return "8";

                case "022":  //退貨
                    return "9";

                case "023": // 調降金額處理中
                    return "10";

                default:
                    return "0";
            }
        }

        /// <summary>
        /// 一頁式電商使用的狀態對應
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ConvertOnePageStatus(this string status)
        {
            switch (status)
            {
                case "001": //尚未操作
                    return "1";

                case "002":  //審核中
                case "023": // 調降金額處理中
                    return "2";

                case "003":  //已核准未請款
                case "004":  //請款中
                case "005":  //已撥款
                case "006":  //婉拒(未通過)
                case "024":  //已核准處理中
                    return "3";

                case "021":  //取消案件
                case "022":  //退貨
                    return "4";

                default:
                    return "0";
            }
        }

        /// <summary>
        /// 一頁式電商使用的狀態對應
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<string> ConvertApiStatus(this OnePageStatusEnum status)
        {
            List<string> result = new List<string>();
            switch (status)
            {
                case OnePageStatusEnum.NoPayment: //顧客未操作付款
                    result.Add("001");//尚未操作
                    return result;

                case OnePageStatusEnum.UnderReview://審核中
                    result.Add("002");//審核中
                    result.Add("023");//調降金額處理中
                    return result;
                case OnePageStatusEnum.UnderReview_Second://審核中(核准/婉拒)
                    result.Add("003");//已核准未請款
                    result.Add("004");//請款中
                    result.Add("005");//已撥款
                    result.Add("006");//婉拒(未通過)
                    result.Add("024");//已核准處理中
                    return result;
                case OnePageStatusEnum.Cancel://已取消
                    result.Add("021");//取消案件
                    result.Add("022");//退貨
                    return result;
                default:
                    return new List<string> { };
            }
        }
    }
}