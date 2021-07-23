using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Util.DiagramDesigner
{
    public class CopyHelper
    {
        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj == null || obj is string || obj.GetType().IsValueType) return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }

        public static T AutoCopy<T>(T source) where T : new()
        {
            T target = new T();
            var Properties = typeof(T).GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(target, Propertie.GetValue(source, null), null);
                }
            }
            return target;
        }

        public static T DeepCopy<T>(T obj)
        {
            if (obj == null) return obj;

            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于初始化新实体
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="s">数据源实体</param>
        /// <returns>返回的新实体</returns>
        public static D Mapper<D, S>(S s)
        {
            D d = Activator.CreateInstance<D>(); //构造新实例
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties().Where(p => p.CanRead))//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties().Where(p => p.CanWrite))
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType)//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }

        public static void CopyPropertyValue(object s, object d, string propertyName = null)
        {
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = d.GetType();
                var sps = Types.GetProperties().Where(p => p.CanRead && (string.IsNullOrEmpty(propertyName) || p.Name == propertyName));//获得类型的属性字段  
                var dps = Typed.GetProperties().Where(p => p.CanWrite && (string.IsNullOrEmpty(propertyName) || p.Name == propertyName));

                foreach (PropertyInfo sp in sps)//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in dps)
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType)//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ColorViewModel Mapper(IColorViewModel s)
        {
            var d = CopyHelper.Mapper<ColorViewModel, IColorViewModel>(s);
            d.LineColor = CopyHelper.Mapper<ColorObject, IColorObject>(s.LineColor);
            d.FillColor = CopyHelper.Mapper<ColorObject, IColorObject>(s.FillColor);
            d.LineColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.LineColor.GradientStop);
            d.FillColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.FillColor.GradientStop);
            return d;
        }

        public static T Mapper<T>(IColorViewModel s) where T: IColorViewModel
        {
            var d = CopyHelper.Mapper<T, IColorViewModel>(s);
            d.LineColor = CopyHelper.Mapper<ColorObjectItem, IColorObject>(s.LineColor);
            d.FillColor = CopyHelper.Mapper<ColorObjectItem, IColorObject>(s.FillColor);
            d.LineColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.LineColor.GradientStop);
            d.FillColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.FillColor.GradientStop);
            return d;
        }

        public static void CopyPropertyValue(IColorViewModel s, IColorViewModel d, string propertyName = null)
        {
            if (propertyName == "LineColor")
            {
                CopyPropertyValue(s.LineColor, d.LineColor);
                d.LineColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.LineColor.GradientStop);
            }
            else if (propertyName == "FillColor")
            {
                CopyPropertyValue(s.FillColor, d.FillColor);
                d.FillColor.GradientStop = CopyHelper.DeepCopy<ObservableCollection<GradientStop>>(s.FillColor.GradientStop);
            }
            else
            {
                CopyPropertyValue((object)s, (object)d, propertyName);
            }
           
        }
    }
}
