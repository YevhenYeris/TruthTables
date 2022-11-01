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

            /*Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;*/

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

            for (int i = 0; i < MainActivity.TruthTable.Variables.Count(); ++i)
            {
                variables.Text += $"{i} - {MainActivity.TruthTable.Variables.ElementAt(i)}\n";
            }

            //var centreX = GetX() + Width / 2;
            //var centreY = GetY() + Height / 2;

            //var startX = textSize / 2;
            //var startY = centreY / 2 + 200;

            //canvas.DrawColor(Color.AliceBlue);

            //var variablesString = new StringBuilder();

            //for (int i = 0; i < TruthTable.Variables.Count(); ++i)
            //{
            //    variablesString.Append($"  {i}");
            //}

            ////canvas.DrawLine(variablesString.Length * textSize / 2, startY, variablesString.Length * textSize / 2, Height, Paint);
            //canvas.DrawText(variablesString.ToString(), startX, startY, Paint);
            ////canvas.DrawLine(startX, startY, Width, startY, Paint);
            //startY += textSize;

            //canvas.DrawLine(startX, startY - textSize + strokeWidth, variablesString.Length * textSize + textSize, startY - textSize + strokeWidth, Paint);
            //canvas.DrawLine(startX + variablesString.Length * textSize / 2, startY - textSize * 2, startX + variablesString.Length * textSize / 2, startY + TruthTable.Table.Count * textSize, Paint);

            //foreach (var row in TruthTable.Table)
            //{
            //    var rowAsString = new StringBuilder();

            //    foreach (var value in row.Key)
            //    {
            //        rowAsString.Append($"  {GetShortBoolString(value)}");
            //    }

            //    rowAsString.Append($"    {GetShortBoolString(row.Value)}");
            //    canvas.DrawText(rowAsString.ToString(), startX, startY, Paint);
            //    startY += textSize;
            //}

            //startY += 300;

            //for (int i = 0; i < TruthTable.Variables.Count(); ++i)
            //{
            //    canvas.DrawText($"{i} - {TruthTable.Variables.ElementAt(i)}", startX, startY, Paint);
            //}
        }

            /*protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
            {
                int width = TruthTable.Variables.Count() * 100;
                int height = TruthTable.Table.Count * 100;
                SetMeasuredDimension(width, height);

            }*/

            private void DrawValuesRow(IEnumerable<string> values, Canvas canvas, float startX, float startY)
            {
                var textStartX = startX + 75;
                var textStartY = startY - 60;

                for (int i = 0; i < values.Count(); ++i)
                {
                    canvas.DrawText(i.ToString(), textStartX, textStartY, Paint);
                    textStartX += i.ToString().Length * textStartX;
                }
            }

            private string GetShortBoolString(bool value) => value ? "T" : "F";
    }
}