using System;
using System.Threading;
using System.Threading.Tasks;

// Need change code for call callback in main thread
// Нужно изменить код таким образом чтобы callback вызывался и исполнялся в главном потоке

namespace ppTest
{
	class MainClass
	{
		static void Main (string [] args)
		{
			MainThreadCallbackTest ();
		}

		static void MainThreadCallbackTest ()
		{
			PrintThreadId ("#Main");

			var callback = new Action (() => {
				PrintThreadId ("#Callback");
			});

			new Task (() => {
				PrintThreadId ("#Task");
				callback.Invoke ();
			}).Start ();

			Console.ReadKey ();
		}

		static void PrintThreadId (String tag)
		{
			Console.WriteLine ("{0} - threadId: {1}", tag, Thread.CurrentThread.ManagedThreadId);
		}
	}
}
