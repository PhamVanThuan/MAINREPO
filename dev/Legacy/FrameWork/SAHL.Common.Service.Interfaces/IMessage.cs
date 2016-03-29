using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SAHL.Common.Service.Interfaces
{
  public enum MessageType : int
  {
    EMail = 0,
    SMS   = 1
  };

  /// <summary>
  /// Represents a message that will be sent to recipients
  /// </summary>
  public interface IMessage
  {
    string From { get; set;}
    string To { get; set; }
    string Body { get; set; }
    MessageType MessageType { get; }
      void ClearAttachments();
      void AddAttachment(IAttachment attachment);
      List<IAttachment> Attachments { get; }
  }

  /// <summary>
  /// Represents an email message
  /// </summary>
  public interface IMailMessage : IMessage
  {
    string Cc { get; set; }
    string Subject { get; set; }
  }

  /// <summary>
  /// Represents an SMS message.
  /// </summary>
  public interface ISMSMessage : IMessage
  {
  }

    public interface IAttachment
    {
        string Name { get; }
        byte[] byData { get; }
        Stream Data { get; }
    }
}
