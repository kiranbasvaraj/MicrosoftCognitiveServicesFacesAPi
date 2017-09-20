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
using System.Threading;
using System.Threading.Tasks;

namespace TestMicrosoftCognitiveServices.Activities
{
    [Activity(Label = "FaceRecognitionActivity", MainLauncher = false)]
    public class FaceRecognitionActivity : Activity
    {

        public ImageView _firstImage;
        public ImageView _secondImage;
        public Button _verifyButton;
        public Bitmap _bitmap1 { get; set; }
        public Bitmap _bitmap2 { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.FaceRecognitionLayout);
            _bitmap1 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Chris_Gayle);
            _bitmap2 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.SampleImage);
            FindViews();
            HandleEvents();
        }

        void FindViews()
        {
            _firstImage = FindViewById<ImageView>(Resource.Id.imageView1);
            _secondImage = FindViewById<ImageView>(Resource.Id.imageView2);
            _verifyButton = FindViewById<Button>(Resource.Id.verifyButton);
            _firstImage.SetImageBitmap(_bitmap1);
            _secondImage.SetImageBitmap(_bitmap2);
        }
        void HandleEvents()
        {
            _verifyButton.Click += _verifyButton_Click;
        }

        private void _verifyButton_Click(object sender, EventArgs e)
        {
            byte[] bitmapData1;
            byte[] bitmapData2;
            MemoryStream stream = new MemoryStream();
            _bitmap1.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            bitmapData1 = stream.ToArray();
            MemoryStream inputStream1 = new MemoryStream(bitmapData1);
            _bitmap2.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            bitmapData2 = stream.ToArray();
            MemoryStream inputStream2 = new MemoryStream(bitmapData2);

            FaceRecognitionHelper faceRecognizer = new FaceRecognitionHelper();
            
            //var x=  faceRecognizer.VerifyFace(inputStream1, inputStream2);
            Task.Factory.StartNew(() =>
            {
                Android.OS.Process.SetThreadPriority(Android.OS.ThreadPriority.Background);
                try
                {
                    //Whatever you would like to do in background 
                    var x=  faceRecognizer.VerifyFace(inputStream1, inputStream2);
                }
                catch (Exception ex)
                {

                }


            });
        }
    }
}