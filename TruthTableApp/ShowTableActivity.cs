using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Lights;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.InputMethodServices.Keyboard;
using static Android.Webkit.WebSettings;
using TruthTableApp.TruthTableBuilder;
using Android.Graphics.Drawables;

namespace TruthTableApp
{
    [Activity(Label = "ShowTableActivity")]
    public class ShowTableActivity : Activity
    {
        public Paint Paint { get; set; } = new Paint();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_showtable);

            var formula = FindViewById<TextView>(Resource.Id.formula);
            formula.Text = MainActivity.TruthTable.Formula;

            var imgView = FindViewById<ImageView>(Resource.Id.imageView);
            InitDrawing();

            Button backButton = FindViewById<Button>(Resource.Id.backHome);

            backButton.Click += (sender, e) =>
            {
                this.Finish();
            };
        }

        private void InitDrawing()
        {
            if (MainActivity.TruthTable == null)
            {
                return;
            }

            int startX = 100;
            int startY = 100;
            var textSize = 75;
            var scale = textSize * 3;
            var strokeWidth = 7;
            var bitmapWidth = MainActivity.TruthTable.Variables.Count() * scale;
            var bitmapHeight = MainActivity.TruthTable.Table.Count() * textSize + textSize * 2;
            Paint.StrokeWidth = strokeWidth;
            Paint.TextSize = textSize;

            Paint.Color = Color.Black;
            ImageView myImageView = FindViewById<ImageView>(Resource.Id.imageView);
    
            Bitmap bitmap = Bitmap.CreateBitmap(bitmapWidth, bitmapHeight, Bitmap.Config.Argb8888);    
            Canvas canvas = new Canvas(bitmap);
            myImageView.SetImageBitmap(bitmap);

            var variablesString = new StringBuilder();

            for (int i = 0; i < MainActivity.TruthTable.Variables.Count(); ++i)
            {
                variablesString.Append($"  {i}");
            }

            canvas.DrawText(variablesString.ToString(), startX, startY, Paint);

            startY += textSize;
            var redPaint = new Paint(Paint);
            redPaint.Color = Color.Purple;

            foreach (var row in MainActivity.TruthTable.Table)
            {
                var rowAsString = new StringBuilder();

                foreach (var value in row.Key)
                {
                    rowAsString.Append($"  {GetShortBoolString(value)}");
                }

                canvas.DrawText(rowAsString.ToString(), startX, startY, Paint);
                canvas.DrawText($"    {GetShortBoolString(row.Value)}", startX + row.Key.Count * textSize, startY, redPaint);
                startY += textSize;
            }

            var variables = FindViewById<TextView>(Resource.Id.variables);
            variables.Text += "Список змінних:\n";

            for (int i = 0; i < MainActivity.TruthTable.Variables.Count(); ++i)
            {
                variables.Text += $"{i} - {MainActivity.TruthTable.Variables.ElementAt(i)}\n";
            }
        }

        private string GetShortBoolString(bool value) => value ? "T" : "F";
    }
}
