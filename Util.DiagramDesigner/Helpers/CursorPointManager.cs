using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// win8下wpf程序测试成功
    /// </summary>
    public class CursorPointManager
    {
        #region 得到光标在屏幕上的位置
        [DllImport("user32")]
        private static extern bool GetCursorPos(out Point lpPoint);

        /// <summary>
        /// 获取光标相对于显示器的位置
        /// </summary>
        /// <returns></returns>
        public static Point GetCursorPosition()
        {
            Point showPoint = new Point();
            GetCursorPos(out showPoint);
            return showPoint;
        }
        #endregion

    }
}
