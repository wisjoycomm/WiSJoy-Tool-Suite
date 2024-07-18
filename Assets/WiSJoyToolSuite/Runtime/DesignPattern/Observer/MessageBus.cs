using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

namespace WiSJoy.DesignPattern
{
    public class MessageBus
    {
        private static MessageBus _instance;
        public static MessageBus I => _instance ?? (_instance = new MessageBus());
        private readonly Dictionary<MessageChannel, Dictionary<Type, List<Delegate>>> _channelSubscribers = new();
        private readonly Dictionary<Type, object> _messagePools = new();

        private readonly object _lock = new object();

        private MessageBus() { }

        /// <summary>
        /// Get the object pool for a message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ObjectPool<T> GetPool<T>() where T : class, new()
        {
            var type = typeof(T);
            if (!_messagePools.TryGetValue(type, out var pool))
            {
                pool = new ObjectPool<T>(
                    createFunc: () => new T(),
                    actionOnGet: (item) => { /* Reset các thuộc tính cần thiết của item ở đây, nếu cần */ },
                    actionOnRelease: ResetMessage,
                    actionOnDestroy: (item) => { /* Có thể log hoặc thực hiện bước cuối cùng trước khi huỷ item */ },
                    maxSize: 1000
                );
                _messagePools[type] = pool;
            }
            return pool as ObjectPool<T>;
        }

        /// <summary>
        /// Publish a message to all subscribers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configure"></param>
        public void Notify<T>(Action<T> configure, params MessageChannel[] channels) where T : class, new()
        {
            var pool = GetPool<T>();
            var message = pool.Get();
            configure?.Invoke(message);
            foreach (var channel in channels)
            {
                Dispatch(channel, message);
            }
            pool.Release(message);
        }

        /// <summary>
        /// Dispatch a message to all subscribers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        private void Dispatch<T>(MessageChannel channel, T message)
        {
            if (!_channelSubscribers.TryGetValue(channel, out var subscribersByType))
            {
                return; // Không có subscriber nào cho channel này
            }

            var messageType = typeof(T);
            if (!subscribersByType.TryGetValue(messageType, out var subscribers))
            {
                return; // Không có subscriber nào cho loại message này trong channel
            }

            // Tạo một bản sao của danh sách subscribers để tránh ConcurrentModificationException
            var subscribersCopy = new List<Delegate>(subscribers);

            foreach (Delegate subscriber in subscribersCopy)
            {
                if (subscriber is Action<T> action)
                {
                    action(message);
                }
            }
        }
        public async UniTask NotifyAsync<T>(Action<T> configure, int delayMilliseconds = 0, params MessageChannel[] channels) where T : class, new()
        {
            var pool = GetPool<T>();
            var message = pool.Get();
            configure?.Invoke(message);
            if (delayMilliseconds > 0)
            {
                await UniTask.Delay(delayMilliseconds); // Trì hoãn bằng UniTask.Delay
            }
            // Tạo một mảng các UniTask để lưu trữ kết quả của từng DispatchAsync
            var dispatchTasks = new UniTask[channels.Length];

            for (int i = 0; i < channels.Length; i++)
            {
                dispatchTasks[i] = DispatchAsync(channels[i], message);
            }

            // Đợi tất cả các DispatchAsync hoàn thành
            await UniTask.WhenAll(dispatchTasks);

            pool.Release(message); // Chỉ release message sau khi tất cả đã hoàn thành
        }

        private async UniTask DispatchAsync<T>(MessageChannel channel, T message)
        {
            if (!_channelSubscribers.TryGetValue(channel, out var subscribersByType))
            {
                return; // Không có subscriber nào cho channel này
            }

            var messageType = typeof(T);
            if (!subscribersByType.TryGetValue(messageType, out var subscribers))
            {
                return; // Không có subscriber nào cho loại message này trong channel
            }

            var subscribersCopy = new List<Delegate>(subscribers);

            foreach (Delegate subscriber in subscribersCopy)
            {
                if (subscriber is Action<T> action)
                {
                    await UniTask.RunOnThreadPool(() => action(message)); // Sử dụng UniTask.RunOnThreadPool
                }
            }
        }

        /// <summary>
        /// Subscribe to a message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void Subscribe<T>(Action<T> subscriber, params MessageChannel[] channels) where T : class, new()
        {
            foreach (var channel in channels)
            {
                if (!_channelSubscribers.TryGetValue(channel, out var subscribersByType))
                {
                    subscribersByType = new Dictionary<Type, List<Delegate>>();
                    _channelSubscribers[channel] = subscribersByType;
                }

                var messageType = typeof(T);
                if (!subscribersByType.TryGetValue(messageType, out var subscribers))
                {
                    subscribers = new List<Delegate>();
                    subscribersByType[messageType] = subscribers;
                }
                subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// Unsubscribe from a message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Unsubscribe<T>(Action<T> subscriber, params MessageChannel[] channels) where T : class, new()
        {
            foreach (var channel in channels)
            {
                if (_channelSubscribers.TryGetValue(channel, out var subscribersByType) &&
                subscribersByType.TryGetValue(typeof(T), out var subscribers))
                {
                    subscribers.Remove(subscriber);
                }
            }

        }

        /// <summary>
        /// Reset the message object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void ResetMessage<T>(T message) where T : class, new()
        {
            if (message is IResettable resettable)
            {
                resettable.Reset();
            }
        }
    }
    /// <summary>
    /// Interface for resettable objects
    /// </summary>
    public interface IResettable
    {
        void Reset();
    }

}
