using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            //找到存储插件的目录
            string strPath = Path.Combine(Environment.CurrentDirectory, "Animals");
            //获取插件目录中所有的插件(dll)
            string[] strings = Directory.GetFiles(strPath);
            //声明一个Type的list集合
            List<Type> types = new List<Type>();
            //遍历插件目录中所有的插件(dll)
            foreach (string str in strings)
            {
                //加载插件的程序集
                Assembly loadFromAssemblyPath = AssemblyLoadContext.Default.LoadFromAssemblyPath(str);
                //获取程序集的类型
                Type[] types1 = loadFromAssemblyPath.GetTypes();
                //遍历类型
                foreach (Type type in types1)
                {
                    //判断是否有Voice方法
                    if (type.GetMethod("Voice") != null)
                    {
                        //添加类型
                        types.Add(type);
                    }
                }
            }

            //迎合业务需要,将此设置为死循环(实际情况中是合理的)
            while (true)
            {
                //遍历打印类型
                for (int i = 0; i < types.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{types[i].Name}");
                }

                Console.WriteLine("**************************************************");

                Console.WriteLine("请选择动物:");
                int index = int.Parse(Console.ReadLine());
                //判断输入的是否合理
                if (index > types.Count || index < 1)
                {
                    Console.WriteLine("没有这样的动物,请重试...");
                    continue;
                }

                int times = int.Parse(Console.ReadLine());
                //获取下标所在的元素
                Type type = types[index - 1];
                //获取方法名
                MethodInfo methodInfo = type.GetMethod("Voice");
                //根据反射创建类型实例
                object instance = Activator.CreateInstance(type);
                //调用实例的方法和显示出现的次数
                methodInfo.Invoke(instance, new object[] { times });

                Console.WriteLine(".............结束............");
                Console.WriteLine();
            }
        }
    }
}
