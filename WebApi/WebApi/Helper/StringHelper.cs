using System.Linq;
using System;

namespace WebApi.Helper
{
    public class StringHelper
    {
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int ProdVariantIdHandle(string s)
        {
            // Lấy về một chuỗi có định dạng vd: "Id: 6 Áo bò" => lấy số 6 parse về kiểu int 
            string[] arrListStr = s.Split(" ");
            return int.Parse(arrListStr[1]);
        }
    }
}
