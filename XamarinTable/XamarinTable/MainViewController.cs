using System;
using UIKit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Foundation;
using CoreGraphics;

namespace XamarinTable
{
	public class MainViewController : UIViewController
	{
		public List<UIImage> Images;

		public MainViewController ()
			:base(null, null)
		{
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Images = new List<UIImage> ();
			var table = new UITableView (new CGRect (0, 70, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 70));
			table.Source = new MyTableSource (this);
			Add (table);

			var btn = new UIButton (new CGRect (0, 20, 80, 50));
			btn.SetTitle ("Reload", UIControlState.Normal);
			btn.TouchUpInside += async (sender, e) => await DownloadImages(table);
			Add (btn);

			await DownloadImages (table);

		}

		private async Task DownloadImages(UITableView table)
		{
			var client = new HttpClient ();
			Images.Clear ();

			for (int i = 0; i < 100; i++) {
				var bytes = await client.GetByteArrayAsync ("http://lorempixel.com/50/50/");
				Images.Add(UIImage.LoadFromData(NSData.FromArray(bytes)));
				table.ReloadData ();
			}
		}
	}

	public class MyTableSource : UITableViewSource
	{
		private readonly MainViewController _vc;

		public MyTableSource(MainViewController vc)
		{
			_vc = vc;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _vc.Images.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, "cell");
			cell.ImageView.Image = _vc.Images[(int)indexPath.Item];
			return cell;
		}
			
	}
}

