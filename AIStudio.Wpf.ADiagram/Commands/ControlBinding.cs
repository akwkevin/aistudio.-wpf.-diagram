using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace AIStudio.Wpf.ADiagram.Commands
{
    [MarkupExtensionReturnType(typeof(object))]
    public class ControlBinding : MarkupExtension
    {
        /// <summary>
        /// 是否禁止处理绑定
        /// </summary>
        public static bool Disabled = false;

        #region Properties

        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public bool NotifyOnValidationError { get; set; }
        public object TargetNullValue { get; set; }
        public object FallBackValue { get; set; }
        public string StringFormat { get; set; }
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        public Collection<ValidationRule> ValidationRules { get; set; }

        public BindingMode Mode { get; set; }
        private int AncestorLevel = 1;
        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        #endregion

        public ControlBinding()
        {
            Mode = BindingMode.Default;
            ValidationRules = new Collection<ValidationRule>();
        }

        public ControlBinding(PropertyPath path)
        {
            Path = path;
            Mode = BindingMode.Default;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            NotifyOnValidationError = true;
            ValidatesOnDataErrors = true;
            ValidatesOnExceptions = true;
            TargetNullValue = null;
            ValidationRules = new Collection<ValidationRule>();
        }

        public ControlBinding(PropertyPath path, string elementName)
            : this(path)
        {
            Path = path;

            if (elementName == "$")
                AncestorLevel = 2;
            else
                ElementName = elementName;
        }

        /// <summary>
        /// 获取代理Binding对象
        /// </summary>
        /// <returns></returns>
        internal Binding GetBinding(object rootObject)
        {
            Binding binding = new Binding();
            binding.StringFormat = StringFormat;
            binding.Path = Path;
            binding.Mode = Mode;
            binding.Converter = Converter;
            binding.ConverterCulture = ConverterCulture;
            binding.ConverterParameter = ConverterParameter;
            binding.UpdateSourceTrigger = UpdateSourceTrigger;
            if (TargetNullValue != null) binding.TargetNullValue = TargetNullValue;
            if (FallBackValue != null)
                binding.FallbackValue = FallBackValue;
            //单向绑定不需要进行数据校验
            NotifyOnValidationError &= (Mode != BindingMode.OneWay && Mode != BindingMode.OneTime);

            binding.ValidatesOnDataErrors = NotifyOnValidationError;
            binding.ValidatesOnExceptions = NotifyOnValidationError;
            binding.NotifyOnValidationError = NotifyOnValidationError;


            //添加ValidationRule
            foreach (ValidationRule vr in ValidationRules)
                binding.ValidationRules.Add(vr);

            if (ElementName != null)
                binding.ElementName = ElementName;
            if (Source != null)
            {
                if (!(Source is string) || (Source as string) != "$")
                    binding.Source = Source;
            }
            if (RelativeSource != null)
                binding.RelativeSource = RelativeSource;

            if (string.IsNullOrEmpty(ElementName) && Source == null && RelativeSource == null)
            {
                if (rootObject == null)
                {
                    RelativeSource = new RelativeSource { AncestorLevel = AncestorLevel, AncestorType = typeof(UserControl), Mode = RelativeSourceMode.FindAncestor };

                    binding.RelativeSource = RelativeSource;
                }
                else
                {
                    binding.Source = rootObject;
                    if (rootObject is UserControl || rootObject is Window)
                    {
                        if (binding.Path == null || binding.Path.Path == ".")
                            binding.Path = new PropertyPath("DataContext");
                        else
                            binding.Path = new PropertyPath("DataContext." + binding.Path.Path, binding.Path.PathParameters);
                    }
                }
            }

            return binding;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider == null)
                return null;


            //当使用ControlMultiBinding或者MultiBinding时
            if (valueProvider.TargetProperty == null && valueProvider.TargetObject is ICollection)
            {
                if (valueProvider.TargetObject is Collection<ControlBinding>
                    || valueProvider.TargetObject.GetType().FullName == "MS.Internal.Data.BindingCollection")
                    return this;
            }

            //如果禁用
            if (ControlBinding.Disabled)
                return (valueProvider.TargetProperty as DependencyProperty).DefaultMetadata.DefaultValue;

            var rootObjProvider = serviceProvider.GetService(typeof(System.Xaml.IRootObjectProvider)) as System.Xaml.IRootObjectProvider;

            #region 如果是针对WinForm的ControlBinding

            if (valueProvider.TargetObject is System.Windows.Forms.Control && rootObjProvider != null)
            {
                System.Windows.Forms.Control ctl = valueProvider.TargetObject as System.Windows.Forms.Control;
                System.Windows.Forms.Binding b = new System.Windows.Forms.Binding((valueProvider.TargetProperty as PropertyInfo).Name, rootObjProvider.RootObject, Path.Path);
                if (Mode == BindingMode.OneWay)
                {
                    b.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.OnPropertyChanged;
                    b.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
                }
                else if (Mode == BindingMode.OneWayToSource)
                {
                    b.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.Never;
                    if (UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.PropertyChanged
                        || UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.Default)
                        b.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged;
                    else if (UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus)
                        b.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.OnValidation;
                }
                else if (Mode == BindingMode.TwoWay || Mode == BindingMode.Default)
                {
                    b.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.OnPropertyChanged;
                    b.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged;
                }
                else if (Mode == BindingMode.OneTime)
                {
                    //这里应该赋值一次:OneTime
                    //......

                    b.ControlUpdateMode = System.Windows.Forms.ControlUpdateMode.Never;
                    b.DataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
                }

                ctl.DataBindings.Add(b);

                return null;
            }

            #endregion

            var bindingTarget = valueProvider.TargetObject as DependencyObject;
            var bindingProperty = valueProvider.TargetProperty as DependencyProperty;
            if (bindingProperty == null)
                return null;

            #region 使用代理Binding返回最终的值

            //得到Binding对象
            Binding binding = GetBinding(rootObjProvider == null ? null : rootObjProvider.RootObject);
            if (binding == null) return null;

            object retValue = binding.ProvideValue(serviceProvider);

            #endregion

            return retValue;
        }
    }

    [ContentProperty("Bindings")]
    public class ControlMultiBinding : MarkupExtension
    {
        #region Properties

        public Collection<ControlBinding> Bindings { get; private set; }

        public IMultiValueConverter Converter { get; set; }

        [DefaultValue("")]
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        [DefaultValue("")]
        public object ConverterParameter { get; set; }

        public BindingMode Mode { get; set; }

        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated { get; set; }

        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated { get; set; }

        [DefaultValue(false)]
        public bool NotifyOnValidationError { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        [DefaultValue(false)]
        public bool ValidatesOnDataErrors { get; set; }

        [DefaultValue(false)]
        public bool ValidatesOnExceptions { get; set; }

        public Collection<ValidationRule> ValidationRules { get; private set; }

        #endregion

        public ControlMultiBinding()
        {
            Bindings = new Collection<ControlBinding>();
            ValidationRules = new Collection<ValidationRule>();
            Mode = BindingMode.Default;
            UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.Default;
        }

        /// <summary>
        /// 获取代理MultiBindings对象
        /// </summary>
        /// <returns></returns>
		internal MultiBinding GetMultiBindings(object rootObject)
        {
            MultiBinding multiBindings = new MultiBinding();

            if (this.Converter != null)
                multiBindings.Converter = this.Converter;
            if (this.ConverterCulture != null)
                multiBindings.ConverterCulture = this.ConverterCulture;
            if (this.ConverterParameter != null)
                multiBindings.ConverterParameter = this.ConverterParameter;
            if (this.UpdateSourceExceptionFilter != null)
                multiBindings.UpdateSourceExceptionFilter = this.UpdateSourceExceptionFilter;
            multiBindings.Mode = this.Mode;
            multiBindings.NotifyOnSourceUpdated = this.NotifyOnSourceUpdated;
            multiBindings.NotifyOnTargetUpdated = this.NotifyOnTargetUpdated;
            multiBindings.NotifyOnValidationError = this.NotifyOnValidationError;
            multiBindings.UpdateSourceTrigger = this.UpdateSourceTrigger;
            multiBindings.ValidatesOnDataErrors = this.ValidatesOnDataErrors;
            multiBindings.ValidatesOnExceptions = this.ValidatesOnExceptions;

            foreach (object bd in Bindings)
            {
                if (bd is BindingBase)
                    multiBindings.Bindings.Add(bd as BindingBase);
                else if (bd is ControlBinding)
                    multiBindings.Bindings.Add((bd as ControlBinding).GetBinding(rootObject) as BindingBase);
            }

            foreach (ValidationRule vr in ValidationRules)
                multiBindings.ValidationRules.Add(vr);

            return multiBindings;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider == null)
                return null;

            var bindingTarget = valueProvider.TargetObject as DependencyObject;
            var bindingProperty = valueProvider.TargetProperty as DependencyProperty;
            if (bindingProperty == null)
                return null;

            //使用MultiBindings返回最终的值
            #region 使用代理Binding返回最终的值

            var rootObjProvider = serviceProvider.GetService(typeof(System.Xaml.IRootObjectProvider)) as System.Xaml.IRootObjectProvider;

            MultiBinding multiBindings = GetMultiBindings(rootObjProvider == null ? null : rootObjProvider.RootObject);
            if (multiBindings == null) return null;

            object retValue = multiBindings.ProvideValue(serviceProvider);

            #endregion

            return retValue;
        }
    }
}
