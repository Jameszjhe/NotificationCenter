A library which implements the similar notification center feature on iOS platform in C#.

### Example:

```C#
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
```
