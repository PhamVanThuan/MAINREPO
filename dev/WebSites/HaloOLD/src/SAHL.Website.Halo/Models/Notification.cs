using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Website.Halo.Models
{
	public class Notification
	{
		public string Message { get; protected set; }
		public NotificationType NotificationType { get; protected set; }
		public Notification(string message, NotificationType notificationType)
		{
			this.Message = message;
			this.NotificationType = notificationType;
		}
	}
	public enum NotificationType
	{
		Information,
		Success,
		Warning,
		Error,
		Alert
	}
}