namespace MWindowLib.Native
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// <summary>structure models the coordinates of a rectangle.</summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
        /// <summary>left minimum coordinate of the rectangle.</summary>
        public int left;

        /// <summary>top minimum coordinate of the rectangle.</summary>
        public int top;

        /// <summary>right maximum coordinate of the rectangle.</summary>
        public int right;

        /// <summary>bottom maximum coordinate of the rectangle.</summary>
        public int bottom;

        /// <summary>Gets a rectangle that has no coordinates set.</summary>
        public static readonly RECT Empty = new RECT();

        /// <summary>Gets the width of the rectangle.</summary>
        public int Width
        {
            get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
        }

        /// <summary>Gets the height of the rectangle.</summary>
        public int Height
        {
            get { return bottom - top; }
        }

        /// <summary>Constructs a rectangle with the specified coordinates.</summary>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>Copy constructs from parameter.</summary>
        public RECT(RECT rcSrc)
        {
            left = rcSrc.left;
            top = rcSrc.top;
            right = rcSrc.right;
            bottom = rcSrc.bottom;
        }

        /// <summary>Determines whether coordinates for a rect have been set or not.</summary>
        public bool IsEmpty
        {
            get
            {
                // BUGBUG : On Bidi OS (hebrew arabic) left > right
                return left >= right || top >= bottom;
            }
        }

        /// <summary>Standard to string method.</summary>
        public override string ToString()
        {
            if (this == Empty)
                return "RECT {Empty}";
            return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
        }

        /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Rect)) { return false; }
            return (this == (RECT)obj);
        }

        /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
        public override int GetHashCode()
        {
            return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
        }

        /// <summary> Determines if 2 RECTs refer to the same coordinates or not. </summary>
        public static bool operator ==(RECT rect1, RECT rect2)
        {
            return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
        }

        /// <summary> Determines if 2 RECTs refer to different coordinates or not. </summary>
        public static bool operator !=(RECT rect1, RECT rect2)
        {
            return !(rect1 == rect2);
        }
    }
}
