using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Util.DiagramDesigner
{
    //[Serializable]
    //public abstract class BindableBase : INotifyPropertyChanged
    //{
    //    #region INotifyPropertyChanged Implementation
    //    /// <summary>
    //    /// Occurs when any properties are changed on this object.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged
    //    {
    //        add { this.propertyChanged += value; }
    //        remove { this.propertyChanged -= value; }
    //    }

    //    protected event PropertyChangedEventHandler propertyChanged;


    //    /// <summary>
    //    /// A helper method that raises the PropertyChanged event for a property.
    //    /// </summary>
    //    /// <param name="propertyNames">The names of the properties that changed.</param>
    //    public virtual void NotifyChanged(params string[] propertyNames)
    //    {
    //        foreach (string name in propertyNames)
    //        {
    //            OnPropertyChanged(new PropertyChangedEventArgs(name));
    //        }
    //    }

    //    /// <summary>
    //    /// Raises the PropertyChanged event.
    //    /// </summary>
    //    /// <param name="e">Event arguments.</param>
    //    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    //    {
    //        if (this.propertyChanged != null)
    //        {
    //            this.propertyChanged(this, e);
    //        }
    //    }
    //    #endregion

    //    public IObservable<string> WhenPropertyChanged
    //    {
    //        get
    //        {
    //            return Observable
    //                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
    //                    h => this.propertyChanged += h,
    //                    h => this.propertyChanged -= h)
    //                .Select(x => x.EventArgs.PropertyName);
    //        }
    //    }
    //}
}
