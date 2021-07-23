using System;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// 类      名：ColorHelper
    /// 功      能：提供从RGB到HSV/HSL色彩空间的相互转换
    /// 日      期：2015-02-08
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    public static class ColorHelper
    {
        public static Color AdjustBrightness(Color c1, double factor)
        {

            double r = ((c1.R * factor) > 255) ? 255 : (c1.R * factor);
            double g = ((c1.G * factor) > 255) ? 255 : (c1.G * factor);
            double b = ((c1.B * factor) > 255) ? 255 : (c1.B * factor);

            Color c = Color.FromArgb(c1.A, (byte)r, (byte)g, (byte)b);
            return c;

        }

        /// <summary>
        /// RGB转换HSV
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static ColorHSV RgbToHsv(ColorRGB rgb)
        {
            float min, max, tmp, H, S, V;
            float R = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
            tmp = Math.Min(R, G);
            min = Math.Min(tmp, B);
            tmp = Math.Max(R, G);
            max = Math.Max(tmp, B);
            // H
            H = 0;
            if (max == min)
            {
                H = 0;
            }
            else if (max == R && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if (max == R && G < B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 360;
            }
            else if (max == G)
            {
                H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
            }
            else if (max == B)
            {
                H = H = 60 * (R - G) * 1.0f / (max - min) + 240;
            }
            // S
            if (max == 0)
            {
                S = 0;
            }
            else
            {
                S = (max - min) * 1.0f / max;
            }
            // V
            V = max;
            return new ColorHSV((int)H, (int)(S * 255), (int)(V * 255));
        }

        /// <summary>
        /// HSV转换RGB
        /// </summary>
        /// <param name="hsv"></param>
        /// <returns></returns>
        public static ColorRGB HsvToRgb(ColorHSV hsv)
        {
            if (hsv.H == 360) hsv.H = 359; // 360为全黑，原因不明
            float R = 0f, G = 0f, B = 0f;
            if (hsv.S == 0)
            {
                return new ColorRGB(hsv.V, hsv.V, hsv.V);
            }
            float S = hsv.S * 1.0f / 255, V = hsv.V * 1.0f / 255;
            int H1 = (int)(hsv.H * 1.0f / 60), H = hsv.H;
            float F = H * 1.0f / 60 - H1;
            float P = V * (1.0f - S);
            float Q = V * (1.0f - F * S);
            float T = V * (1.0f - (1.0f - F) * S);
            switch (H1)
            {
                case 0: R = V; G = T; B = P; break;
                case 1: R = Q; G = V; B = P; break;
                case 2: R = P; G = V; B = T; break;
                case 3: R = P; G = Q; B = V; break;
                case 4: R = T; G = P; B = V; break;
                case 5: R = V; G = P; B = Q; break;
            }
            R = R * 255;
            G = G * 255;
            B = B * 255;
            while (R > 255) R -= 255;
            while (R < 0) R += 255;
            while (G > 255) G -= 255;
            while (G < 0) G += 255;
            while (B > 255) B -= 255;
            while (B < 0) B += 255;
            return new ColorRGB((int)R, (int)G, (int)B);
        }

        /// <summary>
        /// RGB转换HSL
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static ColorHSL RgbToHsl(ColorRGB rgb)
        {
            float min, max, tmp, H, S, L;
            float R = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
            tmp = Math.Min(R, G);
            min = Math.Min(tmp, B);
            tmp = Math.Max(R, G);
            max = Math.Max(tmp, B);
            // H
            H = 0;
            if (max == min)
            {
                H = 0;  // 此时H应为未定义，通常写为0
            }
            else if (max == R && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if (max == R && G < B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 360;
            }
            else if (max == G)
            {
                H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
            }
            else if (max == B)
            {
                H = H = 60 * (R - G) * 1.0f / (max - min) + 240;
            }
            // L 
            L = 0.5f * (max + min);
            // S
            S = 0;
            if (L == 0 || max == min)
            {
                S = 0;
            }
            else if (0 < L && L < 0.5)
            {
                S = (max - min) / (L * 2);
            }
            else if (L > 0.5)
            {
                S = (max - min) / (2 - 2 * L);
            }
            return new ColorHSL((int)H, (int)(S * 255), (int)(L * 255));
        }

        /// <summary>
        /// HSL转换RGB
        /// </summary>
        /// <param name="hsl"></param>
        /// <returns></returns>
        public static ColorRGB HslToRgb(ColorHSL hsl)
        {
            float R = 0f, G = 0f, B = 0f;
            float S = hsl.S * 1.0f / 255, L = hsl.L * 1.0f / 255;
            float temp1, temp2, temp3;
            if (S == 0f) // 灰色
            {
                R = L;
                G = L;
                B = L;
            }
            else
            {
                if (L < 0.5f)
                {
                    temp2 = L * (1.0f + S);
                }
                else
                {
                    temp2 = L + S - L * S;
                }
                temp1 = 2.0f * L - temp2;
                float H = hsl.H * 1.0f / 360;
                // R
                temp3 = H + 1.0f / 3.0f;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                R = temp3;
                // G
                temp3 = H;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                G = temp3;
                // B
                temp3 = H - 1.0f / 3.0f;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                B = temp3;
            }
            R = R * 255;
            G = G * 255;
            B = B * 255;
            return new ColorRGB((int)R, (int)G, (int)B);
        }

        /// <summary>
        /// RGB转换HSV
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static ColorHSV ColorToHsv(Color rgb)
        {
            float min, max, tmp, H, S, V;
            float R = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
            tmp = Math.Min(R, G);
            min = Math.Min(tmp, B);
            tmp = Math.Max(R, G);
            max = Math.Max(tmp, B);
            // H
            H = 0;
            if (max == min)
            {
                H = 0;
            }
            else if (max == R && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if (max == R && G < B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 360;
            }
            else if (max == G)
            {
                H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
            }
            else if (max == B)
            {
                H = H = 60 * (R - G) * 1.0f / (max - min) + 240;
            }
            // S
            if (max == 0)
            {
                S = 0;
            }
            else
            {
                S = (max - min) * 1.0f / max;
            }
            // V
            V = max;
            return new ColorHSV((int)H, (int)(S * 255), (int)(V * 255));
        }

        /// <summary>
        /// HSV转换RGB
        /// </summary>
        /// <param name="hsv"></param>
        /// <returns></returns>
        public static Color HsvToColor(ColorHSV hsv)
        {
            if (hsv.H == 360) hsv.H = 359; // 360为全黑，原因不明
            float R = 0f, G = 0f, B = 0f;
            if (hsv.S == 0)
            {
                return Color.FromArgb(0xff, (byte)hsv.V, (byte)hsv.V, (byte)hsv.V);
            }
            float S = hsv.S * 1.0f / 255, V = hsv.V * 1.0f / 255;
            int H1 = (int)(hsv.H * 1.0f / 60), H = hsv.H;
            float F = H * 1.0f / 60 - H1;
            float P = V * (1.0f - S);
            float Q = V * (1.0f - F * S);
            float T = V * (1.0f - (1.0f - F) * S);
            switch (H1)
            {
                case 0: R = V; G = T; B = P; break;
                case 1: R = Q; G = V; B = P; break;
                case 2: R = P; G = V; B = T; break;
                case 3: R = P; G = Q; B = V; break;
                case 4: R = T; G = P; B = V; break;
                case 5: R = V; G = P; B = Q; break;
            }
            R = R * 255;
            G = G * 255;
            B = B * 255;
            while (R > 255) R -= 255;
            while (R < 0) R += 255;
            while (G > 255) G -= 255;
            while (G < 0) G += 255;
            while (B > 255) B -= 255;
            while (B < 0) B += 255;
            return Color.FromArgb(0xff, (byte)R, (byte)G, (byte)B);
        }

        /// <summary>
        /// RGB转换HSL
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static ColorHSL ColorToHsl(Color rgb)
        {
            float min, max, tmp, H, S, L;
            float R = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
            tmp = Math.Min(R, G);
            min = Math.Min(tmp, B);
            tmp = Math.Max(R, G);
            max = Math.Max(tmp, B);
            // H
            H = 0;
            if (max == min)
            {
                H = 0;  // 此时H应为未定义，通常写为0
            }
            else if (max == R && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if (max == R && G < B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 360;
            }
            else if (max == G)
            {
                H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
            }
            else if (max == B)
            {
                H = H = 60 * (R - G) * 1.0f / (max - min) + 240;
            }
            // L 
            L = 0.5f * (max + min);
            // S
            S = 0;
            if (L == 0 || max == min)
            {
                S = 0;
            }
            else if (0 < L && L < 0.5)
            {
                S = (max - min) / (L * 2);
            }
            else if (L > 0.5)
            {
                S = (max - min) / (2 - 2 * L);
            }
            return new ColorHSL((int)H, (int)(S * 255), (int)(L * 255));
        }

        /// <summary>
        /// HSL转换RGB
        /// </summary>
        /// <param name="hsl"></param>
        /// <returns></returns>
        public static Color HslToColor(ColorHSL hsl)
        {
            float R = 0f, G = 0f, B = 0f;
            float S = hsl.S * 1.0f / 255, L = hsl.L * 1.0f / 255;
            float temp1, temp2, temp3;
            if (S == 0f) // 灰色
            {
                R = L;
                G = L;
                B = L;
            }
            else
            {
                if (L < 0.5f)
                {
                    temp2 = L * (1.0f + S);
                }
                else
                {
                    temp2 = L + S - L * S;
                }
                temp1 = 2.0f * L - temp2;
                float H = hsl.H * 1.0f / 360;
                // R
                temp3 = H + 1.0f / 3.0f;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                R = temp3;
                // G
                temp3 = H;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                G = temp3;
                // B
                temp3 = H - 1.0f / 3.0f;
                if (temp3 < 0) temp3 += 1.0f;
                if (temp3 > 1) temp3 -= 1.0f;
                B = temp3;
            }
            R = R * 255;
            G = G * 255;
            B = B * 255;
            return Color.FromArgb(0xff, (byte)R, (byte)G, (byte)B);
        }
    }

    #region RGB / HSV / HSL 颜色模型类
    /// <summary>
    /// 类      名：ColorHSL
    /// 功      能：H 色相 \ S 饱和度(纯度) \ L 亮度 颜色模型 
    /// 日      期：2015-02-08
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    public class ColorHSL
    {
        public ColorHSL(int h, int s, int l)
        {
            this._h = h;
            this._s = s;
            this._l = l;
        }

        private int _h;
        private int _s;
        private int _l;

        /// <summary>
        /// 色相
        /// </summary>
        public int H
        {
            get { return this._h; }
            set
            {
                this._h = value;
                this._h = this._h > 360 ? 360 : this._h;
                this._h = this._h < 0 ? 0 : this._h;
            }
        }

        /// <summary>
        /// 饱和度(纯度)
        /// </summary>
        public int S
        {
            get { return this._s; }
            set
            {
                this._s = value;
                this._s = this._s > 255 ? 255 : this._s;
                this._s = this._s < 0 ? 0 : this._s;
            }
        }

        /// <summary>
        /// 饱和度
        /// </summary>
        public int L
        {
            get { return this._l; }
            set
            {
                this._l = value;
                this._l = this._l > 255 ? 255 : this._l;
                this._l = this._l < 0 ? 0 : this._l;
            }
        }
    }

    /// <summary>
    /// 类      名：ColorHSV
    /// 功      能：H 色相 \ S 饱和度(纯度) \ V 明度 颜色模型 
    /// 日      期：2015-01-22
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    public class ColorHSV
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        public ColorHSV(int h, int s, int v)
        {
            this._h = h;
            this._s = s;
            this._v = v;
        }

        private int _h;
        private int _s;
        private int _v;

        /// <summary>
        /// 色相
        /// </summary>
        public int H
        {
            get { return this._h; }
            set
            {
                this._h = value;
                this._h = this._h > 360 ? 360 : this._h;
                this._h = this._h < 0 ? 0 : this._h;
            }
        }

        /// <summary>
        /// 饱和度(纯度)
        /// </summary>
        public int S
        {
            get { return this._s; }
            set
            {
                this._s = value;
                this._s = this._s > 255 ? 255 : this._s;
                this._s = this._s < 0 ? 0 : this._s;
            }
        }

        /// <summary>
        /// 明度
        /// </summary>
        public int V
        {
            get { return this._v; }
            set
            {
                this._v = value;
                this._v = this._v > 255 ? 255 : this._v;
                this._v = this._v < 0 ? 0 : this._v;
            }
        }
    }

    /// <summary>
    /// 类      名：ColorRGB
    /// 功      能：R 红色 \ G 绿色 \ B 蓝色 颜色模型
    ///                 所有颜色模型的基类，RGB是用于输出到屏幕的颜色模式，所以所有模型都将转换成RGB输出
    /// 日      期：2015-01-22
    /// 修      改：2015-03-20
    /// 作      者：ls9512
    /// </summary>
    public class ColorRGB
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public ColorRGB(int r, int g, int b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        private int _r;
        private int _g;
        private int _b;

        /// <summary>
        /// 红色
        /// </summary>
        public int R
        {
            get { return this._r; }
            set
            {
                this._r = value;
                this._r = this._r > 255 ? 255 : this._r;
                this._r = this._r < 0 ? 0 : this._r;
            }
        }

        /// <summary>
        /// 绿色
        /// </summary>
        public int G
        {
            get { return this._g; }
            set
            {
                this._g = value;
                this._g = this._g > 255 ? 255 : this._g;
                this._g = this._g < 0 ? 0 : this._g;
            }
        }

        /// <summary>
        /// 蓝色
        /// </summary>
        public int B
        {
            get { return this._b; }
            set
            {
                this._b = value;
                this._b = this._b > 255 ? 255 : this._b;
                this._b = this._b < 0 ? 0 : this._b;
            }
        }

        /// <summary>
        /// 获取实际颜色
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            return Color.FromArgb(0xff, (byte)this._r, (byte)this._g, (byte)this._b);
        }
    }
    #endregion
}
