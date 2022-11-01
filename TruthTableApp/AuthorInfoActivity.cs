using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TruthTableApp
{
    [Activity(Label = "AuthorInfoActivity")]
    public class AuthorInfoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_authorinfo);

            Button backButton = FindViewById<Button>(Resource.Id.backHome);

            backButton.Click += (sender, e) =>
            {
                this.Finish();
            };
        }
    }
}