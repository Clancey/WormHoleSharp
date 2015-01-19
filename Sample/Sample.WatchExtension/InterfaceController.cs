using System;

using WatchKit;
using Foundation;
using MMWormHoleSharp;

namespace Sample.WatchExtension
{
	public partial class InterfaceController : WKInterfaceController
	{
		public InterfaceController (IntPtr handle) : base (handle)
		{
		}
		MMWormhole wormHole;
		public override void Awake (NSObject context)
		{
			base.Awake (context);

			wormHole = new MMWormhole ("group.com.clancey.wormhole", "messageDir");
			wormHole.ListenForMessage<ButtonMessage> (ButtonMessage.MessageType, (message) => {
				SelectionLabel.SetText(message.Id.ToString());
			});
			// Configure interface objects here.
			Console.WriteLine ("{0} awake with context", this);

		}

		partial void DidTapOne ()
		{
			buttonTapped(1);
		}
		partial void DidTapTwo ()
		{
			buttonTapped(2);
		}
		partial void DidTapThree ()
		{
			buttonTapped(3);
		}

		void buttonTapped(int index)
		{
			wormHole.PassMessage ("watchButton",index);
		}

		public override void WillActivate ()
		{
			// This method is called when the watch view controller is about to be visible to the user.
			Console.WriteLine ("{0} will activate", this);

			SelectionLabel.SetText(wormHole.MessageWithIdentifier<string>(ButtonMessage.MessageType));
		}

		public override void DidDeactivate ()
		{
			// This method is called when the watch view controller is no longer visible to the user.
			Console.WriteLine ("{0} did deactivate", this);
		}

		[Serializable]
		class ButtonMessage{
			public const string MessageType = "buttonMessage";
			public int Id {get;set;}
		}
	}
}

