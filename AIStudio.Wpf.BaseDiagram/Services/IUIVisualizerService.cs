﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Wpf.BaseDiagram.Services
{
    /// <summary>
    /// This interface defines a UI controller which can be used to display dialogs
    /// in either modal form from a ViewModel.
    /// </summary>
    public interface IUIVisualizerService
    {
        /// <summary>
        /// This method displays a modal dialog associated with the given key.
        /// </summary>
        /// <param name="dataContextForPopup">Object state to associate with the dialog</param>
        /// <returns>True/False if UI is displayed.</returns>
        bool? ShowDialog(object dataContextForPopup);
    }
}
