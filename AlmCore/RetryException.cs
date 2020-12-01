using HandyControl.Controls;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlmCore
{
    public class RetryException
    {
        /// <summary>
        ///  无返回重试
        /// </summary>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static void DoRetry(Action action, int Times = 3)
        {
            Policy.Handle<Exception>().Retry(Times, (Ex, Count, Context) =>
            {
                Console.WriteLine($"重试次数：{Count}，异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
            }).Execute(action);
        }
        /// <summary>
        /// 有返回重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static T DoRetry<T>(Func<T> action, int Times = 3)
        {
            return Policy.Handle<Exception>().Retry(Times, (Ex, Count, Context) =>
            {
                Console.WriteLine($"重试次数：{Count}，异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
            }).Execute(action);
        }
        /// <summary>
        /// 无返回重试
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static async Task DoRetryAsync(Func<Task> action, int Times = 3)
        {
            await Policy.Handle<Exception>().RetryAsync(Times, (Ex, Count, Context) =>
            {
                Console.WriteLine($"重试次数：{Count}，异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
            }).ExecuteAsync(action);
        }
        /// <summary>
        /// 有返回重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static async Task<T> DoRetryAsync<T>(Func<Task<T>> action, int Times = 3)
        {
            return await Policy.Handle<Exception>().RetryAsync(Times, (Ex, Count, Context) =>
            {
                Console.WriteLine($"重试次数：{Count}，异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
            }).ExecuteAsync(action);
        }
    }
}
