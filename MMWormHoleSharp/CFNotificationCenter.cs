using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using CFNotificationCenterRef=global::System.IntPtr;
using ObjCRuntime;


namespace MMWormHoleSharp
{
	public enum CFNotificationSuspensionBehavior
	{
		Drop = 1,
		Coalesce = 2,
		Hold = 3,
		DeliverImmediately = 4
	}
	delegate void CFNotificationCallback (CFNotificationCenterRef center, IntPtr observer, IntPtr name, IntPtr obj, IntPtr userInfo);
	public class CFNotificationCenter : NSObject
	{
		public delegate void NotificationChangeEventHandler (object sender, string notification);

		public event NotificationChangeEventHandler NotificationChanged;

		CFNotificationCenterRef reference;

		internal CFNotificationCenter (CFNotificationCenterRef reference)
		{
			this.reference = reference;
		}

		static CFNotificationCenter darwinCenter;

		public static CFNotificationCenter DarwinCenter {
			get {
				return darwinCenter ?? (darwinCenter = new CFNotificationCenter (GetDarwinNotifyCenter ()));
			}
		}


		static CFNotificationCenter localCenter;

		public static CFNotificationCenter LocalCenter {
			get {
				return localCenter ?? (localCenter = new CFNotificationCenter (GetLocalCenter ()));
			}
		}

		static Dictionary<CFNotificationCenterRef,CFNotificationCenter> centers = new Dictionary<CFNotificationCenterRef,CFNotificationCenter> ();
		int ObserverCount = 0;

		public void AddObserver (string value, CFNotificationSuspensionBehavior suspensionBehavior = CFNotificationSuspensionBehavior.DeliverImmediately)
		{
			ObserverCount++;
			centers [reference] = this;
			AddObserver (reference, this.Handle, NotificationCallback, new CFString (value).Handle, IntPtr.Zero, suspensionBehavior);
		}

		void notification (CFString name, NSDictionary userInfo)
		{
			var evt = NotificationChanged;
			if (evt == null)
				return;
			evt (this, name.ToString ());
		}

		[ObjCRuntime.MonoPInvokeCallback (typeof(CFNotificationCallback))]
		static void NotificationCallback (CFNotificationCenterRef center, IntPtr observer, IntPtr name, IntPtr obj, IntPtr userInfo)
		{
			CFNotificationCenter cfn;
			if (centers.TryGetValue (center, out cfn))
				cfn.notification (new CFString(name), Runtime.GetNSObject<NSDictionary> (userInfo));
		}

		public void PostNotification(string notification)
		{
			PostNotification (reference, new CFString (notification).Handle, IntPtr.Zero, IntPtr.Zero, true);
		}

		public void RemoveNotificationObserver(string notification)
		{
			ObserverCount--;
			RemoveObserver (reference, this.Handle, NotificationCallback, new CFString (notification).Handle, IntPtr.Zero);
			if (this.ObserverCount <= 0 && centers.ContainsKey(reference))
				centers.Remove (reference);
		}

		public void RemoveEveryObserver()
		{
			ObserverCount = 0;
			RemoveEveryObserver (reference, this.Handle);
			var evt = NotificationChanged.GetInvocationList ();
			foreach (var e in evt)
				NotificationChanged -= (NotificationChangeEventHandler)e;

			if (centers.ContainsKey(reference))
				centers.Remove (reference);
		}


		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterGetDarwinNotifyCenter")]
	 	static extern CFNotificationCenterRef GetDarwinNotifyCenter ();

		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterGetLocalCenter")]
		static extern CFNotificationCenterRef GetLocalCenter ();

		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterAddObserver")]
		static extern unsafe void AddObserver (CFNotificationCenterRef center, IntPtr observer, CFNotificationCallback callback, IntPtr name, IntPtr obj, CFNotificationSuspensionBehavior suspensionBehavior);

		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterPostNotification")]
		static extern unsafe void PostNotification (CFNotificationCenterRef center,IntPtr name,  IntPtr obj, IntPtr userInfo, bool deliverImmediately);

		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterRemoveObserver")]
		static extern unsafe void RemoveObserver (CFNotificationCenterRef center, IntPtr observer, CFNotificationCallback callback, IntPtr name, IntPtr obj);


		[DllImport ("__Internal", CharSet = CharSet.Auto, EntryPoint = "CFNotificationCenterRemoveEveryObserver")]
		static extern unsafe void RemoveEveryObserver (CFNotificationCenterRef center, IntPtr observer);
	}
}

