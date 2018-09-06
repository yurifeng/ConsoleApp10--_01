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
            string strPath = Path.Combine(Environment.CurrentDirectory, "Animals");
            string[] strings = Directory.GetFiles(strPath);
            List<Type> types = new List<Type>();
            foreach (string str in strings)
            {
                Assembly loadFromAssemblyPath = AssemblyLoadContext.Default.LoadFromAssemblyPath(str);
                Type[] types1 = loadFromAssemblyPath.GetTypes();
                foreach (Type type in types1)
                {
                    if (type.GetMethod("Voice") != null)
                    {
                        types.Add(type);
                    }
                }
            }

            while (true)
            {
                for (int i = 0; i < types.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{types[i].Name}");
                }

                Console.WriteLine("**************************************************");

                Console.WriteLine("请选择动物:");
                int index = int.Parse(Console.ReadLine());
                if (index > types.Count || index < 1)
                {
                    Console.WriteLine("没有这样的动物,请重试...");
                    continue;
                }

                int times = int.Parse(Console.ReadLine());
                Type type = types[index - 1];
                MethodInfo methodInfo = type.GetMethod("Voice");
                object instance = Activator.CreateInstance(type);
                methodInfo.Invoke(instance, new object[] { times });

                Console.WriteLine(".............结束............");
                Console.WriteLine();
            }
        }
    }
}
