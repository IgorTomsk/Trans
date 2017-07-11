using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Mono.Data.Sqlite;
using System.Data;
using Android.Content.PM;
using Android.Views.InputMethods;

namespace Trans
{
    [Activity(Label = "UseYandexTranslate", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //   int count = 1;

        EditText inputTxt;
        TextView outputTxt;
        
        string translateDirection;

        ProgressDialog progressDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DataSQLDroid.LoadBase();
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustNothing);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            inputTxt = FindViewById<EditText>(Resource.Id.rusTxt);
            outputTxt = FindViewById<TextView>(Resource.Id.englText);

            var btnTrans = FindViewById<Button>(Resource.Id.Trans);
            var btnToFavor = FindViewById<Button>(Resource.Id.favor);
            var btnSeeFavor = FindViewById<Button>(Resource.Id.history);
            var btnSeeHistory = FindViewById<Button>(Resource.Id.historyH);

            inputTxt.Text = "Введите текст для перевода";
            outputTxt.Text = "Enter the text to be translated in above window ";
            inputTxt.Click += InputTxt_Click;

            RadioButton ruen = FindViewById<RadioButton>(Resource.Id.ruen);
            RadioButton enru = FindViewById<RadioButton>(Resource.Id.enru);

            ruen.Click += RadioButtonClick;
            enru.Click += RadioButtonClick;

            ruen.Checked = true;
            translateDirection = "ru-en";

            btnTrans.Click += BtnTrans_Click;
            btnSeeHistory.Click += BtnSeeHistory_Click;
            btnSeeFavor.Click += BtnSeeFavor_Click;

            var yandexLink = FindViewById<TextView>(Resource.Id.yandexLink);
            yandexLink.Click += delegate
            {
                var intent =
                    new Intent(
                    Intent.ActionView, Android.Net.Uri.Parse("http://translate.yandex.ru/"));
                    StartActivity(intent);
            };

            btnToFavor.Click += BtnToFavor_Click;
        }

        private void BtnSeeFavor_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SeeFavorite));
            StartActivity(intent);
        }

        private void BtnToFavor_Click(object sender, EventArgs e)
        {
            try
            {

                DataSQLDroid.ConnectionDB.Close();
                using (var commander = DataSQLDroid.ConnectionDB.CreateCommand())
                {
                    DataSQLDroid.ConnectionDB.Open();
                    commander.CommandText = "INSERT INTO History (TextFrom, TextTo, Direction, IsFavorite, TranData)"
                        + " VALUES ('" + inputTxt.Text + "', '" + outputTxt.Text + "','" + translateDirection + "',1,'" + DateTime.Now.ToString() + "')";
                    commander.CommandType = CommandType.Text;
                    commander.ExecuteNonQuery();
                    DataSQLDroid.ConnectionDB.Close();

                    Toast.MakeText(this, "Добавлено в Избранное", Android.Widget.ToastLength.Short).Show();
                }

            }
            catch (Exception ex)
            {
                DataSQLDroid.ConnectionDB.Close();
                Toast.MakeText(this, "Ошибка: " + ex, Android.Widget.ToastLength.Long).Show();

            }

        }

        private void BtnSeeHistory_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SeeHistory)); 
            StartActivity(intent);
        }

        private void BtnTrans_Click(object sender, EventArgs e)
        {
            progressDialog = ProgressDialog.Show(this, "Пожалуйста, подождите...", "Идет перевод...", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);

            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(inputTxt.WindowToken, 0);

            new Thread(new ThreadStart(delegate
            {
                //  Thread.Sleep(2000);
                var s = Translate.TranslateTxt(inputTxt.Text, translateDirection);
                //--------  ADD TO HISTORY -----------
                try
                {

                    DataSQLDroid.ConnectionDB.Close();
                    using (var commander = DataSQLDroid.ConnectionDB.CreateCommand())
                    {
                        DataSQLDroid.ConnectionDB.Open();
                        commander.CommandText = "INSERT INTO History (TextFrom, TextTo, Direction, IsFavorite, TranData)"
                            + " VALUES ('" + inputTxt.Text + "', '" + s + "','" + translateDirection + "',0,'" + DateTime.Now.ToString() +   "')";
                        commander.CommandType = CommandType.Text;
                        commander.ExecuteNonQuery();
                        DataSQLDroid.ConnectionDB.Close();
                    }
                 
                }
                catch (Exception ex)
                {
                    DataSQLDroid.ConnectionDB.Close();
                   
                }
                //------------------------------------
                RunOnUiThread(() => outputTxt.Text = s);
                progressDialog.Dismiss();

            })).Start();
         
        }

        private void InputTxt_Click(object sender, EventArgs e)
        {
            if (inputTxt.Text == "Введите текст для перевода")
            {
                inputTxt.Text = "";
                outputTxt.Text = "";
            }
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            translateDirection = rb.Text;
        }
    }
}

