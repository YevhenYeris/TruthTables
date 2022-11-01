using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Android.Text;
using TruthTableApp.TruthTableBuilder;
using System.Linq;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using Android.Views.TextClassifiers;
using static System.Net.Mime.MediaTypeNames;
using Android.Content;
using Android.Graphics;
using System.Text;

namespace TruthTableApp
{
    public class FormulaCalculatedEventArgs: EventArgs
    {
        public bool IsSuccessful { get; set; }

        public TruthTable TruthTable { get; set; }
    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public event EventHandler<FormulaCalculatedEventArgs> FormulaCalculated;
        private readonly string Nothing = "Nothing selected";

        public static TruthTable TruthTable { get; set; }

        private void Init()
        {
            FormulaCalculated += CheckError;
            FormulaCalculated += ShowResults;
            FormulaCalculated += DisplayShowTableButton;

            var calculator = new TruthTableCalculator();
            var scanner = new Scanner();
            var parser = new Parser(scanner);

            var showTableBtn = FindViewById<Button>(Resource.Id.btn_showtable);
            showTableBtn.Click += (object sender, EventArgs e) =>
            {
                Intent nextActivity = new Intent(this, typeof(ShowTableActivity));
                StartActivity(nextActivity);
                //var layoutMain = FindViewById<View>(Resource.Id.layout_main);
                //layoutMain.Visibility = ViewStates.Gone;
                //var layoutTable = FindViewById<View>(Resource.Id.layout_table);
                //layoutTable.Visibility = ViewStates.Visible;
                //var backBtn = FindViewById<Button>(Resource.Id.btn_back);
                //backBtn.Visibility = ViewStates.Visible;

                //TODO: Set view to correct size, add back button to scroll(create new, add a separate method for mutual back onclick)

                //var drawing = FindViewById(Resource.Id.layout_table);
                //DrawTruthTable(TruthTable);
            };


            var entry = FindViewById<EditText>(Resource.Id.entry);
            entry.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                try
                {
                    var parserResult = parser.Parse(string.Concat(e.Text));
                    TruthTable = calculator.GenerateTruthTable(parserResult);

                    OnFormulaCalculated(new FormulaCalculatedEventArgs()
                    {
                        IsSuccessful = true,
                        TruthTable = TruthTable
                    });
                }
                catch(Exception ex)
                {
                    OnFormulaCalculated(new FormulaCalculatedEventArgs()
                    {
                        IsSuccessful = false,
                        TruthTable = null
                    });
                }
            };

            var spinner = FindViewById<Spinner>(Resource.Id.spinner_formulas);
            var importBtn = FindViewById<Button>(Resource.Id.btn_import);
            importBtn.Click += (object sender, EventArgs e) =>
            {
                var formulas = new List<string>() { Nothing };
                formulas.AddRange(ReadFormulas());
                ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.support_simple_spinner_dropdown_item, formulas.ToArray());
                spinner.Adapter = arrayAdapter;
                spinner.Visibility = ViewStates.Visible;
            };

            spinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var itemText = ((Spinner)sender).SelectedItem.ToString();
                var entry = FindViewById<EditText>(Resource.Id.entry);

                if (itemText == Nothing)
                {
                    return;
                }

