using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupplierPlatform.Extension
{
    public static class ExtensionOfCollections
    {
        /// <summary>
        /// 將單筆 int、long、string 等型別轉成 list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static List<T> ToListCollection<T>(this T inputValue) where T : IComparable
        {
            List<T> listResult = new List<T>();
            listResult.Add(inputValue);

            return listResult;
        }

        /// <summary>
        /// 判斷List是否不為空 (不為null ＆＆　筆數大於０)
        /// </summary>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return false; // or throw an exception

            return source.Any();
        }

        /// <summary>
        /// 判斷List是否為null或Count == 0
        /// </summary>
        public static bool IsEmptyOrNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception

            if (source.Any())
                return false;

            return true;
        }
    }
}