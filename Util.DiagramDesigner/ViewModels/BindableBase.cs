using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            if (propertyName == "IsSelected")
            {

            }
            else if (this.ContainsProperty("IsReadOnly"))
            {
                if (object.Equals(this.GetPropertyValue("IsReadOnly"), true))
                    return false;
            }

            var old = storage;
            storage = value;
            RaisePropertyChanged(propertyName, old, value);

            return true;
        }

        private Dictionary<string, object> _oldDic = new Dictionary<string, object>();
        public void SetOldValue<T>(T value, string propertyName, string guid = null)
        {
            _oldDic[propertyName + guid ?? string.Empty] = value;
        }

        public T GetOldValue<T>(string propertyName, string guid = null)
        {
            if (_oldDic.ContainsKey(propertyName + guid ?? string.Empty))
            {
                var old = (T)_oldDic[propertyName + guid ?? string.Empty];
                return old;
            }
            else
            {
                return default(T);
            }
        }

        public void ClearOldValue<T>(string propertyName, string guid = null)
        {
            if (_oldDic.ContainsKey(propertyName + guid ?? string.Empty))
            {
                _oldDic.Remove(propertyName + guid ?? string.Empty);
            }
        }

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <param name="onChanged">Action that is called after the property value has been changed.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null, object oldvalue = null, object newvalue = null)
        {
            OnPropertyChanged(new ValuePropertyChangedEventArgs(propertyName, oldvalue, newvalue));
        }

        protected void RaisePropertyChanged(object sender, [CallerMemberName] string propertyName = null, object oldvalue = null, object newvalue = null)
        {
            PropertyChanged?.Invoke(sender, new ValuePropertyChangedEventArgs(propertyName, oldvalue, newvalue));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public IObservable<string> WhenPropertyChanged
        {
            get
            {
                return Observable
                    .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => this.PropertyChanged += h,
                        h => this.PropertyChanged -= h)
                    .Select(x => x.EventArgs.PropertyName);
            }
        }
    }

    public class ValuePropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public ValuePropertyChangedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue { get; set; }
        public object NewValue { get; set; }

    }



}