                entry.Text = itemText; 
                spinner.Visibility = ViewStates.Gone;
            };

            var exportBtn = FindViewById<Button>(Resource.Id.btn_export);
            exportBtn.Click += (object sender, EventArgs e) => SaveFormula(entry.Text);
        }

        private void ShowResults(object sender, FormulaCalculatedEventArgs eventArgs)
        {
            var textExec = FindViewById<TextView>(Resource.Id.text_exec);
            var textEq = FindViewById<TextView>(Resource.Id.text_eq);

            if (eventArgs.TruthTable == null)
            {
                textExec.Visibility = ViewStates.Invisible;
                textEq.Visibility = ViewStates.Invisible;
                return;
            }

            textExec.Visibility = ViewStates.Visible;
            textEq.Visibility = ViewStates.Visible;

            textExec.Text = eventArgs.TruthTable.IsExecutable ?
                Resources.GetString(Resource.String.exec) :
                Resources.GetString(Resource.String.not_exec);
            textEq.Text = eventArgs.TruthTable.IsEquality ?
                Resources.GetString(Resource.String.eq) :
                Resources.GetString(Resource.String.not_eq);
        }

        private void CheckError(object sender, FormulaCalculatedEventArgs eventArgs)
        {
            var textError = FindViewById<TextView>(Resource.Id.text_error);
            textError.Visibility = eventArgs.IsSuccessful ?
                                     ViewStates.Invisible :
                                     ViewStates.Visible;
        }

        private void DisplayShowTableButton(object sender, FormulaCalculatedEventArgs eventArgs)
        {
            var button = FindViewById<Button>(Resource.Id.btn_showtable);

            if (eventArgs.TruthTable == null)
            {
                button.Visibility = ViewStates.Invisible;
                return;
            }

            button.Visibility = ViewStates.Visible;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            Init();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_info)
            {
                Intent nextActivity = new Intent(this, typeof(AuthorInfoActivity));
                StartActivity(nextActivity);
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private bool SaveFormula(string formula)
        {
            if (formula == null || !formula.Trim().Any())
            {
                return false;
            }
            
            try
            {
                var alltext = string.Empty;
                var file = OpenFileInput("formulas");
                using (var reader = new StreamReader(file))
                {
                    alltext = reader.ReadToEnd();
                }
                file.Close();

                file = OpenFileOutput("formulas", Android.Content.FileCreationMode.Private);
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(alltext);
                    writer.WriteLine(formula);
                }
                file.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private IEnumerable<string> ReadFormulas()
        {
            try
            {
                var text = string.Empty;
                var file = OpenFileInput("formulas");
                using (var reader = new StreamReader(file))
                {
                    text = reader.ReadToEnd();   
                }
                file.Close();
                return text.Split("\n").Where(l => l.Trim().Any());
            }
            catch
            {
            }

            return new List<string>();
        }

        private void OnFormulaCalculated(FormulaCalculatedEventArgs eventArgs)
        {
            if (FormulaCalculated == null)
            {
                return;
            }

            FormulaCalculated(this, eventArgs);
        }

        public class DrawLayer : ImageView
        {
            public Paint Paint { get; set; } = new Paint();

            public DrawLayer(Context context)
                : base(context)
            {
            }

            public DrawLayer(Context context, Android.Util.IAttributeSet attrs)
                : base(context, attrs)
            {

            }

            protected override void OnDraw(Canvas canvas)
            {
                base.OnDraw(canvas);

                var textSize = 100;
                var strokeWidth = 7;

                if (TruthTable == null)
                {
                    return;
                }

                Paint.Color = Color.BlueViolet;
                Paint.StrokeWidth = strokeWidth;
                Paint.TextSize = textSize;

                var centreX = GetX() + Width  / 2;
                var centreY = GetY() + Height / 2;

                var startX = textSize / 2;
                var startY = centreY / 2 + 200;

                canvas.DrawColor(Color.AliceBlue);
                
                var variablesString = new StringBuilder();

                for (int i = 0; i < TruthTable.Variables.Count(); ++i)
                {
                    variablesString.Append($"  {i}");
                }

                //canvas.DrawLine(variablesString.Length * textSize / 2, startY, variablesString.Length * textSize / 2, Height, Paint);
                canvas.DrawText(variablesString.ToString(), startX, startY, Paint);
                //canvas.DrawLine(startX, startY, Width, startY, Paint);
                startY += textSize;

                canvas.DrawLine(startX, startY - textSize + strokeWidth, variablesString.Length * textSize + textSize, startY - textSize + strokeWidth, Paint);
                canvas.DrawLine(startX + variablesString.Length * textSize / 2, startY - textSize * 2, startX + variablesString.Length * textSize / 2, startY + TruthTable.Table.Count * textSize, Paint);

                foreach (var row in TruthTable.Table)
                {
                    var rowAsString = new StringBuilder();
                    
                    foreach (var value in row.Key)
                    {
                        rowAsString.Append($"  {GetShortBoolString(value)}");
                    }

                    rowAsString.Append($"    {GetShortBoolString(row.Value)}");
                    canvas.DrawText(rowAsString.ToString(), startX, startY, Paint);
                    startY += textSize;
                }

                startY += 300;

                for (int i = 0; i < TruthTable.Variables.Count(); ++i)
                {
                    canvas.DrawText($"{i} - {TruthTable.Variables.ElementAt(i)}", startX, startY, Paint);
                }
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
}
