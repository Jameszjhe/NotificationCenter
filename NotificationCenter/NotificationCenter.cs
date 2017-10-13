/*
 * Copyright (c) 2017, James <jameszjhe@gmail.com> All rights reserved.
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of James nor the names of its contributors may
 *       be used to endorse or promote products derived from this software
 *       without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY JAMES "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL JAMES AND CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Gudo.Foundation
{
	public class NotificationCenter
	{
		public delegate void NotificationDelegate(Notification notification);

		private static NotificationCenter _defaultNotificationCenter;

		private static object _globalLock;

		private Hashtable _observers;

		static NotificationCenter()
		{
			_globalLock = new object();
		}

		private NotificationCenter()
		{
			_observers = new Hashtable();
		}

		public static NotificationCenter DefaultNotificationCenter
		{
			get
			{
				if (null == _defaultNotificationCenter)
				{
					lock (_globalLock)
					{
						if (null == _defaultNotificationCenter)
						{
							_defaultNotificationCenter = new NotificationCenter();
						}
					}
				}

				return _defaultNotificationCenter;
			}

		}

		public void PostNotification(Notification notification)
		{
			if (null == notification)
			{
				return;
			}

			string key = KeyFromNotificationName(notification.Name);

			List<NotificationDelegate> delegates = new List<NotificationDelegate>();
			List<NotificationDelegate> observers = null

			lock (_globalLock)
			{
				observers = (List<NotificationDelegate>)_observers[key];
				
				if (null != observers)
				{
					delegates.AddRange(observers);
				}
				
				if (!string.Equals(key, "*"))
				{
					observers = (List<NotificationDelegate>)_observers["*"];
					
					if (null != observers)
					{
						delegates.AddRange(observers);
					}
				}
			}

			foreach (NotificationDelegate notificationDelegate in delegates)
			{
				notificationDelegate(notification);
			}
		}

		public void PostNotification(string notificationName)
		{
			PostNotification(new Notification(notificationName, null));
		}

		public void PostNotification(string notificationName, object sender)
		{
			PostNotification(new Notification(notificationName, sender));
		}

		public void PostNotification(string notificationName, object sender, object userInfo)
		{
			PostNotification(new Notification(notificationName, sender, userInfo));
		}

		public void AddObserver(NotificationDelegate notificationDelegate, string notificationName)
		{
			if (null == notificationDelegate)
			{
				return;
			}

			string key = KeyFromNotificationName(notificationName);

			lock (_globalLock)
			{
				List<NotificationDelegate> delegates = (List<NotificationDelegate>)_observers[key];

				if (null == delegates)
				{
					delegates = new List<NotificationDelegate>();

					_observers[key] = delegates;
				}

				delegates.Add(notificationDelegate);
			}
		}

		public void AddObserver(NotificationDelegate notificationDelegate)
		{
			AddObserver(notificationDelegate, null);
		}

		public void RemoveObserver(NotificationDelegate notificationDelegate)
		{
			RemoveObserver(notificationDelegate, null);
		}

		public void RemoveObserver(NotificationDelegate notificationDelegate, string notificationName)
		{
			if (null == notificationDelegate)
			{
				return;
			}

			string key = KeyFromNotificationName(notificationName);

			lock (_globalLock)
			{
				List<NotificationDelegate> delegates = (List<NotificationDelegate>)_observers[key];

				if (null != delegates)
				{
					delegates.Remove(notificationDelegate);
				}
			}
		}

		private string KeyFromNotificationName(string notificationName)
		{
			return (string.IsNullOrEmpty(notificationName)) ? "*" : notificationName;
		}
	}
}
