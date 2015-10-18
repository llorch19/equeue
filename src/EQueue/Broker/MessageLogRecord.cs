﻿using System;
using System.IO;
using System.Text;
using EQueue.Broker.Storage.LogRecords;
using EQueue.Protocols;
using EQueue.Utils;

namespace EQueue.Broker.Storage
{
    [Serializable]
    public class MessageLogRecord : QueueMessage, ILogRecord
    {
        public MessageLogRecord() { }
        public MessageLogRecord(
            string topic,
            int code,
            byte[] body,
            int queueId,
            long queueOffset,
            DateTime createdTime,
            DateTime storedTime)
            : base(null, topic, code, body, queueId, queueOffset, createdTime, storedTime) { }

        public void WriteTo(long logPosition, BinaryWriter writer)
        {
            LogPosition = logPosition;
            MessageId = MessageIdService.CreateMessageId(logPosition);

            //logPosition
            writer.Write(LogPosition);

            //messageId
            var messageIdBytes = Encoding.UTF8.GetBytes(MessageId);
            writer.Write(messageIdBytes.Length);
            writer.Write(messageIdBytes);

            //topic
            var topicBytes = Encoding.UTF8.GetBytes(Topic);
            writer.Write(topicBytes.Length);
            writer.Write(topicBytes);

            //code
            writer.Write(Code);

            //body
            writer.Write(Body.Length);
            writer.Write(Body);

            //queueId
            writer.Write(QueueId);

            //queueOffset
            writer.Write(QueueOffset);

            //createdTime
            writer.Write(CreatedTime.Ticks);

            //storedTime
            writer.Write(StoredTime.Ticks);
        }
    }
}
