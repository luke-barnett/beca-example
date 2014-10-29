using Cirrious.MvvmCross.Plugins.PictureChooser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Beca.Xamarin.Example.WP.Core.Services
{
	public class MvxPictureChooserTask : IMvxPictureChooserTask
	{
		private int _maxPixelDimension;
		private int _percentQuality;
		private Action<Stream> _pictureAvailable;
		private Action _assumeCancelled;

		public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
		{
			// This method requires that PickFileContinuation is implemented within the Windows Phone project and that
			// the ContinueFileOpenPicker method on the View invokes (via the ViewModel) the ContinueFileOpenPicker method
			// on this instance of the MvxPictureChooserTask
			//
			// http://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn614994.aspx

			_maxPixelDimension = maxPixelDimension;
			_percentQuality = percentQuality;
			_pictureAvailable = pictureAvailable;
			_assumeCancelled = assumeCancelled;

			PickStorageFileFromDisk();
		}

		public void ContinueFileOpenPicker(object args)
		{
			var continuationArgs = args as FileOpenPickerContinuationEventArgs;

			if (continuationArgs != null && continuationArgs.Files.Count > 0)
			{
				var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
				dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					async () =>
					{
						var rawFileStream = await continuationArgs.Files[0].OpenAsync(FileAccessMode.Read);
						var resizedStream =
							await ResizeJpegStreamAsync(_maxPixelDimension, _percentQuality, rawFileStream);

						_pictureAvailable(resizedStream.AsStreamForRead());
					});
			}
			else
			{
				_assumeCancelled();
			}
		}

		public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
		{
			var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
			dispatcher.RunAsync(CoreDispatcherPriority.Normal,
								async () =>
								{
									await
										Process(StorageFileFromCamera, maxPixelDimension, percentQuality, pictureAvailable,
												assumeCancelled);
								});
		}

		public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
		{
			var task = new TaskCompletionSource<Stream>();
			ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
			return task.Task;
		}

		public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
		{
			var task = new TaskCompletionSource<Stream>();
			TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
			return task.Task;
		}

		private async Task Process(Func<Task<StorageFile>> storageFile, int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable, Action assumeCancelled)
		{
			var file = await storageFile();
			if (file == null)
			{
				assumeCancelled();
				return;
			}

			var rawFileStream = await file.OpenAsync(FileAccessMode.Read);
			var resizedStream = await ResizeJpegStreamAsync(maxPixelDimension, percentQuality, rawFileStream);

			pictureAvailable(resizedStream.AsStreamForRead());
		}

		private static async Task<StorageFile> StorageFileFromCamera()
		{
			var filename = String.Format("{0}.jpg", Guid.NewGuid().ToString());

			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
				   filename, CreationCollisionOption.ReplaceExisting);

			var encoding = ImageEncodingProperties.CreateJpeg();

			var capture = new MediaCapture();

			await capture.InitializeAsync();

			await capture.CapturePhotoToStorageFileAsync(encoding, file);

			return file;
		}

		private static void PickStorageFileFromDisk()
		{
			var filePicker = new FileOpenPicker();
			filePicker.FileTypeFilter.Add(".jpg");
			filePicker.FileTypeFilter.Add(".jpeg");
			filePicker.ViewMode = PickerViewMode.Thumbnail;
			filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			//filePicker.SettingsIdentifier = "picker1";
			//filePicker.CommitButtonText = "Open";

			filePicker.PickSingleFileAndContinue();
		}

		private async Task<IRandomAccessStream> ResizeJpegStreamAsyncRubbish(int maxPixelDimension, int percentQuality, IRandomAccessStream input)
		{
			BitmapDecoder decoder = await BitmapDecoder.CreateAsync(input);

			// create a new stream and encoder for the new image
			var ras = new InMemoryRandomAccessStream();
			var enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

			int targetHeight;
			int targetWidth;
			MvxPictureDimensionHelper.TargetWidthAndHeight(maxPixelDimension, (int)decoder.PixelWidth, (int)decoder.PixelHeight, out targetWidth, out targetHeight);

			enc.BitmapTransform.ScaledHeight = (uint)targetHeight;
			enc.BitmapTransform.ScaledWidth = (uint)targetWidth;

			// write out to the stream
			await enc.FlushAsync();

			return ras;
		}


		private async Task<IRandomAccessStream> ResizeJpegStreamAsync(int maxPixelDimension, int percentQuality, IRandomAccessStream input)
		{
			var decoder = await BitmapDecoder.CreateAsync(input);

			int targetHeight;
			int targetWidth;
			MvxPictureDimensionHelper.TargetWidthAndHeight(maxPixelDimension, (int)decoder.PixelWidth, (int)decoder.PixelHeight, out targetWidth, out targetHeight);

			var transform = new BitmapTransform() { ScaledHeight = (uint)targetHeight, ScaledWidth = (uint)targetWidth };
			var pixelData = await decoder.GetPixelDataAsync(
				BitmapPixelFormat.Rgba8,
				BitmapAlphaMode.Straight,
				transform,
				ExifOrientationMode.RespectExifOrientation,
				ColorManagementMode.DoNotColorManage);

			var destinationStream = new InMemoryRandomAccessStream();
			var bitmapPropertiesSet = new BitmapPropertySet();
			bitmapPropertiesSet.Add("ImageQuality", new BitmapTypedValue(((double)percentQuality) / 100.0, PropertyType.Single));
			var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, destinationStream, bitmapPropertiesSet);
			encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)targetWidth, (uint)targetHeight, decoder.DpiX, decoder.DpiY, pixelData.DetachPixelData());
			await encoder.FlushAsync();
			destinationStream.Seek(0L);
			return destinationStream;
		}
	}
}
