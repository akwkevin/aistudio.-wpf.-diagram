using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Util.DiagramDesigner
{
    public class DesignerHelper
    {
        private static bool? _isInDesignMode;

        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                }
                return _isInDesignMode.Value;
            }
        }

        #region IsInMainThread

        /// <summary>
        /// 是否是在主线程中处理
        /// </summary>
        public static bool IsInMainThread
        {
            get
            {
                if (Thread.CurrentThread.IsBackground || Thread.CurrentThread.IsThreadPoolThread) return false;

                if (Thread.CurrentThread.Name == "主线程") return true;

                if (Application.Current == null)
                    return true;

                return Thread.CurrentThread == Application.Current.Dispatcher.Thread;
            }
        }

        #endregion
    }
}