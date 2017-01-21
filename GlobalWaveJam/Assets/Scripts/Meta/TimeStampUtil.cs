using UnityEngine;
using System.Collections;

public class TimeStampUtil {

    // UTIL
    public static long ToUnixTime(System.DateTime date)
    {
        var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return System.Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
    }

    public static long Now()
    {
        return ToUnixTime(System.DateTime.Now);
    }

}
