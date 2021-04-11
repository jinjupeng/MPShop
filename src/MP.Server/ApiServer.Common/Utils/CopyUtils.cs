using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApiServer.Common.Utils
{
    /// <summary>
    /// 对象拷贝工具类
    /// </summary>
    public static class CopyUtils
    {
        //字典缓存
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();

        #region 封装-反射的方式进行实体间的赋值
        /// <summary>
        /// 反射的方式进行实体间的赋值
        /// </summary>
        /// <typeparam name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut ReflectionMapper<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            //外层遍历获取【被赋值的实体类型】的属性
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                //内层遍历获取【赋值的实体类型】的属性
                foreach (var itemIn in tIn.GetType().GetProperties())
                {
                    if (itemOut.Name.Equals(itemIn.Name))
                    {
                        //特别注意这里：SetValue和GetValue的用法
                        itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                        break;
                    }
                }
            }
            return tOut;
        }
        #endregion

        #region 封装-序列化反序列化进行实体见的赋值
        /// <summary>
        /// 序列化反序列化进行实体见的赋值
        /// </summary>
        /// <typeparam name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut SerialzerMapper<TIn, TOut>(TIn tIn)
        {
            return JsonConvert.DeserializeObject<TOut>(JsonConvert.SerializeObject(tIn));
        }
        #endregion

        #region 封装-字典缓存+表达式目录树
        /// <summary>
        /// 序列化反序列化进行实体见的赋值
        /// </summary>
        /// <typeparam name="TIn">赋值的实体类型</typeparam>
        /// <typeparam name="TOut">被赋值的实体类型</typeparam>
        /// <param name="tIn"></param>
        public static TOut DicExpressionMapper<TIn, TOut>(TIn tIn)
        {
            string key = string.Format("funckey_{0}_{1}", typeof(TIn).FullName, typeof(TOut).FullName);
            if (!_Dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
                {
                    parameterExpression
                });
                Func<TIn, TOut> func = lambda.Compile();//拼装是一次性的
                _Dic[key] = func;
            }
            return ((Func<TIn, TOut>)_Dic[key]).Invoke(tIn);
        }
        #endregion

    }

    /// <summary>
    /// 泛型缓存
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class GenericExpressionMapper<TIn, TOut>
    {
        private static Func<TIn, TOut> _FUNC = null;
        static GenericExpressionMapper()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            foreach (var item in typeof(TOut).GetFields())
            {
                MemberExpression property = Expression.Field(parameterExpression, typeof(TIn).GetField(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                     parameterExpression
            });
            _FUNC = lambda.Compile();//拼装是一次性的
        }
        public static TOut Trans(TIn t)
        {
            return _FUNC(t);
        }
    }
}
