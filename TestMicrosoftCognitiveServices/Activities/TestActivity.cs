using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using TestMicrosoftCognitiveServices.Helpers;
using System.Threading.Tasks;
using Xamarin.Cognitive.Face.Droid.Contract;

namespace TestMicrosoftCognitiveServices.Activities
{
    [Activity(Label = "TestActivity", MainLauncher = true)]
    public class TestActivity : Activity
    {
        public ImageView _firstImage;
        public ImageView _secondImage;
        public Button _verifyButton;
        public Bitmap _firstImageBitmap { get; set; }
        public Bitmap _secondImageBitmap { get; set; }
        FaceRecognitionHelperTask _helper;
        ProgressDialog _pd;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FaceRecognitionLayout);
            _firstImageBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.sachin1);
            _secondImageBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.young_sachin);
            _helper = new FaceRecognitionHelperTask();
            _pd = new ProgressDialog(this);
            _pd.Indeterminate = true;
            _pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            _pd.SetMessage(" Please wait...");
            _pd.SetCancelable(false);
            FindViews();
            HandleEvents();

        }

        void FindViews()
        {
            _firstImage = FindViewById<ImageView>(Resource.Id.imageView1);
            _secondImage = FindViewById<ImageView>(Resource.Id.imageView2);
            _verifyButton = FindViewById<Button>(Resource.Id.verifyButton);
            _firstImage.SetImageBitmap(_firstImageBitmap);
            _secondImage.SetImageBitmap(_secondImageBitmap);
        }
        void HandleEvents()
        {
            _verifyButton.Click += _verifyButton_Click;
        }

        private async void _verifyButton_Click(object sender, EventArgs e)
        {
            _pd.Show();
            byte[] firstImageBitmapData;
            byte[] secondImageBitmapData;
            string _faceId1 = string.Empty;
            string _faceId2 = string.Empty;

            MemoryStream stream1 = new MemoryStream();
            _firstImageBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream1);
            firstImageBitmapData = stream1.ToArray();

            MemoryStream stream2 = new MemoryStream();
            _secondImageBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream2);
            secondImageBitmapData = stream2.ToArray();

            MemoryStream inputStream1 = new MemoryStream(firstImageBitmapData);
            MemoryStream inputStream2 = new MemoryStream(secondImageBitmapData);

            string id1 = await _helper.GetFaceId1(inputStream1);
            string id2 = await _helper.GetFaceId2(inputStream2);

            var result = await _helper.VerifyPerson(id1, id2);

            _pd.Dismiss();

            if (result.IsIdentical)
            {
               
                Toast.MakeText(this, "persons are identical" + " and confidence is " + result.Confidence, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "not identical" +result.Confidence, ToastLength.Long).Show();
            }
        }
    }
}