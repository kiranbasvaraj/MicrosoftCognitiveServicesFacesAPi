using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.IO;
using Android.Media;
using TestMicrosoftCognitiveServices.Helpers;

namespace TestMicrosoftCognitiveServices
{
    [Activity(Label = "TestMicrosoftCognitiveServices", MainLauncher = false)]
    public class MainActivity : Activity
    {
        public Bitmap _bitmap { get; set; }
        public ImageView _imageView { get; set; }
        private Button _button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _bitmap = BitmapFactory.DecodeResource(Resources,Resource.Drawable.sample1);
            FindViews();
            HandleEvents();
        }

        void FindViews()
        {
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);
            _imageView.SetImageBitmap(_bitmap);
            _button = FindViewById<Button>(Resource.Id.button);
        }
        void HandleEvents()
        {
            _button.Click += _button_Click;
        }

        private void _button_Click(object sender, System.EventArgs e)
        {
            byte[] bitmapData;
            MemoryStream stream = new MemoryStream();
            _bitmap.Compress(Bitmap.CompressFormat.Jpeg,100, stream);
            bitmapData = stream.ToArray();
            MemoryStream inputStream = new MemoryStream(bitmapData);
            new FaceDetectionHelper(this).Execute(inputStream);
        }
    }

}

