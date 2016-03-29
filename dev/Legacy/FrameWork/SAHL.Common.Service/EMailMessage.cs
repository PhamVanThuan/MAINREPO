using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using System.IO;

namespace SAHL.Common.Service
{
  public abstract class Message : IMessage
  {
    private string _To;
    private string _From;
    private string _Message;
    #region IMessage Members

    public string From
    {
      get
      {
        return _From;
      }
      set
      {
        _From = value;
      }
    }

    public string To
    {
      get
      {
        return _To;
      }
      set
      {
        _To = value;
      }
    }

    public string Body
    {
      get
      {
        return _Message;
      }
      set
      {
        _Message = value;
      }
    }

    public virtual MessageType MessageType
    {
      get
      {
        return MessageType.EMail;
      }
    }

    #endregion
      private List<IAttachment> _Attachments = new List<IAttachment>();
      public List<IAttachment> Attachments { get { return _Attachments; } }
      public void ClearAttachments()
      {
          _Attachments.Clear();
      }
      public void AddAttachment(IAttachment attachment)
      {
          _Attachments.Add(attachment);
      }
  }

    public class Attachment : IAttachment
    {
        byte[] _by;
        string _Name;
        public Attachment(byte[] byData, string FileName)
        {
            this._Name = FileName;
            this._by = byData;
        }
        public Stream Data
        {
            get
            {
                MemoryStream ms = new MemoryStream(_by);
                return ms;
            }
        }
        public byte[] byData { get { return _by; } }
        public string Name { get { return _Name; } }
    }

  /// <summary>
  /// Represents an email message
  /// </summary>
  public class EMailMessage : Message, IMailMessage
  {

    private string _Cc;
    private string _Subject;
    

    public EMailMessage()
    {
    }

    public string Cc
    {
      get
      {
        return _Cc;
      }
      set
      {
        _Cc = value;
      }
    }

    public string Subject
    {
      get
      {
        return _Subject;
      }
      set
      {
        _Subject = value;
      }
    }

    public override MessageType MessageType
    {
      get { return MessageType.EMail; }
    }

  }
}
