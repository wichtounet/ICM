using System;

namespace ICM.Utils
{
    public static class Extensions
    {
        public static int ToInt(this String str)
        {
            return Convert.ToInt16(str);
        }

        public static int? ToIntStrict(this String str)
        {
            if (str == null)
            {
                return null;
            }

            return Convert.ToInt16(str);
        }
    }
}
