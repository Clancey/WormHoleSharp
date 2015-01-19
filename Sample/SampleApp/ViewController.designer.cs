// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SampleApp
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UILabel ButtonLabel { get; set; }

		[Outlet]
		UIKit.UILabel NumberLabel { get; set; }


		[Outlet]
		UIKit.UISegmentedControl SegmentControll { get; set; }

		[Action ("SegmentValueChanged:")]
		partial void SegmentValueChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonLabel != null) {
				ButtonLabel.Dispose ();
				ButtonLabel = null;
			}

			if (SegmentControll != null) {
				SegmentControll.Dispose ();
				SegmentControll = null;
			}
		}
	}
}
