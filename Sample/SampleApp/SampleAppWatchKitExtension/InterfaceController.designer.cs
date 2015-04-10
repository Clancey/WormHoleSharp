//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace SampleAppWatchKitExtension
{
	[Register ("InterfaceController")]
	partial class InterfaceController
	{

		[Action("didTapOne")]
		partial void DidTapOne();

		[Action("didTapTwo")]
		partial void DidTapTwo();

		[Action("didTapThree")]
		partial void DidTapThree();

		[Outlet]
		WatchKit.WKInterfaceLabel SelectionLabel {get;set;}

		void ReleaseDesignerOutlets ()
		{
			if(SelectionLabel != null){
				SelectionLabel.Dispose ();
				SelectionLabel = null;
			}
		}
	}
}

