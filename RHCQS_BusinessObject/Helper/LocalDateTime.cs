using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Helper
{
    public class LocalDateTime
    {
        public static DateTime VNDateTime()
        {
            // Định nghĩa múi giờ Việt Nam
            DateTime utcNow = DateTime.UtcNow;

            // Đảm bảo giờ được chuyển về UTC+7 (bất kể giờ hiện tại là UTC nào)
            return utcNow.AddHours(7);
        }
    }
}
