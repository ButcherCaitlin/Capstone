using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Database;
using Android.Provider;
using Android.Graphics;
using Android.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Capstone.Services;
using Xamarin.Forms;
using Capstone.Droid;
using Capstone.Droid.Helpers;
using Capstone.Models;
using Capstone.Helpers;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.CurrentActivity;
using System.Linq;
using Xamarin.Essentials;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Capstone.Droid
{
    public class PhotoPickerService : IPhotoPickerService
    {
        /*public Task<Dictionary<string,Stream>> GetImageStreamAsync()
          {

              // Define the Intent for getting images
              Intent intent = new Intent();
              intent.SetType("image/*");
             intent.PutExtra(Intent.ExtraAllowMultiple, true);
              intent.SetAction(Intent.ActionGetContent);

              //Start the picture-picker activity (resumes in MainActivity.cs)
              MainActivity.Instance.StartActivityForResult(
                  Intent.CreateChooser(intent, "Select Image"),
                  MainActivity.PickImageId);//used to be MainActivity.PickImageId;

              // Save the TaskCompletionSource object as a MainActivity property
              MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Dictionary<string,Stream>>();

              // Return Task object
              return MainActivity.Instance.PickImageTaskCompletionSource.Task;

          }
         */
        public static PhotoPickerService SharedInstance = new PhotoPickerService();
        int PhotoPickerResultCode = 9793;
        const string TemporalDirectoryName = "TmpMedia";

        public PhotoPickerService()
        {
        }

        public event EventHandler<MediaFile> OnMediaPicked;
        public event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;

        TaskCompletionSource<IList<MediaFile>> mediaPickedTcs;

        public void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            ObservableCollection<MediaFile> mediaPicked = null;

            if (requestCode == PhotoPickerResultCode)
            {
                if (resultCode == Result.Ok)
                {
                    mediaPicked = new ObservableCollection<MediaFile>();
                    if (intent != null)
                    {
                        ClipData clipData = intent.ClipData;
                        if (clipData != null)
                        {
                            for (int i = 0; i < clipData.ItemCount; i++)
                            {
                                ClipData.Item item = clipData.GetItemAt(i);
                                Android.Net.Uri uri = item.Uri;
                                var media = CreateMediaFileFromUri(uri);
                                if (media != null)
                                {
                                    mediaPicked.Add(media);
                                    OnMediaPicked?.Invoke(this, media);
                                }

                            }
                        }
                        else
                        {
                            Android.Net.Uri uri = intent.Data;
                            var media = CreateMediaFileFromUri(uri);
                            if (media != null)
                            {
                                mediaPicked.Add(media);
                                OnMediaPicked?.Invoke(this, media);
                            }
                        }

                        OnMediaPickedCompleted?.Invoke(this, mediaPicked);
                        //MessagingCenter.Send<Object, Object>(this, "Multiplesel", mediaPicked.ToList());
                    }
                }

                mediaPickedTcs?.TrySetResult(mediaPicked);

            }
        }

        MediaFile CreateMediaFileFromUri(Android.Net.Uri uri)
        {
            MediaFile mediaFile = null;
            var type = CrossCurrentActivity.Current.Activity.ContentResolver.GetType(uri);

            var path = GetRealPathFromURI(uri);
            if (path != null)
            {
                string fullPath = string.Empty;
                string thumbnailImagePath = string.Empty;
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                var ext = System.IO.Path.GetExtension(path) ?? string.Empty;
                MediaFileType mediaFileType = MediaFileType.Image;

                if (type.StartsWith(Enum.GetName(typeof(MediaFileType), MediaFileType.Image), StringComparison.CurrentCultureIgnoreCase))
                {
                    var fullImage = ImageHelpers.RotateImage(path, 1);
                    var thumbImage = ImageHelpers.RotateImage(path, 0.25f);


                    fullPath = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileName}{ext}");
                    File.WriteAllBytes(fullPath, fullImage);

                    thumbnailImagePath = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileName}-THUMBNAIL{ext}");
                    File.WriteAllBytes(thumbnailImagePath, thumbImage);

                }
                //else if (type.StartsWith(Enum.GetName(typeof(MediaFileType), MediaFileType.Video), StringComparison.CurrentCultureIgnoreCase))
                //{
                //    fullPath = path;
                //    var bitmap = ThumbnailUtils.CreateAudioThumbnail(path, Size);

                //    thumbnailImagePath = FileHelper.GetOutputPath(MediaFileType.Image, TemporalDirectoryName, $"{fileName}-THUMBNAIL{ext}");
                //    var stream = new FileStream(thumbnailImagePath, FileMode.Create);
                //    bitmap?.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                //    stream.Close();

                //    mediaFileType = MediaFileType.Video;
                //}

                if (!string.IsNullOrEmpty(fullPath) && !string.IsNullOrEmpty(thumbnailImagePath))
                {
                    mediaFile = new MediaFile()
                    {
                        Path = fullPath,
                        Type = mediaFileType,
                        PreviewPath = thumbnailImagePath
                    };
                }

            }

            return mediaFile;
        }
       
        public static string GetRealPathFromURI(Android.Net.Uri contentURI)
        {
            ICursor cursor = null;
            try
            {

                string mediaPath = string.Empty;
                cursor = CrossCurrentActivity.Current.Activity.ContentResolver.Query(contentURI, null, null, null, null);
                cursor.MoveToFirst();
                int idx = cursor.GetColumnIndex(MediaStore.MediaColumns.Data);

                if (idx != -1)
                {
                    var type = CrossCurrentActivity.Current.Activity.ContentResolver.GetType(contentURI);

                    int pIdx = cursor.GetColumnIndex(MediaStore.MediaColumns.Id);

                    var mData = cursor.GetString(idx);

                    mediaPath = mData;

                }
                else
                {

                    var docID = DocumentsContract.GetDocumentId(contentURI);
                    var doc = docID.Split(':');
                    var id = doc[1];
                    var whereSelect = MediaStore.Images.ImageColumns.Id + "=?";
                    var dataConst = MediaStore.Images.ImageColumns.Data;
                    var projections = new string[] { dataConst };
                    var internalUri = MediaStore.Images.Media.InternalContentUri;
                    var externalUri = MediaStore.Images.Media.ExternalContentUri;
                    switch (doc[0])
                    {
                        case "video":
                            internalUri = MediaStore.Video.Media.InternalContentUri;
                            externalUri = MediaStore.Video.Media.ExternalContentUri;
                            whereSelect = MediaStore.Video.VideoColumns.Id + "=?";
                            dataConst = MediaStore.Video.VideoColumns.Data;
                            break;
                        case "image":
                            whereSelect = MediaStore.Video.VideoColumns.Id + "=?";
                            projections = new string[] { MediaStore.Video.VideoColumns.Data };
                            break;
                    }

                    projections = new string[] { dataConst };
                    cursor = CrossCurrentActivity.Current.Activity.ContentResolver.Query(internalUri, projections, whereSelect, new string[] { id }, null);
                    if (cursor.Count == 0)
                    {
                        cursor = CrossCurrentActivity.Current.Activity.ContentResolver.Query(externalUri, projections, whereSelect, new string[] { id }, null);
                    }
                    var colDatax = cursor.GetColumnIndexOrThrow(dataConst);
                    cursor.MoveToFirst();

                    mediaPath = cursor.GetString(colDatax);
                }
                return mediaPath;
            }
            catch (Exception)
            {
                Toast.MakeText(CrossCurrentActivity.Current.Activity, "Unable to get path", ToastLength.Long).Show();

            }
            finally
            {
                if (cursor != null)
                {
                    cursor.Close();
                    cursor.Dispose();
                }
            }

            return null;

        }

        public void Clean()
        {

            var documentsDirectory = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), TemporalDirectoryName);

            if (Directory.Exists(documentsDirectory))
            {
                Directory.Delete(documentsDirectory);
            }
        }

        public async Task<IList<MediaFile>> PickPhotosAsync()
        {
            return await PickMediaAsync("image/*", "Select Images", PhotoPickerResultCode);
        }

        public async Task<IList<MediaFile>> PickVideosAsync()
        {
            return await PickMediaAsync("video/*", "Select Videos", PhotoPickerResultCode);
        }

        async Task<IList<MediaFile>> PickMediaAsync(string type, string title, int resultCode)
        {

            mediaPickedTcs = new TaskCompletionSource<IList<MediaFile>>();

            var imageIntent = new Intent(Intent.ActionPick);
            imageIntent.SetType(type);
            imageIntent.PutExtra(Intent.ExtraAllowMultiple, true);
            CrossCurrentActivity.Current.Activity.StartActivityForResult(Intent.CreateChooser(imageIntent, title), resultCode);

            return await mediaPickedTcs.Task;

        }
    }
}