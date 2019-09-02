using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace CompareFiles
{
    [MarkdownExporter]
    public class FileComparer
    {
        private const string FILE1 = @"C:\Users\WAKU\Desktop\File1";
        private const string FILE2 = @"C:\Users\WAKU\Desktop\File2";

        [Params(4096 * 10, 4096 * 100, 4096 * 1000)]
        public int buffer_size;

        /// <summary>
        /// MD5
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByMD5()
        {
            Win32.ClearFileSystemCache(true);
            // 使用.NET内置的MD5库
            using (var md5 = MD5.Create())
            {
                byte[] one, two;
                using (var fs1 = File.Open(FILE1, FileMode.Open))
                {
                    // 以FileStream读取文件内容,计算HASH值
                    one = md5.ComputeHash(fs1);
                }
                using (var fs2 = File.Open(FILE2, FileMode.Open))
                {
                    // 以FileStream读取文件内容,计算HASH值
                    two = md5.ComputeHash(fs2);
                }
                // 将MD5结果(字节数组)转换成字符串进行比较
                return BitConverter.ToString(one) == BitConverter.ToString(two);
            }
        }

        /// <summary>
        /// MD5 Async
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByMD5Async()
        {
            Win32.ClearFileSystemCache(true);
            var task1 = Task.Run(() =>
            {
                using (var md5 = MD5.Create())
                {
                    using (var fs = File.Open(FILE1, FileMode.Open))
                    {
                        // 以FileStream读取文件内容,计算HASH值
                        return md5.ComputeHash(fs);
                    }
                }
            });

            var task2 = Task.Run(() =>
            {
                using (var md5 = MD5.Create())
                {
                    using (var fs = File.Open(FILE2, FileMode.Open))
                    {
                        // 以FileStream读取文件内容,计算HASH值
                        return md5.ComputeHash(fs);
                    }
                }
            });
            Task.WaitAll(task1, task2);

            // 将MD5结果(字节数组)转换成字符串进行比较
            return BitConverter.ToString(task1.Result) == BitConverter.ToString(task1.Result);
        }

        /// <summary>
        /// https://stackoverflow.com/a/1359947
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByToInt64()
        {
            Win32.ClearFileSystemCache(true);
            const int buffer_size = sizeof(Int64);        // 每次读取8个字节
            int iterations = (int)Math.Ceiling((double)new FileInfo(FILE1).Length / buffer_size); // 计算读取次数

            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];

                for (int i = 0; i < iterations; i++)
                {
                    // 循环读取到字节数组中
                    fs1.Read(one, 0, buffer_size);
                    fs2.Read(two, 0, buffer_size);

                    // 转换为Int64进行数组比较
                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(while循环比较字节数组)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByByteArray()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, buffer_size);
                    int len2 = fs2.Read(two, 0, buffer_size);
                    int index = 0;
                    if (len1 != len2) return false;
                    while (index < len1)
                    {
                        if (one[index] != two[index]) return false;
                        index++;
                    }
                    if (len1 == 0) break;
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(while循环比较字节数组)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByByteArrayAsync()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    // 异步读取文件
                    var task1 = fs1.ReadAsync(one, 0, buffer_size);
                    var task2 = fs2.ReadAsync(two, 0, buffer_size);
                    // 等待两个Task都结束
                    Task.WaitAll(task1, task2);
                    int index = 0;
                    if (task1.Result != task2.Result) return false;
                    while (index < task1.Result)
                    {
                        if (one[index] != two[index]) return false;
                        index++;
                    }
                    if (task1.Result == 0) break;
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(转为String比较)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByString()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, buffer_size);
                    int len2 = fs2.Read(two, 0, buffer_size);
                    if (BitConverter.ToString(one) != BitConverter.ToString(two)) return false;
                    if (len1 == 0 || len2 == 0) break;  // 有文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(使用LINQ的SequenceEqual比较)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareBySequenceEqual()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, buffer_size);
                    int len2 = fs2.Read(two, 0, buffer_size);
                    if (!one.SequenceEqual(two)) return false;
                    if (len1 == 0 || len2 == 0) break;  // 有文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(Win32 API比较字节数组)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByWin32API()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, buffer_size);
                    int len2 = fs2.Read(two, 0, buffer_size);
                    if (!Win32.ByteArrayCompare(one, two)) return false;
                    if (len1 == 0 || len2 == 0) break;  // 有文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(ReadOnlySpan)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByReadOnlySpan()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    int len1 = fs1.Read(one, 0, buffer_size);
                    int len2 = fs2.Read(two, 0, buffer_size);
                    // 字节数组可直接转换为ReadOnlySpan
                    if (!((ReadOnlySpan<byte>)one).SequenceEqual((ReadOnlySpan<byte>)two)) return false;
                    if (len1 == 0 || len2 == 0) break;  // 有文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }

        /// <summary>
        /// 读入到字节数组中比较(ReadOnlySpan - 异步)
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [Benchmark]
        public bool CompareByReadOnlySpanAsync()
        {
            Win32.ClearFileSystemCache(true);
            using (FileStream fs1 = File.Open(FILE1, FileMode.Open))
            using (FileStream fs2 = File.Open(FILE2, FileMode.Open))
            {
                byte[] one = new byte[buffer_size];
                byte[] two = new byte[buffer_size];
                while (true)
                {
                    // 异步读取文件
                    var task1 = fs1.ReadAsync(one, 0, buffer_size);
                    var task2 = fs2.ReadAsync(two, 0, buffer_size);
                    // 等待两个Task都结束
                    Task.WaitAll(task1, task2);
                    // 字节数组可直接转换为ReadOnlySpan
                    if (!((ReadOnlySpan<byte>)one).SequenceEqual((ReadOnlySpan<byte>)two)) return false;
                    if (task1.Result == 0 || task2.Result == 0) break;  // 有文件都读取到了末尾,退出while循环
                }
            }

            return true;
        }
    }
}
