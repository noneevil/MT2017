using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace WebSite.Plugins
{
    internal static class RedisTransactionExtensions
    {
        public static void QueueCommandMap(this IRedisTransaction transaction, Func<IRedisClient, byte[][]> command, Action<IDictionary<string, byte[]>> onSuccessCallback)
        {
            transaction.QueueCommand(command, (multiData) =>
            {
                onSuccessCallback(RedisClientExtensions.MultiByteArrayToDictionary(multiData));
            });
        }
    }
}