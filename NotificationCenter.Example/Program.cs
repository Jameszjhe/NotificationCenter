using Gudo.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gudo.Foundation.Example
{
	class Program
	{
		static void Main(string[] args)
		{
			NotificationCenter.DefaultNotificationCenter.AddObserver(MonitorAllNotifications);
			NotificationCenter.DefaultNotificationCenter.AddObserver(MonitorSpecificNotifications, "TestNotification");

			NotificationCenter.DefaultNotificationCenter.PostNotification(Notification.Empty);
			NotificationCenter.DefaultNotificationCenter.PostNotification("TestNotification");

			Console.ReadKey();
		}


		static void MonitorAllNotifications(Notification notification)
		{
			Console.WriteLine("In method [MonitorAllNotifications], received notification:{0}", notification);
		}

		static void MonitorSpecificNotifications(Notification notification)
		{
			Console.WriteLine("In method [MonitorSpecificNotifications], received notification:{0}", notification);
		}
	}
}
