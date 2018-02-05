
namespace CustomSkin.Windows.Win32
{
    /// <summary>
    /// 显示或隐藏窗口时，产生特殊的效果的类型。
    /// </summary>
    public enum AnimateWindowFlags
    {
        /// <summary>
        /// 激活窗口。 请不要使用此值与AW_HIDE
        /// </summary>
        AW_ACTIVATE = 0x00020000,
        /// <summary>
        /// 采用了淡入淡出效果。 该标志只能用于如果hwnd是一个顶层窗口。
        /// </summary>
        AW_BLEND = 0x00080000,
        /// <summary>
        /// 使窗口出现向内坍塌如果AW_HIDE使用或向外扩展，如果不使用该AW_HIDE。 各个方向的标志没有任何效果。
        /// </summary>
        AW_CENTER = 0x00000010,
        /// <summary>
        /// 隐藏窗口。 默认情况下，显示的窗口。
        /// </summary>
        AW_HIDE = 0x00010000,
        /// <summary>
        /// 动画窗口从左侧到右侧。 此标志可用于滚动或滑动动画。 与AW_CENTER或AW_BLEND使用时，它会被忽略
        /// </summary>
        AW_HOR_POSITIVE = 0x00000001,
        /// <summary>
        /// 从右向左的动画的窗口。 此标志可用于滚动或滑动动画。 与AW_CENTER或AW_BLEND使用时，它会被忽略。
        /// </summary>
        AW_HOR_NEGATIVE = 0x00000002,
        /// <summary>
        /// 使用幻灯片动画。 默认情况下，滚动动画使用。 与AW_CENTER使用时，这个标志被忽略。 
        /// </summary>
        AW_SLIDE = 0x00040000,
        /// <summary>
        /// 动画窗口从上到下。 此标志可用于滚动或滑动动画。 与AW_CENTER或AW_BLEND使用时，它会被忽略。
        /// </summary>
        AW_VER_POSITIVE = 0x00000004,
        /// <summary>
        /// 动画窗口从底部到顶部。 此标志可用于滚动或滑动动画。 与AW_CENTER或AW_BLEND使用时，它会被忽略。
        /// </summary>
        AW_VER_NEGATIVE = 0x00000008
    }
}
